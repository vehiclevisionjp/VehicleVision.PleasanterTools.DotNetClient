using System.Diagnostics;
using System.Net;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>版の問い合わせの記憶・再試行・待ち時間（Issue #87）。</summary>
public sealed class ServerVersionCacheTests
{
    private const string Ok = """{"StatusCode":200,"Response":{"Version":"1.5.9.1"}}""";

    private static readonly Version V1591 = new(1, 5, 9, 1);

    private static PleasanterOptions Options(
        TimeSpan? retry = null,
        TimeSpan? versionTimeout = null,
        string? baseUrl = null) => new()
    {
        // プロセスで共通の記憶を使うテストが、互いに干渉しないよう、接続先を分ける
        BaseUrl = baseUrl ?? $"https://{Guid.NewGuid():N}.example.invalid",
        ApiKey = "secret-key",
        DetectServerVersion = true,
        ServerVersionRetryInterval = retry ?? PleasanterOptions.DefaultServerVersionRetryInterval,
        ServerVersionTimeout = versionTimeout ?? PleasanterOptions.DefaultServerVersionTimeout,
    };

    [Fact]
    public void 既定は_問い合わせが有効で_失敗は短い間隔で再試行する()
    {
        var options = new PleasanterOptions { BaseUrl = "https://example.invalid", ApiKey = "k" };

        Assert.True(options.DetectServerVersion);
        Assert.Equal(TimeSpan.FromMinutes(1), options.ServerVersionRetryInterval);
        Assert.Equal(TimeSpan.FromSeconds(10), options.ServerVersionTimeout);
    }

    [Fact]
    public async Task クライアントを作り直しても_同じ接続先は問い合わせ直さない_AddTransient_相当()
    {
        var handler = FakeHandler.WithServerVersion("1.5.8.1");
        var options = Options();

        for (var i = 0; i < 3; i++)
        {
            var client = new PleasanterApiClient(new HttpClient(handler), options);
            await client.Items.GetAsync(1);
        }

        Assert.Single(handler.Requests, r => r.Path.EndsWith("GetLicenseInfo", StringComparison.Ordinal));
        Assert.Equal(3, handler.ApiRequests.Count());
    }

    [Fact]
    public async Task 接続先が違えば別に問い合わせる()
    {
        var handler = FakeHandler.WithServerVersion("1.5.8.1");
        var a = new PleasanterApiClient(new HttpClient(handler), Options());
        var b = new PleasanterApiClient(new HttpClient(handler), Options());

        await a.GetServerVersionAsync();
        await b.GetServerVersionAsync();

        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task 接続先の大文字小文字と末尾のスラッシュは区別しない()
    {
        var handler = FakeHandler.WithServerVersion("1.5.8.1");
        var host = $"{Guid.NewGuid():N}.example.invalid";
        var a = new PleasanterApiClient(new HttpClient(handler), Options(baseUrl: $"https://{host}"));
        var b = new PleasanterApiClient(new HttpClient(handler), Options(baseUrl: $"https://{host.ToUpperInvariant()}/"));

        await a.GetServerVersionAsync();
        await b.GetServerVersionAsync();

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task 同時の呼び出しでも_問い合わせは_1_回()
    {
        var probes = 0;
        var stub = new StubHandler(async (request, token) =>
        {
            Interlocked.Increment(ref probes);
            await Task.Delay(100, token);
            return FakeHandler.Json(HttpStatusCode.OK, Ok);
        });
        var client = new PleasanterApiClient(new HttpClient(stub), Options());

        var results = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => client.GetServerVersionAsync()));

        Assert.All(results, v => Assert.Equal(V1591, v));
        Assert.Equal(1, probes);
    }

