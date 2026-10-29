using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>
/// 生成したモデルを、接続先の版に合わせて JSON へ読み書きする設定を作る。
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>名前はメンバー名のまま書き、読むときは大文字小文字を区別しない（Pleasanter は Newtonsoft.Json の既定の設定で読み書きする）。</item>
/// <item>null は書かない（サーバー側の既定値に任せる）。</item>
/// <item>値の入ったメンバーが、接続先の版で使えなければ <see cref="PleasanterVersionNotSupportedException"/> を投げる。</item>
/// <item>名前が変わったメンバー（<see cref="RenamedFromAttribute"/>）は、旧い版へは旧い名前で書く。版が不明なら両方の名前で書く。</item>
/// <item>列挙は名前で書き、名前と数値の両方を読む。</item>
/// </list>
/// </remarks>
internal static class PleasanterJson
{
    private static readonly ConcurrentDictionary<(Version?, UnknownServerVersionBehavior), JsonSerializerOptions> Cache = new();

    /// <summary>版と、版が不明のときの振る舞いに合わせた設定を返す（作った設定は覚えて使い回す）。</summary>
    public static JsonSerializerOptions For(Version? serverVersion, UnknownServerVersionBehavior unknown)
        => Cache.GetOrAdd(
            (serverVersion is null ? null : VersionGuard.Normalize(serverVersion), unknown),
            static key => Create(key.Item1, key.Item2));

    private static JsonSerializerOptions Create(Version? serverVersion, UnknownServerVersionBehavior unknown)
    {
        var resolver = new DefaultJsonTypeInfoResolver();
        resolver.Modifiers.Add(typeInfo => ApplyVersion(typeInfo, serverVersion, unknown));
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = resolver,
        };
        options.Converters.Add(new PleasanterEnumConverterFactory(serverVersion, unknown));
        options.MakeReadOnly();
        return options;
    }

    private static void ApplyVersion(JsonTypeInfo typeInfo, Version? serverVersion, UnknownServerVersionBehavior unknown)
    {
        // 版の属性（Since・Until・RenamedFrom）が付いたメンバーだけを扱う。属性の無い型は何も変わらない
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
        {
            return;
        }

        var aliases = new List<JsonPropertyInfo>();
        foreach (var property in typeInfo.Properties)
        {
            if (property.AttributeProvider is not MemberInfo member)
            {
                continue;
            }

            var since = member.GetCustomAttribute<SinceAttribute>()?.Version;
            var until = member.GetCustomAttribute<UntilAttribute>()?.Version;
            var name = $"{member.DeclaringType?.Name}.{member.Name}";
            if (VersionGuard.Check(name, since, until, serverVersion, unknown) is not null && property.Get is { } get)
            {
                // 値が入っているときだけ止める（null は書かないので、サーバーへは届かない）
                property.Get = obj =>
                {
                    var value = get(obj);
                    if (value is not null)
                    {
                        VersionGuard.Ensure(name, since, until, serverVersion, unknown);
                    }

                    return value;
                };
            }

            if (member.GetCustomAttribute<RenamedFromAttribute>() is { } renamed)
            {
                if (serverVersion is not null)
                {
                    if (serverVersion < renamed.Version)
                    {
                        property.Name = renamed.OldName;
                    }
                }
                else
                {
                    // 版が不明: 旧い名前でも書き・読む（知らない名前はサーバーが無視する）
                    var alias = typeInfo.CreateJsonPropertyInfo(property.PropertyType, renamed.OldName);
                    alias.Get = property.Get;
                    alias.Set = property.Set;
                    aliases.Add(alias);
                }
            }
        }

        foreach (var alias in aliases)
        {
            typeInfo.Properties.Add(alias);
        }
    }
}
