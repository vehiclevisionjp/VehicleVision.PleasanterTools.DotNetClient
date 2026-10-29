using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;

/// <summary>
/// 構文木のキャッシュ。内容のキー（git の blob ID など）ごとに 1 回だけ解析し、版をまたいで使い回す。
/// </summary>
internal sealed class ParseCache
{
    private static readonly CSharpParseOptions Options = new(
        LanguageVersion.Latest,
        DocumentationMode.None,
        SourceCodeKind.Regular);

    private readonly Dictionary<string, SyntaxTree> _trees = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IReadOnlyList<string>> _declaredNames = new(StringComparer.Ordinal);
    private readonly Dictionary<string, HashSet<string>> _calledNames = new(StringComparer.Ordinal);

    public SyntaxTree Parse(string key, Func<string> read)
    {
        if (!_trees.TryGetValue(key, out var tree))
        {
            tree = CSharpSyntaxTree.ParseText(read(), Options);
            _trees[key] = tree;
        }
        return tree;
    }

    public IReadOnlyList<string> DeclaredNames(string key, Func<string> read)
    {
        if (!_declaredNames.TryGetValue(key, out var names))
        {
            names = [.. CodeIndex.DeclarationPattern().Matches(read()).Select(m => m.Groups[1].Value).Distinct(StringComparer.Ordinal)];
            _declaredNames[key] = names;
        }
        return names;
    }

    /// <summary>ファイルの中で「名前(」の形で現れる識別子（呼び出し・宣言の候補）。</summary>
    public HashSet<string> CalledNames(string key, Func<string> read)
    {
        if (!_calledNames.TryGetValue(key, out var names))
        {
            names = new HashSet<string>(CodeIndex.CallPattern().Matches(read()).Select(m => m.Groups[1].Value), StringComparer.Ordinal);
            _calledNames[key] = names;
        }
        return names;
    }
}

/// <summary>型の宣言（partial はまとめる）。</summary>
internal sealed class TypeEntry
{
    public required string FullName { get; init; }

    public required string SimpleName { get; init; }

    public required string Kind { get; init; }

    public required int Arity { get; init; }

    public List<(BaseTypeDeclarationSyntax Declaration, string Path)> Declarations { get; } = [];

    public bool IsValueType => Kind is "struct" or "enum" or "record struct";

    public IEnumerable<(MethodDeclarationSyntax Method, string Path)> Methods(string name)
        => Declarations
            .Where(d => d.Declaration is TypeDeclarationSyntax)
            .SelectMany(d => ((TypeDeclarationSyntax)d.Declaration).Members
                .OfType<MethodDeclarationSyntax>()
                .Where(m => m.Identifier.ValueText == name)
                .Select(m => (m, d.Path)));
}

/// <summary>名前を解決するときの文脈（どのファイルの、どの名前空間・型の中か）。</summary>
internal sealed record ResolveContext(string Path, string Namespace, IReadOnlyList<string> Usings, IReadOnlyList<string> ContainingTypes);

/// <summary>
/// ある版のソースツリーの索引。必要になった型の宣言を含むファイルだけを、遅延して解析する。
/// 意味解析（コンパイル）はしない。名前の解決は、構文と名前空間・using からの推定である（限界は文書に書く）。
/// </summary>
internal sealed partial class CodeIndex
{
    // 型の宣言を探すための粗い検索（解析するファイルを絞るためだけに使い、結果は構文木で確かめる）。
    [GeneratedRegex(@"\b(?:class|struct|enum|interface|record)\s+([A-Za-z_][A-Za-z0-9_]*)")]
    internal static partial Regex DeclarationPattern();

    [GeneratedRegex(@"\b([A-Za-z_][A-Za-z0-9_]*)\s*\(")]
    internal static partial Regex CallPattern();

