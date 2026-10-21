using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>Pleasanter の標準 API を叩くクライアント。</summary>
/// <remarks>
/// <para>API ごとのメソッドは、<c>api-surface/history.json</c> から生成している（<see cref="Items"/>・<see cref="Users"/> など）。
/// 生成の規則と、版で使える機能の判定は <c>_documents/クライアントの生成と版の判定.md</c> にある。</para>
/// <para>**API キーはリクエストボディに載せる。ヘッダ方式は無い**
/// （Pleasanter 本体の <c>Libraries/Requests/Context.cs</c>）。例外は <c>Binaries/upload</c> で、<c>Authorization: Bearer</c> で送る。</para>
/// <para>**このクラスは再試行しない。** 失敗の扱いは呼び出し側が決める。</para>
/// <para>**実機の Pleasanter では確かめていない。** 単体テストは通信のフェイクだけで行っている。</para>
/// </remarks>
public sealed partial class PleasanterApiClient
{
    /// <summary>版を問い合わせるエンドポイント（1.4.13.0〜。<c>Response.Version</c> は 1.4.21.0〜）。</summary>
    internal const string VersionPath = "api/utility/GetLicenseInfo";

    private readonly HttpClient _httpClient;
    private readonly IPleasanterOptionsProvider _optionsProvider;

