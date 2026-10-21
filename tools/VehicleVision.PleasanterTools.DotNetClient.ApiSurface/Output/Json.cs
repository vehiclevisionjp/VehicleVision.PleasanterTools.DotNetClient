using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Output;

/// <summary>出力の JSON の書式（インデント 2・改行 LF・序数順の並びは呼び出し側でそろえる）。</summary>
internal static class Json
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        WriteIndented = true,
        IndentSize = 2,
        NewLine = "\n",
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // 型名の < > をエスケープせず、そのまま読めるようにする（出力はファイルで、HTML へ埋め込まない）
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options) + "\n";

    public static JsonNode? ToNode<T>(T value) => JsonSerializer.SerializeToNode(value, Options);

    /// <summary>比較用の 1 行の表現。</summary>
    public static string Canonical(JsonNode? node) => node?.ToJsonString(new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) ?? "null";

    public static void Write<T>(string path, T value)
    {
        var dir = Path.GetDirectoryName(Path.GetFullPath(path));
        if (dir is not null)
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(path, Serialize(value), new System.Text.UTF8Encoding(false));
    }
}
