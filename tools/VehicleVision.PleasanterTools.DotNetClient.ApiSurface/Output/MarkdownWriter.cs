using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Output;

/// <summary>履歴を、人が読む一覧（Markdown）にする。日時は入れない（同じ入力なら同じ出力にするため）。</summary>
internal static class MarkdownWriter
{
    private static readonly (string Category, string Title)[] ChangeSections =
    [
        ("type", "型"),
        ("member", "メンバー（プロパティ・フィールド）"),
        ("enum", "列挙値"),
        ("parameter", "API の既定値（Parameters/Api.json）"),
        ("route", "規約ルーティング（Startup の MapControllerRoute）"),
    ];

    public static string Write(HistoryDocument history, string documentLink)
    {
        var tags = history.Tags;
        var sb = new StringBuilder();
        sb.Append("# Pleasanter API 面の版差\n\n");
        sb.Append("<!-- このファイルは tools/VehicleVision.PleasanterTools.DotNetClient.ApiSurface が生成する。手で編集しない。 -->\n\n");
        sb.Append($"手順・読み方・限界は [API 面の抽出]({documentLink}) にある。機械可読の版は `history.json`、最新の版の全体は `latest.json`。\n\n");
        sb.Append($"- 対象: {tags.Count} 版（`{tags[0].Label}` 〜 `{tags[^1].Label}`）\n");
        sb.Append($"- **Since** が `{Short(tags[0].Label)}` のものは「その版以前から在る」の意味。**Until** は「その版から無い」の意味\n");
        sb.Append("- Pleasanter のソースからの機械抽出であり、**実機の Pleasanter では確かめていない**\n\n");

        sb.Append("## 版ごとの件数\n\n");
        sb.Append("| 版 | エンドポイント | 型 | メンバー・列挙値 |\n| --- | --- | --- | --- |\n");
        foreach (var t in tags)
        {
            sb.Append(Row($"`{Short(t.Label)}`", $"{t.Endpoints}", $"{t.Types}", $"{t.Members}"));
        }
        sb.Append('\n');

        sb.Append("## エンドポイント\n\n");
        sb.Append("変更の列は、変わった版と、変わった要素（`history.json` の revisions で値を確かめる）。\n\n");
        sb.Append("| エンドポイント | 領域 | Since | Until | 変更 |\n| --- | --- | --- | --- | --- |\n");
        foreach (var item in history.Items.Where(i => i.Category == "endpoint"))
        {
            var area = item.Revisions[^1].Value?["area"]?.GetValue<string>() ?? "?";
            sb.Append(Row($"`{item.Key["endpoint:".Length..]}`", area, Since(item), Until(item), Changes(item)));
        }
        sb.Append('\n');

        sb.Append("## 途中の版で増減・変化した項目\n\n");
        sb.Append($"全版（`{Short(tags[0].Label)}` 〜 `{Short(tags[^1].Label)}`）で在り、中身も変わらない項目は省き、件数だけを末尾に書く。\n\n");
        foreach (var (category, title) in ChangeSections)
        {
            var changed = history.Items.Where(i => i.Category == category && IsInteresting(i, tags[0].Label)).ToList();
            sb.Append($"### {title}\n\n");
            if (changed.Count == 0)
            {
                sb.Append("なし。\n\n");
                continue;
            }
            sb.Append("| 項目 | Since | Until | 変更 |\n| --- | --- | --- | --- |\n");
            foreach (var item in changed)
            {
                sb.Append(Row($"`{item.Key[(item.Key.IndexOf(':', StringComparison.Ordinal) + 1)..]}`", Since(item), Until(item), Changes(item)));
            }
            sb.Append('\n');
        }

        sb.Append("## 全版で変わらない項目の数\n\n");
        sb.Append("| 分類 | 件数 |\n| --- | --- |\n");
        foreach (var group in history.Items.Where(i => !IsInteresting(i, tags[0].Label)).GroupBy(i => i.Category).OrderBy(g => g.Key, StringComparer.Ordinal))
        {
            sb.Append(Row(group.Key, $"{group.Count()}"));
        }
        return sb.ToString();
    }

    /// <summary>表の 1 行。空のセルは空白 1 つにする（markdownlint の MD060 compact）。</summary>
    private static string Row(params string[] cells)
        => "|" + string.Join("|", cells.Select(c => c.Length == 0 ? " " : $" {c} ")) + "|\n";

    private static bool IsInteresting(HistoryItem item, string firstTag)
        => item.Since != firstTag || item.Until is not null || item.Changes is not null || item.Intervals is not null;

    private static string Since(HistoryItem item)
        => item.Intervals is { } intervals
            ? string.Join("; ", intervals.Select(i => $"`{Short(i.Since)}`〜{(i.Until is null ? string.Empty : $"`{Short(i.Until)}`")}"))
            : $"`{Short(item.Since)}`";

    private static string Until(HistoryItem item) => item.Until is null ? string.Empty : $"`{Short(item.Until)}`";

    private static string Changes(HistoryItem item)
        => item.Changes is null ? string.Empty : string.Join("; ", item.Changes.Select(c => $"`{Short(c.Tag)}`（{string.Join(", ", c.Facets)}）"));

    /// <summary>タグ名の「Pleasanter_」のような接頭辞を除き、版番号だけにする。</summary>
    public static string Short(string label)
    {
        var i = label.LastIndexOf('_');
        return i >= 0 ? label[(i + 1)..] : label;
    }
}
