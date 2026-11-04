namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>BaseUrl の検査（Issue #85）。API キーは本文に載るので、安全でない URL へは送らない。</summary>
public sealed class BaseUrlTests
{
    private const string ApiKey = "secret-key-do-not-leak";

    private static PleasanterOptions Options(string baseUrl) => new()
    {
        BaseUrl = baseUrl,
        ApiKey = ApiKey,
        DetectServerVersion = false,
    };

    [Theory]
    [InlineData("https://pleasanter.example.com")]
    [InlineData("https://pleasanter.example.com/")]
    [InlineData("https://pleasanter.example.com:8443")]
    [InlineData("https://pleasanter.example.com/pleasanter")]
    [InlineData("HTTPS://Pleasanter.Example.com")]
    [InlineData("http://localhost")]
    [InlineData("http://localhost:8080")]
    [InlineData("http://127.0.0.1:8080")]
    [InlineData("http://[::1]:8080")]
    [InlineData("https://localhost:8080")]
    public void 使える_BaseUrl_は通る(string baseUrl)
    {
        Options(baseUrl).Validate();
    }

    [Theory]
    [InlineData("http://pleasanter.example.com")]
    [InlineData("http://192.0.2.10")]
    [InlineData("http://localhost.example.com")]
    [InlineData("http://127.0.0.1.example.com")]
    [InlineData("https://user:pass@pleasanter.example.com")]
    [InlineData("https://user@pleasanter.example.com")]
    [InlineData("http://user:pass@localhost")]
    [InlineData("https://pleasanter.example.com?x=1")]
    [InlineData("https://pleasanter.example.com/?")]
    [InlineData("https://pleasanter.example.com/#frag")]
    [InlineData("https://pleasanter.example.com/a?apikey=1#b")]
    [InlineData("ftp://pleasanter.example.com")]
    [InlineData("file:///etc/passwd")]
    [InlineData("pleasanter.example.com")]
    [InlineData("/relative")]
    [InlineData("")]
    [InlineData("   ")]
    public void 使えない_BaseUrl_は拒否する(string baseUrl)
    {
        var ex = Assert.Throws<ArgumentException>(() => Options(baseUrl).Validate());

        Assert.Equal("BaseUrl", ex.ParamName);
    }

    [Theory]
    [InlineData("https://admin:hunter2@pleasanter.example.com/?token=abc#frag")]
    [InlineData("http://admin:hunter2@pleasanter.example.com")]
    [InlineData("https://admin:hunter2@pleasanter.example.com")]
    public void 拒否の文言に_API_キーも_URL_の資格情報も出さない(string baseUrl)
    {
        var ex = Assert.Throws<ArgumentException>(() => Options(baseUrl).Validate());

        Assert.DoesNotContain(ApiKey, ex.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("hunter2", ex.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("admin", ex.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("abc", ex.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("pleasanter.example.com", ex.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task 使えない_BaseUrl_なら_通信せずに例外にする()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options("http://pleasanter.example.com"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Items.GetAsync(1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.PostJsonAsync("api/items/1/get", null));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CheckConnectionAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetServerVersionAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.RefreshServerVersionAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.DeserializeAsync<object>(System.Text.Json.JsonDocument.Parse("{}").RootElement));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task 版の問い合わせでも_使えない_BaseUrl_へは送らない()
    {
        var handler = new FakeHandler();
        var options = new PleasanterOptions { BaseUrl = "http://pleasanter.example.com", ApiKey = ApiKey, DetectServerVersion = true };
        var client = new PleasanterApiClient(new HttpClient(handler), options);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Items.GetAsync(1));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task ループバックの_http_は送れる()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options("http://localhost:8080"));

        var response = await client.Items.GetAsync(1);

        Assert.True(response.IsSuccess);
        Assert.Equal("http://localhost:8080/api/Items/1/Get", handler.Last.Uri);
    }

    [Fact]
    public async Task 接続設定を解決するたびに検査する()
    {
        var handler = new FakeHandler();
        var provider = new SwitchingProvider(Options("https://pleasanter.example.com"));
        var client = new PleasanterApiClient(new HttpClient(handler), provider);

        Assert.True((await client.Items.GetAsync(1)).IsSuccess);

        provider.Current = Options("http://pleasanter.example.com");
        await Assert.ThrowsAsync<ArgumentException>(() => client.Items.GetAsync(1));
        Assert.Single(handler.Requests);
    }

    private sealed class SwitchingProvider(PleasanterOptions initial) : IPleasanterOptionsProvider
    {
        public PleasanterOptions Current { get; set; } = initial;

        public Task<PleasanterOptions> GetAsync(CancellationToken cancellationToken = default) => Task.FromResult(Current);
    }
}
