using VehicleVision.PleasanterTools.DotNetClient.Models;

namespace VehicleVision.PleasanterTools.DotNetClient.ContractTests;

/// <summary>全テストで 1 つの Pleasanter を共有する（試験は 1 つずつ動かす）。</summary>
[CollectionDefinition(Name)]
public sealed class ContractCollection : ICollectionFixture<ContractFixture>
{
    public const string Name = "contract";
}

/// <summary>クライアントを持ち、試験用のサイトを作る。</summary>
public sealed class ContractFixture : IDisposable
{
    private readonly HttpClient _http = new();

    public ContractFixture()
    {
        Client = ContractEnvironment.Enabled
            ? new PleasanterApiClient(_http, ContractEnvironment.CreateOptions())
            : null!;
    }

    public PleasanterApiClient Client { get; }

    /// <summary>試験ごとに一意な接尾辞（前回の残りと混ざらないように）。</summary>
    public static string Unique() => Guid.NewGuid().ToString("N")[..8];

    /// <summary>表（Results）のサイトを、最上位に作る。</summary>
    public async Task<long> CreateResultsSiteAsync(string title)
    {
        var response = await Client.Items.CreateSiteAsync(0, new SiteApiModel
        {
            Title = title,
            ReferenceType = "Results",
            SiteSettings = new SiteSettings { ReferenceType = "Results", Version = 1.017m },
        });
        Assert.True(response.IsSuccess, $"サイトの作成に失敗: {response.StatusCode} {response.Message}");
        return response.Id!.Value;
    }

    public void Dispose() => _http.Dispose();
}
