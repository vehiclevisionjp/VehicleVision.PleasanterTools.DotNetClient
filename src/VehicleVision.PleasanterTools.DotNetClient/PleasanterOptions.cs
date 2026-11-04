namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>Pleasanter の接続設定を呼び出し時に解決する。</summary>
public interface IPleasanterOptionsProvider
{
    /// <summary>接続設定を取得する。</summary>
    /// <param name="cancellationToken">取り消し。</param>
    Task<PleasanterOptions> GetAsync(CancellationToken cancellationToken = default);
}

/// <summary>接続先の Pleasanter の版が分からないときの振る舞い。</summary>
public enum UnknownServerVersionBehavior
{
    /// <summary>
    /// 拒否せず送る（既定）。サーバーに無いエンドポイントは 404 になり、無いメンバーはサーバーが黙って無視する。
    /// </summary>
    Attempt,

    /// <summary>
    /// 版を問わず使えるもの（1.3.0.0 以前から在り、無くなっていないもの）以外は、
    /// <see cref="PleasanterVersionNotSupportedException"/> を投げて送らない。
    /// </summary>
    Reject,
}

/// <summary>接続先 Pleasanter の設定。</summary>
/// <remarks>
/// 実値は環境変数か Git 管理外の設定ファイルから読む。
/// **API キーを設定ファイルへ直書きしてコミットしないこと。**
/// </remarks>
public sealed class PleasanterOptions
{
    /// <summary>API のバージョンの既定。</summary>
    /// <remarks>Pleasanter の <c>App_Data/Parameters/Api.json</c> の <c>Version</c> と同じ値（1.3.13.0〜1.5.9.0 で変わっていない。1.3.12.0 以前は <c>Api.json</c> に <c>Version</c> が無く、サーバー側の既定は 1.000。Issue #80）。</remarks>
    public const decimal DefaultApiVersion = 1.1m;

    /// <summary>1 回の呼び出しの上限の既定。</summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>版の問い合わせの上限の既定。</summary>
    public static readonly TimeSpan DefaultServerVersionTimeout = TimeSpan.FromSeconds(10);

    /// <summary>版を取れなかったときに、次に問い合わせるまでの間隔の既定。</summary>
    public static readonly TimeSpan DefaultServerVersionRetryInterval = TimeSpan.FromMinutes(1);

    /// <summary>ベース URL。末尾のスラッシュは付けない。</summary>
    /// <remarks>
    /// API キーはリクエストの本文に載るため、次を満たさない URL は、通信の前に <see cref="ArgumentException"/> で拒否する（<see cref="Validate"/>）。
    /// <list type="bullet">
    /// <item><description><c>https</c> であること。<c>http</c> はループバック（<c>localhost</c>・<c>127.0.0.1</c>・<c>[::1]</c>）だけ</description></item>
    /// <item><description>資格情報（<c>user:pass@</c>）・クエリ（<c>?</c>）・フラグメント（<c>#</c>）を含まないこと</description></item>
    /// </list>
    /// パス（<c>https://example.com/pleasanter</c>）は付けてよい。
    /// </remarks>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// ループバック以外の <c>http</c> も許す。**検証用の環境（コンテナ間の通信など）だけで有効にする。** 既定は無効。
    /// </summary>
    /// <remarks>
    /// 有効にすると、API キーを含むリクエストの本文が**平文**で流れる。本番では有効にしない。
    /// 資格情報（<c>user:pass@</c>）・クエリ・フラグメント・<c>http</c>／<c>https</c> 以外のスキームは、有効にしても拒否する。
    /// 環境変数 <c>PLEASANTER_ALLOW_INSECURE_HTTP</c> が <c>true</c> のとき、<see cref="FromEnvironment"/> が有効にする。
    /// </remarks>
    public bool AllowInsecureHttp { get; init; }

    /// <summary>API キー。**サーバ側だけが持つ。ブラウザへ渡さない。**</summary>
    public required string ApiKey { get; init; }

    /// <summary>API のバージョン。リクエストボディの <c>ApiVersion</c> に載せる。</summary>
    public decimal ApiVersion { get; init; } = DefaultApiVersion;

    /// <summary>1 回の呼び出しの上限。</summary>
    public TimeSpan Timeout { get; init; } = DefaultTimeout;

    /// <summary>
    /// 接続先の Pleasanter の版（例: <c>1.4.6.0</c>）。指定すると、サーバーへ問い合わせずにこの版で判定する。
    /// </summary>
    /// <remarks>
    /// **1.3.x〜1.4.20.0 は、API で版を取れない**ので、版で使える機能を判定したいときは指定する
    /// （<c>_documents/クライアントの生成と版の判定.md</c>）。
    /// </remarks>
    public Version? ServerVersion { get; init; }

