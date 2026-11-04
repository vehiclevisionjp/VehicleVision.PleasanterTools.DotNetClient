using System.Collections.Concurrent;

namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>
/// 接続先（<c>BaseUrl</c>）ごとの、問い合わせた版の記憶。既定の <see cref="Shared"/> はプロセスで共通。
/// </summary>
/// <remarks>
/// 結論が出た結果（版が取れた・版が無い・404）は覚え続ける（<c>RefreshServerVersionAsync</c> で更新）。
/// 取れなかった結果（到達不能・認証失敗・5xx など）は、短い間だけ覚え、その間は問い合わせない。
/// </remarks>
internal sealed class ServerVersionCache(TimeProvider? timeProvider = null)
{
    private readonly TimeProvider _time = timeProvider ?? TimeProvider.System;
    private readonly ConcurrentDictionary<string, Entry> _entries = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _gates = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>プロセスで共通の記憶。</summary>
    public static ServerVersionCache Shared { get; } = new();

    /// <summary>覚えている結果があれば <c>true</c>（結論が出ているか、取れなかった結果の期限内）。</summary>
    public bool TryGet(string key, out Version? version)
    {
        if (_entries.TryGetValue(key, out var entry)
            && (entry.Conclusive || _time.GetUtcNow() < entry.RetryAt))
        {
            version = entry.Version;
            return true;
        }

        version = null;
        return false;
    }

    /// <summary>結論が出た結果を覚える（<paramref name="version"/> が <c>null</c> は「版が無い」）。</summary>
    public void SetConclusive(string key, Version? version)
        => _entries[key] = new Entry(version, Conclusive: true, default);

    /// <summary>
    /// 取れなかったことを <paramref name="retryInterval"/> のあいだ覚える。返す値は、そのときの版（<c>null</c> か、残した版）。
    /// </summary>
    /// <param name="key">接続先。</param>
    /// <param name="retryInterval">この間は問い合わせない。<see cref="TimeSpan.Zero"/> 以下なら覚えない。</param>
    /// <param name="keepConclusive">覚えていた結論（版）があれば、それを残して返す。</param>
    public Version? SetFailure(string key, TimeSpan retryInterval, bool keepConclusive)
    {
        if (keepConclusive && _entries.TryGetValue(key, out var existing) && existing.Conclusive)
        {
            return existing.Version;
        }

        if (retryInterval > TimeSpan.Zero)
        {
            _entries[key] = new Entry(null, Conclusive: false, _time.GetUtcNow() + retryInterval);
        }
        else
        {
            _entries.TryRemove(key, out _);
        }

        return null;
    }

    /// <summary>接続先ごとの、同時に 1 つだけ問い合わせるための排他。</summary>
    public SemaphoreSlim GateFor(string key) => _gates.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

    private readonly record struct Entry(Version? Version, bool Conclusive, DateTimeOffset RetryAt);
}
