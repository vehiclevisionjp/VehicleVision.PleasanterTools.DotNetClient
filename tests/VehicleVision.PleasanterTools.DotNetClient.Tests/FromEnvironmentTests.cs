using VehicleVision.PleasanterTools.Core.Contracts;

namespace VehicleVision.PleasanterTools.DotNetClient.Tests;

/// <summary>
/// <see cref="PleasanterOptions.FromEnvironment()"/> の環境変数（Issue #116）。
/// 読むのは新名（<c>VVPT_PLEASANTER_*</c>）だけ。旧名（<c>PLEASANTER_*</c>）は読まない（Issue #124）。
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
    [InlineData("PLEASANTER_BASE_URL")]
    [InlineData("PLEASANTER_API_KEY")]
    [InlineData("PLEASANTER_SERVER_VERSION")]
    [InlineData("PLEASANTER_ALLOW_INSECURE_HTTP")]
    public void 旧名だけが設定されていても新名が未設定として失敗する(string legacy)
    {
        var ex = Throws(Env((legacy, Secret)));

        Assert.Equal("Environment variable VVPT_PLEASANTER_BASE_URL is not set.", ex.Message);
        AssertAscii(ex.Message);
        Assert.DoesNotContain($"variable {legacy} ", ex.Message, StringComparison.Ordinal);
        Assert.DoesNotContain(Secret, ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void 旧名の4つがすべて設定されていても新名の値を使い旧名を値として使わない()
    {
        var options = PleasanterOptions.FromEnvironment(Env(
            [
                .. RequiredVariables,
                (PleasanterEnvironmentNames.ServerVersion, "1.4.6.0"),
                ("PLEASANTER_BASE_URL", "https://legacy.example.com"),
                ("PLEASANTER_API_KEY", "legacy-key"),
                ("PLEASANTER_SERVER_VERSION", "9.9.9.9"),
                ("PLEASANTER_ALLOW_INSECURE_HTTP", "true"),
            ]));

        Assert.Equal("https://pleasanter.example.com", options.BaseUrl);
        Assert.Equal(Secret, options.ApiKey);
        Assert.Equal(new Version(1, 4, 6, 0), options.ServerVersion);
        Assert.False(options.AllowInsecureHttp);
    }

    [Theory]
    [InlineData("PLEASANTER_API_KEY_FILE")]
    [InlineData("FEEDINGEST_PLEASANTER_BASE_URL")]
    [InlineData("FEEDINGEST_PLEASANTER_API_KEY")]
    [InlineData("FEEDINGEST_PLEASANTER_API_KEY_FILE")]
    [InlineData("CALENDARSTYLE_PLEASANTER_API_KEY")]
    [InlineData("PLEASANTER_CONTRACT_TESTS")]
    [InlineData("PLEASANTER_EXPECTED_VERSION")]
    public void この型が読まない変数は無視する(string other)
    {
        var options = PleasanterOptions.FromEnvironment(Env([.. RequiredVariables, (other, "x")]));

        Assert.Equal(Secret, options.ApiKey);
    }
}