using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface;

/// <summary>ある版のソースツリーから API 表を作る。</summary>
internal sealed class SurfaceExtractor
{
    private readonly ExtractorOptions _options;
    private readonly ParseCache _cache;

    public SurfaceExtractor(ExtractorOptions options, ParseCache? cache = null)
    {
        _options = options;
        _cache = cache ?? new ParseCache();
    }

    public SurfaceDocument Extract(ISourceTree tree)
    {
        var index = new CodeIndex(tree, _options, _cache);
        var models = new ModelCollector(index);
        var endpoints = new EndpointExtractor(index, _options, models).Extract();
        foreach (var name in _options.ExtraModelTypes)
        {
            foreach (var t in index.TypesNamed(name))
            {
                models.Enqueue(t);
            }
        }
        var types = models.Collect();
        return new SurfaceDocument
        {
            Source = new SourceInfo(tree.Label, tree.Revision),
            ApiParameters = ReadParameters(tree),
            ConventionalRoutes = ReadConventionalRoutes(index),
            Endpoints = endpoints,
            Types = types,
            UnresolvedTypes = [.. models.Unresolved],
            AmbiguousTypes = [.. models.Ambiguous],
        };
    }

    private JsonObject? ReadParameters(ISourceTree tree)
    {
        var text = tree.Read(_options.ApiParametersFile);
        if (text is null)
        {
            return null;
        }
        var node = JsonNode.Parse(text, documentOptions: new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
        return node as JsonObject;
    }

    private IReadOnlyList<ConventionalRoute> ReadConventionalRoutes(CodeIndex index)
    {
        if (!index.Tree.Files.Contains(_options.StartupFile))
        {
            return [];
        }
        var routes = new List<ConventionalRoute>();
        var root = index.Parse(_options.StartupFile).GetRoot();
        foreach (var invocation in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (invocation.Expression is not MemberAccessExpressionSyntax { Name.Identifier.ValueText: "MapControllerRoute" })
            {
                continue;
            }
            string? name = null;
            string? pattern = null;
            var defaults = new SortedDictionary<string, string>(StringComparer.Ordinal);
            var constraints = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var arg in invocation.ArgumentList.Arguments)
            {
                switch (arg.NameColon?.Name.Identifier.ValueText)
                {
                    case "name":
                        name = Literal(arg.Expression);
                        break;
                    case "pattern":
                        pattern = Literal(arg.Expression);
                        break;
                    case "defaults":
                        Members(arg.Expression, defaults);
                        break;
                    case "constraints":
                        Members(arg.Expression, constraints);
                        break;
                }
            }
            routes.Add(new ConventionalRoute(name ?? "?", pattern, defaults, constraints));
        }
        return [.. routes.OrderBy(r => r.Name, StringComparer.Ordinal).ThenBy(r => r.Pattern, StringComparer.Ordinal)];
    }

    private static string? Literal(ExpressionSyntax e)
        => e is LiteralExpressionSyntax lit ? lit.Token.ValueText : null;

    private static void Members(ExpressionSyntax e, IDictionary<string, string> into)
    {
        if (e is not AnonymousObjectCreationExpressionSyntax anon)
        {
            return;
        }
        foreach (var init in anon.Initializers)
        {
            var key = init.NameEquals?.Name.Identifier.ValueText;
            if (key is not null)
            {
                // リテラル以外（変数など）は値を推定しない
                into[key] = Literal(init.Expression) ?? "?";
            }
        }
    }
}
