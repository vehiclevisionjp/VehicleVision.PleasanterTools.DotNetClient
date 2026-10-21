using System.Text.Json.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;

/// <summary>
/// API のリクエスト・応答に使われる型を起点に、メンバーの型をたどって型の一覧を作る。
/// ソースの中で見つからない型（外部ライブラリなど）はたどらず、名前だけを未解決として残す。
/// </summary>
internal sealed class ModelCollector
{
    private readonly CodeIndex _index;
    private readonly Queue<TypeEntry> _queue = new();
    private readonly HashSet<string> _queued = new(StringComparer.Ordinal);
    private readonly SortedSet<string> _unresolved = new(StringComparer.Ordinal);
    private readonly SortedSet<string> _ambiguous = new(StringComparer.Ordinal);
    private HashSet<string> _typeParameters = new(StringComparer.Ordinal);

    public ModelCollector(CodeIndex index)
    {
        _index = index;
    }

    public IReadOnlyCollection<string> Unresolved => _unresolved;

    public IReadOnlyCollection<string> Ambiguous => _ambiguous;

    /// <summary>型の構文に出てくる名前を解決し、見つかった型を収集の対象に加える。解決できた完全名を返す。</summary>
    public IReadOnlyList<string> Require(TypeSyntax type, ResolveContext context)
    {
        var resolved = new List<string>();
        foreach (var (parts, arity) in SyntaxFacts.TypeNames(type))
        {
            var result = _index.Resolve(parts, arity, context);
            if (result.Entry is { } entry)
            {
                Enqueue(entry);
                resolved.Add(entry.FullName);
            }
            else if (result.Ambiguous)
            {
                _ambiguous.Add(string.Join('.', parts));
            }
            else if (!IsBuiltIn(parts) && !(parts.Count == 1 && _typeParameters.Contains(parts[0])))
            {
                _unresolved.Add(string.Join('.', parts));
            }
        }
        return resolved;
    }

    /// <summary>型の構文に出てくる名前を解決するが、収集の対象には加えない。</summary>
    public IReadOnlyList<TypeEntry> Peek(TypeSyntax type, ResolveContext context)
        => [.. SyntaxFacts.TypeNames(type).Select(n => _index.Resolve(n.Parts, n.Arity, context).Entry).OfType<TypeEntry>()];

    public void Enqueue(TypeEntry entry)
    {
        if (_queued.Add(entry.FullName))
        {
            _queue.Enqueue(entry);
        }
    }

    /// <summary>キューが空になるまで型を処理し、型の一覧を返す（完全名の順）。</summary>
    public IReadOnlyList<TypeModel> Collect()
    {
        var models = new SortedDictionary<string, TypeModel>(StringComparer.Ordinal);
        while (_queue.Count > 0)
        {
            var entry = _queue.Dequeue();
            models[entry.FullName] = Build(entry);
        }
        return [.. models.Values];
    }

