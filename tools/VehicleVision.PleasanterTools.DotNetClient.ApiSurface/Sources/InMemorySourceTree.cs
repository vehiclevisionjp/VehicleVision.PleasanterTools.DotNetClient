using System.Security.Cryptography;
using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

/// <summary>
/// メモリ上のファイル集合。テストと、ディレクトリから読む場合に使う。
/// </summary>
internal sealed class InMemorySourceTree : ISourceTree
{
    private readonly SortedDictionary<string, string> _files;

    public InMemorySourceTree(string label, IDictionary<string, string> files, string? revision = null)
    {
        Label = label;
        Revision = revision;
        _files = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var (path, text) in files)
        {
            _files[path.Replace('\\', '/')] = text;
        }
        Files = [.. _files.Keys];
    }

    public string Label { get; }

    public string? Revision { get; }

    public IReadOnlyList<string> Files { get; }

    public string ContentKey(string path)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(_files[path])));

    public string? Read(string path) => _files.TryGetValue(path, out var text) ? text : null;

    /// <summary>ディレクトリの下にある、指定した拡張子のファイルを読む。</summary>
    public static InMemorySourceTree FromDirectory(string root, IReadOnlyCollection<string> roots, IReadOnlyCollection<string> extensions)
    {
        var files = new Dictionary<string, string>(StringComparer.Ordinal);
        var full = Path.GetFullPath(root);
        foreach (var sub in roots)
        {
            var dir = Path.Combine(full, sub);
            if (!Directory.Exists(dir))
            {
                continue;
            }
            foreach (var file in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories))
            {
                if (!extensions.Any(e => file.EndsWith(e, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                var relative = Path.GetRelativePath(full, file).Replace('\\', '/');
                files[relative] = TextDecoding.Decode(File.ReadAllBytes(file));
            }
        }
        return new InMemorySourceTree(Path.GetFileName(full.TrimEnd(Path.DirectorySeparatorChar)), files);
    }
}

internal static class TextDecoding
{
    private static readonly UTF8Encoding Utf8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

    /// <summary>UTF-8 として読み、先頭の BOM を除く。</summary>
    public static string Decode(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
        {
            bytes = bytes[3..];
        }
        return Utf8.GetString(bytes);
    }
}
