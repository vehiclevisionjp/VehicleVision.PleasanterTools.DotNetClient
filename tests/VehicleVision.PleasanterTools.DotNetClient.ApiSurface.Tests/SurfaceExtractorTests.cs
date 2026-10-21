using System.Text.Json.Nodes;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Analysis;
using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Model;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Tests;

public sealed class SurfaceExtractorTests
{
    private static SurfaceDocument Extract() => new SurfaceExtractor(Fixture.Options).Extract(Fixture.Tree());

    private static Endpoint EndpointOf(SurfaceDocument s, string key) => s.Endpoints.Single(e => e.Key == key);

    private static TypeModel TypeOf(SurfaceDocument s, string name) => s.Types.Single(t => t.Name == name);

    [Fact]
    public void 属性ルーティングと旧来の規約ルーティングのエンドポイントを抽出し_画面用は除く()
    {
        var keys = Extract().Endpoints.Select(e => e.Key).ToList();

        Assert.Equal(
            [
                "POST api/things/get",
                "POST api/things/{id}/create",
                "POST api/things/{id}/get",
                "POST legacy:Api_Things/Get",
            ],
            keys);
    }

    [Fact]
    public void パスはテンプレートの綴りのままで_コントローラ名を埋め_ルートの引数を判別する()
    {
        var e = EndpointOf(Extract(), "POST api/things/{id}/get");

        Assert.Equal("api/Things/{id}/Get", e.Path);
        Assert.Equal("api", e.Area);
        Assert.Equal("attribute", e.Routing);
        var p = Assert.Single(e.Parameters);
        Assert.Equal(("id", "int", "route"), (p.Name, p.Type, p.Source));
        Assert.Equal(0m, p.Default!["value"]!.GetValue<decimal>());

        var withoutId = EndpointOf(Extract(), "POST api/things/get");
        Assert.Equal("notInTemplate", Assert.Single(withoutId.Parameters).Source);
    }

    [Fact]
    public void 旧来のコントローラは_パスを推定せず_null_にする()
    {
        var e = EndpointOf(Extract(), "POST legacy:Api_Things/Get");

        Assert.Null(e.Path);
        Assert.Equal("conventional", e.Routing);
        Assert.Equal("legacy", e.Area);
        Assert.True(e.Auth.ChecksAuthenticated);
    }

    [Fact]
    public void 認証の確認とレート制限と入力の手がかりを記録する()
    {
        var s = Extract();
        var get = EndpointOf(s, "POST api/things/{id}/get");
        var create = EndpointOf(s, "POST api/things/{id}/create");

        Assert.Equal(new EndpointAuth(true, false, true, false), get.Auth);
        Assert.Equal(new EndpointAuth(true, false, true, true), create.Auth);
        Assert.Equal("Api", get.RateLimiting);
        Assert.Equal("ApiHeavy", create.RateLimiting);
        Assert.Equal(["Request.Body"], get.Input.Body);
        Assert.Equal(["Authorization"], create.Input.Headers);
        Assert.Equal([new NamedAccess("dryrun", "Bool")], create.Input.QueryStrings);
        Assert.Equal(["Parameters.Service.ThingsEnabled"], create.Gates);
    }

    [Fact]
    public void 処理を辿って_リクエストボディの型と応答の手がかりを集める()
    {
        var s = Extract();
        var get = EndpointOf(s, "POST api/things/{id}/get");
        var create = EndpointOf(s, "POST api/things/{id}/create");

        Assert.Equal(["Sample.App.Requests.Api"], get.RequestTypes);
        Assert.Equal(["Get", "Unauthorized"], get.Response.ApiResults);
        Assert.Equal(["Data", "Offset", "TotalCount"], get.Response.GetMembers);
        Assert.Equal(["Sample.App.Models.ThingApiModel"], get.Response.ModelTypes);
        Assert.Contains("ThingUtilities.GetByApi", get.Handlers);

        // RequestDataString 以外からの Deserialize（NotARequest）は数えない
        Assert.Equal(["Sample.App.Models.ThingApiModel"], create.RequestTypes);
        Assert.Equal(["Success", "Unauthorized"], create.Response.ApiResults);
        Assert.Equal(["ThingModel.CreateByApi", "ThingUtilities.CreateByApi", "ThingsController.Header"], create.Handlers);
    }

    [Fact]
    public void モデルのメンバーの型_null_可否_既定値を抽出する()
    {
        var t = TypeOf(Extract(), "Sample.App.Models.ThingApiModel");
        var m = t.Members.ToDictionary(x => x.Name);

        Assert.Equal(["Sample.App.Models.BaseApiModel"], t.Bases);
        Assert.Equal(["Serializable"], t.Attributes);
        Assert.Equal(
            ["Cache", "Computed", "Count", "Details", "Flag", "Id", "Kind", "Number", "TableType", "Title", "Version"],
            m.Keys);
        Assert.Equal((true, "string"), (m["Title"].Nullable!.Value, m["Title"].Type));
        Assert.Equal((true, "int?"), (m["Count"].Nullable!.Value, m["Count"].Type));
        Assert.False(m["Id"].Nullable);
        Assert.False(m["Kind"].Nullable); // ソース内の enum は値型
        Assert.False(m["TableType"].Nullable);
        Assert.Equal(["Sample.Lib.Data.Sqls.TableTypes"], m["TableType"].TypeRefs);
        Assert.True(m["Flag"].Default!["value"]!.GetValue<bool>());
        Assert.Equal("Parameters.Api.Version", m["Version"].Default!["ref"]!.GetValue<string>());
        Assert.Equal("Kinds.B", m["Kind"].Default!["ref"]!.GetValue<string>());
        Assert.Equal("List<Detail>", m["Details"].Default!["new"]!.GetValue<string>());
        Assert.Equal(-1m, m["Number"].Default!["value"]!.GetValue<decimal>());
        Assert.Equal("field", m["Number"].Kind);
        Assert.True(m["Computed"].ReadOnly);
        Assert.Null(m["Title"].Default);
        Assert.Equal(["NonSerialized"], m["Cache"].Attributes);
    }

