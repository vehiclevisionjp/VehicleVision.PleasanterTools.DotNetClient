using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>送られたリクエストを記録し、決めた応答を返す通信のフェイク。</summary>
internal sealed class FakeHandler : HttpMessageHandler
{
    private readonly Func<RecordedRequest, HttpResponseMessage> _respond;

    public FakeHandler(Func<RecordedRequest, HttpResponseMessage>? respond = null)
        => _respond = respond ?? (_ => Json(HttpStatusCode.OK, """{"Id":0,"StatusCode":200,"Message":"ok"}"""));

    public List<RecordedRequest> Requests { get; } = [];

    public bool ThrowOnSend { get; init; }

    /// <summary>版の問い合わせ（GetLicenseInfo）以外のリクエスト。</summary>
    public IEnumerable<RecordedRequest> ApiRequests => Requests.Where(r => !r.Path.EndsWith("GetLicenseInfo", StringComparison.Ordinal));

    public RecordedRequest Last => ApiRequests.Last();

    public static HttpResponseMessage Json(HttpStatusCode status, string body)
        => new(status) { Content = new StringContent(body, Encoding.UTF8, "application/json") };

    /// <summary>GetLicenseInfo に版を答え、ほかは成功を返す。</summary>
    public static FakeHandler WithServerVersion(string? version, Func<RecordedRequest, HttpResponseMessage>? others = null)
        => new(r => r.Path.EndsWith("GetLicenseInfo", StringComparison.Ordinal)
            ? Json(HttpStatusCode.OK, version is null
                ? """{"StatusCode":200,"Response":{"CommercialLicense":false,"Environment":"x"}}"""
                : $$$"""{"StatusCode":200,"Response":{"CommercialLicense":false,"Version":"{{{version}}}"}}""")
            : others?.Invoke(r) ?? Json(HttpStatusCode.OK, """{"Id":0,"StatusCode":200,"Message":"ok"}"""));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (ThrowOnSend)
        {
            throw new HttpRequestException("stub failure");
        }

        var recorded = new RecordedRequest
        {
            Method = request.Method.Method,
            Uri = request.RequestUri!.ToString(),
            Authorization = request.Headers.Authorization?.ToString(),
            ContentType = request.Content?.Headers.ContentType?.MediaType,
        };
        if (request.Content is MultipartFormDataContent multipart)
        {
            foreach (var part in multipart)
            {
                var name = part.Headers.ContentDisposition?.Name?.Trim('"') ?? string.Empty;
                recorded.Parts[name] = (
                    await part.ReadAsStringAsync(cancellationToken),
                    part.Headers.ContentDisposition?.FileName?.Trim('"'));
            }
        }
        else if (request.Content is not null)
        {
            recorded.Body = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        Requests.Add(recorded);
        return _respond(recorded);
    }
}

/// <summary>記録したリクエスト。</summary>
internal sealed class RecordedRequest
{
    public required string Method { get; init; }

    public required string Uri { get; init; }

    public string Path => new Uri(Uri).AbsolutePath;

    public string? Authorization { get; init; }

    public string? ContentType { get; init; }

    public string? Body { get; set; }

    public Dictionary<string, (string Text, string? FileName)> Parts { get; } = [];

    public JsonObject Json => JsonNode.Parse(Body ?? Parts["parameters"].Text)!.AsObject();
}
