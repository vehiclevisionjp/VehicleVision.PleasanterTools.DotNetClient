using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>入力と出力の場所（リポジトリの中の決まった場所）。</summary>
internal sealed class GeneratorPaths(string root)
{
    public string Root { get; } = root;

    public string History => Path.Combine(Root, "api-surface", "history.json");

    public string Config => Path.Combine(Root, "tools", "VehicleVision.PleasanterTools.DotNetClient.Generator", "generation.json");

    public string Output => Path.Combine(Root, "src", "VehicleVision.PleasanterTools.DotNetClient", "Generated");

    /// <summary>ソリューション（*.slnx）のあるディレクトリを、上へたどって探す。</summary>
    public static string FindRoot(string start)
    {
        for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
        {
            if (dir.EnumerateFiles("VehicleVision.PleasanterTools.DotNetClient.slnx").Any())
            {
                return dir.FullName;
            }
        }

        throw new GenerationException($"{start} から上にリポジトリ（VehicleVision.PleasanterTools.DotNetClient.slnx）が見つかりません。");
    }
}

/// <summary>生成・書き出し・比較。</summary>
internal static class GeneratorRunner
{
    private static readonly UTF8Encoding Utf8 = new(encoderShouldEmitUTF8Identifier: false);

    public static SortedDictionary<string, string> Generate(GeneratorPaths paths)
    {
        var history = History.Load(File.ReadAllText(paths.History, Utf8));
        var config = GenerationConfig.Load(File.ReadAllText(paths.Config, Utf8));
        return new ClientGenerator(history, config).Generate();
    }

    /// <summary>生成物を書き、生成しなくなった古い <c>*.g.cs</c> を消す。</summary>
    public static void Write(GeneratorPaths paths, IReadOnlyDictionary<string, string> files)
    {
        foreach (var stale in Existing(paths).Keys.Where(k => !files.ContainsKey(k)))
        {
            File.Delete(Path.Combine(paths.Output, stale));
        }

        foreach (var (relative, text) in files)
        {
            var full = Path.Combine(paths.Output, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(full)!);
            File.WriteAllText(full, text, Utf8);
        }
    }

    /// <summary>生成物と、置いてあるファイルの違い（無ければ空）。改行の違い（CRLF・LF）は無視する。</summary>
    public static List<string> Compare(GeneratorPaths paths, IReadOnlyDictionary<string, string> files)
    {
        var existing = Existing(paths);
        var differences = new List<string>();
        foreach (var (relative, text) in files)
        {
            if (!existing.TryGetValue(relative, out var current))
            {
                differences.Add($"missing: {relative}");
            }
            else if (Normalize(current) != Normalize(text))
            {
                differences.Add($"changed: {relative}");
            }
        }

        differences.AddRange(existing.Keys.Where(k => !files.ContainsKey(k)).Select(k => $"stale: {k}"));
        return differences;
    }

    private static Dictionary<string, string> Existing(GeneratorPaths paths)
        => Directory.Exists(paths.Output)
            ? Directory.EnumerateFiles(paths.Output, "*.g.cs", SearchOption.AllDirectories)
                .ToDictionary(
                    f => Path.GetRelativePath(paths.Output, f).Replace('\\', '/'),
                    f => File.ReadAllText(f, Utf8),
                    StringComparer.Ordinal)
            : [];

    private static string Normalize(string text) => text.Replace("\r\n", "\n", StringComparison.Ordinal);
}
