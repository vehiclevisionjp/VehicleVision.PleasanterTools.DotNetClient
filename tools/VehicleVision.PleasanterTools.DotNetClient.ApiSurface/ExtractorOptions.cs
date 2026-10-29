namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface;

/// <summary>
/// 抽出の設定。既定値は Pleasanter 1.4.0.0〜1.5.9.0 のソース配置に合わせている
/// （根拠は _documents/調査メモ_API定義の場所.md）。テストでは別の配置を渡す。
/// </summary>
internal sealed record ExtractorOptions
{
    /// <summary>型・メソッドを探すディレクトリ（*.cs）。</summary>
    public IReadOnlyList<string> SourceRoots { get; init; } = ["Implem.Pleasanter", "Implem.Libraries"];

    /// <summary>コントローラを探すディレクトリ（再帰）。</summary>
    public string ControllersDirectory { get; init; } = "Implem.Pleasanter/Controllers";

    /// <summary>規約ルーティング（MapControllerRoute）を読むファイル。</summary>
    public string StartupFile { get; init; } = "Implem.Pleasanter/Startup.cs";

    /// <summary>API の既定値（ApiVersion・PageSize など）を読む JSON。</summary>
    public string ApiParametersFile { get; init; } = "Implem.Pleasanter/App_Data/Parameters/Api.json";

    /// <summary>属性ルーティングを持たない旧来の API コントローラの名前の接頭辞。</summary>
    public string LegacyControllerPrefix { get; init; } = "Api_";

    /// <summary>リクエストボディ（JSON 文字列）を持つメンバーの名前。<c>X.Deserialize&lt;T&gt;()</c> の X の末尾。</summary>
    public string RequestBodyMember { get; init; } = "RequestDataString";

    /// <summary>応答を組み立てる型（呼び出しを記録するが、中へは辿らない）。</summary>
    public IReadOnlyList<string> ResponseBuilderTypes { get; init; } = ["ApiResults"];

    /// <summary>中へ辿らない型。</summary>
    public IReadOnlyList<string> TerminalTypes { get; init; } = ["ApiResults", "ApiResponses"];

    /// <summary>応答のモデルとみなす型名の接尾辞（辿ったメソッドの戻り値の型）。</summary>
    public string ResponseModelSuffix { get; init; } = "ApiModel";

    /// <summary>コントローラ直下（深さ 0）で、名前に関係なく中へ辿る型名の接尾辞。</summary>
    public string HandlerTypeSuffix { get; init; } = "Utilities";

    /// <summary>深さ 1 以降で中へ辿るメソッド名に含まれる文字列。</summary>
    public string FollowMethodNameContains { get; init; } = "Api";

    /// <summary>エンドポイントから辿れなくても型の一覧に入れる型（単純名）。応答の外枠など。</summary>
    public IReadOnlyList<string> ExtraModelTypes { get; init; } = ["ApiResponse"];

    /// <summary>辿る深さの上限。</summary>
    public int MaxDepth { get; init; } = 6;

    /// <summary>ツリーから読む拡張子。</summary>
    public IReadOnlyList<string> Extensions { get; init; } = [".cs", ".json"];

    /// <summary>prepare で本体を取得するファイル（SourceRoots の *.cs に加えて）。</summary>
    public IReadOnlyList<string> ExtraFiles => [StartupFile, ApiParametersFile];
}
