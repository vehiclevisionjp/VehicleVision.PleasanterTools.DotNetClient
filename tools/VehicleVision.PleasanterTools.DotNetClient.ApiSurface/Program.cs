using System.Diagnostics;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Output;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

// Pleasanter の API 面を抽出する（Issue #18）。使い方は _documents/API面の抽出.md。
const string Usage = """
    使い方:
      prepare --repo <作業リポジトリ> [--min 1.4.0.0] [--remote <URL>]
          タグを浅く取得し、抽出に要るファイルの本体だけを取得する（ネットワークを使う）
      run --repo <作業リポジトリ> --out-dir <出力先> [--min 1.4.0.0] [--snapshots <ディレクトリ>] [--doc-link <相対パス>]
          全タグを抽出し、latest.json・history.json・history.md を書く（ネットワークを使わない）
      extract (--repo <作業リポジトリ> --tag <タグ> | --source-dir <ディレクトリ>) --out <ファイル>
          1 つの版だけを抽出する
    """;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var options = new ExtractorOptions();
if (args.Length == 0)
{
    Console.Error.WriteLine(Usage);
    return 2;
}
var named = ParseArgs(args.Skip(1).ToArray());
try
{
    switch (args[0])
    {
        case "prepare":
            Prepare(named);
            return 0;
        case "run":
            RunAll(named);
            return 0;
        case "extract":
            ExtractOne(named);
            return 0;
        default:
            Console.Error.WriteLine(Usage);
            return 2;
    }
}
catch (Exception ex) when (ex is InvalidOperationException or ArgumentException or KeyNotFoundException)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

void Prepare(Dictionary<string, string> a)
{
    using var repo = new GitRepository(a["repo"]);
    repo.Init(a.GetValueOrDefault("remote") ?? "https://github.com/Implem/Implem.Pleasanter.git");
    var tags = SelectTags(repo.ListRemoteTags("Pleasanter_*"), a.GetValueOrDefault("min"));
    Console.WriteLine($"Fetching {tags.Count} tags (trees only)...");
    repo.FetchTagsTreeOnly(tags);
    var oids = new List<string>();
    foreach (var tag in tags)
    {
        foreach (var (path, oid) in repo.ListBlobs(tag, [.. options.SourceRoots]))
        {
            if (path.EndsWith(".cs", StringComparison.Ordinal) || options.ExtraFiles.Contains(path))
            {
                oids.Add(oid);
            }
        }
    }
    var fetched = repo.FetchMissingBlobs(oids);
    Console.WriteLine($"Fetched {fetched} missing blobs ({oids.Distinct().Count()} needed).");
}

void RunAll(Dictionary<string, string> a)
{
    using var repo = new GitRepository(a["repo"]);
    var tags = SelectTags(repo.ListLocalTags("Pleasanter_*"), a.GetValueOrDefault("min"));
    if (tags.Count == 0)
    {
        throw new InvalidOperationException("対象のタグがありません。先に prepare を実行してください。");
    }
    var cache = new ParseCache();
    var extractor = new SurfaceExtractor(options, cache);
    var surfaces = new List<SurfaceDocument>();
    var snapshots = a.GetValueOrDefault("snapshots");
    foreach (var tag in tags)
    {
        var sw = Stopwatch.StartNew();
        var surface = extractor.Extract(new GitTagSourceTree(repo, tag, [.. options.SourceRoots]));
        surfaces.Add(surface);
        Console.WriteLine($"{tag}: endpoints={surface.Endpoints.Count} types={surface.Types.Count} ({sw.ElapsedMilliseconds} ms)");
        if (snapshots is not null)
        {
            Json.Write(Path.Combine(snapshots, tag + ".json"), surface);
        }
    }
    var outDir = a["out-dir"];
    Json.Write(Path.Combine(outDir, "latest.json"), surfaces[^1]);
    var history = HistoryBuilder.Build(surfaces);
    Json.Write(Path.Combine(outDir, "history.json"), history);
    File.WriteAllText(
        Path.Combine(outDir, "history.md"),
        MarkdownWriter.Write(history, a.GetValueOrDefault("doc-link") ?? "../_documents/API面の抽出.md"),
        new System.Text.UTF8Encoding(false));
    Console.WriteLine($"Wrote {outDir} (items={history.Items.Count}).");
}

void ExtractOne(Dictionary<string, string> a)
{
    var extractor = new SurfaceExtractor(options);
    SurfaceDocument surface;
    if (a.TryGetValue("source-dir", out var dir))
    {
        surface = extractor.Extract(InMemorySourceTree.FromDirectory(dir, options.SourceRoots.ToList(), options.Extensions.ToList()));
    }
    else
    {
        using var repo = new GitRepository(a["repo"]);
        surface = extractor.Extract(new GitTagSourceTree(repo, a["tag"], [.. options.SourceRoots]));
    }
    Json.Write(a["out"], surface);
    Console.WriteLine($"endpoints={surface.Endpoints.Count} types={surface.Types.Count}");
}

static List<string> SelectTags(IEnumerable<string> tags, string? min)
{
    var minimum = Version.Parse(min ?? "1.4.0.0");
    return [.. tags
        .Select(t => (Tag: t, Version: TagVersion(t)))
        .Where(x => x.Version is not null && x.Version >= minimum)
        .OrderBy(x => x.Version)
        .Select(x => x.Tag)];
}

static Version? TagVersion(string tag)
{
    var i = tag.LastIndexOf('_');
    return Version.TryParse(i >= 0 ? tag[(i + 1)..] : tag, out var v) ? v : null;
}

static Dictionary<string, string> ParseArgs(string[] args)
{
    var result = new Dictionary<string, string>(StringComparer.Ordinal);
    for (var i = 0; i < args.Length; i++)
    {
        if (!args[i].StartsWith("--", StringComparison.Ordinal) || i + 1 >= args.Length)
        {
            throw new ArgumentException($"引数を解釈できません: {args[i]}");
        }
        result[args[i][2..]] = args[++i];
    }
    return result;
}
