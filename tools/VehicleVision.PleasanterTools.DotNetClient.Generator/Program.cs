using VehicleVision.PleasanterTools.DotNetClient.Generator;

// api-surface/history.json から、クライアントのモデルとメソッドを生成する（Issue #20）。
//   generate [--root <リポジトリ>]  生成物を書く（古いファイルは消す）
//   check    [--root <リポジトリ>]  生成物が最新かを確かめる（差分があれば終了コード 1）
if (args.Length == 0 || args[0] is not ("generate" or "check"))
{
    Console.Error.WriteLine("usage: generate|check [--root <repository>]");
    return 2;
}

var root = args.Length >= 3 && args[1] == "--root" ? Path.GetFullPath(args[2]) : GeneratorPaths.FindRoot(Directory.GetCurrentDirectory());
try
{
    var paths = new GeneratorPaths(root);
    var files = GeneratorRunner.Generate(paths);
    if (args[0] == "generate")
    {
        GeneratorRunner.Write(paths, files);
        Console.WriteLine($"Wrote {files.Count} files to {paths.Output}.");
        return 0;
    }

    var differences = GeneratorRunner.Compare(paths, files);
    foreach (var d in differences)
    {
        Console.Error.WriteLine(d);
    }

    Console.WriteLine(differences.Count == 0 ? "Generated code is up to date." : $"Generated code is out of date ({differences.Count} files). Run: generate");
    return differences.Count == 0 ? 0 : 1;
}
catch (GenerationException ex)
{
    Console.Error.WriteLine($"生成を止めた: {ex.Message}");
    return 1;
}
