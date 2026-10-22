using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using VehicleVision.PleasanterTools.DotNetClient.Models;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>送受信の基本（URL・本文・応答の分類）。通信はフェイクで、実機の Pleasanter には当てない。</summary>
public sealed class PleasanterApiClientTests
{
    private static PleasanterOptions Options(string? serverVersion = "1.5.9.0") => new()
    {
        BaseUrl = "https://example.invalid/",
        ApiKey = "secret-key",
        ServerVersion = serverVersion is null ? null : Version.Parse(serverVersion),
        DetectServerVersion = false,
    };

    [Theory]
    [InlineData(HttpStatusCode.OK, PleasanterErrorKind.None)]
    [InlineData(HttpStatusCode.Unauthorized, PleasanterErrorKind.Unauthorized)]
    [InlineData(HttpStatusCode.BadRequest, PleasanterErrorKind.Permanent)]
    [InlineData(HttpStatusCode.TooManyRequests, PleasanterErrorKind.Transient)]
    [InlineData(HttpStatusCode.InternalServerError, PleasanterErrorKind.Transient)]
    public void Classify_はステータスを扱い方へ分類する(HttpStatusCode status, PleasanterErrorKind expected)
        => Assert.Equal(expected, PleasanterApiClient.Classify(status));

    [Fact]
    public async Task Items_CreateAsync_は_ApiKey_と_ApiVersion_とモデルの値を本文へ載せる()
    {
        var handler = new FakeHandler(_ => FakeHandler.Json(HttpStatusCode.OK, """{"Id":42,"StatusCode":200,"Message":"ok"}"""));
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        var response = await client.Items.CreateAsync(100, new IssueApiModel { Title = "t", ClassA = "a", NumA = 1.5m });

        Assert.True(response.IsSuccess);
        Assert.Equal(42, response.Id);
        Assert.Equal("ok", response.Message);
        var request = handler.Last;
        Assert.Equal("POST", request.Method);
        Assert.Equal("https://example.invalid/api/Items/100/Create", request.Uri);
        Assert.Equal("application/json", request.ContentType);
        var body = request.Json;
        Assert.Equal("secret-key", (string?)body["ApiKey"]);
        Assert.Equal(1.1m, (decimal?)body["ApiVersion"]);
        Assert.Equal("t", (string?)body["Title"]);
        Assert.Equal("a", (string?)body["ClassA"]);
        Assert.Equal(1.5m, (decimal?)body["NumA"]);
        // null のメンバーは送らない（サーバー側の既定値に任せる）
        Assert.False(body.ContainsKey("Body"));
        Assert.False(body.ContainsKey("ClassB"));
    }

    [Fact]
    public async Task Items_GetAsync_は_Api_の値と列挙の名前を送る()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.Items.GetAsync(7, new Api
        {
            Offset = 10,
            TableType = TableTypes.Deleted,
            View = new View
            {
                ColumnFilterHash = new() { ["Status"] = "[100]" },
                ColumnSorterHash = new() { ["UpdatedTime"] = SqlOrderByTypes.desc },
            },
        });

