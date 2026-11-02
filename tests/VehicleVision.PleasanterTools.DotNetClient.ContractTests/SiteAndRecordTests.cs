using System.Text.Json;
using VehicleVision.PleasanterTools.DotNetClient.Models;

namespace VehicleVision.PleasanterTools.DotNetClient.ContractTests;

/// <summary>サイトとレコード（<c>Items</c>）の主な口を、実機の Pleasanter に対して確かめる。</summary>
[Collection(ContractCollection.Name)]
[Trait("Category", "Contract")]
public sealed class SiteAndRecordTests(ContractFixture fixture)
{
    private PleasanterApiClient Client => fixture.Client;

    private static List<ResultApiModel> Rows(PleasanterResponse<PleasanterEnvelope<ItemsGetResponse>> response)
    {
        Assert.True(response.IsSuccess, $"{response.StatusCode} {response.Message}");
        return response.Value!.Response!.Data.Deserialize<List<ResultApiModel>>() ?? [];
    }

    private static Api AscById => new()
    {
        View = new View { ColumnSorterHash = new() { ["ResultId"] = SqlOrderByTypes.asc } },
    };

    [ContractFact]
    public async Task サイトを作成_取得_更新_削除できる()
    {
        var title = "契約-site-" + ContractFixture.Unique();
        var id = await fixture.CreateResultsSiteAsync(title);
        try
        {
            var got = await Client.Items.GetSiteAsync(id);
            Assert.True(got.IsSuccess, $"{got.StatusCode} {got.Message}");
            var data = got.Value!.Response!.Data!.Value;
            Assert.Equal(title, data.GetProperty("Title").GetString());
            Assert.Equal("Results", data.GetProperty("ReferenceType").GetString());
            Assert.Equal(id, data.GetProperty("SiteId").GetInt64());

            var renamed = title + "-2";
            var updated = await Client.Items.UpdateSiteAsync(id, new SiteApiModel { Title = renamed });
            Assert.True(updated.IsSuccess, $"{updated.StatusCode} {updated.Message}");
            Assert.Equal(id, updated.Id);

            var settings = await Client.Items.UpdateSiteSettingsAsync(id, siteSettings: new SiteSettingsApiModel());
            Assert.True(settings.IsSuccess, $"{settings.StatusCode} {settings.Message}");

            var again = await Client.Items.GetSiteAsync(id);
            Assert.Equal(renamed, again.Value!.Response!.Data!.Value.GetProperty("Title").GetString());
        }
        finally
        {
            var deleted = await Client.Items.DeleteSiteAsync(id);
            Assert.True(deleted.IsSuccess, $"{deleted.StatusCode} {deleted.Message}");
        }

        var gone = await Client.Items.GetSiteAsync(id);
        Assert.False(gone.IsSuccess);
    }

    [ContractFact]
    public async Task レコードを作成_取得_更新_削除できる()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-rec-" + ContractFixture.Unique());
        try
        {
            var created = await Client.Items.CreateAsync(site, new ResultApiModel { Title = "r1", Body = "本文" });
            Assert.True(created.IsSuccess, $"{created.StatusCode} {created.Message}");
            var id = created.Id!.Value;

            var row = Assert.Single(Rows(await Client.Items.GetAsync(id)));
            Assert.Equal("r1", row.Title);
            Assert.Equal("本文", row.Body);
            Assert.Equal(site, row.SiteId);
            Assert.Equal(id, row.ResultId);

            var updated = await Client.Items.UpdateAsync(id, new ResultApiModel { Title = "r2" });
            Assert.True(updated.IsSuccess, $"{updated.StatusCode} {updated.Message}");
            var after = Assert.Single(Rows(await Client.Items.GetAsync(id)));
            Assert.Equal("r2", after.Title);
            Assert.Equal("本文", after.Body);

            var deleted = await Client.Items.DeleteAsync(id);
            Assert.True(deleted.IsSuccess, $"{deleted.StatusCode} {deleted.Message}");
            Assert.False((await Client.Items.GetAsync(id)).IsSuccess);
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }

