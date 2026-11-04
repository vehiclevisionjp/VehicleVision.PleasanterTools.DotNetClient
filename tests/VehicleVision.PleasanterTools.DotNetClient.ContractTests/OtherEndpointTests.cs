using System.Text;
using VehicleVision.PleasanterTools.DotNetClient.Models;

namespace VehicleVision.PleasanterTools.DotNetClient.ContractTests;

/// <summary>ユーザー・組織・グループ・添付・セッション・版の問い合わせ・エラーの形を、実機の Pleasanter に対して確かめる。</summary>
[Collection(ContractCollection.Name)]
[Trait("Category", "Contract")]
public sealed class OtherEndpointTests(ContractFixture fixture)
{
    private PleasanterApiClient Client => fixture.Client;

    [ContractFact]
    public async Task 版は_GetLicenseInfo_から取れる()
    {
        var version = await Client.GetServerVersionAsync();
        Assert.NotNull(version);
        Assert.True(version >= new Version(1, 4, 21, 0));
        if (Environment.GetEnvironmentVariable("PLEASANTER_EXPECTED_VERSION") is { Length: > 0 } expected)
        {
            Assert.Equal(Version.Parse(expected), version);
        }

        var response = await Client.Utility.GetLicenseInfoAsync();
        Assert.True(response.IsSuccess);
        Assert.Equal(version.ToString(), response.Body!["Response"]!["Version"]!.GetValue<string>());
    }

    [ContractFact]
    public async Task 接続の確認は成功し_誤ったキーは_Unauthorized_になる()
    {
        Assert.True(await Client.CheckConnectionAsync());

        using var http = new HttpClient();
        var wrong = new PleasanterApiClient(http, new PleasanterOptions
        {
            BaseUrl = ContractEnvironment.CreateOptions().BaseUrl,
            ApiKey = "wrong-" + ContractFixture.Unique(),
            DetectServerVersion = false,
        });
        var response = await wrong.Items.GetSiteAsync(1);
        Assert.Equal(PleasanterErrorKind.Unauthorized, response.ErrorKind);
        Assert.Equal(401, response.StatusCode);
    }

    [ContractFact]
    public async Task 無いレコードは恒久的な失敗として返る()
    {
        var response = await Client.Items.GetAsync(999_999_999);
        Assert.False(response.IsSuccess);
        Assert.Equal(PleasanterErrorKind.Permanent, response.ErrorKind);
        Assert.NotNull(response.StatusCode);
    }

    [ContractFact]
    public async Task ユーザー_グループ_組織を作成_取得_更新_削除できる()
    {
        var unique = ContractFixture.Unique();

        var user = await Client.Users.CreateAsync(new UserApiModel
        {
            LoginId = "contract-" + unique,
            Name = "契約 " + unique,
            Password = "Pw#" + Guid.NewGuid().ToString("N"),
            TenantId = 1,
        });
        Assert.True(user.IsSuccess, $"{user.StatusCode} {user.Message}");
        var userId = (int)user.Id!.Value;
        try
        {
            var got = await Client.Users.GetAsync(userId);
            Assert.True(got.IsSuccess, $"{got.StatusCode} {got.Message}");
            Assert.Equal("contract-" + unique, got.Body!["Response"]!["Data"]![0]!["LoginId"]!.GetValue<string>());

            var updated = await Client.Users.UpdateAsync(userId, new UserApiModel { Name = "契約 改 " + unique });
            Assert.True(updated.IsSuccess, $"{updated.StatusCode} {updated.Message}");
            var after = await Client.Users.GetAsync(userId);
            Assert.Equal("契約 改 " + unique, after.Body!["Response"]!["Data"]![0]!["Name"]!.GetValue<string>());
        }
        finally
        {
            Assert.True((await Client.Users.DeleteAsync(userId)).IsSuccess);
        }

        var group = await Client.Groups.CreateAsync(new GroupApiModel { GroupName = "契約G-" + unique });
        Assert.True(group.IsSuccess, $"{group.StatusCode} {group.Message}");
        var groupId = (int)group.Id!.Value;
        try
        {
            var got = await Client.Groups.GetAsync(groupId);
            Assert.Equal("契約G-" + unique, got.Body!["Response"]!["Data"]![0]!["GroupName"]!.GetValue<string>());
            Assert.True((await Client.Groups.UpdateAsync(groupId, new GroupApiModel { GroupName = "契約G改-" + unique })).IsSuccess);
        }
        finally
        {
            Assert.True((await Client.Groups.DeleteAsync(groupId)).IsSuccess);
        }

        var dept = await Client.Depts.CreateAsync(new DeptApiModel { DeptCode = "c" + unique, DeptName = "契約課-" + unique });
        Assert.True(dept.IsSuccess, $"{dept.StatusCode} {dept.Message}");
        var deptId = (int)dept.Id!.Value;
        try
        {
            var got = await Client.Depts.GetAsync(deptId);
            Assert.Equal("契約課-" + unique, got.Body!["Response"]!["Data"]![0]!["DeptName"]!.GetValue<string>());
            Assert.True((await Client.Depts.UpdateAsync(deptId, new DeptApiModel { DeptName = "契約課改-" + unique })).IsSuccess);
        }
        finally
        {
            Assert.True((await Client.Depts.DeleteAsync(deptId)).IsSuccess);
        }
    }

