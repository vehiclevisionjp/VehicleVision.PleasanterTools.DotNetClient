using VehicleVision.PleasanterTools.Core.Contracts;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>
/// <see cref="PleasanterOptions.FromEnvironment()"/> の環境変数（Issue #116）。
/// 新名は <c>VVPT_PLEASANTER_*</c>。旧名（<c>PLEASANTER_*</c>）は、新名との併用も含めて例外にする。
/// 環境変数そのものは書き換えず、取得を差し替える。
/// </summary>
public sealed class FromEnvironmentTests
{
    private const string Secret = "secret-key-do-not-leak";

    private static readonly (string Name, string Value)[] RequiredVariables =
    [
        (PleasanterEnvironmentNames.BaseUrl, "https://pleasanter.example.com"),
        (PleasanterEnvironmentNames.ApiKey, Secret),
    ];

    private static Func<string, string?> Env(params (string Name, string Value)[] variables)
    {
        var map = variables.ToDictionary(v => v.Name, v => v.Value, StringComparer.Ordinal);
        return name => map.GetValueOrDefault(name);
    }

    private static InvalidOperationException Throws(Func<string, string?> env)
        => Assert.Throws<InvalidOperationException>(() => PleasanterOptions.FromEnvironment(env));

    private static void AssertAscii(string message)
    {
        Assert.All(message, c => Assert.True(c is >= ' ' and <= '~', $"ASCII 以外の文字 U+{(int)c:X4}"));
    }

    [Fact]
    public void 新名だけで作れる()
    {
        var options = PleasanterOptions.FromEnvironment(Env(RequiredVariables));

        Assert.Equal("https://pleasanter.example.com", options.BaseUrl);
        Assert.Equal(Secret, options.ApiKey);
        Assert.Null(options.ServerVersion);
        Assert.False(options.AllowInsecureHttp);
    }