    [ContractFact]
    public async Task 一覧はページ指定_並べ替え_絞り込みを列挙の名前で送れる()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-list-" + ContractFixture.Unique());
        try
        {
            foreach (var (title, cls) in new[] { ("a", "x"), ("b", "y"), ("c", "x") })
            {
                var r = await Client.Items.CreateAsync(site, new ResultApiModel { Title = title, ClassHash = new() { ["ClassA"] = cls } });
                Assert.True(r.IsSuccess, $"{r.StatusCode} {r.Message}");
            }

            var all = await Client.Items.GetAsync(site);
            Assert.Equal(3, all.Value!.Response!.TotalCount!.Value.GetInt32());

            // 並べ替え: SqlOrderByTypes を名前（"desc"）で送る
            var desc = Rows(await Client.Items.GetAsync(site, new Api
            {
                View = new View { ColumnSorterHash = new() { ["ResultId"] = SqlOrderByTypes.desc } },
            }));
            Assert.Equal(["c", "b", "a"], desc.Select(r => r.Title));
            Assert.Equal(["a", "b", "c"], Rows(await Client.Items.GetAsync(site, AscById)).Select(r => r.Title));

            // ページ指定
            var page = await Client.Items.GetAsync(site, new Api
            {
                Offset = 1,
                PageSize = 1,
                View = new View { ColumnSorterHash = new() { ["ResultId"] = SqlOrderByTypes.asc } },
            });
            Assert.Equal(1, page.Value!.Response!.Offset!.Value.GetInt32());
            Assert.Equal(1, page.Value.Response.PageSize!.Value.GetInt32());
            Assert.Equal(3, page.Value.Response.TotalCount!.Value.GetInt32());
            Assert.Equal("b", Assert.Single(Rows(page)).Title);

            // 絞り込み
            var filtered = Rows(await Client.Items.GetAsync(site, new Api
            {
                View = new View { ColumnFilterHash = new() { ["ClassA"] = "y" } },
            }));
            Assert.Equal("b", Assert.Single(filtered).Title);
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }

    [ContractFact]
    public async Task Upsert_と_BulkUpsert_と_BulkDelete()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-upsert-" + ContractFixture.Unique());
        try
        {
            var first = await Client.Items.UpsertAsync(
                site,
                new ResultApiModel { Title = "u1", ClassHash = new() { ["ClassA"] = "k1" } },
                new Api { Keys = ["ClassA"] });
            Assert.True(first.IsSuccess, $"{first.StatusCode} {first.Message}");
            var again = await Client.Items.UpsertAsync(
                site,
                new ResultApiModel { Title = "u1-2", ClassHash = new() { ["ClassA"] = "k1" } },
                new Api { Keys = ["ClassA"] });
            Assert.True(again.IsSuccess, $"{again.StatusCode} {again.Message}");

            var bulk = await Client.Items.BulkUpsertAsync(site, new ResultBulkUpsertApiModel
            {
                Data = [new ResultApiModel { Title = "u2", ClassHash = new() { ["ClassA"] = "k2" } }],
                Keys = ["ClassA"],
                KeyNotFoundCreate = true,
            });
            Assert.True(bulk.IsSuccess, $"{bulk.StatusCode} {bulk.Message}");

            Assert.Equal(["u1-2", "u2"], Rows(await Client.Items.GetAsync(site, AscById)).Select(r => r.Title));

            var removed = await Client.Items.BulkDeleteAsync(
                site,
                new ApiDeleteOption { PhysicalDelete = true },
                new RecordSelector { All = true });
            Assert.True(removed.IsSuccess, $"{removed.StatusCode} {removed.Message}");
            Assert.Empty(Rows(await Client.Items.GetAsync(site)));
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }

    [ContractFact]
    public async Task 一覧の出力_Export_は名前と内容を返す()
    {
        var site = await fixture.CreateResultsSiteAsync("契約-export-" + ContractFixture.Unique());
        try
        {
            var response = await Client.Items.ExportAsync(site, new ExportApi());
            Assert.True(response.IsSuccess, $"{response.StatusCode} {response.Message}");
            Assert.False(string.IsNullOrEmpty(response.Value!.Response!.Name!.Value.GetString()));
            Assert.Contains("Title", response.Value.Response.Content!.Value.GetString(), StringComparison.Ordinal);
        }
        finally
        {
            await Client.Items.DeleteSiteAsync(site);
        }
    }
}
