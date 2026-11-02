namespace VehicleVision.PleasanterTools.DotNetClient.ContractTests;

/// <summary>接続先は環境変数で受ける。API キーはコード・ログ・例外の文言に出さない。</summary>
internal static class ContractEnvironment
{
    public static bool Enabled => Environment.GetEnvironmentVariable(ContractFactAttribute.OptInVariable) == "1";

    public static PleasanterOptions CreateOptions()
        => new()
        {
            BaseUrl = Required("PLEASANTER_BASE_URL"),
            ApiKey = Required("PLEASANTER_API_KEY"),
        };

    private static string Required(string name)
        => Environment.GetEnvironmentVariable(name) is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException($"環境変数 {name} が要る");
}
