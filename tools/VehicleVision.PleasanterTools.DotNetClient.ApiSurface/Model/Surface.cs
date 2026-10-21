using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;

// API 表（ある版の API 面）。中身は識別子・型名・パス・リテラルの値といった事実だけで、
// ソースの文章やコードの断片は入れない。並びはすべて序数順にそろえ、差分を読みやすくする。

internal sealed record SurfaceDocument
{
    public string Schema { get; init; } = "pleasanter-api-surface/1";

    public required SourceInfo Source { get; init; }

    /// <summary>API の既定値（Parameters/Api.json の値）。読めなければ null。</summary>
    public JsonObject? ApiParameters { get; init; }

    public required IReadOnlyList<ConventionalRoute> ConventionalRoutes { get; init; }

    public required IReadOnlyList<Endpoint> Endpoints { get; init; }

    public required IReadOnlyList<TypeModel> Types { get; init; }

    /// <summary>ソースの中で見つからなかった型名（外部ライブラリなど）。メンバーは未抽出。</summary>
    public required IReadOnlyList<string> UnresolvedTypes { get; init; }

    /// <summary>候補が複数あり、決められなかった型名。</summary>
    public required IReadOnlyList<string> AmbiguousTypes { get; init; }
}

internal sealed record SourceInfo(string Label, string? Commit);

internal sealed record ConventionalRoute(
    string Name,
    string? Pattern,
    IReadOnlyDictionary<string, string> Defaults,
    IReadOnlyDictionary<string, string> Constraints);

internal sealed record Endpoint
{
    /// <summary>差分の照合に使うキー。属性ルーティングは「メソッド 小文字のパス」、規約ルーティングは「メソッド legacy:コントローラ/アクション」。</summary>
    public required string Key { get; init; }

    /// <summary>api / scim / legacy / other（パスの先頭や、規約ルーティングかで決める）。</summary>
    public required string Area { get; init; }

    public required string HttpMethod { get; init; }

    /// <summary>テンプレートのとおりのパス（大文字小文字も元のまま）。規約ルーティングは null（Startup のどの規約に当たるかは推定しない）。</summary>
    public string? Path { get; init; }

    /// <summary>attribute / conventional</summary>
    public required string Routing { get; init; }

    public required string Controller { get; init; }

    public required string Action { get; init; }

    public required string File { get; init; }

    public required IReadOnlyList<EndpointParameter> Parameters { get; init; }

    public required EndpointAttributes Attributes { get; init; }

    /// <summary>実際に効くレート制限のポリシー名（メソッドの指定がクラスより優先）。無ければ null。</summary>
    public string? RateLimiting { get; init; }

    public required EndpointAuth Auth { get; init; }

    public required EndpointInput Input { get; init; }

    /// <summary>コントローラのメソッドが参照する <c>Parameters.*</c>（設定による有効・無効の条件の手がかり）。</summary>
    public required IReadOnlyList<string> Gates { get; init; }

    /// <summary>辿ったメソッド（型.メソッド）。版の差分の対象にはしない（内部の実装のため）。</summary>
    public required IReadOnlyList<string> Handlers { get; init; }

    /// <summary>リクエストボディを解釈する型（辿ったメソッドの中の <c>RequestDataString.Deserialize&lt;T&gt;()</c> と [FromBody] の型）。</summary>
    public required IReadOnlyList<string> RequestTypes { get; init; }

    public required EndpointResponse Response { get; init; }
}

internal sealed record EndpointParameter(string Name, string Type, string Source, JsonNode? Default);

internal sealed record EndpointAttributes(IReadOnlyList<string> Class, IReadOnlyList<string> Method);

/// <summary>
/// 認証の手がかり。Pleasanter の API はどれも ASP.NET の認証を素通し（AllowAnonymous）にして、
/// コントローラの中で Context の認証結果を確かめる形なので、その確認の有無を記録する。
/// </summary>
internal sealed record EndpointAuth(bool AllowAnonymous, bool Authorize, bool ChecksAuthenticated, bool ChecksPrivilege);

internal sealed record EndpointInput(
    IReadOnlyList<string> Body,
    IReadOnlyList<string> Headers,
    IReadOnlyList<NamedAccess> QueryStrings,
    IReadOnlyList<NamedAccess> Forms,
    bool Files);

internal sealed record NamedAccess(string Name, string Accessor);

internal sealed record EndpointResponse(
    IReadOnlyList<string> ApiResults,
    IReadOnlyList<string> GetMembers,
    IReadOnlyList<string> ModelTypes,
    string? ReturnType);

internal sealed record TypeModel
{
    public required string Name { get; init; }

    /// <summary>class / struct / enum / interface / record</summary>
    public required string Kind { get; init; }

    public required IReadOnlyList<string> Files { get; init; }

    /// <summary>基底の型・インターフェイス（解決できたものは完全名、できなければ書かれたとおりの名前）。</summary>
    public required IReadOnlyList<string> Bases { get; init; }

    public required IReadOnlyList<string> Attributes { get; init; }

    public required IReadOnlyList<MemberModel> Members { get; init; }

    public required IReadOnlyList<EnumValue> EnumValues { get; init; }
}

internal sealed record MemberModel
{
    public required string Name { get; init; }

    /// <summary>property / field</summary>
    public required string Kind { get; init; }

    /// <summary>書かれたとおりの型（空白を除く）。</summary>
    public required string Type { get; init; }

    /// <summary>参照先の型の完全名（ソースで見つかったものだけ）。</summary>
    public required IReadOnlyList<string> TypeRefs { get; init; }

    /// <summary>true / false。参照型か値型かが構文から分からないときは null（未判定）。</summary>
    public bool? Nullable { get; init; }

    /// <summary>初期化子。リテラルは値、識別子の連なりは {"ref":…}、new は {"new":…}、それ以外は {"unextracted":true}。</summary>
    public JsonNode? Default { get; init; }

    /// <summary>set / init が無い（JSON から値を受け取れない）。</summary>
    public bool ReadOnly { get; init; }

    public required IReadOnlyList<string> Attributes { get; init; }
}

internal sealed record EnumValue(string Name, JsonNode? Value);
