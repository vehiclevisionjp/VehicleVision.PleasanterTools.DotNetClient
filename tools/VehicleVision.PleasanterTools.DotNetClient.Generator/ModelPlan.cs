using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>生成する型の種類。</summary>
internal enum TypeKind
{
    /// <summary>クラス（構造体・レコードも含む）。</summary>
    Class,

    /// <summary>列挙。</summary>
    Enum,

    /// <summary>インターフェース。生成しない。</summary>
    Interface,

    /// <summary><c>List&lt;X&gt;</c> を継承する型。<c>List&lt;X&gt;</c> として扱い、生成しない。</summary>
    ListAlias,

    /// <summary><c>List&lt;T&gt;</c> を継承する総称型（<c>SettingList&lt;T&gt;</c>）。<c>List&lt;引数&gt;</c> として扱い、生成しない。</summary>
    GenericListAlias,
}

/// <summary>生成する型 1 つ。</summary>
internal sealed class TypePlan
{
    public required string FullName { get; init; }

    public required TypeKind Kind { get; init; }

    public required HistoryItem Item { get; init; }

    /// <summary><see cref="TypeKind.ListAlias"/> の要素の型の完全名。</summary>
    public string? ListElement { get; init; }

    /// <summary>基底のクラスの完全名（生成するものだけ）。</summary>
    public string? Base { get; set; }

    /// <summary>入れ子なら、外側の型の完全名。</summary>
    public string? Parent { get; set; }

    /// <summary>宣言する名前。</summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>Models 名前空間の中での名前（入れ子なら <c>View.ApiDataTypes</c>）。</summary>
    public string CSharpName { get; set; } = string.Empty;

    public List<TypePlan> Nested { get; } = [];
}

/// <summary>history.json の型から、生成する型の一覧と名前を決める。</summary>
internal sealed partial class ModelPlan
{
    private static readonly HashSet<string> IgnoreAttributes = new(StringComparer.Ordinal) { "NonSerialized", "JsonIgnore", "IgnoreDataMember" };

    private readonly History _history;
    private readonly GenerationConfig _config;
    private readonly SortedDictionary<string, TypePlan> _types = new(StringComparer.Ordinal);

    public ModelPlan(History history, GenerationConfig config, IEnumerable<string> roots)
    {
        _history = history;
        _config = config;
        foreach (var root in roots)
        {
            Visit(root);
        }

        Name();
    }

    /// <summary>生成する型（クラスと列挙）。完全名の順。</summary>
    public IEnumerable<TypePlan> Generated => _types.Values.Where(t => t.Kind is TypeKind.Class or TypeKind.Enum);

    /// <summary>入れ子でない、生成する型。</summary>
    public IEnumerable<TypePlan> TopLevel => Generated.Where(t => t.Parent is null);

    public TypePlan? Find(string fullName) => _types.GetValueOrDefault(fullName);

    public TypePlan Get(string fullName) => _types.TryGetValue(fullName, out var t)
        ? t
        : throw new GenerationException($"型 {fullName} は生成の対象にありません。");

    /// <summary>JSON に出るメンバー（送らない属性と、クライアントが入れるメンバーを除く）。</summary>
    public IEnumerable<(string Name, HistoryItem Item)> SerializedMembers(string fullName)
        => (_history.Members.GetValueOrDefault(fullName) ?? [])
            .Where(m => !IsIgnored(m.Item) && !_config.ClientManagedMembers.Contains(m.Name));

    /// <summary>Models の名前空間の中で、型を完全に修飾して書く。</summary>
    public static string Qualified(TypePlan type) => $"global::{ClientGenerator.ModelNamespace}.{type.CSharpName}";

    public static bool IsIgnored(HistoryItem member)
        => Attributes(member.Latest).Any(a => IgnoreAttributes.Contains(a.Split('(')[0]));

    public static IEnumerable<string> Attributes(JsonNode value)
        => value["attributes"]?.AsArray().Select(a => (string)a!) ?? [];

