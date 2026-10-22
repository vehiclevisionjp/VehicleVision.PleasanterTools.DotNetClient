namespace VehicleVision.PleasanterTools.DotNetClient.Generator.Tests;

/// <summary>生成器（リポジトリの api-surface/history.json と generation.json を入力にする）。</summary>
public sealed class GeneratorTests
{
    private static readonly GeneratorPaths Paths = new(GeneratorPaths.FindRoot(AppContext.BaseDirectory));

    private static History LoadHistory() => History.Load(File.ReadAllText(Paths.History));

    private static GenerationConfig LoadConfig() => GenerationConfig.Load(File.ReadAllText(Paths.Config));

    [Fact]
    public void コミットした生成物が最新である()
    {
        var files = GeneratorRunner.Generate(Paths);

        var differences = GeneratorRunner.Compare(Paths, files);

        Assert.True(
            differences.Count == 0,
            "生成物が古い。dotnet run --project tools/VehicleVision.PleasanterTools.DotNetClient.Generator -- generate を実行する: "
                + string.Join(", ", differences));
    }

    [Fact]
    public void 同じ入力からは同じ出力になる()
        => Assert.Equal(GeneratorRunner.Generate(Paths), GeneratorRunner.Generate(Paths));

    [Fact]
    public void すべてのエンドポイントが_生成するか対象外のどちらかになる()
    {
        var history = LoadHistory();

        var generator = new ClientGenerator(history, LoadConfig());

        Assert.Equal(history.Endpoints.Count, generator.Endpoints.Count + generator.Excluded.Count);
        Assert.All(generator.Excluded.Values, reason => Assert.False(string.IsNullOrWhiteSpace(reason)));
        Assert.Contains(generator.Endpoints, e => e.Key == "POST api/items/{id}/get");
        Assert.Contains("POST api/demo/register", generator.Excluded.Keys);
        Assert.Contains(generator.Excluded.Keys, k => k.StartsWith("GET scim/", StringComparison.Ordinal));
    }

    [Fact]
    public void 名前の変更の版が_history_と合わなければ止める()
    {
        var config = LoadConfig();
        var wrong = new GenerationConfig
        {
            MinimumVersion = config.MinimumVersion,
            ExcludedAreas = config.ExcludedAreas,
            ExcludedEndpoints = config.ExcludedEndpoints,
            RecordTypes = config.RecordTypes,
            ExtraTypes = config.ExtraTypes,
            ClientManagedMembers = config.ClientManagedMembers,
            Renames = [.. config.Renames.Select(r => r with { Version = "Pleasanter_1.4.8.0" })],
        };

        var ex = Assert.Throws<GenerationException>(() => new ClientGenerator(LoadHistory(), wrong));

        Assert.Contains("Extention", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void 対象外に書いたエンドポイントが_history_に無ければ止める()
    {
        var config = LoadConfig();
        var wrong = new GenerationConfig
        {
            MinimumVersion = config.MinimumVersion,
            ExcludedAreas = config.ExcludedAreas,
            ExcludedEndpoints = new Dictionary<string, string>(config.ExcludedEndpoints) { ["POST api/nothing"] = "x" },
            RecordTypes = config.RecordTypes,
            ExtraTypes = config.ExtraTypes,
            ClientManagedMembers = config.ClientManagedMembers,
            Renames = config.Renames,
        };

        Assert.Throws<GenerationException>(() => new ClientGenerator(LoadHistory(), wrong));
    }

    [Fact]
    public void 生成物に_Pleasanter_の名前空間を入れない()
    {
        var prefix = "Impl" + "em."; // このテスト自体も参照の分離の検査の対象なので、文字列を分けて書く

        Assert.All(GeneratorRunner.Generate(Paths), f => Assert.DoesNotContain(prefix, f.Value, StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("Dictionary<string,List<int?>>?", "Dictionary<string,List<int>>")]
    [InlineData("int?[]", "int[]")]
    [InlineData("Permissions.Types?", "Permissions.Types")]
    public void 型の書き方を読む(string text, string shape)
        => Assert.Equal(shape, TypeText.Parse(text).Shape());
}