    // BCL・外部ライブラリでよく使う名前。ソース内の同名の入れ子の型と取り違えないよう、解決しない。
    private static readonly HashSet<string> WellKnownExternal = new(StringComparer.Ordinal)
    {
        "String", "Object", "List", "IList", "Dictionary", "IDictionary", "IEnumerable", "ICollection",
        "IReadOnlyList", "IReadOnlyDictionary", "IReadOnlyCollection", "HashSet", "ISet", "Nullable",
        "DateTime", "DateTimeOffset", "TimeSpan", "Guid", "Decimal", "Int32", "Int64", "Boolean", "Double",
        "JObject", "JToken", "JArray", "JValue", "ExpandoObject", "KeyValuePair", "Tuple", "Stream",
    };

    private readonly ISourceTree _tree;
    private readonly ParseCache _cache;
    private readonly List<string> _csFiles;
    private readonly HashSet<string> _indexedFiles = new(StringComparer.Ordinal);
    private readonly Dictionary<string, TypeEntry> _types = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<TypeEntry>> _bySimpleName = new(StringComparer.Ordinal);
    private Dictionary<string, List<string>>? _filesByDeclaredName;

    public CodeIndex(ISourceTree tree, ExtractorOptions options, ParseCache cache)
    {
        _tree = tree;
        _cache = cache;
        _csFiles = [.. tree.Files.Where(f => f.EndsWith(".cs", StringComparison.Ordinal)
            && options.SourceRoots.Any(r => f.StartsWith(r.TrimEnd('/') + "/", StringComparison.Ordinal)))];
    }

    public ISourceTree Tree => _tree;

    public IReadOnlyList<string> CsFiles => _csFiles;

    public SyntaxTree Parse(string path) => _cache.Parse(_tree.ContentKey(path), () => _tree.Read(path)!);

