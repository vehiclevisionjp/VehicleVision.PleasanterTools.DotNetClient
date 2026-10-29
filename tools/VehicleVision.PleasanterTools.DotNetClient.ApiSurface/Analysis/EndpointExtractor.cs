using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;

/// <summary>
/// コントローラからエンドポイントを抽出し、処理の呼び出しを辿って、リクエストの型と応答の手がかりを集める。
/// </summary>
internal sealed class EndpointExtractor
{
    private static readonly Dictionary<string, string> HttpAttributes = new(StringComparer.Ordinal)
    {
        ["HttpGet"] = "GET",
        ["HttpPost"] = "POST",
        ["HttpPut"] = "PUT",
        ["HttpDelete"] = "DELETE",
        ["HttpPatch"] = "PATCH",
        ["HttpHead"] = "HEAD",
        ["HttpOptions"] = "OPTIONS",
    };

    private static readonly HashSet<string> BindingAttributes = new(StringComparer.Ordinal)
    {
        "FromBody", "FromQuery", "FromRoute", "FromForm", "FromHeader", "FromServices",
    };

    private readonly CodeIndex _index;
    private readonly ExtractorOptions _options;
    private readonly ModelCollector _models;

    public EndpointExtractor(CodeIndex index, ExtractorOptions options, ModelCollector models)
    {
        _index = index;
        _options = options;
        _models = models;
    }