    [ContractFact]
    public async Task セッションの値を設定_取得_削除できる()
    {
        var key = "k-" + ContractFixture.Unique();
        Assert.True((await Client.Sessions.SetAsync(new SessionApi { SessionKey = key, SessionValue = "v" })).IsSuccess);
        var got = await Client.Sessions.GetAsync(new SessionApi { SessionKey = key });
        Assert.Equal("v", got.Body!["Response"]!["Value"]!.GetValue<string>());
        Assert.True((await Client.Sessions.DeleteAsync(new SessionApi { SessionKey = key })).IsSuccess);
    }

    [ContractFact]
    public async Task 添付ファイルを送り_取得し_ストリームで読める()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-bin-" + ContractFixture.Unique());
        try
        {
            var record = await Client.Items.CreateAsync(site, new ResultApiModel { Title = "添付" });
            var recordId = record.Id!.Value;
            var bytes = Encoding.UTF8.GetBytes("hello 契約");
            var upload = await Client.Binaries.UploadAsync(
                new PleasanterFile(new MemoryStream(bytes), "a.txt", "text/plain"),
                recordId);
            Assert.True(upload.IsSuccess, $"{upload.StatusCode} {upload.Message}");
            var guid = upload.Message!;
            Assert.Equal(32, guid.Length);

            var meta = await Client.Binaries.GetAsync(guid);
            Assert.True(meta.IsSuccess, $"{meta.StatusCode} {meta.Message}");

            var stream = await Client.Binaries.GetStreamAsync(guid);
            Assert.True(stream.IsSuccess, $"{stream.StatusCode}");
            Assert.Equal(bytes, stream.Content);
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }

    [ContractFact]
    public async Task 無い添付の取得は_Permanent_で_中身が無い()
    {
        var stream = await Client.Binaries.GetStreamAsync(new string('0', 32));
        Assert.False(stream.IsSuccess);
        Assert.Equal(PleasanterErrorKind.Permanent, stream.ErrorKind);
        Assert.Null(stream.Content);

        var meta = await Client.Binaries.GetAsync(new string('0', 32));
        Assert.False(meta.IsSuccess);
    }

    [ContractFact]
    public async Task クエリ文字列の真偽値は_true_で受け付けられる()
    {
        var response = await Client.BackgroundTasks.RebuildSearchIndexesAsync(noLog: true);
        Assert.True(response.IsSuccess, $"{response.StatusCode} {response.Message}");
    }

    [ContractFact]
    public async Task メール送信の口は表の種類を入れて届く()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-mail-" + ContractFixture.Unique());
        try
        {
            var record = await Client.Items.CreateAsync(site, new ResultApiModel { Title = "m" });
            var response = await Client.OutgoingMails.SendAsync(
                "Results",
                record.Id!.Value,
                new OutgoingMailApiModel { To = "to@example.invalid", Title = "t", Body = "b" });
            // 口には届く（404 ではない）。送信できるかはサーバーのメール設定・権限による
            Assert.NotEqual(404, response.StatusCode);
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }
}
