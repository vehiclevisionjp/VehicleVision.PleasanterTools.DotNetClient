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
    /// 版を問わず使えるもの（1.4.0.0 以前から在り、無くなっていないもの）以外は、
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
    /// <remarks>Pleasanter の <c>App_Data/Parameters/Api.json</c> の <c>Version</c> と同じ値（1.4.0.0〜1.5.9.0 で変わっていない）。</remarks>
    public const decimal DefaultApiVersion = 1.1m;

    /// <summary>1 回の呼び出しの上限の既定。</summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary>ベース URL。末尾のスラッシュは付けない。</summary>
    public required string BaseUrl { get; init; }

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
    /// **1.4.0.0〜1.4.20.0 は、API で版を取れない**ので、版で使える機能を判定したいときは指定する
    /// （<c>_documents/クライアントの生成と版の判定.md</c>）。
    /// </remarks>
    public Version? ServerVersion { get; init; }

    /// <summary>
    /// <see cref="ServerVersion"/> が無いとき、最初の呼び出しの前に <c>api/utility/GetLicenseInfo</c> で版を問い合わせるか（既定 <c>true</c>）。
    /// </summary>
    /// <remarks>
    /// 問い合わせは、接続先ごとに 1 回（結果を覚える）。API キーが要る呼び出しで、1.5.6.0 からはレート制限の対象。
    /// 版が取れるのは 1.4.21.0 以降。
    /// </remarks>
    public bool DetectServerVersion { get; init; } = true;

    /// <summary>接続先の版が分からないときの振る舞い（既定 <see cref="UnknownServerVersionBehavior.Attempt"/>）。</summary>
    public UnknownServerVersionBehavior UnknownServerVersion { get; init; } = UnknownServerVersionBehavior.Attempt;

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
