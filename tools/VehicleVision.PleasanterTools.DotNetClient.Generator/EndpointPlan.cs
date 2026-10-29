using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>本文の送り方。</summary>
internal enum Transport
{
    /// <summary>JSON の本文（<c>Request.Body</c>）。</summary>
    Json,

    /// <summary>フォームの <c>parameters</c> に JSON、ファイルを添付（<c>Request.Form["parameters"]</c> とファイル）。</summary>
    Import,

    /// <summary><c>Authorization</c> ヘッダとファイル・フォームの値。</summary>
    Upload,
}

/// <summary>応答の読み方。</summary>
internal enum ResponseKind
{
    /// <summary>型付けしない JSON。</summary>
    Untyped,

    /// <summary>包みと、メンバー名だけの <c>Response</c>。</summary>
    Envelope,

    /// <summary>ファイル（<c>FileStreamResult</c>）。</summary>
    Binary,
}

/// <summary>ルートの引数。</summary>
internal sealed record RouteParameter(string Name, string Type);

/// <summary>クエリ文字列・フォームの値。</summary>
internal sealed record ValueParameter(string Name, string Type);

/// <summary>生成するエンドポイント 1 つ。</summary>
internal sealed class EndpointPlan
{
    public required HistoryItem Item { get; init; }

    public required string Group { get; init; }

    public required string Action { get; init; }

    public required string Path { get; init; }

    public required Transport Transport { get; init; }

    public required ResponseKind Response { get; init; }

    public required IReadOnlyList<RouteParameter> Route { get; init; }

    public required IReadOnlyList<string> RequestTypes { get; init; }

    public required IReadOnlyList<ValueParameter> Queries { get; init; }

    public required IReadOnlyList<ValueParameter> Forms { get; init; }

    public required IReadOnlyList<string> GetMembers { get; init; }

    public required IReadOnlyList<string> ModelTypes { get; init; }

    public string? RateLimiting { get; init; }

    public bool ChecksPrivilege { get; init; }

    public string Key => Item.Key["endpoint:".Length..];

    /// <summary>エンドポイントを読み、生成のしかたを決める。決められなければ止める。</summary>
    public static EndpointPlan From(HistoryItem item)
    {
        var v = item.Latest;
        var method = (string)v["httpMethod"]!;
        if (method != "POST")
        {
            throw new GenerationException($"{item.Key}: POST 以外（{method}）は生成できません。");
        }

        var path = (string?)v["path"] ?? throw new GenerationException($"{item.Key}: パスが分かりません。");
        var controller = (string)v["controller"]!;
        var group = controller.EndsWith("Controller", StringComparison.Ordinal) ? controller[..^"Controller".Length] : controller;
        var action = (string)v["action"]!;
        var parameters = v["parameters"]?.AsArray() ?? [];
        var route = parameters
            .Where(p => (string?)p!["source"] == "route")
            .Select(p => new RouteParameter((string)p!["name"]!, (string)p["type"]!))
            .ToList();
        var input = v["input"]!;
        var body = Strings(input["body"]);
        var headers = Strings(input["headers"]);
        var files = (bool?)input["files"] ?? false;
        var transport = files && body.Contains("Request.Form[parameters]") ? Transport.Import
            : files && headers.Contains("Authorization") ? Transport.Upload
            : body.Contains("Request.Body") ? Transport.Json
            : throw new GenerationException($"{item.Key}: 本文の送り方が分かりません（input: {input.ToJsonString()}）。");
        var response = v["response"]!;
        var getMembers = Strings(response["getMembers"]);
        var kind = (string?)response["returnType"] == "FileStreamResult" ? ResponseKind.Binary
            : getMembers.Count > 0 ? ResponseKind.Envelope
            : ResponseKind.Untyped;
        if (transport != Transport.Json && kind != ResponseKind.Untyped)
        {
            throw new GenerationException($"{item.Key}: フォームで送り、型付きの応答を返す形は扱えません。");
        }

        return new EndpointPlan
        {
            Item = item,
            Group = group,
            Action = action,
            Path = path,
            Transport = transport,
            Response = kind,
            Route = route,
            RequestTypes = transport == Transport.Upload ? [] : Strings(v["requestTypes"]),
            Queries = Values(input["queryStrings"]),
            Forms = Values(input["forms"]),
            GetMembers = getMembers,
            ModelTypes = Strings(response["modelTypes"]),
            RateLimiting = (string?)v["rateLimiting"],
            ChecksPrivilege = (bool?)v["auth"]?["checksPrivilege"] ?? false,
        };
    }

    private static List<string> Strings(JsonNode? node)
        => node?.AsArray().Select(n => (string)n!).ToList() ?? [];

    /// <summary>クエリ文字列・フォームの値。同じ名前は 1 つにし、型は読み方（Bool・Long など）から決める。</summary>
    private static List<ValueParameter> Values(JsonNode? node)
    {
        var result = new List<ValueParameter>();
        foreach (var group in (node?.AsArray() ?? []).GroupBy(n => (string)n!["name"]!, StringComparer.Ordinal))
        {
            // ContainsKey は「あるか」を見るだけなので、ほかの読み方があればそちらの型にする
            var accessors = group.Select(n => (string)n!["accessor"]!).Where(a => a != "ContainsKey").ToList();
            var type = accessors.FirstOrDefault() switch
            {
                "Bool" => "bool?",
                "Long" => "long?",
                "Int" => "int?",
                "Decimal" => "decimal?",
                _ => "string?",
            };
            result.Add(new ValueParameter(group.Key, type));
        }

        return result;
    }
}