    private TypeModel Build(TypeEntry entry)
    {
        var members = new SortedDictionary<string, MemberModel>(StringComparer.Ordinal);
        var enumValues = new List<EnumValue>();
        var bases = new SortedSet<string>(StringComparer.Ordinal);
        var attributes = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var (decl, path) in entry.Declarations)
        {
            // 型引数（T など）は、未解決の型として数えない
            _typeParameters = new HashSet<string>(
                decl.AncestorsAndSelf().OfType<TypeDeclarationSyntax>()
                    .SelectMany(t => t.TypeParameterList?.Parameters.Select(p => p.Identifier.ValueText) ?? []),
                StringComparer.Ordinal);
            foreach (var a in SyntaxFacts.Attributes(decl.AttributeLists))
            {
                attributes.Add(SyntaxFacts.AttributeText(a));
            }
            var context = CodeIndex.ContextOf(decl, path);
            if (decl.BaseList is { } baseList)
            {
                foreach (var b in baseList.Types)
                {
                    var resolved = Require(b.Type, context);
                    bases.Add(BaseName(b.Type, resolved));
                }
            }
            if (decl is EnumDeclarationSyntax e)
            {
                // 値を書いていないメンバーは、C# の規則（先頭は 0、以降は直前の値 + 1）で値が決まる。
                // 直前の値が数値として取れないとき（式など）は、決められないので null のままにする
                decimal? previous = null;
                var first = true;
                foreach (var m in e.Members)
                {
                    var value = m.EqualsValue?.Value switch
                    {
                        null => first ? JsonValue.Create(0) : previous is { } p ? JsonValue.Create(p + 1) : null,
                        LiteralExpressionSyntax lit => SyntaxFacts.LiteralValue(lit),
                        var other => SyntaxFacts.DefaultOf(other, null),
                    };
                    enumValues.Add(new EnumValue(m.Identifier.ValueText, value));
                    previous = NumberOf(value);
                    first = false;
                }
                continue;
            }
            var isInterface = decl is InterfaceDeclarationSyntax;
            foreach (var member in ((TypeDeclarationSyntax)decl).Members)
            {
                switch (member)
                {
                    case PropertyDeclarationSyntax p when isInterface || IsPublicInstance(p.Modifiers):
                        members[p.Identifier.ValueText] = Member(p.Identifier.ValueText, "property", p.Type, p.Initializer?.Value, IsReadOnly(p), p.AttributeLists, context);
                        break;
                    case FieldDeclarationSyntax f when IsPublicInstance(f.Modifiers) && !f.Modifiers.Any(SyntaxKind.ConstKeyword):
                        foreach (var v in f.Declaration.Variables)
                        {
                            var readOnly = f.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);
                            members[v.Identifier.ValueText] = Member(v.Identifier.ValueText, "field", f.Declaration.Type, v.Initializer?.Value, readOnly, f.AttributeLists, context);
                        }
                        break;
                }
            }
            if (decl is RecordDeclarationSyntax { ParameterList: { } pl })
            {
                foreach (var p in pl.Parameters.Where(p => p.Type is not null))
                {
                    members[p.Identifier.ValueText] = Member(p.Identifier.ValueText, "property", p.Type!, p.Default?.Value, false, p.AttributeLists, context);
                }
            }
        }
        return new TypeModel
        {
            Name = entry.FullName,
            Kind = entry.Kind,
            Files = [.. entry.Declarations.Select(d => d.Path).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)],
            Bases = [.. bases],
            Attributes = [.. attributes],
            Members = [.. members.Values],
            EnumValues = enumValues,
        };
    }

    /// <summary>
    /// 基底の型の名前。ソースにある型ならその完全名。
    /// ソースに無い総称型（<c>List&lt;Attachment&gt;</c> など）は、外側の名前を残し、型引数だけを完全名にする
    /// （型引数の完全名だけを書くと、「その型を継承する」と読めてしまうため）。
    /// </summary>
    private static string BaseName(TypeSyntax type, IReadOnlyList<string> resolved)
    {
        var generic = type switch
        {
            GenericNameSyntax g => g,
            QualifiedNameSyntax { Right: GenericNameSyntax rg } => rg,
            _ => null,
        };
        if (generic is not null)
        {
            var outerIsSource = resolved.Count > 0 && resolved[0].EndsWith("." + generic.Identifier.ValueText, StringComparison.Ordinal);
            if (!outerIsSource)
            {
                var args = generic.TypeArgumentList.Arguments;
                return resolved.Count == args.Count
                    ? $"{generic.Identifier.ValueText}<{string.Join(",", resolved)}>"
                    : SyntaxFacts.TypeText(type);
            }
        }
        return resolved.Count > 0 ? resolved[0] : SyntaxFacts.TypeText(type);
    }

    private static decimal? NumberOf(JsonNode? value)
        => value is JsonValue v && decimal.TryParse(v.ToJsonString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var d)
            ? d
            : null;

    private MemberModel Member(
        string name,
        string kind,
        TypeSyntax type,
        ExpressionSyntax? initializer,
        bool readOnly,
        SyntaxList<AttributeListSyntax> attributeLists,
        ResolveContext context)
    {
        var attributes = SyntaxFacts.Attributes(attributeLists).Select(SyntaxFacts.AttributeText).Order(StringComparer.Ordinal).ToList();
        // JSON に出ないメンバーの型は、たどらない（名前だけ記録する）
        var ignored = attributes.Any(a => IgnoreAttributes.Contains(a.Split('(')[0]));
        var refs = ignored ? Peek(type, context).Select(e => e.FullName).ToList() : Require(type, context);
        return new MemberModel
        {
            Name = name,
            Kind = kind,
            Type = SyntaxFacts.TypeText(type),
            TypeRefs = [.. refs.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)],
            Nullable = SyntaxFacts.Nullable(type, t => IsValueTypeInSource(t, context)),
            Default = SyntaxFacts.DefaultOf(initializer, type),
            ReadOnly = readOnly,
            Attributes = attributes,
        };
    }

    private bool? IsValueTypeInSource(TypeSyntax type, ResolveContext context)
    {
        if (type is not NameSyntax name)
        {
            return null;
        }
        var parts = SyntaxFacts.NameParts(name);
        var arity = name is GenericNameSyntax g ? g.TypeArgumentList.Arguments.Count
            : name is QualifiedNameSyntax { Right: GenericNameSyntax rg } ? rg.TypeArgumentList.Arguments.Count : 0;
        return _index.Resolve(parts, arity, context).Entry?.IsValueType;
    }

    /// <summary>JSON のシリアライズから外す属性（Newtonsoft.Json・System.Text.Json・DataContract）。</summary>
    private static readonly HashSet<string> IgnoreAttributes = new(StringComparer.Ordinal)
    {
        "NonSerialized", "JsonIgnore", "IgnoreDataMember",
    };

    private static bool IsPublicInstance(SyntaxTokenList modifiers)
        => modifiers.Any(SyntaxKind.PublicKeyword) && !modifiers.Any(SyntaxKind.StaticKeyword);

    private static bool IsReadOnly(PropertyDeclarationSyntax p)
    {
        if (p.AccessorList is null)
        {
            return true; // 式本体（=>）のプロパティ
        }
        var setter = p.AccessorList.Accessors.FirstOrDefault(a => a.IsKind(SyntaxKind.SetAccessorDeclaration) || a.IsKind(SyntaxKind.InitAccessorDeclaration));
        return setter is null || setter.Modifiers.Any(SyntaxKind.PrivateKeyword) || setter.Modifiers.Any(SyntaxKind.ProtectedKeyword);
    }

    // System 名前空間などの、よく使う外部の型は未解決の一覧に載せない（ノイズになるため）。
    private static readonly HashSet<string> BuiltIn = new(StringComparer.Ordinal)
    {
        "String", "Object", "List", "IList", "Dictionary", "IDictionary", "IEnumerable", "ICollection",
        "IReadOnlyList", "IReadOnlyDictionary", "IReadOnlyCollection", "HashSet", "ISet", "Nullable",
        "DateTime", "DateTimeOffset", "TimeSpan", "Guid", "Decimal", "KeyValuePair", "Tuple", "var", "dynamic",
    };

    private static bool IsBuiltIn(IReadOnlyList<string> parts) => parts.Count == 1 && BuiltIn.Contains(parts[0]);
}