    [Fact]
    public async Task 到達不能なら_問い合わせは_ServerVersionTimeout_で切り上げ_次の間隔まで繰り返さない()
    {
        var probes = 0;
        var stub = new StubHandler(async (request, token) =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("GetLicenseInfo", StringComparison.Ordinal))
            {
                Interlocked.Increment(ref probes);
                await Task.Delay(Timeout.Infinite, token);
            }

            return FakeHandler.Json(HttpStatusCode.OK, """{"Id":1,"StatusCode":200,"Message":"ok"}""");
        });
        var client = new PleasanterApiClient(
            new HttpClient(stub),
            Options(versionTimeout: TimeSpan.FromMilliseconds(200)));

        var stopwatch = Stopwatch.StartNew();
        var first = await client.Items.GetAsync(1);
        var second = await client.Items.GetAsync(1);

        // 既定の Timeout は 30 秒。版の問い合わせは 200 ミリ秒で切り上げ、2 回目は問い合わせない
        Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(10), $"待ちが長い: {stopwatch.Elapsed}");
        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Equal(1, probes);
    }

    [Fact]
    public async Task 取れなかった結果は_間隔のあいだ覚え_過ぎたら再試行し_取れたら覚え続ける()
    {
        var time = new ManualTimeProvider();
        var cache = new ServerVersionCache(time);
        var failing = true;
        var probes = 0;
        var stub = new StubHandler((request, token) =>
        {
            probes++;
            return Task.FromResult(failing
                ? FakeHandler.Json(HttpStatusCode.ServiceUnavailable, "{}")
                : FakeHandler.Json(HttpStatusCode.OK, Ok));
        });
        var options = Options(retry: TimeSpan.FromSeconds(30));
        var client = new PleasanterApiClient(new HttpClient(stub), new StaticPleasanterOptionsProvider(options), cache);

        Assert.Null(await client.GetServerVersionAsync());
        Assert.Null(await client.GetServerVersionAsync());
        Assert.Equal(1, probes);

        failing = false;
        time.Advance(TimeSpan.FromSeconds(29));
        Assert.Null(await client.GetServerVersionAsync());
        Assert.Equal(1, probes);

        time.Advance(TimeSpan.FromSeconds(2));
        Assert.Equal(V1591, await client.GetServerVersionAsync());
        Assert.Equal(2, probes);

        // 取れたら、時間が過ぎても問い合わせない
        time.Advance(TimeSpan.FromDays(7));
        Assert.Equal(V1591, await client.GetServerVersionAsync());
        Assert.Equal(2, probes);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.OK)] // JSON でない 200（経路の途中の応答など）
    public async Task 結論が出ない応答は_永続的には覚えない(HttpStatusCode status)
    {
        var time = new ManualTimeProvider();
        var cache = new ServerVersionCache(time);
        var probes = 0;
        var stub = new StubHandler((request, token) =>
        {
            probes++;
            return Task.FromResult(probes == 1
                ? new HttpResponseMessage(status) { Content = new StringContent("<html>", System.Text.Encoding.UTF8, "text/html") }
                : FakeHandler.Json(HttpStatusCode.OK, Ok));
        });
        var client = new PleasanterApiClient(new HttpClient(stub), new StaticPleasanterOptionsProvider(Options()), cache);

        Assert.Null(await client.GetServerVersionAsync());
        time.Advance(TimeSpan.FromMinutes(2));

        Assert.Equal(V1591, await client.GetServerVersionAsync());
    }

    [Fact]
    public async Task 通信の失敗も_間隔のあいだ覚える()
    {
        var time = new ManualTimeProvider();
        var cache = new ServerVersionCache(time);
        var probes = 0;
        var stub = new StubHandler((request, token) =>
        {
            probes++;
            throw new HttpRequestException("unreachable");
        });
        var client = new PleasanterApiClient(new HttpClient(stub), new StaticPleasanterOptionsProvider(Options()), cache);

        Assert.Null(await client.GetServerVersionAsync());
        Assert.Null(await client.GetServerVersionAsync());
        Assert.Equal(1, probes);

        time.Advance(TimeSpan.FromMinutes(2));
        Assert.Null(await client.GetServerVersionAsync());
        Assert.Equal(2, probes);
    }

    [Fact]
    public async Task 間隔が_0_なら_従来どおり呼び出しのたびに問い合わせる()
    {
        var probes = 0;
        var stub = new StubHandler((request, token) =>
        {
            probes++;
            return Task.FromResult(FakeHandler.Json(HttpStatusCode.ServiceUnavailable, "{}"));
        });
        var client = new PleasanterApiClient(new HttpClient(stub), Options(retry: TimeSpan.Zero));

        await client.GetServerVersionAsync();
        await client.GetServerVersionAsync();

        Assert.Equal(2, probes);
    }

    [Fact]
    public async Task 版の無い_200_と_404_は結論として覚える()
    {
        var time = new ManualTimeProvider();
        var cache = new ServerVersionCache(time);
        var probes = 0;
        var stub = new StubHandler((request, token) =>
        {
            probes++;
            return Task.FromResult(FakeHandler.Json(HttpStatusCode.OK, """{"StatusCode":200,"Response":{"CommercialLicense":false}}"""));
        });
        var client = new PleasanterApiClient(new HttpClient(stub), new StaticPleasanterOptionsProvider(Options()), cache);

        Assert.Null(await client.GetServerVersionAsync());
        time.Advance(TimeSpan.FromDays(1));
        Assert.Null(await client.GetServerVersionAsync());

        Assert.Equal(1, probes);
    }

    [Fact]
    public async Task 更新は問い合わせ直し_取れなければ覚えていた版を残す()
    {
        var mode = "ok1";
        var stub = new StubHandler((request, token) => Task.FromResult(mode switch
        {
            "ok1" => FakeHandler.Json(HttpStatusCode.OK, Ok),
            "ok2" => FakeHandler.Json(HttpStatusCode.OK, """{"StatusCode":200,"Response":{"Version":"1.5.9.2"}}"""),
            _ => FakeHandler.Json(HttpStatusCode.ServiceUnavailable, "{}"),
        }));
        var client = new PleasanterApiClient(new HttpClient(stub), Options());

        Assert.Equal(V1591, await client.GetServerVersionAsync());

        mode = "down";
        Assert.Equal(V1591, await client.RefreshServerVersionAsync());
        Assert.Equal(V1591, await client.GetServerVersionAsync());

        mode = "ok2";
        Assert.Equal(new Version(1, 5, 9, 2), await client.RefreshServerVersionAsync());
        Assert.Equal(new Version(1, 5, 9, 2), await client.GetServerVersionAsync());
    }

    [Fact]
    public async Task ServerVersion_の指定か_検出の無効なら_問い合わせない()
    {
        var handler = FakeHandler.WithServerVersion("1.5.9.0");
        var specified = Options();
        var disabled = new PleasanterOptions { BaseUrl = specified.BaseUrl, ApiKey = "k", DetectServerVersion = false };
        var fixedVersion = new PleasanterOptions { BaseUrl = specified.BaseUrl, ApiKey = "k", ServerVersion = new Version(1, 4, 6, 0) };

        Assert.Null(await new PleasanterApiClient(new HttpClient(handler), disabled).GetServerVersionAsync());
        Assert.Equal(new Version(1, 4, 6, 0), await new PleasanterApiClient(new HttpClient(handler), fixedVersion).GetServerVersionAsync());
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task PostJsonAsync_と_CheckConnectionAsync_は問い合わせない()
    {
        var handler = FakeHandler.WithServerVersion("1.5.9.0");
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.PostJsonAsync("api/items/1/get", null);
        await client.CheckConnectionAsync();

        Assert.DoesNotContain(handler.Requests, r => r.Path.EndsWith("GetLicenseInfo", StringComparison.Ordinal));
        Assert.Equal(2, handler.Requests.Count);
    }

    private sealed class StubHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => respond(request, cancellationToken);
    }

    private sealed class ManualTimeProvider : TimeProvider
    {
        private DateTimeOffset _now = new(2026, 11, 4, 0, 0, 0, TimeSpan.Zero);

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan span) => _now += span;
    }
}
