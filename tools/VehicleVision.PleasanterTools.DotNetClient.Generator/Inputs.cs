using System.Text.Json;
using System.Text.Json.Nodes;

namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>生成を止める入力の不整合。人の判断が要る（generation.json を直すなど）。</summary>
internal sealed class GenerationException(string message) : Exception(message);

/// <summary>history.json の 1 項目の値の版。</summary>
internal sealed record Revision(string Since, JsonNode? Value);

/// <summary>history.json の 1 項目。</summary>
internal sealed record HistoryItem(string Key, string Category, string Since, string? Until, bool HasIntervals, IReadOnlyList<Revision> Revisions)
{
    public JsonNode Latest => Revisions[^1].Value ?? throw new GenerationException($"{Key} に値がありません。");
}

/// <summary>history.json（スキーマ pleasanter-api-history/1）を読む。</summary>
internal sealed class History
{
    public required IReadOnlyList<string> Tags { get; init; }

    public required IReadOnlyList<HistoryItem> Endpoints { get; init; }

    public required IReadOnlyDictionary<string, HistoryItem> Types { get; init; }

    /// <summary>型の完全名 → メンバー（名前の序数順）。</summary>
    public required IReadOnlyDictionary<string, IReadOnlyList<(string Name, HistoryItem Item)>> Members { get; init; }

    /// <summary>型の完全名 → 列挙値（history.json の並び。値の順ではない）。</summary>
    public required IReadOnlyDictionary<string, IReadOnlyList<(string Name, HistoryItem Item)>> EnumValues { get; init; }

    public string LatestTag => Tags[^1];

    public static History Load(string json)
    {
        var root = JsonNode.Parse(json)?.AsObject() ?? throw new GenerationException("history.json が空です。");
        if ((string?)root["schema"] != "pleasanter-api-history/1")
        {
            throw new GenerationException($"history.json のスキーマ {(string?)root["schema"]} は扱えません。");
        }

        var tags = root["tags"]!.AsArray().Select(t => (string)t!["label"]!).ToList();
        var endpoints = new List<HistoryItem>();
        var types = new SortedDictionary<string, HistoryItem>(StringComparer.Ordinal);
        var members = new SortedDictionary<string, List<(string, HistoryItem)>>(StringComparer.Ordinal);
        var enums = new SortedDictionary<string, List<(string, HistoryItem)>>(StringComparer.Ordinal);
        foreach (var node in root["items"]!.AsArray())
        {
            var item = new HistoryItem(
                (string)node!["key"]!,
                (string)node["category"]!,
                (string)node["since"]!,
                (string?)node["until"],
                node["intervals"] is not null,
                [.. node["revisions"]!.AsArray().Select(r => new Revision((string)r!["since"]!, r["value"]?.DeepClone()))]);
            var body = item.Key[(item.Key.IndexOf(':', StringComparison.Ordinal) + 1)..];
            switch (item.Category)
            {
                case "endpoint":
                    endpoints.Add(item);
                    break;
                case "type":
                    types[body] = item;
                    break;
                case "member":
                case "enum":
                    var hash = body.LastIndexOf('#');
                    var target = item.Category == "member" ? members : enums;
                    var owner = body[..hash];
                    if (!target.TryGetValue(owner, out var list))
                    {
                        target[owner] = list = [];
                    }

                    list.Add((body[(hash + 1)..], item));
                    break;
            }
        }

        return new History
        {
            Tags = tags,
            Endpoints = endpoints,
            Types = types,
            Members = members.ToDictionary(p => p.Key, p => (IReadOnlyList<(string, HistoryItem)>)p.Value, StringComparer.Ordinal),
            EnumValues = enums.ToDictionary(p => p.Key, p => (IReadOnlyList<(string, HistoryItem)>)p.Value, StringComparer.Ordinal),
        };
    }
}

/// <summary>名前の変更（人が対応づけたもの）。</summary>
internal sealed record Rename(string Type, string From, string To, string Version);

/// <summary>generation.json（人が決めること）。</summary>
internal sealed class GenerationConfig
{
    public required string MinimumVersion { get; init; }

    public required IReadOnlyDictionary<string, string> ExcludedAreas { get; init; }

    public required IReadOnlyDictionary<string, string> ExcludedEndpoints { get; init; }

    public required IReadOnlySet<string> RecordTypes { get; init; }

    public required IReadOnlyList<string> ExtraTypes { get; init; }

    public required IReadOnlySet<string> ClientManagedMembers { get; init; }

    public required IReadOnlyList<Rename> Renames { get; init; }

    public static GenerationConfig Load(string json)
    {
        var root = JsonNode.Parse(json, documentOptions: new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip })?.AsObject()
            ?? throw new GenerationException("generation.json が空です。");
        static Dictionary<string, string> Map(JsonNode? node) =>
            node?.AsObject().ToDictionary(p => p.Key, p => (string)p.Value!, StringComparer.Ordinal) ?? [];
        static List<string> List(JsonNode? node) => node?.AsArray().Select(n => (string)n!).ToList() ?? [];
        return new GenerationConfig
        {
            MinimumVersion = (string?)root["minimumVersion"] ?? throw new GenerationException("minimumVersion がありません。"),
            ExcludedAreas = Map(root["excludedAreas"]),
            ExcludedEndpoints = Map(root["excludedEndpoints"]),
            RecordTypes = List(root["recordTypes"]).ToHashSet(StringComparer.Ordinal),
            ExtraTypes = List(root["extraTypes"]),
            ClientManagedMembers = List(root["clientManagedMembers"]).ToHashSet(StringComparer.Ordinal),
            Renames = [.. (root["renames"]?.AsArray() ?? []).Select(r => new Rename(
                (string)r!["type"]!,
                (string)r["from"]!,
                (string)r["to"]!,
                (string)r["version"]!))],
        };
    }
}
