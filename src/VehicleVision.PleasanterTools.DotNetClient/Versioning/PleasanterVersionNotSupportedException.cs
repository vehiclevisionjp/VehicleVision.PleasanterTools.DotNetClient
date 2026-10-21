namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>接続先の Pleasanter の版では使えない機能を呼ぼうとした。リクエストは送っていない。</summary>
/// <remarks>
/// 版の範囲は <c>api-surface/history.json</c>（ソースからの抽出）による。実機では確かめていない。
/// 版が分からないときに投げるのは、<see cref="PleasanterOptions.UnknownServerVersion"/> が
/// <see cref="UnknownServerVersionBehavior.Reject"/> のときだけ（<see cref="ServerVersion"/> は <c>null</c>）。
/// </remarks>
public sealed class PleasanterVersionNotSupportedException : InvalidOperationException
{
    /// <summary>既定の文言で作る。</summary>
    public PleasanterVersionNotSupportedException()
        : this("（不明）", null, null, null)
    {
    }

    /// <summary>文言を指定して作る。</summary>
    /// <param name="message">文言。</param>
    public PleasanterVersionNotSupportedException(string message)
        : base(message)
    {
        Member = "（不明）";
    }

    /// <summary>文言と内側の例外を指定して作る。</summary>
    /// <param name="message">文言。</param>
    /// <param name="innerException">内側の例外。</param>
    public PleasanterVersionNotSupportedException(string message, Exception innerException)
        : base(message, innerException)
    {
        Member = "（不明）";
    }

    /// <summary>使えない機能と、版の範囲を指定して作る。</summary>
    /// <param name="member">使えない機能（例: <c>Items.MoveSiteAsync</c>・<c>Api.SsCache</c>）。</param>
    /// <param name="since">その機能が在る最初の版。</param>
    /// <param name="until">その機能が無くなった版。</param>
    /// <param name="serverVersion">判定に使ったサーバーの版。分からなければ <c>null</c>。</param>
    public PleasanterVersionNotSupportedException(string member, Version? since, Version? until, Version? serverVersion)
        : base(BuildMessage(member, since, until, serverVersion))
    {
        Member = member;
        Since = since;
        Until = until;
        ServerVersion = serverVersion;
    }

    /// <summary>使えない機能。</summary>
    public string Member { get; }

    /// <summary>その機能が在る最初の版（<c>1.4.0.0</c> は「1.4.0.0 以前から」）。</summary>
    public Version? Since { get; }

    /// <summary>その機能が無くなった版（排他）。無くなっていなければ <c>null</c>。</summary>
    public Version? Until { get; }

    /// <summary>判定に使ったサーバーの版。分からなければ <c>null</c>。</summary>
    public Version? ServerVersion { get; }

    private static string BuildMessage(string member, Version? since, Version? until, Version? serverVersion)
    {
        var range = until is null ? $"{since} 以降" : $"{since} 以上 {until} 未満";
        return serverVersion is null
            ? $"{member} は Pleasanter {range} で使えます。接続先の版が分からないため送りません（PleasanterOptions.ServerVersion で版を指定できます）。"
            : $"{member} は Pleasanter {range} で使えます。接続先の版は {serverVersion} です。";
    }
}
