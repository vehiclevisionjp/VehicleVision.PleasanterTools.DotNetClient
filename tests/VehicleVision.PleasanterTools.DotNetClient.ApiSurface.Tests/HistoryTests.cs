using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Output;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Tests;

public sealed class HistoryTests
{
    // v1: 基本形。v2: Count を long? へ変え、Extra を追加。v3: Extra を削除し、エンドポイントを 1 つ削除。v4: Extra が戻る。
    private static IReadOnlyList<SurfaceDocument> Surfaces()
    {
        var v1 = Fixture.Files();
        var v2 = Fixture.Files();
        v2["App/Models/ThingApiModel.cs"] = Fixture.Models
            .Replace("public int? Count", "public long? Count", StringComparison.Ordinal)
            .Replace("public long Id", "public string Extra { get; set; }\n        public long Id", StringComparison.Ordinal);
        var v3 = Fixture.Files();
        v3["App/Controllers/Api/ThingsController.cs"] = Fixture.ThingsController.Replace("[HttpPost(\"Get\")]", string.Empty, StringComparison.Ordinal);
        v3["App/Models/ThingApiModel.cs"] = Fixture.Models.Replace("public int? Count", "public long? Count", StringComparison.Ordinal);
        var v4 = new Dictionary<string, string>(v2);
        v4["App/Controllers/Api/ThingsController.cs"] = v3["App/Controllers/Api/ThingsController.cs"];
        var extractor = new SurfaceExtractor(Fixture.Options);
        return
        [
            extractor.Extract(Fixture.Tree("P_1.0.0.0", v1)),
            extractor.Extract(Fixture.Tree("P_1.0.1.0", v2)),
            extractor.Extract(Fixture.Tree("P_1.1.0.0", v3)),
            extractor.Extract(Fixture.Tree("P_1.2.0.0", v4)),
        ];
    }

    private static HistoryItem Item(HistoryDocument h, string key) => h.Items.Single(i => i.Key == key);

    [Fact]
    public void 導入_変更_廃止の版を出す()
    {
        var h = HistoryBuilder.Build(Surfaces());

        var count = Item(h, "member:Sample.App.Models.ThingApiModel#Count");
        Assert.Equal("P_1.0.0.0", count.Since);
        Assert.Null(count.Until);
        var change = Assert.Single(count.Changes!);
        Assert.Equal("P_1.0.1.0", change.Tag);
        Assert.Equal(["type"], change.Facets);
        Assert.Equal(["P_1.0.0.0", "P_1.0.1.0"], count.Revisions.Select(r => r.Since));
        Assert.Equal("long?", count.Revisions[1].Value!["type"]!.GetValue<string>());

        var removed = Item(h, "endpoint:POST api/things/get");
        Assert.Equal("P_1.0.0.0", removed.Since);
        Assert.Equal("P_1.1.0.0", removed.Until);
        Assert.Null(removed.Intervals);
    }

    [Fact]
    public void 無くなって戻った項目は区間を持つ()
    {
        var h = HistoryBuilder.Build(Surfaces());

        var extra = Item(h, "member:Sample.App.Models.ThingApiModel#Extra");
        Assert.Equal("P_1.0.1.0", extra.Since);
        Assert.Null(extra.Until);
        Assert.Equal([new Interval("P_1.0.1.0", "P_1.1.0.0"), new Interval("P_1.2.0.0", null)], extra.Intervals);
    }

    [Fact]
    public void 内部の実装の違いは変更として数えない()
    {
        var h = HistoryBuilder.Build(Surfaces());

        // handlers・file・ルートの属性は比較から外している（v3 で同じアクションの別テンプレートが消えても、変更にならない）
        var get = Item(h, "endpoint:POST api/things/{id}/get");
        Assert.Null(get.Changes);
        Assert.False(get.Revisions[0].Value!.AsObject().ContainsKey("handlers"));
        Assert.Equal(4, h.Tags.Count);
        Assert.Equal(4, h.Tags[0].Endpoints);
    }

    [Fact]
    public void 一覧の_Markdown_は表と版の短い名前を含む()
    {
        var md = MarkdownWriter.Write(HistoryBuilder.Build(Surfaces()), "../doc.md");

        Assert.Contains("| `POST api/things/get` | api | `1.0.0.0` | `1.1.0.0` | |", md, StringComparison.Ordinal);
        Assert.Contains("`Sample.App.Models.ThingApiModel#Count` | `1.0.0.0` | | `1.0.1.0`（type） |", md, StringComparison.Ordinal);
        Assert.Contains("`1.0.1.0`〜`1.1.0.0`; `1.2.0.0`〜", md, StringComparison.Ordinal);
        Assert.DoesNotContain("<br>", md, StringComparison.Ordinal);
        Assert.EndsWith("\n", md, StringComparison.Ordinal);
    }
}
