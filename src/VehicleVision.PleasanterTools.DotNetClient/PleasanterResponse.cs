using System.Text.Json;
using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>Pleasanter の応答をどう扱うか。</summary>
/// <remarks>分類を誤ると、直らないものを再送し続けるか、直るものを捨てる。</remarks>
public enum PleasanterErrorKind
{
    /// <summary>成功。</summary>
    None,

    /// <summary>一時的。再送してよい。</summary>
    Transient,

    /// <summary>恒久的。再送しても通らない。</summary>
    Permanent,

    /// <summary>認証に失敗。キーの失効・設定ミスのため人が直す。</summary>
    Unauthorized,

    /// <summary>応答が返らなかった。できたかどうか分からないので、照合してから判断する。</summary>
    Unknown,
}

/// <summary>Pleasanter 呼び出しの結果。</summary>
/// <param name="ErrorKind">扱い方。</param>
/// <param name="StatusCode">HTTP のステータス。応答が無ければ <c>null</c>。</param>
/// <param name="Id">作成・更新されたレコードの ID（応答の <c>Id</c>）。</param>
/// <param name="Message">Pleasanter が返した文言（応答の <c>Message</c>）。</param>
/// <param name="Body">応答の中身（型付けしない JSON）。JSON でなければ <c>null</c>。</param>
public record PleasanterResponse(
    PleasanterErrorKind ErrorKind,
    int? StatusCode = null,
    long? Id = null,
    string? Message = null,
    JsonNode? Body = null)
{
    /// <summary>成功したか（エラーの分類が <see cref="PleasanterErrorKind.None"/>）。</summary>
    public bool IsSuccess => ErrorKind is PleasanterErrorKind.None;
}

/// <summary>型付きの Pleasanter 呼び出しの結果。</summary>
/// <typeparam name="T">応答の型。</typeparam>
/// <param name="ErrorKind">扱い方。</param>
/// <param name="StatusCode">HTTP のステータス。応答が無ければ <c>null</c>。</param>
/// <param name="Id">応答の <c>Id</c>。</param>
/// <param name="Message">応答の <c>Message</c>。</param>
/// <param name="Body">応答の中身（型付けしない JSON）。</param>
/// <param name="Value">応答を型へ読んだもの。成功していない・読めないときは <c>null</c>。</param>
public sealed record PleasanterResponse<T>(
    PleasanterErrorKind ErrorKind,
    int? StatusCode,
    long? Id,
    string? Message,
    JsonNode? Body,
    T? Value)
    : PleasanterResponse(ErrorKind, StatusCode, Id, Message, Body)
    where T : class;

/// <summary>ファイルを返す呼び出しの結果。</summary>
/// <param name="ErrorKind">扱い方。</param>
/// <param name="StatusCode">HTTP のステータス。応答が無ければ <c>null</c>。</param>
/// <param name="Content">ファイルの中身。成功していなければ <c>null</c>。</param>
/// <param name="ContentType">応答の Content-Type。</param>
/// <param name="FileName">応答の Content-Disposition のファイル名。</param>
public sealed record PleasanterBinaryResponse(
    PleasanterErrorKind ErrorKind,
    int? StatusCode = null,
    byte[]? Content = null,
    string? ContentType = null,
    string? FileName = null)
{
    /// <summary>成功したか。</summary>
    public bool IsSuccess => ErrorKind is PleasanterErrorKind.None;
}

/// <summary>
/// 一覧などの応答の包み（<c>StatusCode</c>・<c>LimitPerDate</c>・<c>LimitRemaining</c>・<c>Response</c>）。
/// </summary>
/// <remarks>
/// 包みの形は、Pleasanter の <c>Libraries/Responses/ApiResults.cs</c> の <c>Get(int statusCode, int limitPerDate, int limitRemaining, object response)</c> による
/// （<c>Pleasanter_1.5.8.1</c> で確認。参照日 2026-10-21）。<c>api-surface/history.json</c> には包みの型は無い。実機では確かめていない。
/// </remarks>
/// <typeparam name="TResponse"><c>Response</c> の型。</typeparam>
public sealed class PleasanterEnvelope<TResponse>
    where TResponse : class
{
    /// <summary>応答のステータス。</summary>
    public int? StatusCode { get; set; }

    /// <summary>1 日の上限。上限が無ければ <c>null</c>。</summary>
    public int? LimitPerDate { get; set; }

    /// <summary>1 日の残り。上限が無ければ <c>null</c>。</summary>
    public int? LimitRemaining { get; set; }

    /// <summary>中身。</summary>
    public TResponse? Response { get; set; }
}

/// <summary>送るファイル。</summary>
/// <param name="Content">中身。呼び出しの間だけ読む（閉じない）。</param>
/// <param name="FileName">ファイル名。</param>
/// <param name="ContentType">Content-Type（例: <c>text/csv</c>）。無ければ <c>application/octet-stream</c>。</param>
public sealed record PleasanterFile(Stream Content, string FileName, string? ContentType = null);

/// <summary>型付けしていない JSON の値を、利用者が選んだ型へ読む。</summary>
public static class PleasanterJsonElementExtensions
{
    /// <summary>
    /// 値を <typeparamref name="T"/> へ読む。<see cref="PleasanterApiClient.DeserializeAsync{T}(JsonElement?, CancellationToken)"/> と違い、版に合わせた読み替えをしない。
    /// </summary>
    /// <typeparam name="T">読む型。</typeparam>
    /// <param name="element">値。</param>
    public static T? Deserialize<T>(this JsonElement? element)
        => element is { } e
            ? e.Deserialize<T>(PleasanterJson.For(null, UnknownServerVersionBehavior.Attempt))
            : default;
}