    // 接続先（BaseUrl）ごとの版。値が null は「問い合わせたが分からなかった」
    private readonly ConcurrentDictionary<string, Version?> _detectedVersions = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>接続設定を呼び出しのたびに解決して作る。</summary>
    /// <param name="httpClient">通信に使う <see cref="HttpClient"/>。</param>
    /// <param name="optionsProvider">接続設定。</param>
    public PleasanterApiClient(HttpClient httpClient, IPleasanterOptionsProvider optionsProvider)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(optionsProvider);
        _httpClient = httpClient;
        _optionsProvider = optionsProvider;
    }

    /// <summary>固定の接続設定で作る。</summary>
    /// <param name="httpClient">通信に使う <see cref="HttpClient"/>。</param>
    /// <param name="options">接続設定。</param>
    public PleasanterApiClient(HttpClient httpClient, PleasanterOptions options)
        : this(httpClient, new StaticPleasanterOptionsProvider(options))
    {
    }

    /// <summary>接続先へ到達し、API キーが拒否されないことを確かめる。</summary>
    /// <param name="cancellationToken">取り消し。</param>
    public async Task<bool> CheckConnectionAsync(CancellationToken cancellationToken = default)
    {
        var response = await PostJsonAsync("api/users/Get", null, cancellationToken).ConfigureAwait(false);
        return response.ErrorKind is not (PleasanterErrorKind.Unauthorized or PleasanterErrorKind.Unknown)
            && response.StatusCode is not (int)HttpStatusCode.NotFound;
    }

    /// <summary>
    /// 接続先の版を返す。<see cref="PleasanterOptions.ServerVersion"/> があればそれ、
    /// 無ければ <c>api/utility/GetLicenseInfo</c> へ問い合わせた結果（接続先ごとに覚える）。分からなければ <c>null</c>。
    /// </summary>
    /// <remarks>版が取れるのは Pleasanter 1.4.21.0 以降。それより前は <c>null</c> になる。</remarks>
    /// <param name="cancellationToken">取り消し。</param>
    public async Task<Version?> GetServerVersionAsync(CancellationToken cancellationToken = default)
    {
        var options = await _optionsProvider.GetAsync(cancellationToken).ConfigureAwait(false);
        return await ResolveServerVersionAsync(options, refresh: false, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>覚えている版を捨て、もう一度問い合わせる。</summary>
    /// <param name="cancellationToken">取り消し。</param>
    public async Task<Version?> RefreshServerVersionAsync(CancellationToken cancellationToken = default)
    {
        var options = await _optionsProvider.GetAsync(cancellationToken).ConfigureAwait(false);
        return await ResolveServerVersionAsync(options, refresh: true, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 型付けしていない呼び出し。<paramref name="body"/> に <c>ApiKey</c>・<c>ApiVersion</c> を入れて POST する。
    /// 生成の対象外の口や、モデルに無い項目を送るときに使う。版の判定はしない。
    /// </summary>
    /// <param name="path">ベース URL からの相対パス（例: <c>api/items/1/get</c>）。</param>
    /// <param name="body">本文。<c>null</c> なら <c>ApiKey</c>・<c>ApiVersion</c> だけ。</param>
    /// <param name="cancellationToken">取り消し。</param>
    public async Task<PleasanterResponse> PostJsonAsync(string path, JsonObject? body, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        var options = await _optionsProvider.GetAsync(cancellationToken).ConfigureAwait(false);
        var payload = body?.DeepClone().AsObject() ?? [];
        payload["ApiKey"] = options.ApiKey;
        payload["ApiVersion"] = options.ApiVersion;
        var (response, raw) = await SendAsync(options, path, null, () => JsonContent(payload), bearer: false, cancellationToken).ConfigureAwait(false);
        return ToResponse(response, raw);
    }

    /// <summary>
    /// 型付けしていない値（<see cref="JsonElement"/>）を、利用者が選んだ型へ読む。接続先の版に合わせて読み替える（名前の変更・列挙の数値）。
    /// </summary>
    /// <typeparam name="T">読む型（例: <c>List&lt;IssueApiModel&gt;</c>）。</typeparam>
    /// <param name="element">値。</param>
    /// <param name="cancellationToken">取り消し（版の問い合わせに使う）。</param>
    public async Task<T?> DeserializeAsync<T>(JsonElement? element, CancellationToken cancellationToken = default)
    {
        if (element is not { } e)
        {
            return default;
        }

        var options = await _optionsProvider.GetAsync(cancellationToken).ConfigureAwait(false);
        var version = await ResolveServerVersionAsync(options, refresh: false, cancellationToken).ConfigureAwait(false);
        return e.Deserialize<T>(PleasanterJson.For(version, options.UnknownServerVersion));
    }

    internal static PleasanterErrorKind Classify(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.OK => PleasanterErrorKind.None,
        HttpStatusCode.Unauthorized => PleasanterErrorKind.Unauthorized,
        HttpStatusCode.BadRequest or HttpStatusCode.Forbidden or HttpStatusCode.NotFound
            => PleasanterErrorKind.Permanent,
        HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests
            => PleasanterErrorKind.Transient,
        _ => (int)statusCode >= 500 ? PleasanterErrorKind.Transient : PleasanterErrorKind.Permanent,
    };

    /// <summary>JSON の本文で呼び、型付けしない応答を返す（生成したメソッドが使う）。</summary>
    internal async Task<PleasanterResponse> SendJsonAsync(
        PleasanterEndpoint endpoint,
        string path,
        object?[] bodies,
        IReadOnlyList<KeyValuePair<string, object?>>? query,
        CancellationToken cancellationToken)
    {
        var (response, raw, _) = await SendJsonCoreAsync(endpoint, path, bodies, query, cancellationToken).ConfigureAwait(false);
        return ToResponse(response, raw);
    }

    /// <summary>JSON の本文で呼び、応答を <typeparamref name="T"/> へ読む（生成したメソッドが使う）。</summary>
    internal async Task<PleasanterResponse<T>> SendJsonAsync<T>(
        PleasanterEndpoint endpoint,
        string path,
        object?[] bodies,
        IReadOnlyList<KeyValuePair<string, object?>>? query,
        CancellationToken cancellationToken)
        where T : class
    {
        var (response, raw, json) = await SendJsonCoreAsync(endpoint, path, bodies, query, cancellationToken).ConfigureAwait(false);
        return ToTyped<T>(ToResponse(response, raw), json);
    }

    /// <summary>JSON の本文で呼び、ファイルを受け取る（生成したメソッドが使う）。</summary>
    internal async Task<PleasanterBinaryResponse> SendForBinaryAsync(
        PleasanterEndpoint endpoint,
        string path,
        object?[] bodies,
        CancellationToken cancellationToken)
    {
        var (options, _, json) = await PrepareAsync(endpoint, cancellationToken).ConfigureAwait(false);
        var payload = BuildBody(options, bodies, json);
        var (response, raw) = await SendAsync(options, path, null, () => JsonContent(payload), bearer: false, cancellationToken, readBytes: true).ConfigureAwait(false);
        if (response is null)
        {
            return new PleasanterBinaryResponse(PleasanterErrorKind.Unknown);
        }

        var kind = Classify(response.Value.Status);
        var bytes = raw.Bytes;
        // ファイルが無い・認証に失敗したときは、中身の無い応答になる（実機では確かめていない）
        if (kind is PleasanterErrorKind.None && (bytes is null || bytes.Length == 0))
        {
            kind = PleasanterErrorKind.Permanent;
        }

        return new PleasanterBinaryResponse(
            kind,
            (int)response.Value.Status,
            kind is PleasanterErrorKind.None ? bytes : null,
            response.Value.ContentType,
            response.Value.FileName);
    }

    /// <summary>フォームの <c>parameters</c> に JSON、ファイルを添付して呼ぶ（<c>Import</c>。生成したメソッドが使う）。</summary>
    internal async Task<PleasanterResponse> SendImportAsync(
        PleasanterEndpoint endpoint,
        string path,
        object?[] bodies,
        PleasanterFile file,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        var (options, _, json) = await PrepareAsync(endpoint, cancellationToken).ConfigureAwait(false);
        var payload = BuildBody(options, bodies, json);
        var (response, raw) = await SendAsync(
            options,
            path,
            null,
            () =>
            {
                var form = new MultipartFormDataContent
                {
                    { new StringContent(payload.ToJsonString(), Encoding.UTF8), "parameters" },
                    { FileContent(file), "file", file.FileName },
                };
                return form;
            },
            bearer: false,
            cancellationToken).ConfigureAwait(false);
        return ToResponse(response, raw);
    }

    /// <summary>
    /// <c>Authorization: Bearer</c> で、ファイルとフォームの値を送る（<c>Binaries/upload</c>。生成したメソッドが使う）。
    /// </summary>
    internal async Task<PleasanterResponse> SendUploadAsync(
        PleasanterEndpoint endpoint,
        string path,
        PleasanterFile file,
        IReadOnlyList<KeyValuePair<string, object?>>? query,
        IReadOnlyList<KeyValuePair<string, object?>>? forms,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        var (options, _, _) = await PrepareAsync(endpoint, cancellationToken).ConfigureAwait(false);
        var (response, raw) = await SendAsync(
            options,
            path,
            query,
            () =>
            {
                var form = new MultipartFormDataContent { { FileContent(file), "file", file.FileName } };
                foreach (var (name, value) in forms ?? [])
                {
                    if (value is not null)
                    {
                        form.Add(new StringContent(Format(value), Encoding.UTF8), name);
                    }
                }

                return form;
            },
            bearer: true,
            cancellationToken).ConfigureAwait(false);
        return ToResponse(response, raw);
    }

    /// <summary>パスの引数を URL に入れる形にする。</summary>
    internal static string Segment(object value) => Uri.EscapeDataString(Format(value));

    private static string Format(object value) => value switch
    {
        bool b => b ? "true" : "false",
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString() ?? string.Empty,
    };

    private static StringContent JsonContent(JsonObject payload)
        => new(payload.ToJsonString(), Encoding.UTF8, "application/json");

    private static StreamContent FileContent(PleasanterFile file)
    {
        var content = new StreamContent(new NonClosingStream(file.Content));
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType ?? "application/octet-stream");
        return content;
    }

    private static JsonObject BuildBody(PleasanterOptions options, object?[] bodies, JsonSerializerOptions json)
    {
        // Pleasanter は同じ本文を複数の型で読むので、型ごとの JSON を 1 つに重ねる（後のものが勝つ）
        var payload = new JsonObject();
        foreach (var body in bodies)
        {
            if (body is null)
            {
                continue;
            }

            if (JsonSerializer.SerializeToNode(body, body.GetType(), json) is JsonObject node)
            {
                foreach (var (name, value) in node.ToList())
                {
                    node.Remove(name);
                    payload[name] = value;
                }
            }
        }

        payload["ApiKey"] = options.ApiKey;
        payload["ApiVersion"] = options.ApiVersion;
        return payload;
    }

    private static PleasanterResponse<T> ToTyped<T>(PleasanterResponse response, JsonSerializerOptions json)
        where T : class
    {
        T? value = null;
        if (response.IsSuccess && response.Body is JsonObject)
        {
            try
            {
                value = response.Body.Deserialize<T>(json);
            }
            catch (JsonException)
            {
                // 形が違う（生成した型と合わない）。Body から読めるので、値は無しにする
            }
        }

        return new PleasanterResponse<T>(response.ErrorKind, response.StatusCode, response.Id, response.Message, response.Body, value);
    }

    private static PleasanterResponse ToResponse(ResponseHead? response, RawBody raw)
    {
        if (response is null)
        {
            return new PleasanterResponse(PleasanterErrorKind.Unknown);
        }

        JsonNode? node = null;
        try
        {
            node = raw.Text is { Length: > 0 } text ? JsonNode.Parse(text) : null;
        }
        catch (JsonException)
        {
            // Pleasanter は JSON を返す。返らないなら経路の問題とみなす
        }

        return new PleasanterResponse(
            Classify(response.Value.Status),
            (int)response.Value.Status,
            ReadLong(node, "Id"),
            ReadString(node, "Message"),
            node);
    }

    private static long? ReadLong(JsonNode? node, string name)
        => node is JsonObject o && o[name] is JsonValue v && v.TryGetValue<long>(out var value) ? value : null;

    private static string? ReadString(JsonNode? node, string name)
        => node is JsonObject o && o[name] is JsonValue v && v.TryGetValue<string>(out var value) ? value : null;

    private async Task<(ResponseHead? Response, RawBody Raw, JsonSerializerOptions Json)> SendJsonCoreAsync(
        PleasanterEndpoint endpoint,
        string path,
        object?[] bodies,
        IReadOnlyList<KeyValuePair<string, object?>>? query,
        CancellationToken cancellationToken)
    {
        var (options, _, json) = await PrepareAsync(endpoint, cancellationToken).ConfigureAwait(false);
        // 版で使えないメンバーがあれば、ここ（送る前）で例外になる
        var payload = BuildBody(options, bodies, json);
        var (response, raw) = await SendAsync(options, path, query, () => JsonContent(payload), bearer: false, cancellationToken).ConfigureAwait(false);
        return (response, raw, json);
    }

    private async Task<(PleasanterOptions Options, Version? Version, JsonSerializerOptions Json)> PrepareAsync(
        PleasanterEndpoint endpoint,
        CancellationToken cancellationToken)
    {
        var options = await _optionsProvider.GetAsync(cancellationToken).ConfigureAwait(false);
        var version = await ResolveServerVersionAsync(options, refresh: false, cancellationToken).ConfigureAwait(false);
        VersionGuard.Ensure(endpoint.Name, endpoint.Since, endpoint.Until, version, options.UnknownServerVersion);
        return (options, version, PleasanterJson.For(version, options.UnknownServerVersion));
    }

    private async Task<Version?> ResolveServerVersionAsync(PleasanterOptions options, bool refresh, CancellationToken cancellationToken)
    {
        if (options.ServerVersion is { } configured)
        {
            return VersionGuard.Normalize(configured);
        }

        if (!options.DetectServerVersion)
        {
            return null;
        }

        var key = options.BaseUrl.TrimEnd('/');
        if (!refresh && _detectedVersions.TryGetValue(key, out var known))
        {
            return known;
        }

        var payload = new JsonObject
        {
            ["ApiKey"] = options.ApiKey,
            ["ApiVersion"] = options.ApiVersion,
        };
        var (response, raw) = await SendAsync(options, VersionPath, null, () => JsonContent(payload), bearer: false, cancellationToken).ConfigureAwait(false);
        if (response is null)
        {
            // 応答が返らなかった。分からないまま、覚えずに次の呼び出しで試し直す
            return null;
        }

        Version? detected = null;
        try
        {
            if (response.Value.Status == HttpStatusCode.OK
                && raw.Text is { Length: > 0 } text
                && JsonNode.Parse(text) is JsonObject root
                && root["Response"] is JsonObject inner
                && inner["Version"] is JsonValue v
                && v.TryGetValue<string>(out var s)
                && Version.TryParse(s, out var parsed))
            {
                detected = VersionGuard.Normalize(parsed);
            }
        }
        catch (JsonException)
        {
            // JSON でなければ分からない
        }

        _detectedVersions[key] = detected;
        return detected;
    }

    private async Task<(ResponseHead? Response, RawBody Raw)> SendAsync(
        PleasanterOptions options,
        string path,
        IReadOnlyList<KeyValuePair<string, object?>>? query,
        Func<HttpContent> content,
        bool bearer,
        CancellationToken cancellationToken,
        bool readBytes = false)
    {
        var url = new StringBuilder(options.BaseUrl.TrimEnd('/')).Append('/').Append(path.TrimStart('/'));
        var separator = '?';
        foreach (var (name, value) in query ?? [])
        {
            if (value is null)
            {
                continue;
            }

            url.Append(separator).Append(Uri.EscapeDataString(name)).Append('=').Append(Uri.EscapeDataString(Format(value)));
            separator = '&';
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, url.ToString()) { Content = content() };
        if (bearer)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);
        }

        HttpResponseMessage response;
        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(options.Timeout);
            response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseContentRead, timeout.Token)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // 送れたのか届いたのか分からない。照合してから判断する
            return (null, default);
        }

        using (response)
        {
            var head = new ResponseHead(
                response.StatusCode,
                response.Content.Headers.ContentType?.ToString(),
                response.Content.Headers.ContentDisposition?.FileNameStar ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"'));
            if (readBytes)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
                return (head, new RawBody(null, bytes));
            }

            var text = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return (head, new RawBody(text, null));
        }
    }

    private readonly record struct ResponseHead(HttpStatusCode Status, string? ContentType, string? FileName);

    private readonly record struct RawBody(string? Text, byte[]? Bytes);

    /// <summary>送るあいだにファイルを閉じない（閉じるのは呼び出し側）。</summary>
    private sealed class NonClosingStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => false;

        public override long Length => inner.Length;

        public override long Position
        {
            get => inner.Position;
            set => inner.Position = value;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => inner.ReadAsync(buffer, cancellationToken);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => inner.ReadAsync(buffer, offset, count, cancellationToken);

        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}

/// <summary>生成したメソッドが持つ、エンドポイントの名前と版の範囲。</summary>
/// <param name="Name">名前（例: <c>Items.GetAsync</c>）。例外の文言に使う。</param>
/// <param name="Since">在る最初の版。</param>
/// <param name="Until">無くなった版。</param>
internal sealed record PleasanterEndpoint(string Name, Version Since, Version? Until)
{
    /// <summary>文字列の版から作る。</summary>
    public PleasanterEndpoint(string name, string since, string? until)
        : this(name, Version.Parse(since), until is null ? null : Version.Parse(until))
    {
    }
}
