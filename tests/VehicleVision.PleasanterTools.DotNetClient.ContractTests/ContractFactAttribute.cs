namespace VehicleVision.PleasanterTools.DotNetClient.ContractTests;

/// <summary>
/// 実機の Pleasanter に当てる契約テスト。環境変数 <c>PLEASANTER_CONTRACT_TESTS=1</c> のときだけ動き、
/// 通常の <c>dotnet test</c> では読み飛ばす（既定のビルド・CI を遅くしない）。
/// </summary>
/// <remarks>実行は <c>bash tests/contract/run.sh</c>（_documents/契約テスト.md）。</remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ContractFactAttribute : FactAttribute
{
    public const string OptInVariable = "PLEASANTER_CONTRACT_TESTS";

    public ContractFactAttribute()
    {
        if (!ContractEnvironment.Enabled)
        {
            Skip = $"実機の契約テストは {OptInVariable}=1 のときだけ動く（bash tests/contract/run.sh）";
        }
    }
}
