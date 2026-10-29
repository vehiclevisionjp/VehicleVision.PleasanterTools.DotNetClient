using VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Tests;

/// <summary>
/// 抽出のテスト用の、小さな架空のソースツリー。Pleasanter のコードではなく、形（属性・呼び出しの形）だけをまねて書いたもの。
/// </summary>
internal static class Fixture
{
    public static ExtractorOptions Options { get; } = new()
    {
        SourceRoots = ["App", "Lib"],
        ControllersDirectory = "App/Controllers",
        StartupFile = "App/Startup.cs",
        ApiParametersFile = "App/Parameters/Api.json",
    };

    public const string ThingsController = """
        using Sample.App.Models;
        using Sample.App.Responses;
        namespace Sample.App.Controllers.Api
        {
            [AllowAnonymous]
            [ApiController]
            [Route("api/[controller]")]
            [EnableRateLimiting("Api")]
            public class ThingsController : ControllerBase
            {
                [HttpPost("Get")]
                [HttpPost("{id}/Get")]
                public ContentResult Get(int id = 0)
                {
                    using (var reader = new StreamReader(Request.Body)) body = reader.ReadToEnd();
                    var context = new Context(apiRequestBody: body);
                    var result = context.Authenticated
                        ? ThingUtilities.GetByApi(context: context, id: id)
                        : ApiResults.Unauthorized(context: context);
                    return result;
                }

                [HttpPost("{id}/Create")]
                [EnableRateLimiting("ApiHeavy")]
                public ContentResult Create(long id)
                {
                    var context = new Context(apiRequestBody: Header());
                    if (context.QueryStrings.Bool("dryrun")) { }
                    if (Parameters.Service.ThingsEnabled) { }
                    return context.Authenticated && context.HasPrivilege
                        ? new ThingModel(context: context).CreateByApi(context: context)
                        : ApiResults.Unauthorized(context: context);
                }

                [NonAction]
                public void Helper() { }

                private string Header() => (string)Request.Headers["Authorization"];
            }
        }
        """;

    public const string LegacyController = """
        namespace Sample.App.Controllers
        {
            public class Api_ThingsController : Controller
            {
                [HttpPost]
                public ContentResult Get(int id = 0)
                {
                    var context = new Context();
                    return context.Authenticated ? ThingUtilities.GetByApi(context: context, id: id) : null;
                }

                public ContentResult NotAnAction() => null;
            }
        }
        """;

    public const string ScreenController = """
        namespace Sample.App.Controllers
        {
            public class ThingsController : Controller
            {
                [HttpPost]
                public ContentResult Index() => null;
            }
        }
        """;

    public const string Utilities = """
        using Sample.App.Requests;
        using Sample.App.Responses;
        namespace Sample.App.Models
        {
            public static class ThingUtilities
            {
                public static ContentResult GetByApi(Context context, int id)
                {
                    var api = context.RequestDataString.Deserialize<Api>();
                    var list = new ThingModel(context: context).GetByApi(context: context);
                    return ApiResults.Get(
                        statusCode: 200,
                        response: new
                        {
                            Offset = api?.Offset ?? 0,
                            list.TotalCount,
                            Data = list
                        });
                }

                public static ContentResult CreateByApi(Context context)
                {
                    var model = context.RequestDataString.Deserialize<ThingApiModel>();
                    var ignored = context.Other.Deserialize<NotARequest>();
                    return ApiResults.Success(id: 1, message: "ok");
                }
            }

            public class ThingModel
            {
                public ThingModel(Context context) { }

                public ContentResult CreateByApi(Context context) => ThingUtilities.CreateByApi(context: context);

                public ThingApiModel GetByApi(Context context) => null;
            }

            public class NotARequest
            {
                public string Secret { get; set; }
            }
        }
        """;

    public const string Models = """
        using System;
        using System.Collections.Generic;
        using Sample.Lib.Data;
        namespace Sample.App.Models
        {
            [Serializable]
            public class ThingApiModel : BaseApiModel
            {
                public string Title { get; set; }
                public int? Count { get; set; }
                public long Id { get; set; }
                public bool Flag { get; set; } = true;
                public decimal Version { get; set; } = Parameters.Api.Version;
                public Kinds Kind { get; set; } = Kinds.B;
                public List<Detail> Details { get; set; } = new List<Detail>();
                public DetailList More { get; set; }
                public Sqls.TableTypes TableType { get; set; }
                public string Computed => Title;
                public int Number = -1;
                [NonSerialized]
                public Hidden Cache;
                public static string NotAMember { get; set; }
                private string NotPublic { get; set; }

                public enum Kinds
                {
                    A,
                    B = 5,
                    C,
                }
            }

            public class DetailList : List<Detail>
            {
            }

            public class BaseApiModel
            {
                public decimal ApiVersion { get; set; }
            }

            public class Detail
            {
                [JsonProperty("name")]
                public string Name { get; set; }
            }

            public class Hidden
            {
                public string Value { get; set; }
            }
        }
        """;

    public const string Requests = """
        namespace Sample.App.Requests
        {
            public class Api
            {
                public string ApiKey { get; set; }
                public int Offset { get; set; }
            }
        }
        """;

    public const string Responses = """
        namespace Sample.App.Responses
        {
            public class ApiResponse
            {
                public long Id;
                public string Message;
            }

            public static class ApiResults
            {
                public static ContentResult Get(int statusCode, object response) => null;
                public static ContentResult Success(long id, string message) => null;
                public static ContentResult Unauthorized(Context context) => null;
            }
        }
        """;

    public const string Lib = """
        namespace Sample.Lib.Data
        {
            public static class Sqls
            {
                public enum TableTypes
                {
                    Normal,
                    History,
                }
            }
        }
        """;

    public const string Startup = """
        namespace Sample.App
        {
            public class Startup
            {
                public void Configure(IApplicationBuilder app)
                {
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            name: "Item",
                            pattern: "{controller}/{id}/{action}",
                            defaults: new { Controller = "Items", Action = "Edit" },
                            constraints: new { Id = "[0-9]+" });
                    });
                }
            }
        }
        """;

    public const string ApiJson = """
        {
            "Version": 1.1,
            "PageSize": 200
        }
        """;

    public static Dictionary<string, string> Files() => new()
    {
        ["App/Controllers/Api/ThingsController.cs"] = ThingsController,
        ["App/Controllers/Api_ThingsController.cs"] = LegacyController,
        ["App/Controllers/ThingsController.cs"] = ScreenController,
        ["App/Models/ThingUtilities.cs"] = Utilities,
        ["App/Models/ThingApiModel.cs"] = Models,
        ["App/Requests/Api.cs"] = Requests,
        ["App/Responses/ApiResults.cs"] = Responses,
        ["Lib/Data/Sqls.cs"] = Lib,
        ["App/Startup.cs"] = Startup,
        ["App/Parameters/Api.json"] = ApiJson,
        ["Other/Ignored.cs"] = "namespace X { public class Api { } }",
    };

    public static InMemorySourceTree Tree(string label = "v1", Dictionary<string, string>? files = null)
        => new(label, files ?? Files());
}
