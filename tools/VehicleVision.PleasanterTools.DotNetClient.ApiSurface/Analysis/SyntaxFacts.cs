using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;

/// <summary>構文から事実（名前・型名・リテラル）を取り出す小さな関数群。</summary>
internal static partial class SyntaxFacts
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();

    private static readonly HashSet<string> ValueTypeNames = new(StringComparer.Ordinal)
    {
        "DateTime", "DateTimeOffset", "TimeSpan", "Guid", "Decimal", "Int16", "Int32", "Int64", "Boolean",
        "Double", "Single", "Byte", "Char", "DateOnly", "TimeOnly", "KeyValuePair",
    };

    private static readonly HashSet<string> ReferenceTypeNames = new(StringComparer.Ordinal)
    {
        "String", "Object", "List", "IList", "Dictionary", "IDictionary", "IEnumerable", "ICollection",
        "IReadOnlyList", "IReadOnlyDictionary", "IReadOnlyCollection", "HashSet", "ISet", "JObject", "JToken",
        "JArray", "ExpandoObject", "Stream",
    };

    /// <summary>型の書き方から空白を除いたもの。</summary>
    public static string TypeText(TypeSyntax type) => Whitespace().Replace(type.ToString(), string.Empty);

    /// <summary>型の構文に出てくる名前（修飾の部分と型引数の数）。型引数の中も含む。</summary>
    public static IEnumerable<(IReadOnlyList<string> Parts, int Arity)> TypeNames(TypeSyntax type)
    {
        switch (type)
        {
            case PredefinedTypeSyntax:
                yield break;
            case NullableTypeSyntax n:
                foreach (var x in TypeNames(n.ElementType))
                {
                    yield return x;
                }

                yield break;
            case ArrayTypeSyntax a:
                foreach (var x in TypeNames(a.ElementType))
                {
                    yield return x;
                }

                yield break;
            case TupleTypeSyntax t:
                foreach (var e in t.Elements)
                {
                    foreach (var x in TypeNames(e.Type))
                    {
                        yield return x;
                    }
                }
                yield break;
            case NameSyntax name:
                yield return (NameParts(name), ArityOf(name));
                foreach (var g in name.DescendantNodesAndSelf().OfType<GenericNameSyntax>())
                {
                    foreach (var arg in g.TypeArgumentList.Arguments)
                    {
                        foreach (var x in TypeNames(arg))
                        {
                            yield return x;
                        }
                    }
                }
                yield break;
        }
    }

    public static IReadOnlyList<string> NameParts(NameSyntax name) => name switch
    {
        IdentifierNameSyntax i => [i.Identifier.ValueText],
        GenericNameSyntax g => [g.Identifier.ValueText],
        QualifiedNameSyntax q => [.. NameParts(q.Left), .. NameParts(q.Right)],
        AliasQualifiedNameSyntax a => NameParts(a.Name),
        _ => [name.ToString()],
    };

    private static int ArityOf(NameSyntax name) => name switch
    {
        GenericNameSyntax g => g.TypeArgumentList.Arguments.Count,
        QualifiedNameSyntax q => ArityOf(q.Right),
        AliasQualifiedNameSyntax a => ArityOf(a.Name),
        _ => 0,
    };

    /// <summary>
    /// null を取りうるか。<c>?</c> 付きは true、値型と分かるものは false、参照型と分かるものは true、
    /// 構文だけでは分からないものは null（Pleasanter は Nullable 参照型を無効にしているため、参照型は null を取りうる）。
    /// </summary>
    public static bool? Nullable(TypeSyntax type, Func<TypeSyntax, bool?> isValueTypeInSource)
    {
        switch (type)
        {
            case NullableTypeSyntax:
                return true;
            case PredefinedTypeSyntax p:
                return p.Keyword.Kind() is SyntaxKind.StringKeyword or SyntaxKind.ObjectKeyword;
            case ArrayTypeSyntax:
                return true;
            case NameSyntax name:
                var simple = NameParts(name)[^1];
                var inSource = isValueTypeInSource(type);
                if (inSource is { } v)
                {
                    return !v;
                }
                if (ValueTypeNames.Contains(simple))
                {
                    return false;
                }
                if (ReferenceTypeNames.Contains(simple))
                {
                    return true;
                }
                return null;
            default:
                return null;
        }
    }

    /// <summary>初期化子を、事実として表せる形にする（コードの断片は残さない）。</summary>
    public static JsonNode? DefaultOf(ExpressionSyntax? expression, TypeSyntax? declaredType)
    {
        if (expression is null)
        {
            return null;
        }
        switch (expression)
        {
            case LiteralExpressionSyntax lit:
                return new JsonObject { ["value"] = LiteralValue(lit) };
            case PrefixUnaryExpressionSyntax { OperatorToken.RawKind: (int)SyntaxKind.MinusToken, Operand: LiteralExpressionSyntax num }
                when num.IsKind(SyntaxKind.NumericLiteralExpression):
                return new JsonObject { ["value"] = JsonValue.Create(-Convert.ToDecimal(num.Token.Value, CultureInfo.InvariantCulture)) };
            case IdentifierNameSyntax or MemberAccessExpressionSyntax when IsIdentifierChain(expression):
                return new JsonObject { ["ref"] = Whitespace().Replace(expression.ToString(), string.Empty) };
            case ObjectCreationExpressionSyntax oc:
                return new JsonObject { ["new"] = TypeText(oc.Type) };
            case ImplicitObjectCreationExpressionSyntax when declaredType is not null:
                return new JsonObject { ["new"] = TypeText(declaredType) };
            case ArrayCreationExpressionSyntax ac:
                return new JsonObject { ["new"] = TypeText(ac.Type) };
            case CollectionExpressionSyntax:
                return new JsonObject { ["new"] = "[]" };
            case DefaultExpressionSyntax:
                return new JsonObject { ["default"] = true };
            default:
                return new JsonObject { ["unextracted"] = true };
        }
    }

    public static JsonNode? LiteralValue(LiteralExpressionSyntax lit) => lit.Kind() switch
    {
        SyntaxKind.NullLiteralExpression => null,
        SyntaxKind.TrueLiteralExpression => JsonValue.Create(true),
        SyntaxKind.FalseLiteralExpression => JsonValue.Create(false),
        SyntaxKind.StringLiteralExpression => JsonValue.Create(lit.Token.ValueText),
        SyntaxKind.CharacterLiteralExpression => JsonValue.Create(lit.Token.ValueText),
        SyntaxKind.NumericLiteralExpression => JsonValue.Create(Convert.ToDecimal(lit.Token.Value, CultureInfo.InvariantCulture)),
        SyntaxKind.DefaultLiteralExpression => JsonValue.Create("default"),
        _ => JsonValue.Create(lit.Token.ValueText),
    };

    public static bool IsIdentifierChain(ExpressionSyntax e) => e switch
    {
        IdentifierNameSyntax => true,
        MemberAccessExpressionSyntax m when m.IsKind(SyntaxKind.SimpleMemberAccessExpression) => m.Name is IdentifierNameSyntax && IsIdentifierChain(m.Expression),
        _ => false,
    };

    /// <summary>属性の名前（Attribute の接尾辞は除く）。</summary>
    public static string AttributeName(AttributeSyntax a)
    {
        var name = NameParts(a.Name)[^1];
        return name.EndsWith("Attribute", StringComparison.Ordinal) ? name[..^"Attribute".Length] : name;
    }

    /// <summary>属性を「名前(引数)」の形にする。引数はリテラルだけを残し、それ以外は ? にする。</summary>
    public static string AttributeText(AttributeSyntax a)
    {
        var name = AttributeName(a);
        if (a.ArgumentList is null || a.ArgumentList.Arguments.Count == 0)
        {
            return name;
        }
        var args = a.ArgumentList.Arguments.Select(arg =>
        {
            var prefix = arg.NameEquals is { } ne ? ne.Name.Identifier.ValueText + "="
                : arg.NameColon is { } nc ? nc.Name.Identifier.ValueText + ":" : string.Empty;
            var value = arg.Expression is LiteralExpressionSyntax lit
                ? (lit.IsKind(SyntaxKind.StringLiteralExpression) ? "\"" + lit.Token.ValueText + "\"" : lit.Token.Text)
                : IsIdentifierChain(arg.Expression) ? Whitespace().Replace(arg.Expression.ToString(), string.Empty) : "?";
            return prefix + value;
        });
        return $"{name}({string.Join(",", args)})";
    }

    public static IEnumerable<AttributeSyntax> Attributes(SyntaxList<AttributeListSyntax> lists)
        => lists.SelectMany(l => l.Attributes);

    /// <summary>最初の位置引数（または指定した名前の引数）が文字列リテラルなら、その値。</summary>
    public static string? StringArgument(AttributeSyntax a, string? namedParameter = null)
    {
        if (a.ArgumentList is null)
        {
            return null;
        }
        foreach (var arg in a.ArgumentList.Arguments)
        {
            var named = arg.NameEquals?.Name.Identifier.ValueText ?? arg.NameColon?.Name.Identifier.ValueText;
            if (named is not null && named != namedParameter)
            {
                continue;
            }
            return arg.Expression is LiteralExpressionSyntax lit && lit.IsKind(SyntaxKind.StringLiteralExpression)
                ? lit.Token.ValueText
                : null;
        }
        return null;
    }
}