        var request = handler.Last;
        Assert.Equal("https://example.invalid/api/Items/7/Get", request.Uri);
        var body = request.Json;
        Assert.Equal(10, (int?)body["Offset"]);
        Assert.False(body.ContainsKey("PageSize"));
        Assert.Equal("Deleted", (string?)body["TableType"]);
        Assert.Equal("[100]", (string?)body["View"]!["ColumnFilterHash"]!["Status"]);
        Assert.Equal("desc", (string?)body["View"]!["ColumnSorterHash"]!["UpdatedTime"]);
    }

    [Fact]
    public async Task Items_UpsertAsync_は_Api_とレコードの型を_1_つの本文に重ねる()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.Items.UpsertAsync(9, new ResultApiModel { Title = "x", ClassA = "k" }, new Api { Keys = ["ClassA"] });

        var body = handler.Last.Json;
        Assert.Equal("https://example.invalid/api/Items/9/Upsert", handler.Last.Uri);
        Assert.Equal("x", (string?)body["Title"]);
        Assert.Equal("ClassA", (string?)body["Keys"]![0]);
        Assert.Equal("secret-key", (string?)body["ApiKey"]);
    }

    [Fact]
    public async Task ルートの引数の有無で_URL_を選ぶ()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.Users.GetAsync();
        await client.Users.GetAsync(3);
        await client.Depts.DeleteAsync(4);
        await client.Items.DeleteAsync(5);

        Assert.Equal(
            [
                "https://example.invalid/api/Users/Get",
                "https://example.invalid/api/Users/3/Get",
                "https://example.invalid/api/Depts/4/Delete",
                "https://example.invalid/api/Items/5/Delete",
            ],
            handler.ApiRequests.Select(r => r.Uri));
        // 本文の型が無いエンドポイントは、ApiKey・ApiVersion だけを送る
        Assert.Equal(["ApiKey", "ApiVersion"], handler.Last.Json.Select(p => p.Key).Order(StringComparer.Ordinal));
    }

    [Fact]
    public async Task OutgoingMails_SendAsync_はルートの_controller_をコントローラ名にする()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.OutgoingMails.SendAsync("items", 12, new OutgoingMailApiModel { Title = "件名", To = "a@example.invalid" });

        Assert.Equal("https://example.invalid/api/items/12/OutgoingMails/Send", handler.Last.Uri);
        Assert.Equal("件名", (string?)handler.Last.Json["Title"]);
    }

    [Fact]
    public async Task クエリ文字列の値を_URL_へ付ける()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.BackgroundTasks.RebuildSearchIndexesAsync(1, noLog: true);

        Assert.Equal("https://example.invalid/api/BackgroundTasks/1/RebuildSearchIndexes?NoLog=true", handler.Last.Uri);
    }

    [Fact]
    public async Task Import_はフォームの_parameters_に本文を入れ_ファイルを添付する()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());
        using var csv = new MemoryStream(Encoding.UTF8.GetBytes("Title\na\n"));

        await client.Items.ImportAsync(20, new PleasanterFile(csv, "data.csv", "text/csv"), new ImportApi { Encoding = "UTF-8", UpdatableImport = true });

        var request = handler.Last;
        Assert.Equal("https://example.invalid/api/Items/20/Import", request.Uri);
        Assert.Equal("multipart/form-data", request.ContentType);
        var parameters = request.Json;
        Assert.Equal("secret-key", (string?)parameters["ApiKey"]);
        Assert.Equal("UTF-8", (string?)parameters["Encoding"]);
        Assert.True((bool?)parameters["UpdatableImport"]);
        Assert.Equal(("Title\na\n", "data.csv"), request.Parts["file"]);
        Assert.True(csv.CanRead); // 渡したストリームは閉じない
    }

    [Fact]
    public async Task Binaries_UploadAsync_は_Bearer_でキーを送り_本文に_ApiKey_を入れない()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());
        using var file = new MemoryStream([1, 2, 3]);

        await client.Binaries.UploadAsync(new PleasanterFile(file, "a.bin"), id: 5, fileHash: "h");

        var request = handler.Last;
        Assert.Equal("https://example.invalid/api/Binaries/upload?id=5", request.Uri);
        Assert.Equal("Bearer secret-key", request.Authorization);
        Assert.Equal("h", request.Parts["FileHash"].Text);
        Assert.Equal("a.bin", request.Parts["file"].FileName);
        Assert.DoesNotContain(request.Parts.Values, p => p.Text.Contains("secret-key", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Binaries_GetStreamAsync_はバイト列を返す()
    {
        var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent([9, 8, 7]),
        });
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        var response = await client.Binaries.GetStreamAsync("ABC");

        Assert.True(response.IsSuccess);
        Assert.Equal([9, 8, 7], response.Content);
        Assert.Equal("https://example.invalid/api/Binaries/ABC/getstream", handler.Last.Uri);
    }

    [Fact]
    public async Task Items_GetAsync_の応答を包みと_Response_へ読み_Data_は利用者が型を選ぶ()
    {
        var handler = new FakeHandler(_ => FakeHandler.Json(
            HttpStatusCode.OK,
            """
            {"StatusCode":200,"LimitPerDate":1000,"LimitRemaining":999,
             "Response":{"Offset":0,"PageSize":200,"TotalCount":1,
               "Data":[{"IssueId":1,"Title":"a","Status":100,"AttachmentsHash":{"AttachmentsA":[{"Guid":"G","Extension":".txt"}]}}]}}
            """));
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        var response = await client.Items.GetAsync(1);

        Assert.True(response.IsSuccess);
        var envelope = response.Value!;
        Assert.Equal(200, envelope.StatusCode);
        Assert.Equal(999, envelope.LimitRemaining);
        Assert.Equal(1, envelope.Response!.TotalCount!.Value.GetInt32());
        var records = await client.DeserializeAsync<List<IssueApiModel>>(envelope.Response.Data);
        Assert.Equal("a", records![0].Title);
        Assert.Equal(100, records[0].Status);
        Assert.Equal(".txt", records[0].AttachmentsHash!["AttachmentsA"][0].Extension);
    }

    [Fact]
    public async Task 失敗の応答は値を読まず_分類して返す()
    {
        var handler = new FakeHandler(_ => FakeHandler.Json(HttpStatusCode.Unauthorized, """{"Id":0,"StatusCode":401,"Message":"Unauthorized"}"""));
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        var response = await client.Items.GetAsync(1);

        Assert.Equal(PleasanterErrorKind.Unauthorized, response.ErrorKind);
        Assert.Null(response.Value);
        Assert.Equal("Unauthorized", response.Message);
    }

    [Fact]
    public async Task 通信に失敗したら_Unknown_を返す()
    {
        var client = new PleasanterApiClient(new HttpClient(new FakeHandler { ThrowOnSend = true }), Options());

        var response = await client.Items.GetAsync(1);

        Assert.Equal(PleasanterErrorKind.Unknown, response.ErrorKind);
    }

    [Fact]
    public async Task PostJsonAsync_は型付けしない本文に_ApiKey_を入れて送る()
    {
        var handler = new FakeHandler();
        var client = new PleasanterApiClient(new HttpClient(handler), Options());

        await client.PostJsonAsync("api/items/1/get", new JsonObject { ["View"] = new JsonObject { ["Incomplete"] = true } });

        Assert.Equal("https://example.invalid/api/items/1/get", handler.Last.Uri);
        Assert.Equal("secret-key", (string?)handler.Last.Json["ApiKey"]);
        Assert.True((bool?)handler.Last.Json["View"]!["Incomplete"]);
    }
}