    /// <summary>ファイルを解析し、中の型を索引へ加えたうえで構文木を返す。</summary>
    public SyntaxTree IndexFile(string path)
    {
        var syntaxTree = Parse(path);
        if (!_indexedFiles.Add(path))
        {
            return syntaxTree;
        }
        foreach (var decl in syntaxTree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
        {
            var fullName = FullNameOf(decl);
            if (!_types.TryGetValue(fullName, out var entry))
            {
                entry = new TypeEntry
                {
                    FullName = fullName,
                    SimpleName = decl.Identifier.ValueText,
                    Kind = KindOf(decl),
                    Arity = (decl as TypeDeclarationSyntax)?.TypeParameterList?.Parameters.Count ?? 0,
                };
                _types[fullName] = entry;
                if (!_bySimpleName.TryGetValue(entry.SimpleName, out var list))
                {
                    _bySimpleName[entry.SimpleName] = list = [];
                }
                list.Add(entry);
            }
            entry.Declarations.Add((decl, path));
        }
        return syntaxTree;
    }

    public TypeEntry? GetType(string fullName) => _types.GetValueOrDefault(fullName);

    /// <summary>指定した単純名の型を宣言していそうなファイルを解析する。</summary>
    public IReadOnlyList<TypeEntry> TypesNamed(string simpleName)
    {
        _filesByDeclaredName ??= BuildDeclaredNameIndex();
        if (_filesByDeclaredName.TryGetValue(simpleName, out var files))
        {
            foreach (var f in files)
            {
                IndexFile(f);
            }
        }
        return _bySimpleName.TryGetValue(simpleName, out var list) ? list : [];
    }

    /// <summary>ディレクトリ直下の .cs のうち、指定した名前のメソッドを含みそうなファイルの型を返す。</summary>
    public IEnumerable<TypeEntry> TypesInDirectoryWithMethod(string directory, string methodName)
    {
        foreach (var f in _csFiles.Where(f => DirectoryOf(f) == directory))
        {
            if (!_cache.CalledNames(_tree.ContentKey(f), () => _tree.Read(f)!).Contains(methodName))
            {
                continue;
            }
            var syntaxTree = IndexFile(f);
            foreach (var decl in syntaxTree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
            {
                if (_types.TryGetValue(FullNameOf(decl), out var entry) && entry.Methods(methodName).Any())
                {
                    yield return entry;
                }
            }
        }
    }

    public static string DirectoryOf(string path)
    {
        var i = path.LastIndexOf('/');
        return i < 0 ? string.Empty : path[..i];
    }

    /// <summary>名前（修飾つき可）を型へ解決する。見つからなければ Unresolved、候補が複数なら Ambiguous。</summary>
    public ResolveResult Resolve(IReadOnlyList<string> parts, int arity, ResolveContext context)
    {
        var simple = parts[^1];
        if (parts.Count == 1 && WellKnownExternal.Contains(simple))
        {
            return ResolveResult.Unresolved;
        }
        var joined = string.Join('.', parts);
        var candidates = TypesNamed(simple)
            .Where(t => t.Arity == arity && (t.FullName == joined || t.FullName.EndsWith("." + joined, StringComparison.Ordinal)))
            .ToList();
        if (candidates.Count == 0)
        {
            return ResolveResult.Unresolved;
        }
        if (candidates.Count == 1)
        {
            return new ResolveResult(candidates[0], false);
        }
        var scored = candidates
            .Select(c => (Entry: c, Score: Score(c.FullName.Length == joined.Length ? string.Empty : c.FullName[..(c.FullName.Length - joined.Length - 1)], context)))
            .OrderByDescending(x => x.Score)
            .ToList();
        if (scored[0].Score > scored[1].Score)
        {
            return new ResolveResult(scored[0].Entry, false);
        }
        return new ResolveResult(null, true);
    }

    private static int Score(string prefix, ResolveContext context)
    {
        if (context.ContainingTypes.Contains(prefix))
        {
            return 4;
        }
        if (prefix == context.Namespace)
        {
            return 3;
        }
        if (prefix.Length > 0 && context.Namespace.StartsWith(prefix + ".", StringComparison.Ordinal))
        {
            return 2;
        }
        if (context.Usings.Contains(prefix))
        {
            return 1;
        }
        return prefix.Length == 0 ? 1 : 0;
    }

    /// <summary>構文ノードの位置から、名前解決の文脈を作る。</summary>
    public static ResolveContext ContextOf(SyntaxNode node, string path)
    {
        var namespaces = node.AncestorsAndSelf().OfType<BaseNamespaceDeclarationSyntax>().Reverse().Select(n => n.Name.ToString());
        var ns = string.Join('.', namespaces);
        var usings = new List<string>();
        foreach (var scope in node.AncestorsAndSelf())
        {
            var list = scope switch
            {
                CompilationUnitSyntax cu => cu.Usings,
                BaseNamespaceDeclarationSyntax n => n.Usings,
                _ => default,
            };
            usings.AddRange(list.Where(u => u.Alias is null && u.StaticKeyword.IsKind(SyntaxKind.None) && u.Name is not null).Select(u => u.Name!.ToString()));
        }
        var containing = node.AncestorsAndSelf().OfType<BaseTypeDeclarationSyntax>().Select(FullNameOf).ToList();
        return new ResolveContext(path, ns, usings, containing);
    }

    public static string FullNameOf(BaseTypeDeclarationSyntax decl)
    {
        var parts = new List<string>();
        foreach (var a in decl.AncestorsAndSelf())
        {
            switch (a)
            {
                case BaseTypeDeclarationSyntax t:
                    parts.Add(t.Identifier.ValueText);
                    break;
                case BaseNamespaceDeclarationSyntax n:
                    parts.Add(n.Name.ToString());
                    break;
            }
        }
        parts.Reverse();
        return string.Join('.', parts);
    }

    private static string KindOf(BaseTypeDeclarationSyntax decl) => decl switch
    {
        EnumDeclarationSyntax => "enum",
        StructDeclarationSyntax => "struct",
        InterfaceDeclarationSyntax => "interface",
        RecordDeclarationSyntax r when r.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword) => "record struct",
        RecordDeclarationSyntax => "record",
        _ => "class",
    };

    private Dictionary<string, List<string>> BuildDeclaredNameIndex()
    {
        var map = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        foreach (var f in _csFiles)
        {
            foreach (var name in _cache.DeclaredNames(_tree.ContentKey(f), () => _tree.Read(f)!))
            {
                if (!map.TryGetValue(name, out var list))
                {
                    map[name] = list = [];
                }
                list.Add(f);
            }
        }
        return map;
    }
}

internal sealed record ResolveResult(TypeEntry? Entry, bool Ambiguous)
{
    public static ResolveResult Unresolved { get; } = new(null, false);
}
