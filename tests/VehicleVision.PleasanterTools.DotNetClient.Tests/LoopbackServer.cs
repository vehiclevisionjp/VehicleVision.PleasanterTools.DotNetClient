using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>
/// ループバックで待ち受ける最小の HTTP サーバー（テスト用）。リダイレクト先へ本文が届くかを、実際の通信で確かめるために使う。
/// 1 接続 1 リクエストで閉じる（Connection: close）。
/// </summary>
internal sealed class LoopbackServer : IDisposable
{
    private readonly TcpListener _listener = new(IPAddress.Loopback, 0);
    private readonly Func<ReceivedRequest, string> _respond;
    private readonly CancellationTokenSource _stop = new();
    private readonly Task _loop;
    private readonly List<ReceivedRequest> _requests = [];

    public LoopbackServer(Func<ReceivedRequest, string>? respond = null)
    {
        _respond = respond ?? (_ => Ok("""{"Id":0,"StatusCode":200,"Message":"ok"}"""));
        _listener.Start();
        _loop = Task.Run(AcceptLoopAsync);
    }

    public int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

    public string BaseUrl => $"http://127.0.0.1:{Port}";

    /// <summary>受け取ったリクエスト（呼び出した時点の写し）。</summary>
    public IReadOnlyList<ReceivedRequest> Requests
    {
        get
        {
            lock (_requests)
            {
                return [.. _requests];
            }
        }
    }

    public static string Ok(string json)
        => $"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: {Encoding.UTF8.GetByteCount(json)}\r\nConnection: close\r\n\r\n{json}";

    public static string Redirect(int status, string location)
        => $"HTTP/1.1 {status} Redirect\r\nLocation: {location}\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";

    public void Dispose()
    {
        _stop.Cancel();
        _listener.Stop();
        try
        {
            _loop.Wait(TimeSpan.FromSeconds(5));
        }
        catch (AggregateException)
        {
            // 停止時の中断
        }

        _stop.Dispose();
    }

    private static async Task<ReceivedRequest> ReadRequestAsync(NetworkStream stream)
    {
        var buffer = new List<byte>();
        var one = new byte[1];
        // ヘッダの終わり（空行）まで読む
        while (!EndsWithBlankLine(buffer))
        {
            if (await stream.ReadAsync(one) == 0)
            {
                break;
            }

            buffer.Add(one[0]);
        }

        var head = Encoding.ASCII.GetString([.. buffer]);
        var lines = head.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        var first = lines[0].Split(' ');
        var headers = lines.Skip(1)
            .Select(l => l.Split(':', 2))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0].Trim(), p => p[1].Trim(), StringComparer.OrdinalIgnoreCase);
        var length = headers.TryGetValue("Content-Length", out var l) ? int.Parse(l, CultureInfo.InvariantCulture) : 0;
        var body = new byte[length];
        var read = 0;
        while (read < length)
        {
            var n = await stream.ReadAsync(body.AsMemory(read));
            if (n == 0)
            {
                break;
            }

            read += n;
        }

        return new ReceivedRequest(first[0], first[1], headers, Encoding.UTF8.GetString(body, 0, read));
    }

    private static bool EndsWithBlankLine(List<byte> b)
        => b.Count >= 4 && b[^4] == '\r' && b[^3] == '\n' && b[^2] == '\r' && b[^1] == '\n';

    private async Task AcceptLoopAsync()
    {
        while (!_stop.IsCancellationRequested)
        {
            TcpClient client;
            try
            {
                client = await _listener.AcceptTcpClientAsync(_stop.Token);
            }
            catch (Exception ex) when (ex is OperationCanceledException or ObjectDisposedException or SocketException)
            {
                return;
            }

            _ = Task.Run(() => HandleAsync(client));
        }
    }

    private async Task HandleAsync(TcpClient client)
    {
        using (client)
        {
            try
            {
                var stream = client.GetStream();
                var request = await ReadRequestAsync(stream);
                lock (_requests)
                {
                    _requests.Add(request);
                }

                var bytes = Encoding.UTF8.GetBytes(_respond(request));
                await stream.WriteAsync(bytes);
                await stream.FlushAsync();
            }
            catch (IOException)
            {
                // 相手が先に閉じた
            }
        }
    }
}

/// <summary>サーバーが受け取ったリクエスト。</summary>
internal sealed record ReceivedRequest(string Method, string Target, Dictionary<string, string> Headers, string Body);