    /// <summary>
    /// <see cref="ServerVersion"/> が無いとき、最初の呼び出しの前に <c>api/utility/GetLicenseInfo</c> で版を問い合わせるか（既定 <c>true</c>）。
    /// </summary>
    /// <remarks>
    /// <para>問い合わせは API キーが要る呼び出しで、1.5.6.0 からはレート制限の対象。
    /// 版が取れるのは 1.4.21.0 以降（1.3.x〜1.4.20.0 は、問い合わせが 404 などになり、版は不明のまま）。</para>
    /// <para>結果は接続先（<see cref="BaseUrl"/>）ごとに、**プロセスで共通に**覚える（クライアントを作り直しても、DI の <c>AddTransient</c> でも、問い合わせ直さない）。
    /// 版が取れた・版の無い応答・404 は、結論としてずっと覚える（<c>RefreshServerVersionAsync</c> で更新）。
    /// 到達不能・認証失敗・5xx・JSON でない応答など、結論が出なかったときは <see cref="ServerVersionRetryInterval"/> のあいだ覚え、その間は問い合わせず、版は不明として扱う。
    /// 1 回の問い合わせは <see cref="ServerVersionTimeout"/> で打ち切る。</para>
    /// <para>追加の通信を避けたいときは、<see cref="ServerVersion"/> を指定するか、<c>false</c> にする（版で使えない機能の判定は、版が不明の扱い（<see cref="UnknownServerVersion"/>）になる）。</para>
    /// </remarks>
    public bool DetectServerVersion { get; init; } = true;

    /// <summary>版を問い合わせるときの上限（既定 10 秒）。<see cref="Timeout"/> のほうが短ければ、そちら。</summary>
    /// <remarks>到達できない接続先で、本来の呼び出しの前に <see cref="Timeout"/> の分だけ待たされないようにする。</remarks>
    public TimeSpan ServerVersionTimeout { get; init; } = DefaultServerVersionTimeout;

    /// <summary>
    /// 版を取れなかったときに、次に問い合わせるまでの間隔（既定 1 分）。<see cref="TimeSpan.Zero"/> なら、呼び出しのたびに問い合わせる（従来の挙動）。
    /// </summary>
    public TimeSpan ServerVersionRetryInterval { get; init; } = DefaultServerVersionRetryInterval;

    /// <summary>接続先の版が分からないときの振る舞い（既定 <see cref="UnknownServerVersionBehavior.Attempt"/>）。</summary>
    public UnknownServerVersionBehavior UnknownServerVersion { get; init; } = UnknownServerVersionBehavior.Attempt;

    /// <summary><see cref="BaseUrl"/> が安全に使える形かを検査する。通信の前に、呼び出しごとに実行される。</summary>
    /// <remarks>
    /// 例外の文言には、API キーも URL（資格情報を含みうる）も出さない。理由だけを書く。
    /// </remarks>
    /// <exception cref="ArgumentException"><see cref="BaseUrl"/> が上の条件を満たさない。</exception>
    public void Validate()
    {
        if (BaseUrlProblem(BaseUrl, AllowInsecureHttp) is { } problem)
        {
            throw new ArgumentException($"BaseUrl が使えません: {problem}", nameof(BaseUrl));
        }
    }

    /// <summary>問題があればその理由（URL を含まない文言）、無ければ <c>null</c>。</summary>
    internal static string? BaseUrlProblem(string? baseUrl, bool allowInsecureHttp = false)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return "空です。";
        }

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
        {
            return "絶対 URL として読めません。";
        }

        if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
        {
            return "スキームは https（ループバックだけ http）にしてください。";
        }

        if (uri.UserInfo.Length > 0 || baseUrl.Contains('@', StringComparison.Ordinal))
        {
            return "資格情報（user:pass@）を含めないでください。";
        }

        if (baseUrl.Contains('?', StringComparison.Ordinal) || baseUrl.Contains('#', StringComparison.Ordinal))
        {
            return "クエリ（?）とフラグメント（#）を含めないでください。";
        }

        if (uri.Scheme == Uri.UriSchemeHttp && !uri.IsLoopback && !allowInsecureHttp)
        {
            return "http はループバック（localhost・127.0.0.1・[::1]）だけ使えます。https にしてください。";
        }

        return null;
    }

    /// <summary>
    /// 環境変数 <c>PLEASANTER_BASE_URL</c> / <c>PLEASANTER_API_KEY</c> から作る。
    /// <c>PLEASANTER_SERVER_VERSION</c> があれば <see cref="ServerVersion"/> にする。
    /// </summary>
    public static PleasanterOptions FromEnvironment() => new()
    {
        BaseUrl = Required("PLEASANTER_BASE_URL"),
        ApiKey = Required("PLEASANTER_API_KEY"),
        ServerVersion = Environment.GetEnvironmentVariable("PLEASANTER_SERVER_VERSION") is { Length: > 0 } version
            ? Version.Parse(version)
            : null,
        AllowInsecureHttp = string.Equals(Environment.GetEnvironmentVariable("PLEASANTER_ALLOW_INSECURE_HTTP"), "true", StringComparison.OrdinalIgnoreCase),
    };

    private static string Required(string name) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException($"環境変数 {name} が設定されていません。");
}

/// <summary>固定した Pleasanter 接続設定を返す。</summary>
public sealed class StaticPleasanterOptionsProvider(PleasanterOptions options)
    : IPleasanterOptionsProvider
{
    /// <inheritdoc />
    public Task<PleasanterOptions> GetAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(options);
}