    /// <summary><c>JsonProperty(PropertyName="x")</c>・<c>JsonProperty("x")</c> の名前。</summary>
    public static string? JsonName(JsonNode value)
        => Attributes(value).Select(a => JsonPropertyPattern().Match(a)).FirstOrDefault(m => m.Success)?.Groups[1].Value;

    private void Visit(string fullName)
    {
        if (_types.ContainsKey(fullName))
        {
            return;
        }

        if (!_history.Types.TryGetValue(fullName, out var item))
        {
            throw new GenerationException($"型 {fullName} が history.json にありません。");
        }

        var value = item.Latest;
        var kind = (string?)value["kind"];
        var bases = value["bases"]?.AsArray().Select(b => (string)b!).ToList() ?? [];
        var listBase = bases.FirstOrDefault(b => b.StartsWith("List<", StringComparison.Ordinal));
        var plan = new TypePlan
        {
            FullName = fullName,
            Item = item,
            Kind = kind switch
            {
                "interface" => TypeKind.Interface,
                "enum" => TypeKind.Enum,
                _ when listBase == "List<T>" => TypeKind.GenericListAlias,
                _ when listBase is not null => TypeKind.ListAlias,
                "class" or "struct" or "record" or "record struct" => TypeKind.Class,
                _ => throw new GenerationException($"型 {fullName} の種類 {kind} は扱えません。"),
            },
            ListElement = listBase is not null && listBase != "List<T>" ? listBase[5..^1] : null,
        };
        _types[fullName] = plan;

        switch (plan.Kind)
        {
            case TypeKind.ListAlias:
                Visit(plan.ListElement!);
                break;
            case TypeKind.Class:
                foreach (var b in bases.Where(_history.Types.ContainsKey))
                {
                    Visit(b);
                    if (_types[b].Kind == TypeKind.Class)
                    {
                        plan.Base = plan.Base is null
                            ? b
                            : throw new GenerationException($"型 {fullName} に基底のクラスが 2 つあります。");
                    }
                }

                foreach (var (_, member) in SerializedMembers(fullName))
                {
                    foreach (var r in member.Latest["typeRefs"]?.AsArray().Select(x => (string)x!) ?? [])
                    {
                        if (_history.Types.ContainsKey(r))
                        {
                            Visit(r);
                        }
                    }
                }

                break;
        }
    }

    private void Name()
    {
        foreach (var t in Generated)
        {
            var dot = t.FullName.LastIndexOf('.');
            var parent = t.FullName[..dot];
            if (_types.TryGetValue(parent, out var p) && p.Kind == TypeKind.Class)
            {
                t.Parent = parent;
                p.Nested.Add(t);
            }

            t.Identifier = t.FullName[(dot + 1)..].TrimStart('_');
        }

        // 入れ子でない型の名前が重なる・汎用すぎる（Types）ときは、外側の名前（名前空間ではなく型）を前に付ける
        var top = TopLevel.ToList();
        foreach (var group in top.GroupBy(t => t.Identifier, StringComparer.Ordinal))
        {
            if (group.Count() > 1 || group.Key == "Types")
            {
                foreach (var t in group)
                {
                    var segments = t.FullName.Split('.');
                    t.Identifier = segments[^2].TrimStart('_') + t.Identifier;
                }
            }
        }

        var duplicated = top.GroupBy(t => t.Identifier, StringComparer.Ordinal).FirstOrDefault(g => g.Count() > 1);
        if (duplicated is not null)
        {
            throw new GenerationException($"型の名前 {duplicated.Key} が重なります: {string.Join(", ", duplicated.Select(t => t.FullName))}");
        }

        foreach (var t in Generated.OrderBy(t => t.FullName.Length))
        {
            t.CSharpName = t.Parent is null ? t.Identifier : _types[t.Parent].CSharpName + "." + t.Identifier;
            if (t.Parent is not null && _types[t.Parent].Identifier == t.Identifier)
            {
                throw new GenerationException($"入れ子の型 {t.FullName} の名前が、外側の型と同じです。");
            }
        }
    }

    [GeneratedRegex("^JsonProperty\\((?:PropertyName\\s*=\\s*)?\"([^\"]*)\"")]
    private static partial Regex JsonPropertyPattern();
}
