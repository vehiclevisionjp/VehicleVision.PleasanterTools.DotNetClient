namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>Since・Until と、サーバーの版から、使えるかを判定する。</summary>
internal static class VersionGuard
{
    /// <summary>抽出の対象の最初の版。Since がこの版なら「この版以前から在る」。</summary>
    public static readonly Version MinimumSupported = new(1, 4, 0, 0);

    /// <summary>使えなければ、投げる例外を返す。使えるなら <c>null</c>。</summary>
    /// <param name="member">機能の名前（例外の文言に使う）。</param>
    /// <param name="since">在る最初の版。無ければ <c>null</c>（制限なし）。</param>
    /// <param name="until">無くなった版。無ければ <c>null</c>。</param>
    /// <param name="serverVersion">サーバーの版。不明なら <c>null</c>。</param>
    /// <param name="unknown">版が不明のときの振る舞い。</param>
    public static PleasanterVersionNotSupportedException? Check(
        string member,
        Version? since,
        Version? until,
        Version? serverVersion,
        UnknownServerVersionBehavior unknown)
    {
        if (serverVersion is null)
        {
            // 版を問わず使えるもの（Since が最初の版以前で、Until が無い）は、不明でも送ってよい
            var alwaysAvailable = (since is null || since <= MinimumSupported) && until is null;
            return unknown is UnknownServerVersionBehavior.Reject && !alwaysAvailable
                ? new PleasanterVersionNotSupportedException(member, since, until, null)
                : null;
        }

        // Since が最初の版のときは、それより古い版（サポート外）でも止めない（抽出していないので分からない）
        var tooOld = since is not null && since > MinimumSupported && Normalize(serverVersion) < since;
        var tooNew = until is not null && Normalize(serverVersion) >= until;
        return tooOld || tooNew
            ? new PleasanterVersionNotSupportedException(member, since, until, serverVersion)
            : null;
    }

    /// <summary>使えなければ例外を投げる。</summary>
    public static void Ensure(string member, Version? since, Version? until, Version? serverVersion, UnknownServerVersionBehavior unknown)
    {
        if (Check(member, since, until, serverVersion, unknown) is { } ex)
        {
            throw ex;
        }
    }

    /// <summary>省いた部分を 0 にそろえる（<c>1.5</c> と <c>1.5.0.0</c> を同じに比べるため）。</summary>
    public static Version Normalize(Version version) => new(
        version.Major,
        version.Minor,
        Math.Max(version.Build, 0),
        Math.Max(version.Revision, 0));
}