    public IReadOnlyList<Endpoint> Extract()
    {
        var endpoints = new List<Endpoint>();
        var prefix = _options.ControllersDirectory.TrimEnd('/') + "/";
        foreach (var path in _index.Tree.Files.Where(f => f.StartsWith(prefix, StringComparison.Ordinal) && f.EndsWith(".cs", StringComparison.Ordinal)))
        {
            var root = _index.IndexFile(path).GetRoot();
            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                endpoints.AddRange(FromController(cls, path));
            }
        }
        // 同じキーが 2 つできることは想定していない。できた場合は、取りこぼさないよう添え字で区別する。
        return [.. endpoints
            .GroupBy(e => e.Key, StringComparer.Ordinal)
            .SelectMany(g => g.Count() == 1 ? g : g.Select((e, i) => e with { Key = $"{e.Key}#{i + 1}" }))
            .OrderBy(e => e.Key, StringComparer.Ordinal)];
    }

    private IEnumerable<Endpoint> FromController(ClassDeclarationSyntax cls, string path)
    {
        var className = cls.Identifier.ValueText;
        if (!className.EndsWith("Controller", StringComparison.Ordinal) || cls.Modifiers.Any(SyntaxKind.AbstractKeyword))
        {
            yield break;
        }
        var classAttributes = SyntaxFacts.Attributes(cls.AttributeLists).ToList();
        var classRoutes = classAttributes.Where(a => SyntaxFacts.AttributeName(a) == "Route").Select(a => SyntaxFacts.StringArgument(a, "template")).OfType<string>().ToList();
        var isLegacy = className.StartsWith(_options.LegacyControllerPrefix, StringComparison.Ordinal);
        var controllerName = className[..^"Controller".Length];
        var classRate = RateLimit(classAttributes);

        foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
        {
            if (!method.Modifiers.Any(SyntaxKind.PublicKeyword) || method.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                continue;
            }
            var methodAttributes = SyntaxFacts.Attributes(method.AttributeLists).ToList();
            if (methodAttributes.Any(a => SyntaxFacts.AttributeName(a) == "NonAction"))
            {
                continue;
            }
            var routes = new List<(string Verb, string? Template)>();
            foreach (var a in methodAttributes)
            {
                if (HttpAttributes.TryGetValue(SyntaxFacts.AttributeName(a), out var verb))
                {
                    routes.Add((verb, SyntaxFacts.StringArgument(a, "template")));
                }
            }
            var methodRouteTemplates = methodAttributes.Where(a => SyntaxFacts.AttributeName(a) == "Route").Select(a => SyntaxFacts.StringArgument(a, "template")).OfType<string>().ToList();
            if (routes.Count == 0 && methodRouteTemplates.Count > 0)
            {
                routes.AddRange(methodRouteTemplates.Select(t => ("ANY", (string?)t)));
            }
            if (routes.Count == 0)
            {
                continue;
            }
            var attributeRouted = classRoutes.Count > 0 || routes.Any(r => r.Template is not null);
            if (!attributeRouted && !isLegacy)
            {
                continue; // 画面用の規約ルーティングのコントローラは対象外
            }
            var analysis = Analyze(method, path);
            var rate = RateLimit(methodAttributes) ?? classRate;
            // ルートの属性（Route・Http*）はパスとメソッドに表れているので、ここには入れない
            // （入れると、同じアクションの別のテンプレートの増減が、このエンドポイントの変更に見える）
            var classAttributeTexts = AttributeTexts(classAttributes);
            var methodAttributeTexts = AttributeTexts(methodAttributes);
            var auth = new EndpointAuth(
                AllowAnonymous: classAttributes.Concat(methodAttributes).Any(a => SyntaxFacts.AttributeName(a) == "AllowAnonymous"),
                Authorize: classAttributes.Concat(methodAttributes).Any(a => SyntaxFacts.AttributeName(a) == "Authorize"),
                ChecksAuthenticated: analysis.ChecksAuthenticated,
                ChecksPrivilege: analysis.ChecksPrivilege);
            var methodContext = CodeIndex.ContextOf(method, path);
            var returnType = SyntaxFacts.TypeText(method.ReturnType);

            var paths = new List<(string Verb, string? Path)>();
            foreach (var (verb, template) in routes)
            {
                if (!attributeRouted)
                {
                    paths.Add((verb, null));
                    continue;
                }
                var bases = classRoutes.Count > 0 ? classRoutes : [string.Empty];
                foreach (var b in bases)
                {
                    paths.Add((verb, Combine(b, template, controllerName, method.Identifier.ValueText)));
                }
            }
            foreach (var (verb, routePath) in paths)
            {
                var routeParameters = routePath is null ? [] : RouteParameters(routePath);
                var parameters = method.ParameterList.Parameters.Select(p => Parameter(p, routeParameters, methodContext)).ToList();
                var requestTypes = new SortedSet<string>(analysis.RequestTypes, StringComparer.Ordinal);
                foreach (var p in method.ParameterList.Parameters.Where(p => p.Type is not null && HasAttribute(p, "FromBody")))
                {
                    foreach (var t in _models.Require(p.Type!, methodContext))
                    {
                        requestTypes.Add(t);
                    }
                }
                var key = routePath is null
                    ? $"{verb} legacy:{controllerName}/{method.Identifier.ValueText}"
                    : $"{verb} {routePath.ToLowerInvariant()}";
                yield return new Endpoint
                {
                    Key = key,
                    Area = routePath is null ? "legacy" : AreaOf(routePath),
                    HttpMethod = verb,
                    Path = routePath,
                    Routing = routePath is null ? "conventional" : "attribute",
                    Controller = className,
                    Action = method.Identifier.ValueText,
                    File = path,
                    Parameters = parameters,
                    Attributes = new EndpointAttributes(classAttributeTexts, methodAttributeTexts),
                    RateLimiting = rate,
                    Auth = auth,
                    Input = new EndpointInput(
                        [.. analysis.Body],
                        [.. analysis.Headers],
                        [.. analysis.QueryStrings.Select(q => new NamedAccess(q.Name, q.Accessor))],
                        [.. analysis.Forms.Select(q => new NamedAccess(q.Name, q.Accessor))],
                        analysis.Files),
                    Gates = [.. analysis.Gates],
                    Handlers = [.. analysis.Handlers],
                    RequestTypes = [.. requestTypes],
                    Response = new EndpointResponse([.. analysis.ApiResults], [.. analysis.GetMembers], [.. analysis.ResponseModels], returnType),
                };
            }
        }
    }

    private static List<string> AttributeTexts(IEnumerable<AttributeSyntax> attributes)
        => [.. attributes
            .Where(a => SyntaxFacts.AttributeName(a) is var n && n != "Route" && !HttpAttributes.ContainsKey(n))
            .Select(SyntaxFacts.AttributeText)
            .Distinct()
            .Order(StringComparer.Ordinal)];

    private static bool HasAttribute(ParameterSyntax p, string name)
        => SyntaxFacts.Attributes(p.AttributeLists).Any(a => SyntaxFacts.AttributeName(a) == name);

    private EndpointParameter Parameter(ParameterSyntax p, IReadOnlyCollection<string> routeParameters, ResolveContext context)
    {
        var binding = SyntaxFacts.Attributes(p.AttributeLists).Select(SyntaxFacts.AttributeName).FirstOrDefault(BindingAttributes.Contains);
        var name = p.Identifier.ValueText;
        var source = binding is not null ? char.ToLowerInvariant(binding[0]) + binding[1..]
            : routeParameters.Contains(name, StringComparer.OrdinalIgnoreCase) ? "route"
            : "notInTemplate";
        var type = p.Type is null ? "?" : SyntaxFacts.TypeText(p.Type);
        if (p.Type is not null && binding is "FromBody" or "FromQuery" or "FromForm")
        {
            _models.Require(p.Type, context);
        }
        return new EndpointParameter(name, type, source, SyntaxFacts.DefaultOf(p.Default?.Value, p.Type));
    }

    private static string? RateLimit(IEnumerable<AttributeSyntax> attributes)
        => attributes.Where(a => SyntaxFacts.AttributeName(a) == "EnableRateLimiting").Select(a => SyntaxFacts.StringArgument(a) ?? "?").LastOrDefault();

    internal static string Combine(string classRoute, string? template, string controller, string action)
    {
        string combined;
        if (template is not null && (template.StartsWith('/') || template.StartsWith("~/", StringComparison.Ordinal)))
        {
            combined = template.TrimStart('~').TrimStart('/');
        }
        else
        {
            var parts = new[] { classRoute.Trim('/'), (template ?? string.Empty).Trim('/') }.Where(s => s.Length > 0);
            combined = string.Join('/', parts);
        }
        return combined
            .Replace("[controller]", controller, StringComparison.OrdinalIgnoreCase)
            .Replace("[action]", action, StringComparison.OrdinalIgnoreCase);
    }

    internal static IReadOnlyList<string> RouteParameters(string path)
    {
        var result = new List<string>();
        var i = 0;
        while ((i = path.IndexOf('{', i)) >= 0)
        {
            var end = path.IndexOf('}', i);
            if (end < 0)
            {
                break;
            }
            var name = path[(i + 1)..end].TrimStart('*');
            var cut = name.IndexOfAny([':', '=', '?']);
            result.Add(cut < 0 ? name : name[..cut]);
            i = end + 1;
        }
        return result;
    }

    private static string AreaOf(string path)
    {
        var first = path.Split('/')[0].ToLowerInvariant();
        return first is "api" or "scim" ? first : "other";
    }

    private CallAnalysis Analyze(MethodDeclarationSyntax action, string path)
    {
        var analysis = new CallAnalysis();
        var owner = _index.GetType(CodeIndex.FullNameOf((BaseTypeDeclarationSyntax)action.Parent!));
        Visit(action, path, owner, 0, analysis);
        return analysis;
    }

    private void Visit(MethodDeclarationSyntax method, string path, TypeEntry? owner, int depth, CallAnalysis analysis)
    {
        if (!analysis.Visited.Add(method) || depth > _options.MaxDepth)
        {
            return;
        }
        var context = CodeIndex.ContextOf(method, path);
        if (depth > 0)
        {
            analysis.Handlers.Add($"{owner?.SimpleName ?? "?"}.{method.Identifier.ValueText}");
            if (method.ReturnType is { } rt)
            {
                foreach (var entry in _models.Peek(rt, context).Where(e => e.SimpleName.EndsWith(_options.ResponseModelSuffix, StringComparison.Ordinal)))
                {
                    _models.Enqueue(entry);
                    analysis.ResponseModels.Add(entry.FullName);
                }
            }
        }
        var body = (SyntaxNode?)method.Body ?? method.ExpressionBody;
        if (body is null)
        {
            return;
        }
        var isController = depth == 0 || (owner is not null && owner.SimpleName.EndsWith("Controller", StringComparison.Ordinal));
        if (isController)
        {
            CollectControllerFacts(body, analysis);
        }
        foreach (var invocation in body.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            var (receiver, name, generic) = Split(invocation.Expression);
            if (name is null)
            {
                continue;
            }
            CollectInvocationFacts(invocation, receiver, name, generic, context, analysis, isController);
            foreach (var (target, targetPath, targetOwner) in Targets(receiver, name, owner, path, depth, context))
            {
                Visit(target, targetPath, targetOwner, depth + 1, analysis);
            }
        }
    }

    private void CollectControllerFacts(SyntaxNode body, CallAnalysis analysis)
    {
        foreach (var node in body.DescendantNodes())
        {
            switch (node)
            {
                case MemberAccessExpressionSyntax m when m.Name.Identifier.ValueText == "Authenticated" && IsContextAccess(m.Expression):
                    analysis.ChecksAuthenticated = true;
                    break;
                case MemberAccessExpressionSyntax m when m.Name.Identifier.ValueText == "HasPrivilege" && IsContextAccess(m.Expression):
                    analysis.ChecksPrivilege = true;
                    break;
                case MemberAccessExpressionSyntax m when m.Expression is IdentifierNameSyntax { Identifier.ValueText: "Request" }:
                    var member = m.Name.Identifier.ValueText;
                    if (member is "Body" or "Form")
                    {
                        analysis.Body.Add("Request." + member);
                    }
                    if (member == "Form" && m.Parent is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Files" })
                    {
                        analysis.Files = true;
                    }
                    if (member == "Headers" && m.Parent is ElementAccessExpressionSyntax ea && FirstStringLiteral(ea.ArgumentList) is { } header)
                    {
                        analysis.Headers.Add(header);
                    }
                    if (member == "Form" && m.Parent is ElementAccessExpressionSyntax fa && FirstStringLiteral(fa.ArgumentList) is { } field)
                    {
                        analysis.Body.Add($"Request.Form[{field}]");
                    }
                    break;
                case MemberAccessExpressionSyntax m when m.Parent is not MemberAccessExpressionSyntax
                    && m.ToString().StartsWith("Parameters.", StringComparison.Ordinal) && SyntaxFacts.IsIdentifierChain(m):
                    analysis.Gates.Add(m.ToString());
                    break;
            }
        }
    }

    private static bool IsContextAccess(ExpressionSyntax e)
        => e is IdentifierNameSyntax { Identifier.ValueText: "context" }
            || e is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "context" };

    private static string? FirstStringLiteral(BracketedArgumentListSyntax list)
        => list.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax lit && lit.IsKind(SyntaxKind.StringLiteralExpression) ? lit.Token.ValueText : null;

    private void CollectInvocationFacts(
        InvocationExpressionSyntax invocation,
        ExpressionSyntax? receiver,
        string name,
        GenericNameSyntax? generic,
        ResolveContext context,
        CallAnalysis analysis,
        bool isController)
    {
        if (name == "Deserialize" && generic is not null && receiver is not null
            && LastName(receiver) == _options.RequestBodyMember)
        {
            foreach (var arg in generic.TypeArgumentList.Arguments)
            {
                foreach (var t in _models.Require(arg, context))
                {
                    analysis.RequestTypes.Add(t);
                }
            }
        }
        if (receiver is IdentifierNameSyntax rid && _options.ResponseBuilderTypes.Contains(rid.Identifier.ValueText))
        {
            analysis.ApiResults.Add(name);
            if (name == "Get")
            {
                foreach (var arg in invocation.ArgumentList.Arguments.Where(a => a.NameColon?.Name.Identifier.ValueText == "response"))
                {
                    if (arg.Expression is AnonymousObjectCreationExpressionSyntax anon)
                    {
                        foreach (var init in anon.Initializers)
                        {
                            var member = init.NameEquals?.Name.Identifier.ValueText ?? LastName(init.Expression);
                            if (member is not null)
                            {
                                analysis.GetMembers.Add(member);
                            }
                        }
                    }
                }
            }
        }
        var firstLiteral = invocation.ArgumentList.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax lit
            && lit.IsKind(SyntaxKind.StringLiteralExpression) ? lit.Token.ValueText : null;
        if (isController && firstLiteral is not null && receiver is not null)
        {
            var last = LastName(receiver);
            if (last == "QueryStrings")
            {
                analysis.QueryStrings.Add((firstLiteral, name));
            }
            else if (last == "Forms")
            {
                analysis.Forms.Add((firstLiteral, name));
            }
        }
    }

    private static string? LastName(ExpressionSyntax e) => e switch
    {
        IdentifierNameSyntax i => i.Identifier.ValueText,
        MemberAccessExpressionSyntax m => m.Name.Identifier.ValueText,
        MemberBindingExpressionSyntax b => b.Name.Identifier.ValueText,
        ConditionalAccessExpressionSyntax c => LastName(c.WhenNotNull),
        _ => null,
    };

    private static (ExpressionSyntax? Receiver, string? Name, GenericNameSyntax? Generic) Split(ExpressionSyntax expression) => expression switch
    {
        IdentifierNameSyntax i => (null, i.Identifier.ValueText, null),
        GenericNameSyntax g => (null, g.Identifier.ValueText, g),
        MemberAccessExpressionSyntax m => (m.Expression, m.Name.Identifier.ValueText, m.Name as GenericNameSyntax),
        MemberBindingExpressionSyntax b => (b, b.Name.Identifier.ValueText, b.Name as GenericNameSyntax),
        _ => (null, null, null),
    };

    private IEnumerable<(MethodDeclarationSyntax Method, string Path, TypeEntry? Owner)> Targets(
        ExpressionSyntax? receiver,
        string name,
        TypeEntry? owner,
        string path,
        int depth,
        ResolveContext context)
    {
        var nameMatches = name.Contains(_options.FollowMethodNameContains, StringComparison.Ordinal);
        switch (receiver)
        {
            case null:
                // 同じ型のメソッド（コントローラの補助メソッドは名前に関係なく辿る）
                if (owner is not null && (depth == 0 || nameMatches))
                {
                    foreach (var (m, p) in owner.Methods(name))
                    {
                        yield return (m, p, owner);
                    }
                }
                yield break;
            case ThisExpressionSyntax:
                goto case null;
            case ObjectCreationExpressionSyntax oc when nameMatches:
                foreach (var t in ResolveTypes(oc.Type, context))
                {
                    foreach (var (m, p) in t.Methods(name))
                    {
                        yield return (m, p, t);
                    }
                }
                yield break;
            case IdentifierNameSyntax or MemberAccessExpressionSyntax when StaticType(receiver, context) is { } type:
                if (_options.TerminalTypes.Contains(type.SimpleName))
                {
                    yield break;
                }
                if (nameMatches || (depth == 0 && type.SimpleName.EndsWith(_options.HandlerTypeSuffix, StringComparison.Ordinal)))
                {
                    foreach (var (m, p) in type.Methods(name))
                    {
                        yield return (m, p, type);
                    }
                }
                yield break;
            default:
                // 受け手の型が構文から分からない（ローカル変数など）。同じディレクトリで同名のメソッドを持つ型を候補にする。
                if (nameMatches)
                {
                    foreach (var t in _index.TypesInDirectoryWithMethod(CodeIndex.DirectoryOf(path), name).ToList())
                    {
                        if (_options.TerminalTypes.Contains(t.SimpleName))
                        {
                            continue;
                        }
                        foreach (var (m, p) in t.Methods(name))
                        {
                            yield return (m, p, t);
                        }
                    }
                }
                yield break;
        }
    }

    /// <summary>受け手が型名（大文字で始まる識別子の連なり）で、ソースの型へ解決できるなら、その型。</summary>
    private TypeEntry? StaticType(ExpressionSyntax receiver, ResolveContext context)
    {
        if (!SyntaxFacts.IsIdentifierChain(receiver))
        {
            return null;
        }
        var parts = SyntaxFacts.NameParts(ToName(receiver));
        return char.IsUpper(parts[0][0]) ? _index.Resolve(parts, 0, context).Entry : null;
    }

    private IEnumerable<TypeEntry> ResolveTypes(TypeSyntax type, ResolveContext context)
    {
        if (type is NameSyntax n && _index.Resolve(SyntaxFacts.NameParts(n), 0, context).Entry is { } t)
        {
            yield return t;
        }
    }

    private static NameSyntax ToName(ExpressionSyntax e) => e switch
    {
        IdentifierNameSyntax i => i,
        MemberAccessExpressionSyntax m => SyntaxFactory.QualifiedName(ToName(m.Expression), (SimpleNameSyntax)m.Name),
        _ => throw new ArgumentException("識別子の連なりではありません。", nameof(e)),
    };

    private sealed class CallAnalysis
    {
        public HashSet<MethodDeclarationSyntax> Visited { get; } = [];

        public SortedSet<string> Handlers { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> RequestTypes { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> ApiResults { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> GetMembers { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> ResponseModels { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> Body { get; } = new(StringComparer.Ordinal);

        public SortedSet<string> Headers { get; } = new(StringComparer.Ordinal);

        public SortedSet<(string Name, string Accessor)> QueryStrings { get; } = new(OrdinalPairComparer);

        public SortedSet<(string Name, string Accessor)> Forms { get; } = new(OrdinalPairComparer);

        private static readonly Comparer<(string Name, string Accessor)> OrdinalPairComparer = Comparer<(string Name, string Accessor)>.Create((a, b) =>
        {
            var c = string.CompareOrdinal(a.Name, b.Name);
            return c != 0 ? c : string.CompareOrdinal(a.Accessor, b.Accessor);
        });

        public SortedSet<string> Gates { get; } = new(StringComparer.Ordinal);

        public bool Files { get; set; }

        public bool ChecksAuthenticated { get; set; }

        public bool ChecksPrivilege { get; set; }
    }
}
