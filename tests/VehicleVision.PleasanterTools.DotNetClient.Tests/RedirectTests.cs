using System.Net;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>リダイレクトを辿らない（Issue #86）。API キーは本文に載るので、リダイレクト先のホストへ再送しない。</summary>
public sealed class RedirectTests
{
    private const string ApiKey = "secret-key-do-not-leak";

    private static PleasanterOptions Options(string baseUrl) => new()
    {
        BaseUrl = baseUrl,
        ApiKey = ApiKey,
        DetectServerVersion = false,
    };

    [Fact]
    public void 辿らないハンドラーは_自動のリダイレクトと_Cookie_を無効にする()
    {
        using var handler = PleasanterHttp.CreateHandler();

        Assert.False(handler.AllowAutoRedirect);
        Assert.False(handler.UseCookies);
    }

    [Fact]
    public void ハンドラーは呼ぶたびに別のインスタンス()
    {
        using var a = PleasanterHttp.CreateHandler();
        using var b = PleasanterHttp.CreateHandler();

        Assert.NotSame(a, b);
    }

    [Theory]
    [InlineData(301)]
    [InlineData(302)]
    [InlineData(303)]
    [InlineData(307)]
    [InlineData(308)]
    public async Task 辿らないクライアントは_3xx_を_Permanent_で返し_リダイレクト先へは何も送らない(int status)
    {
        using var target = new LoopbackServer();
        using var origin = new LoopbackServer(_ => LoopbackServer.Redirect(status, $"{target.BaseUrl}/api/items/1/get"));
        using var http = PleasanterHttp.CreateClient();
        var client = new PleasanterApiClient(http, Options(origin.BaseUrl));

        var response = await client.Items.GetAsync(1);

        Assert.Equal(PleasanterErrorKind.Permanent, response.ErrorKind);
        Assert.Equal(status, response.StatusCode);
        Assert.False(response.IsSuccess);
        Assert.Single(origin.Requests);
        // 本文（API キー）は 1 度だけ、BaseUrl のホストへ送られた
        Assert.Contains(ApiKey, origin.Requests[0].Body, StringComparison.Ordinal);
        Assert.Empty(target.Requests);
    }

    [Theory]
    [InlineData(307)]
    [InlineData(308)]
    public async Task 辿らないクライアントは_ファイルの送信でも_307_308_で本文を再送しない(int status)
    {
        using var target = new LoopbackServer();
        using var origin = new LoopbackServer(_ => LoopbackServer.Redirect(status, $"{target.BaseUrl}/api/binaries/upload"));
        using var http = PleasanterHttp.CreateClient();
        var client = new PleasanterApiClient(http, Options(origin.BaseUrl));
        using var content = new MemoryStream([1, 2, 3]);

        var response = await client.Binaries.UploadAsync(new PleasanterFile(content, "a.bin"));

        Assert.Equal(PleasanterErrorKind.Permanent, response.ErrorKind);
        Assert.Equal(status, response.StatusCode);
        Assert.Single(origin.Requests);
        // Binaries/upload は Authorization: Bearer で API キーを送る。これも再送されない
        Assert.Equal($"Bearer {ApiKey}", origin.Requests[0].Headers["Authorization"]);
        Assert.Empty(target.Requests);
    }

    [Theory]
    [InlineData(307)]
    [InlineData(308)]
    public async Task 対照_既定のハンドラーは_307_308_で本文をリダイレクト先へ再送する(int status)
    {
        // このテストが通ることで、上のテストが「辿らない設定だから」届かなかったことを裏づける
        using var target = new LoopbackServer();
        using var origin = new LoopbackServer(_ => LoopbackServer.Redirect(status, $"{target.BaseUrl}/api/items/1/get"));
        using var http = new HttpClient(new HttpClientHandler());
        var client = new PleasanterApiClient(http, Options(origin.BaseUrl));

        var response = await client.Items.GetAsync(1);

        Assert.True(response.IsSuccess);
        Assert.Single(target.Requests);
        Assert.Contains(ApiKey, target.Requests[0].Body, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(HttpStatusCode.MovedPermanently)]
    [InlineData(HttpStatusCode.Redirect)]
    [InlineData(HttpStatusCode.RedirectMethod)]
    [InlineData(HttpStatusCode.TemporaryRedirect)]
    [InlineData(HttpStatusCode.PermanentRedirect)]
    [InlineData(HttpStatusCode.NotModified)]
    public void 分類は_3xx_を_Permanent_にする(HttpStatusCode status)
    {
        Assert.Equal(PleasanterErrorKind.Permanent, PleasanterApiClient.Classify(status));
    }
}
