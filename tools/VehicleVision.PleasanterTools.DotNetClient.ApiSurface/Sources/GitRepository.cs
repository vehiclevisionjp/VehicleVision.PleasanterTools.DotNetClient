using System.Diagnostics;
using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

/// <summary>
/// 抽出用の作業リポジトリ（Pleasanter のタグだけを浅く取得した、リポジトリ外の一時ディレクトリ）を git コマンドで扱う。
/// 読み取りは <c>GIT_NO_LAZY_FETCH=1</c> で行い、知らないうちにネットワークへ出ない。
/// </summary>
internal sealed class GitRepository : IDisposable
{
    private readonly string _directory;
    private Process? _catFile;

    public GitRepository(string directory)
    {
        _directory = Path.GetFullPath(directory);
    }

    public string Directory => _directory;

    /// <summary>作業リポジトリを用意する（無ければ作る）。</summary>
    public void Init(string remoteUrl)
    {
        System.IO.Directory.CreateDirectory(_directory);
        if (!System.IO.Directory.Exists(Path.Combine(_directory, ".git")))
        {
            Run(["init", "-q"]);
        }
        var remotes = Run(["remote"]).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (remotes.Contains("origin"))
        {
            Run(["remote", "set-url", "origin", remoteUrl]);
        }
        else
        {
            Run(["remote", "add", "origin", remoteUrl]);
        }
    }

    /// <summary>リモートのタグ名を返す。</summary>
    public IReadOnlyList<string> ListRemoteTags(string pattern)
        => [.. Run(["ls-remote", "--tags", "--refs", "origin", pattern])
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split('\t')[1].Trim())
            .Select(r => r["refs/tags/".Length..])];

    /// <summary>手元のタグ名を返す。</summary>
    public IReadOnlyList<string> ListLocalTags(string pattern)
        => [.. Run(["tag", "--list", pattern]).Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim())];

    /// <summary>タグを、履歴 1 件・ファイル本体なしで取得する（ツリーだけ）。</summary>
    public void FetchTagsTreeOnly(IReadOnlyList<string> tags)
    {
        foreach (var chunk in tags.Chunk(20))
        {
            var args = new List<string> { "fetch", "-q", "--depth", "1", "--filter=blob:none", "--no-tags", "origin" };
            foreach (var tag in chunk)
            {
                args.Add($"+refs/tags/{tag}:refs/tags/{tag}");
            }
            Run(args, lazyFetch: true);
        }
    }

    /// <summary>手元に無いオブジェクトだけを、ID 指定でまとめて取得する。</summary>
    public int FetchMissingBlobs(IEnumerable<string> oids)
    {
        var unique = oids.Distinct(StringComparer.Ordinal).ToList();
        var input = string.Join('\n', unique) + "\n";
        var missing = Run(["cat-file", "--batch-check"], input)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.EndsWith(" missing", StringComparison.Ordinal))
            .Select(line => line.Split(' ')[0])
            .ToList();
        foreach (var chunk in missing.Chunk(400))
        {
            var args = new List<string> { "-c", "fetch.negotiationAlgorithm=noop", "fetch", "-q", "--no-tags", "--no-write-fetch-head", "origin" };
            args.AddRange(chunk);
            Run(args, lazyFetch: true);
        }
        return missing.Count;
    }

    /// <summary>タグが指すコミットの ID。</summary>
    public string ResolveCommit(string tag) => Run(["rev-parse", $"{tag}^{{commit}}"]).Trim();

    /// <summary>タグのツリーにある blob を (パス, ID) で返す。</summary>
    public IReadOnlyList<(string Path, string Oid)> ListBlobs(string tag, IReadOnlyCollection<string> roots)
    {
        var args = new List<string> { "ls-tree", "-r", "-z", tag, "--" };
        args.AddRange(roots);
        var result = new List<(string, string)>();
        foreach (var entry in Run(args).Split('\0', StringSplitOptions.RemoveEmptyEntries))
        {
            // <mode> SP <type> SP <oid> TAB <path>
            var tab = entry.IndexOf('\t', StringComparison.Ordinal);
            var meta = entry[..tab].Split(' ');
            if (meta[1] == "blob")
            {
                result.Add((entry[(tab + 1)..], meta[2]));
            }
        }
        return result;
    }

    /// <summary>blob の内容を読む（常駐する <c>git cat-file --batch</c> を使う）。</summary>
    public byte[] ReadBlob(string oid)
    {
        _catFile ??= Start(["cat-file", "--batch"], lazyFetch: false);
        var stdin = _catFile.StandardInput;
        stdin.Write(oid + "\n");
        stdin.Flush();
        var stdout = _catFile.StandardOutput.BaseStream;
        var header = ReadLine(stdout);
        var parts = header.Split(' ');
        if (parts.Length < 3 || parts[1] != "blob")
        {
            throw new InvalidOperationException(
                $"blob {oid} を読めません（{header}）。先に prepare を実行して、ファイル本体を取得してください。");
        }
        var size = int.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
        var buffer = new byte[size];
        stdout.ReadExactly(buffer);
        stdout.ReadByte(); // 末尾の改行
        return buffer;
    }

    public void Dispose()
    {
        if (_catFile is { } p)
        {
            p.StandardInput.Close();
            p.WaitForExit();
            p.Dispose();
            _catFile = null;
        }
    }

    private static string ReadLine(Stream stream)
    {
        var bytes = new List<byte>();
        int b;
        while ((b = stream.ReadByte()) >= 0 && b != '\n')
        {
            bytes.Add((byte)b);
        }
        return Encoding.ASCII.GetString([.. bytes]);
    }

    private Process Start(IReadOnlyList<string> args, bool lazyFetch)
    {
        var psi = new ProcessStartInfo("git")
        {
            WorkingDirectory = _directory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = Encoding.UTF8,
        };
        foreach (var a in args)
        {
            psi.ArgumentList.Add(a);
        }
        if (!lazyFetch)
        {
            psi.Environment["GIT_NO_LAZY_FETCH"] = "1";
        }
        psi.Environment["GIT_TERMINAL_PROMPT"] = "0";
        return Process.Start(psi) ?? throw new InvalidOperationException("git を起動できません。");
    }

    private string Run(IReadOnlyList<string> args, string? input = null, bool lazyFetch = false)
    {
        using var p = Start(args, lazyFetch);
        // 入力と出力を同時に流す（大きな入力を書き切ってから読むと、出力のパイプが詰まって止まる）
        var inputTask = Task.Run(() =>
        {
            if (input is not null)
            {
                p.StandardInput.Write(input);
            }
            p.StandardInput.Close();
        });
        var errorTask = p.StandardError.ReadToEndAsync();
        var output = p.StandardOutput.ReadToEnd();
        inputTask.GetAwaiter().GetResult();
        p.WaitForExit();
        var error = errorTask.GetAwaiter().GetResult();
        if (p.ExitCode != 0)
        {
            throw new InvalidOperationException($"git {string.Join(' ', args.Take(4))} が失敗しました（{p.ExitCode}）: {error.Trim()}");
        }
        return output;
    }
}