    [Fact]
    public void 新名の_ServerVersion_を読む()
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, (PleasanterEnvironmentNames.ServerVersion, "1.4.6.0")]));

        Assert.Equal(new Version(1, 4, 6, 0), options.ServerVersion);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("TRUE", true)]
    [InlineData("tRuE", true)]
    [InlineData("false", false)]
    [InlineData("1", false)]
    [InlineData("yes", false)]
    [InlineData(" true", false)]
    [InlineData("", false)]
    public void AllowInsecureHttp_は_true_だけが真(string value, bool expected)
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, (PleasanterEnvironmentNames.AllowInsecureHttp, value)]));

        Assert.Equal(expected, options.AllowInsecureHttp);
    }

    [Fact]
    public void 空の_ServerVersion_は未設定として扱う()
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, (PleasanterEnvironmentNames.ServerVersion, "")]));

        Assert.Null(options.ServerVersion);
    }

    [Theory]
    [InlineData(PleasanterEnvironmentNames.BaseUrl)]
    [InlineData(PleasanterEnvironmentNames.ApiKey)]
    public void 必須の変数が無ければ英語_ASCII_の例外(string missing)
    {
        var ex = Throws(Env(RequiredVariables.Where(v => v.Name != missing).ToArray()));

        Assert.Equal($"Environment variable {missing} is not set.", ex.Message);
        AssertAscii(ex.Message);
    }

    [Theory]
    [InlineData(PleasanterEnvironmentNames.BaseUrl)]
    [InlineData(PleasanterEnvironmentNames.ApiKey)]
    public void 必須の変数が空なら未設定として扱う(string empty)
    {
        var ex = Throws(Env(RequiredVariables.Select(v => v.Name == empty ? (v.Name, "") : v).ToArray()));

        Assert.Equal($"Environment variable {empty} is not set.", ex.Message);
    }

    [Fact]
    public void 何も無ければ_BaseUrl_が先に報告される()
    {
        var ex = Throws(Env());

        Assert.Equal($"Environment variable {PleasanterEnvironmentNames.BaseUrl} is not set.", ex.Message);
    }

    [Theory]
    [InlineData("PLEASANTER_BASE_URL", PleasanterEnvironmentNames.BaseUrl)]
    [InlineData("PLEASANTER_API_KEY", PleasanterEnvironmentNames.ApiKey)]
    [InlineData("PLEASANTER_SERVER_VERSION", PleasanterEnvironmentNames.ServerVersion)]
    [InlineData("PLEASANTER_ALLOW_INSECURE_HTTP", PleasanterEnvironmentNames.AllowInsecureHttp)]
    public void 旧名だけが設定されていれば例外で新名を案内する(string legacy, string replacement)
    {
        var ex = Throws(Env((legacy, Secret)));

        Assert.Equal($"Environment variable {legacy} is no longer supported. Use {replacement}.", ex.Message);
        AssertAscii(ex.Message);
        Assert.DoesNotContain(Secret, ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("PLEASANTER_BASE_URL", PleasanterEnvironmentNames.BaseUrl)]
    [InlineData("PLEASANTER_API_KEY", PleasanterEnvironmentNames.ApiKey)]
    [InlineData("PLEASANTER_SERVER_VERSION", PleasanterEnvironmentNames.ServerVersion)]
    [InlineData("PLEASANTER_ALLOW_INSECURE_HTTP", PleasanterEnvironmentNames.AllowInsecureHttp)]
    public void 旧名は新名と併用されていても例外にし値として使わない(string legacy, string replacement)
    {
        var ex = Throws(Env([.. RequiredVariables, (PleasanterEnvironmentNames.ServerVersion, "1.4.6.0"), (legacy, "https://legacy.example.com")]));

        Assert.Equal($"Environment variable {legacy} is no longer supported. Use {replacement}.", ex.Message);
        Assert.DoesNotContain(Secret, ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void 設定されている旧名をすべて報告する()
    {
        var ex = Throws(Env(
            ("PLEASANTER_API_KEY", Secret),
            ("PLEASANTER_BASE_URL", "https://legacy.example.com"),
            ("PLEASANTER_ALLOW_INSECURE_HTTP", "true")));

        Assert.Contains($"Environment variable PLEASANTER_BASE_URL is no longer supported. Use {PleasanterEnvironmentNames.BaseUrl}.", ex.Message, StringComparison.Ordinal);
        Assert.Contains($"Environment variable PLEASANTER_API_KEY is no longer supported. Use {PleasanterEnvironmentNames.ApiKey}.", ex.Message, StringComparison.Ordinal);
        Assert.Contains($"Environment variable PLEASANTER_ALLOW_INSECURE_HTTP is no longer supported. Use {PleasanterEnvironmentNames.AllowInsecureHttp}.", ex.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("PLEASANTER_SERVER_VERSION", ex.Message, StringComparison.Ordinal);
        AssertAscii(ex.Message);
        Assert.DoesNotContain(Secret, ex.Message, StringComparison.Ordinal);
        Assert.DoesNotContain("legacy.example.com", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void 空の旧名は未設定として扱う()
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, ("PLEASANTER_BASE_URL", ""), ("PLEASANTER_API_KEY", "")]));

        Assert.Equal("https://pleasanter.example.com", options.BaseUrl);
    }

    [Theory]
    [InlineData("PLEASANTER_API_KEY_FILE")]
    [InlineData("FEEDINGEST_PLEASANTER_BASE_URL")]
    [InlineData("FEEDINGEST_PLEASANTER_API_KEY")]
    [InlineData("FEEDINGEST_PLEASANTER_API_KEY_FILE")]
    [InlineData("CALENDARSTYLE_PLEASANTER_API_KEY")]
    [InlineData("PLEASANTER_CONTRACT_TESTS")]
    [InlineData("PLEASANTER_REFERENCE_DIR")]
    [InlineData("PLEASANTER_EXPECTED_VERSION")]
    public void この型が読まない変数の旧名や開発用の変数は検出しない(string other)
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, (other, "x")]));

        Assert.Equal(Secret, options.ApiKey);
    }

    [Fact]
    public void 旧名の検出対象は_Legacy_のうち_PLEASANTER_接頭辞で_API_KEY_FILE_以外のもの()
    {
        // 対応表の増減で検出の漏れ・過剰が起きたら気づけるよう、対象を固定する。
        var targets = PleasanterEnvironmentNames.Legacy
            .Where(pair => pair.Key.StartsWith("PLEASANTER_", StringComparison.Ordinal) && pair.Value != PleasanterEnvironmentNames.ApiKeyFile)
            .Select(pair => pair.Key)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(
            ["PLEASANTER_ALLOW_INSECURE_HTTP", "PLEASANTER_API_KEY", "PLEASANTER_BASE_URL", "PLEASANTER_SERVER_VERSION"],
            targets);
    }
}