    [Fact]
    public void メンバーの型をたどって型の一覧を作り_JSON_に出ないメンバーの型はたどらない()
    {
        var s = Extract();
        var names = s.Types.Select(t => t.Name).ToList();

        Assert.Contains("Sample.App.Models.Detail", names);
        Assert.Contains("Sample.App.Models.BaseApiModel", names);
        Assert.Contains("Sample.Lib.Data.Sqls.TableTypes", names);
        Assert.Contains("Sample.App.Responses.ApiResponse", names); // 応答の外枠（ExtraModelTypes）
        Assert.DoesNotContain("Sample.App.Models.Hidden", names);
        Assert.DoesNotContain("Sample.App.Models.NotARequest", names);
        Assert.DoesNotContain("X.Api", names); // SourceRoots の外

        var kinds = TypeOf(s, "Sample.App.Models.ThingApiModel.Kinds");
        Assert.Equal("enum", kinds.Kind);
        Assert.Equal(["A", "B"], kinds.EnumValues.Select(v => v.Name));
        Assert.Null(kinds.EnumValues[0].Value);
        Assert.Equal(5m, kinds.EnumValues[1].Value!.GetValue<decimal>());
        Assert.Equal(["JsonProperty(\"name\")"], TypeOf(s, "Sample.App.Models.Detail").Members[0].Attributes);
    }

    [Fact]
    public void 規約ルーティングとAPIの既定値を読む()
    {
        var s = Extract();

        var route = Assert.Single(s.ConventionalRoutes);
        Assert.Equal("Item", route.Name);
        Assert.Equal("{controller}/{id}/{action}", route.Pattern);
        Assert.Equal("Edit", route.Defaults["Action"]);
        Assert.Equal("[0-9]+", route.Constraints["Id"]);
        Assert.Equal(1.1m, s.ApiParameters!["Version"]!.GetValue<decimal>());
    }

    [Fact]
    public void 同じ単純名の型は_using_と名前空間で決め_決められなければ曖昧とする()
    {
        var files = new Dictionary<string, string>
        {
            ["App/A.cs"] = "namespace N1 { public class Dup { } } namespace N2 { public class Dup { } }",
            ["App/Use.cs"] = "using N2; namespace N3 { public class User { public Dup Value { get; set; } } }",
            ["App/Ambiguous.cs"] = "using N1; using N2; namespace N4 { public class Other { public Dup Value { get; set; } } }",
        };
        var index = new CodeIndex(Fixture.Tree("v", files), Fixture.Options, new ParseCache());
        index.TypesNamed("User");
        index.TypesNamed("Other");
        var user = index.GetType("N3.User")!.Declarations[0];
        var other = index.GetType("N4.Other")!.Declarations[0];

        var resolved = index.Resolve(["Dup"], 0, CodeIndex.ContextOf(user.Declaration, user.Path));
        var ambiguous = index.Resolve(["Dup"], 0, CodeIndex.ContextOf(other.Declaration, other.Path));

        Assert.Equal("N2.Dup", resolved.Entry!.FullName);
        Assert.Null(ambiguous.Entry);
        Assert.True(ambiguous.Ambiguous);
    }

    [Theory]
    [InlineData("api/[controller]", "{id}/Get", "api/Things/{id}/Get")]
    [InlineData("api/[controller]", null, "api/Things")]
    [InlineData("api", "{reference}/{id}/{controller}/Send", "api/{reference}/{id}/{controller}/Send")]
    [InlineData("api/[controller]", "/root/[action]", "root/Act")]
    [InlineData("", "x/[action]", "x/Act")]
    public void ルートを組み立てる(string classRoute, string? template, string expected)
        => Assert.Equal(expected, EndpointExtractor.Combine(classRoute, template, "Things", "Act"));

    [Fact]
    public void ルートの引数名を取り出す()
        => Assert.Equal(["id", "rest", "x"], EndpointExtractor.RouteParameters("a/{id:int}/{*rest}/{x?}"));

    [Fact]
    public void 同じ入力からは同じ_JSON_が出る()
    {
        var a = Output.Json.Serialize(Extract());
        var b = Output.Json.Serialize(new SurfaceExtractor(Fixture.Options).Extract(Fixture.Tree()));

        Assert.Equal(a, b);
        Assert.DoesNotContain("\r", a, StringComparison.Ordinal);
        Assert.Contains("\"List<Detail>\"", a, StringComparison.Ordinal);
        Assert.NotNull(JsonNode.Parse(a));
    }
}
