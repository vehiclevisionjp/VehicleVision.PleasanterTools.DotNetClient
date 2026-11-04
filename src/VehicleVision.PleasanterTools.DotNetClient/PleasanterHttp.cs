namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>
/// リダイレクトを辿らない <see cref="HttpClient"/>（の元になる <see cref="HttpClientHandler"/>）を作る。
/// </summary>
/// <remarks>
/// <para>**API キーはリクエストの本文に載る。** 既定の <see cref="HttpClientHandler"/> は 3xx を自動で辿り、
/// 307・308 では本文（API キー）を、リダイレクト先のホストへそのまま再送する。
/// 辿らない設定にしておけば、3xx は応答（<see cref="PleasanterResponse.StatusCode"/> が 3xx、
/// <see cref="PleasanterResponse.ErrorKind"/> が <see cref="PleasanterErrorKind.Permanent"/>）としてそのまま返り、本文は再送されない。</para>
/// <para>**<see cref="PleasanterApiClient"/> は、渡された <see cref="HttpClient"/> の設定を変えられない。** リダイレクトを辿るかは、
/// <see cref="HttpClient"/> を作る側で決める。このクラスの <see cref="CreateHandler"/> の戻り値を使うか、
/// 同じ設定（<see cref="HttpClientHandler.AllowAutoRedirect"/> を <c>false</c>）にすること。</para>
/// <para>IHttpClientFactory で使うときは、<c>ConfigurePrimaryHttpMessageHandler(PleasanterHttp.CreateHandler)</c> を指定する（README の「HttpClient の作り方」）。</para>
/// </remarks>
public static class PleasanterHttp
{
    /// <summary>リダイレクトを辿らず、Cookie を保持しない <see cref="HttpClientHandler"/> を作る。</summary>
    /// <remarks>
    /// 呼ぶたびに新しいインスタンスを返す。<see cref="HttpClient"/> へ渡すと、その <see cref="HttpClient"/> を破棄するときに一緒に破棄される。
    /// <see cref="HttpClientHandler.UseCookies"/> も <c>false</c> にする（API キーは本文で送るため Cookie は要らず、別の呼び出しへ持ち越さない）。
    /// </remarks>
    public static HttpClientHandler CreateHandler() => new()
    {
        AllowAutoRedirect = false,
        UseCookies = false,
    };

    /// <summary>リダイレクトを辿らない <see cref="HttpClient"/> を作る。</summary>
    /// <remarks>
    /// IHttpClientFactory を使わない単純な用途向け。<see cref="HttpClient"/> は 1 つを使い回し、呼び出しごとに作らない
    /// （ソケットが枯渇する）。接続の寿命（DNS の変更への追従）を管理したいときは IHttpClientFactory を使う。
    /// 1 回の呼び出しの上限は <see cref="PleasanterOptions.Timeout"/> で決まる。
    /// </remarks>
    public static HttpClient CreateClient() => new(CreateHandler(), disposeHandler: true);
}
