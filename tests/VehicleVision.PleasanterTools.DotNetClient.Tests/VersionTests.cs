using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using VehicleVision.PleasanterTools.DotNetClient.Models;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>Since・Until による版の判定と、名前の変更・列挙の数値の読み替え。</summary>
public sealed class VersionTests
{
    private static PleasanterOptions Options(
        string? serverVersion,
        bool detect = false,
        UnknownServerVersionBehavior unknown = UnknownServerVersionBehavior.Attempt) => new()
    {
        BaseUrl = "https://example.invalid",
        ApiKey = "secret-key",
        ServerVersion = serverVersion is null ? null : Version.Parse(serverVersion),
        DetectServerVersion = detect,
        UnknownServerVersion = unknown,
    };

    [Fact]
    public async Task サーバーの版が足りないエンドポイントは_送らずに例外にする()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options("1.5.8.0"));

        var ex = await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(
            () => client.Items.MoveSiteAsync(1, new SiteApiModel { ParentId = 2 }));

        Assert.Contains("Items.MoveSiteAsync", ex.Member, StringComparison.Ordinal);
        Assert.Equal(new Version(1, 5, 9, 0), ex.Since);
        Assert.Null(ex.Until);
        Assert.Equal(new Version(1, 5, 8, 0), ex.ServerVersion);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task サーバーの版が足りていれば送る()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options("1.5.9.0"));

        var response = await client.Items.MoveSiteAsync(1, new SiteApiModel { ParentId = 2 });

        Assert.True(response.IsSuccess);
        Assert.Equal("https://example.invalid/api/Items/1/MoveSite", handler.Last.Uri);
    }

    [Fact]
    public async Task サーバーの版で使えないメンバーに値があれば_送らずに例外にする()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options("1.5.5.0"));

        var ex = await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(
            () => client.Items.GetAsync(1, new Api { SsCache = true }));

        Assert.Equal("Api.SsCache", ex.Member);
        Assert.Equal(new Version(1, 5, 6, 0), ex.Since);
        Assert.Empty(handler.Requests);

        // 値が無ければ（null は送らないので）止めない
        await client.Items.GetAsync(1, new Api { Offset = 0 });
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task 継承したメンバーも版で判定する()
    {
        var client = new PleasanterApiClient(new HttpClient(new FakeHandler()), Options("1.4.12.0"));

        // ExportApi は Api を継承する。PageSize は 1.4.13.0 から
        var ex = await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(
            () => client.Items.ExportAsync(1, new ExportApi { PageSize = 10 }));

        Assert.Equal("Api.PageSize", ex.Member);
    }

    [Fact]
    public void Until_を過ぎた版では使えない()
    {
        var unknown = UnknownServerVersionBehavior.Attempt;
        var since = new Version(1, 4, 0, 0);
        var until = new Version(1, 5, 6, 0);

        Assert.Null(VersionGuard.Check("X", since, until, new Version(1, 5, 5, 0), unknown));
        var ex = VersionGuard.Check("X", since, until, new Version(1, 5, 6, 0), unknown);
        Assert.NotNull(ex);
        Assert.Equal(until, ex.Until);
        Assert.NotNull(VersionGuard.Check("X", since, until, new Version(1, 5, 9, 0), unknown));
    }

    [Fact]
    public void Until_のメンバーに値があれば_JSON_へ書く前に例外にする()
    {
        var json = PleasanterJson.For(new Version(1, 5, 6, 0), UnknownServerVersionBehavior.Attempt);

        var ex = Assert.Throws<PleasanterVersionNotSupportedException>(
            () => JsonSerializer.Serialize(new RemovedMemberSample { Old = "x" }, json));

        Assert.Equal("RemovedMemberSample.Old", ex.Member);
        Assert.Equal("{}", JsonSerializer.Serialize(new RemovedMemberSample(), json));
        Assert.Equal("""{"Old":"x"}""", JsonSerializer.Serialize(new RemovedMemberSample { Old = "x" }, PleasanterJson.For(new Version(1, 5, 5, 0), UnknownServerVersionBehavior.Attempt)));
    }

    [Fact]
    public async Task 版が不明のとき_既定では拒否せず送る()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null));

        await client.Items.MoveSiteAsync(1, new SiteApiModel());
        await client.Items.GetAsync(1, new Api { SsCache = true });

        Assert.Equal(2, handler.Requests.Count);
        Assert.True((bool?)handler.Last.Json["SsCache"]);
    }

    [Fact]
    public async Task 版が不明で_Reject_なら_版を問わず使えるもの以外は例外にする()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null, unknown: UnknownServerVersionBehavior.Reject));

        var ex = await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(() => client.Items.MoveSiteAsync(1, new SiteApiModel()));
        Assert.Null(ex.ServerVersion);
        await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(() => client.Items.GetAsync(1, new Api { SsCache = true }));
        Assert.Empty(handler.Requests);

        // 1.4.0.0 以前から在るものは送る
        await client.Items.GetAsync(1, new Api { Offset = 5 });
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetLicenseInfo_の_Version_で版を取り_接続先ごとに覚える()
    {
        var handler = FakeHandler.WithServerVersion("1.5.8.1");
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null, detect: true));

        Assert.Equal(new Version(1, 5, 8, 1), await client.GetServerVersionAsync());
        await Assert.ThrowsAsync<PleasanterVersionNotSupportedException>(() => client.Items.MoveSiteAsync(1, new SiteApiModel()));
        await client.Items.GetAsync(1);

        var probes = handler.Requests.Where(r => r.Path == "/api/utility/GetLicenseInfo").ToList();
        Assert.Single(probes);
        Assert.Equal("secret-key", (string?)probes[0].Json["ApiKey"]);
        Assert.Single(handler.ApiRequests);
    }

    [Fact]
    public async Task GetLicenseInfo_に_Version_が無ければ_版は不明()
    {
        // 1.4.13.0〜1.4.20.0 の応答の形（Version が無い）
        var handler = FakeHandler.WithServerVersion(null);
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null, detect: true));

        Assert.Null(await client.GetServerVersionAsync());
        await client.Items.MoveSiteAsync(1, new SiteApiModel()); // 既定（Attempt）では送る
        Assert.Single(handler.ApiRequests);
    }

    [Fact]
    public async Task GetLicenseInfo_が_404_なら版は不明で_問い合わせを繰り返さない()
    {
        var handler = new FakeHandler(r => r.Path.EndsWith("GetLicenseInfo", StringComparison.Ordinal)
            ? new HttpResponseMessage(HttpStatusCode.NotFound)
            : FakeHandler.Json(HttpStatusCode.OK, "{}"));
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null, detect: true));

        Assert.Null(await client.GetServerVersionAsync());
        Assert.Null(await client.GetServerVersionAsync());
        Assert.Single(handler.Requests);

        // やり直しは明示する
        Assert.Null(await client.RefreshServerVersionAsync());
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task 利用者が版を指定すれば問い合わせない()
    {
        var handler = FakeHandler.WithServerVersion("1.5.9.0");
        var client = new PleasanterApiClient(new HttpClient(handler), Options("1.4.6.0", detect: true));

        Assert.Equal(new Version(1, 4, 6, 0), await client.GetServerVersionAsync());
        Assert.Empty(handler.Requests);
    }

    [Theory]
    [InlineData("1.4.6.0", "Extention")]
    [InlineData("1.4.7.0", "Extension")]
    [InlineData("1.5.9.0", "Extension")]
    public async Task 名前が変わったメンバーは_旧い版へは旧い名前で送る(string serverVersion, string expected)
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options(serverVersion));

        await client.Items.UpdateAsync(1, new IssueApiModel { AttachmentsHash = new() { ["AttachmentsA"] = [new Attachment { Name = "a.txt", Extension = ".txt" }] } });

        var attachment = handler.Last.Json["AttachmentsHash"]!["AttachmentsA"]![0]!.AsObject();
        Assert.Equal(".txt", (string?)attachment[expected]);
        Assert.Single(attachment, p => p.Key.StartsWith("Exten", StringComparison.Ordinal));
    }

    [Fact]
    public async Task 名前が変わったメンバーは_版が不明なら両方の名前で送る()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options(null));

        await client.Items.UpdateAsync(1, new IssueApiModel { AttachmentsHash = new() { ["AttachmentsA"] = [new Attachment { Extension = ".txt" }] } });

        var attachment = handler.Last.Json["AttachmentsHash"]!["AttachmentsA"]![0]!;
        Assert.Equal(".txt", (string?)attachment["Extension"]);
        Assert.Equal(".txt", (string?)attachment["Extention"]);
    }

    [Theory]
    [InlineData("1.4.6.0", """{"Extention":".csv"}""")]
    [InlineData("1.5.9.0", """{"Extension":".csv"}""")]
    [InlineData(null, """{"Extention":".csv"}""")]
    [InlineData(null, """{"Extension":".csv"}""")]
    public void 名前が変わったメンバーは_旧い名前でも新しい名前でも読める(string? serverVersion, string json)
    {
        var options = PleasanterJson.For(serverVersion is null ? null : Version.Parse(serverVersion), UnknownServerVersionBehavior.Attempt);

        var attachment = JsonSerializer.Deserialize<Attachment>(json, options);

        Assert.Equal(".csv", attachment!.Extension);
    }

    [Theory]
    [InlineData("1.4.9.0", 3, DataChange.Types.InputDate)]
    [InlineData("1.4.10.0", 3, DataChange.Types.InputValueFormula)]
    [InlineData("1.5.9.0", 7, DataChange.Types.InputDept)]
    [InlineData(null, 3, DataChange.Types.InputValueFormula)]
    public void 数値が版で変わった列挙は_サーバーの版に合わせて読む(string? serverVersion, int number, DataChange.Types expected)
    {
        var options = PleasanterJson.For(serverVersion is null ? null : Version.Parse(serverVersion), UnknownServerVersionBehavior.Attempt);

        var change = JsonSerializer.Deserialize<DataChange>($$"""{"Type":{{number}}}""", options);

        Assert.Equal(expected, change!.Type);
    }

    [Fact]
    public void 列挙は名前で書き_名前と数値の両方を読む()
    {
        var options = PleasanterJson.For(new Version(1, 5, 9, 0), UnknownServerVersionBehavior.Attempt);

        Assert.Equal("""{"Type":"InputDate"}""", JsonSerializer.Serialize(new DataChange { Type = DataChange.Types.InputDate }, options));
        Assert.Equal(DataChange.Types.InputUser, JsonSerializer.Deserialize<DataChange>("""{"Type":"inputuser"}""", options)!.Type);

        // 名前の無い値（組み合わせ）は数値で書く
        var permissions = (PermissionsTypes)((long)PermissionsTypes.Read | (long)PermissionsTypes.Update);
        Assert.Equal("5", JsonSerializer.Serialize(permissions, options));
    }

    [Fact]
    public void 版で使えない列挙値は_書く前に例外にする()
    {
        // Notification.Types.LineWorks は 1.5.1.0 から
        var options = PleasanterJson.For(new Version(1, 5, 0, 0), UnknownServerVersionBehavior.Attempt);

        var ex = Assert.Throws<PleasanterVersionNotSupportedException>(
            () => JsonSerializer.Serialize(new Notification { Type = Notification.Types.LineWorks }, options));

        Assert.Equal("Types.LineWorks", ex.Member);
        Assert.Equal("""{"Type":"Slack"}""", JsonSerializer.Serialize(new Notification { Type = Notification.Types.Slack }, options));
    }

    [Fact]
    public void 応答の匿名型は型付けせず_JsonElement_で受ける()
    {
        var node = JsonNode.Parse("""{"Offset":0,"TotalCount":2,"Data":[1,2]}""");

        var response = node.Deserialize<ItemsGetResponse>(PleasanterJson.For(null, UnknownServerVersionBehavior.Attempt));

        Assert.Equal(JsonValueKind.Array, response!.Data!.Value.ValueKind);
        Assert.Equal(2, response.TotalCount.Deserialize<int>());
    }

    /// <summary>Until を持つメンバーの例（生成したモデルには、送るメンバーで Until を持つものが無いため）。</summary>
    private sealed class RemovedMemberSample
    {
        [Since("1.4.0.0")]
        [Until("1.5.6.0")]
        public string? Old { get; set; }
    }
}
