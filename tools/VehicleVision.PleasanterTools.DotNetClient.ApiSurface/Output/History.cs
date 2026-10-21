using System.Text.Json.Nodes;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Output;

/// <summary>
/// 版をまたいだ API 面の履歴。項目ごとに、現れた版（Since）・無くなった版（Until。その版では無い）・中身が変わった版を持つ。
/// revisions を順にたどれば、どの版の値も復元できる。
/// </summary>
internal sealed record HistoryDocument
{
    public string Schema { get; init; } = "pleasanter-api-history/1";

    /// <summary>対象にした版（古い順）。Since・Until はこの並びの中の版を指す。</summary>
    public required IReadOnlyList<HistoryTag> Tags { get; init; }

    public required IReadOnlyList<HistoryItem> Items { get; init; }
}

internal sealed record HistoryTag(string Label, string? Commit, int Endpoints, int Types, int Members);

internal sealed record HistoryItem
{
    /// <summary>endpoint:… / type:… / member:型#名前 / enum:型#名前 / parameter:… / route:…</summary>
    public required string Key { get; init; }

    public required string Category { get; init; }

    /// <summary>最初に現れた版。対象の最初の版なら「その版以前から」の意味。</summary>
    public required string Since { get; init; }

    /// <summary>最後に無くなった版（この版からは無い）。最新の版にあれば null。</summary>
    public string? Until { get; init; }

    /// <summary>途中で無くなって戻った場合だけ、在った区間の一覧（until は排他）。</summary>
    public IReadOnlyList<Interval>? Intervals { get; init; }

    /// <summary>中身が変わった版と、変わった要素の名前。</summary>
    public IReadOnlyList<Change>? Changes { get; init; }

    /// <summary>中身の版ごとの値（since から次の revision の手前まで有効）。</summary>
    public required IReadOnlyList<Revision> Revisions { get; init; }
}

internal sealed record Interval(string Since, string? Until);

internal sealed record Change(string Tag, IReadOnlyList<string> Facets);

internal sealed record Revision(string Since, JsonNode? Value);

internal static class HistoryBuilder
{
    public static HistoryDocument Build(IReadOnlyList<SurfaceDocument> surfaces)
    {
        var states = new SortedDictionary<string, ItemState>(StringComparer.Ordinal);
        var tags = new List<HistoryTag>();
        for (var i = 0; i < surfaces.Count; i++)
        {
            var surface = surfaces[i];
            var label = surface.Source.Label;
            var items = Flatten(surface);
            tags.Add(new HistoryTag(label, surface.Source.Commit, surface.Endpoints.Count, surface.Types.Count, surface.Types.Sum(t => t.Members.Count + t.EnumValues.Count)));
            foreach (var (key, (category, value)) in items)
            {
                if (!states.TryGetValue(key, out var state))
                {
                    states[key] = state = new ItemState(category);
                }
                state.Observe(label, value);
            }
            foreach (var (key, state) in states)
            {
                if (!items.ContainsKey(key))
                {
                    state.Absent(label);
                }
            }
        }
        return new HistoryDocument
        {
            Tags = tags,
            Items = [.. states.Select(kv => kv.Value.ToItem(kv.Key))],
        };
    }

    /// <summary>API 表を、キーごとの「分類と比較用の値」へ平らにする。</summary>
    public static SortedDictionary<string, (string Category, JsonObject Value)> Flatten(SurfaceDocument surface)
    {
        var items = new SortedDictionary<string, (string, JsonObject)>(StringComparer.Ordinal);
        foreach (var e in surface.Endpoints)
        {
            var node = (JsonObject)Json.ToNode(e)!;
            // 内部の実装（辿ったメソッド・ファイルの場所）は、API の変化ではないので比較から外す
            node.Remove("key");
            node.Remove("handlers");
            node.Remove("file");
            items["endpoint:" + e.Key] = ("endpoint", node);
        }
        foreach (var t in surface.Types)
        {
            items["type:" + t.Name] = ("type", new JsonObject
            {
                ["kind"] = t.Kind,
                ["bases"] = Json.ToNode(t.Bases),
                ["attributes"] = Json.ToNode(t.Attributes),
            });
            foreach (var m in t.Members)
            {
                var node = (JsonObject)Json.ToNode(m)!;
                node.Remove("name");
                items[$"member:{t.Name}#{m.Name}"] = ("member", node);
            }
            foreach (var v in t.EnumValues)
            {
                items[$"enum:{t.Name}#{v.Name}"] = ("enum", new JsonObject { ["value"] = v.Value?.DeepClone() });
            }
        }
        if (surface.ApiParameters is { } parameters)
        {
            foreach (var (name, value) in parameters)
            {
                items["parameter:Api." + name] = ("parameter", new JsonObject { ["value"] = value?.DeepClone() });
            }
        }
        foreach (var group in surface.ConventionalRoutes.GroupBy(r => r.Name, StringComparer.Ordinal))
        {
            var i = 0;
            foreach (var r in group)
            {
                var node = (JsonObject)Json.ToNode(r)!;
                node.Remove("name");
                items[group.Count() == 1 ? "route:" + r.Name : $"route:{r.Name}#{++i}"] = ("route", node);
            }
        }
        return items;
    }

    private sealed class ItemState(string category)
    {
        private readonly List<(string Since, string? Until)> _intervals = [];
        private readonly List<Change> _changes = [];
        private readonly List<Revision> _revisions = [];
        private JsonObject? _last;
        private bool _present;

        public void Observe(string tag, JsonObject value)
        {
            if (!_present)
            {
                _intervals.Add((tag, null));
                _present = true;
            }
            if (_last is null)
            {
                _revisions.Add(new Revision(tag, value.DeepClone()));
            }
            else if (Json.Canonical(_last) != Json.Canonical(value))
            {
                var changed = _last.Select(p => p.Key).Union(value.Select(p => p.Key))
                    .Where(k => Json.Canonical(_last[k]) != Json.Canonical(value[k]))
                    .Order(StringComparer.Ordinal)
                    .ToList();
                _changes.Add(new Change(tag, changed));
                _revisions.Add(new Revision(tag, value.DeepClone()));
            }
            _last = value;
        }

        public void Absent(string tag)
        {
            if (_present)
            {
                _intervals[^1] = (_intervals[^1].Since, tag);
                _present = false;
            }
        }

        public HistoryItem ToItem(string key) => new()
        {
            Key = key,
            Category = category,
            Since = _intervals[0].Since,
            Until = _intervals[^1].Until,
            Intervals = _intervals.Count > 1 ? [.. _intervals.Select(i => new Interval(i.Since, i.Until))] : null,
            Changes = _changes.Count > 0 ? _changes : null,
            Revisions = _revisions,
        };
    }
}
