using System.Net;
using System.Net.Http.Headers;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>
/// 応答の <c>Retry-After</c>（<see cref="PleasanterResponse.RetryAfter"/>）の読み取り。通信はフェイクで、実機の Pleasanter には当てない。
/// </summary>
public sealed class RetryAfterTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 5, 3, 0, 0, TimeSpan.Zero);

    private static PleasanterOptions Options() => new()
    {
        BaseUrl = "https://example.invalid/",
        ApiKey = "secret-key",
        ServerVersion = new Version(1, 5, 9, 0),
        DetectServerVersion = false,
    };

    private static PleasanterApiClient ClientFor(Func<HttpResponseMessage> respond)
        => new(
            new HttpClient(new FakeHandler(_ => respond())),
            new StaticPleasanterOptionsProvider(Options()),
            new ServerVersionCache(),
            new FixedTimeProvider(Now));

    private static HttpResponseMessage Response(HttpStatusCode status, Action<HttpResponseMessage>? configure = null, string body = """{"StatusCode":429,"Message":"x"}""")
    {
        var response = FakeHandler.Json(status, body);
        configure?.Invoke(response);
        return response;
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(30, 30)]
    [InlineData(86400, 86400)]
    public async Task 秒指定は待つ長さへ読む_上限の丸めはしない(int seconds, int expectedSeconds)
    {
        var client = ClientFor(() => Response(HttpStatusCode.TooManyRequests, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(seconds))));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), response.RetryAfter);
        Assert.Equal(429, response.StatusCode);
        Assert.Equal(PleasanterErrorKind.Transient, response.ErrorKind);
    }

    [Fact]
    public async Task HTTP_date_が未来なら現在時刻との差()
    {
        var client = ClientFor(() => Response(HttpStatusCode.ServiceUnavailable, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(Now.AddSeconds(45))));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Equal(TimeSpan.FromSeconds(45), response.RetryAfter);
        Assert.Equal(503, response.StatusCode);
        Assert.Equal(PleasanterErrorKind.Transient, response.ErrorKind);
    }

    [Fact]
    public async Task HTTP_date_が過去なら_0()
    {
        var client = ClientFor(() => Response(HttpStatusCode.TooManyRequests, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(Now.AddSeconds(-30))));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Equal(TimeSpan.Zero, response.RetryAfter);
    }

    [Fact]
    public void 負の秒は_0()
        => Assert.Equal(TimeSpan.Zero, PleasanterApiClient.ParseRetryAfter(new RetryConditionHeaderValue(TimeSpan.FromSeconds(-5)), Now));

    [Fact]
    public void ヘッダが無ければ_null()
        => Assert.Null(PleasanterApiClient.ParseRetryAfter(null, Now));

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("-5")]
    [InlineData("1.5")]
    [InlineData("Mon, 99 Foo 2026 25:61:61 GMT")]
    public async Task 不正な値は_null_で_ステータスと分類は変わらない(string value)
    {
        var client = ClientFor(() => Response(HttpStatusCode.TooManyRequests, r => r.Headers.TryAddWithoutValidation("Retry-After", value)));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        // 秒の負の値（-5）は TimeSpan として読めずヘッダの構文エラーになり、null（クライアントは丸めて 0 にもしない）
        Assert.Null(response.RetryAfter);
        Assert.Equal(429, response.StatusCode);
        Assert.Equal(PleasanterErrorKind.Transient, response.ErrorKind);
    }

    [Fact]
    public async Task ヘッダが無い_429_は_null()
    {
        var client = ClientFor(() => Response(HttpStatusCode.TooManyRequests));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Null(response.RetryAfter);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.MovedPermanently)]
    [InlineData(HttpStatusCode.TemporaryRedirect)]
    public async Task ステータスに依らず_ヘッダがあれば読む_分類は変わらない(HttpStatusCode status)
    {
        var expected = PleasanterApiClient.Classify(status);
        var client = ClientFor(() => Response(status, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7)), body: "{}"));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Equal(TimeSpan.FromSeconds(7), response.RetryAfter);
        Assert.Equal(expected, response.ErrorKind);
        Assert.Equal((int)status, response.StatusCode);
    }

    [Fact]
    public async Task 本文の_StatusCode_由来の失敗でも_HTTP_ヘッダだけを見る()
    {
        // HTTP は 200・本文だけ 429。ヘッダが無いので null（本文に待ち時間の指定は無い）
        var noHeader = await ClientFor(() => Response(HttpStatusCode.OK, body: """{"StatusCode":429}""")).PostJsonAsync("api/items/1/get", null);
        Assert.Equal(PleasanterErrorKind.Transient, noHeader.ErrorKind);
        Assert.Equal(200, noHeader.StatusCode);
        Assert.Null(noHeader.RetryAfter);

        var withHeader = await ClientFor(() => Response(HttpStatusCode.OK, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(3)), body: """{"StatusCode":429}""")).PostJsonAsync("api/items/1/get", null);
        Assert.Equal(PleasanterErrorKind.Transient, withHeader.ErrorKind);
        Assert.Equal(TimeSpan.FromSeconds(3), withHeader.RetryAfter);
    }

    [Fact]
    public async Task 型付きの応答にも引き継ぐ()
    {
        var client = ClientFor(() => Response(HttpStatusCode.TooManyRequests, r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(12))));

        var response = await client.Items.GetAsync(1);

        Assert.Equal(TimeSpan.FromSeconds(12), response.RetryAfter);
        Assert.Equal(PleasanterErrorKind.Transient, response.ErrorKind);
    }

    [Fact]
    public async Task 応答が返らなかったときは_null()
    {
        var client = new PleasanterApiClient(
            new HttpClient(new FakeHandler { ThrowOnSend = true }),
            new StaticPleasanterOptionsProvider(Options()),
            new ServerVersionCache(),
            new FixedTimeProvider(Now));

        var response = await client.PostJsonAsync("api/items/1/get", null);

        Assert.Equal(PleasanterErrorKind.Unknown, response.ErrorKind);
        Assert.Null(response.RetryAfter);
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }
}
