namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>Pleasanter のその版から在る機能（メソッド・型・メンバー・列挙値）。</summary>
/// <remarks>
/// <para>版は <c>api-surface/history.json</c>（ソースからの抽出）による。実機では確かめていない。</para>
/// <para>**<c>1.4.0.0</c> は「1.4.0.0 以前から在る」**という意味（抽出の対象の最初の版のため）。</para>
/// </remarks>
[AttributeUsage(
    AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false)]
public sealed class SinceAttribute : Attribute
{
    /// <summary>版を指定する。</summary>
    /// <param name="version">4 つの数の版（例: <c>1.4.13.0</c>）。</param>
    public SinceAttribute(string version) => Version = System.Version.Parse(version);

    /// <summary>その版から在る。</summary>
    public Version Version { get; }
}

/// <summary>Pleasanter のその版からは無い機能（排他。その版では使えない）。</summary>
/// <remarks>版は <c>api-surface/history.json</c>（ソースからの抽出）による。実機では確かめていない。</remarks>
[AttributeUsage(
    AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false)]
public sealed class UntilAttribute : Attribute
{
    /// <summary>版を指定する。</summary>
    /// <param name="version">4 つの数の版（例: <c>1.5.6.0</c>）。</param>
    public UntilAttribute(string version) => Version = System.Version.Parse(version);

    /// <summary>その版からは無い。</summary>
    public Version Version { get; }
}

/// <summary>JSON の名前が、ある版で変わったメンバー。モデルは新しい名前に統一し、旧い版へは旧い名前で送る。</summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class RenamedFromAttribute : Attribute
{
    /// <summary>旧い名前と、変わった版を指定する。</summary>
    /// <param name="oldName">旧い JSON の名前。</param>
    /// <param name="version">新しい名前になった版。この版より前は <paramref name="oldName"/>。</param>
    public RenamedFromAttribute(string oldName, string version)
    {
        OldName = oldName;
        Version = System.Version.Parse(version);
    }

    /// <summary>旧い JSON の名前。</summary>
    public string OldName { get; }

    /// <summary>新しい名前になった版。</summary>
    public Version Version { get; }
}

/// <summary>列挙値の数値が、ある版で変わったことを示す。その版より前は <see cref="Value"/> だった。</summary>
/// <remarks>読むとき（サーバーが数値で返したとき）に、サーバーの版に合わせて読み替えるために使う。</remarks>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class ValueBeforeAttribute : Attribute
{
    /// <summary>版と、その版より前の数値を指定する。</summary>
    /// <param name="version">数値が変わった版。</param>
    /// <param name="value">その版より前の数値。</param>
    public ValueBeforeAttribute(string version, long value)
    {
        Version = System.Version.Parse(version);
        Value = value;
    }

    /// <summary>数値が変わった版。</summary>
    public Version Version { get; }

    /// <summary>その版より前の数値。</summary>
    public long Value { get; }
}
