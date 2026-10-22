using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleVision.PleasanterTools.DotNetClient;

/// <summary>生成した列挙を、名前で書き、名前と数値の両方から読む。</summary>
internal sealed class PleasanterEnumConverterFactory(Version? serverVersion, UnknownServerVersionBehavior unknown)
    : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsEnum && typeToConvert.Assembly == typeof(PleasanterEnumConverterFactory).Assembly;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (JsonConverter)Activator.CreateInstance(
            typeof(PleasanterEnumConverter<>).MakeGenericType(typeToConvert),
            serverVersion,
            unknown)!;
}

/// <summary>
/// 列挙を名前で書く（数値は版で変わることがあるが、名前は変わっていないため）。
/// 読むときは、名前（大文字小文字を問わない）と数値を受ける。数値は、<see cref="ValueBeforeAttribute"/> に従い、サーバーの版に合わせて読み替える。
/// </summary>
/// <typeparam name="T">列挙の型。</typeparam>
internal sealed class PleasanterEnumConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    private static readonly bool IsFlags = typeof(T).IsDefined(typeof(FlagsAttribute), inherit: false);

    private readonly Version? _serverVersion;
    private readonly UnknownServerVersionBehavior _unknown;
    private readonly Dictionary<long, T> _byNumber = [];
    private readonly Dictionary<long, (string Name, Version? Since, Version? Until)> _fields = [];

    public PleasanterEnumConverter(Version? serverVersion, UnknownServerVersionBehavior unknown)
    {
        _serverVersion = serverVersion;
        _unknown = unknown;
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (T)field.GetValue(null)!;
            var current = Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
            _fields[current] = (
                field.Name,
                field.GetCustomAttribute<SinceAttribute>()?.Version,
                field.GetCustomAttribute<UntilAttribute>()?.Version);
            _byNumber[NumberFor(field, current)] = value;
        }
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                var text = reader.GetString();
                if (Enum.TryParse<T>(text, ignoreCase: true, out var parsed))
                {
                    return parsed;
                }

                throw new JsonException($"{typeof(T).Name} に {text} はありません。");
            case JsonTokenType.Number when reader.TryGetInt64(out var number):
                return _byNumber.TryGetValue(number, out var mapped)
                    ? mapped
                    : (T)Enum.ToObject(typeof(T), number);
            default:
                throw new JsonException($"{typeof(T).Name} を {reader.TokenType} から読めません。");
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var number = Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture);
        foreach (var (field, info) in _fields)
        {
            // 組み合わせ（Flags）なら、含まれる値ごとに確かめる
            var included = field == number || (IsFlags && field != 0 && (number & field) == field);
            if (included)
            {
                VersionGuard.Ensure($"{typeof(T).Name}.{info.Name}", info.Since, info.Until, _serverVersion, _unknown);
            }
        }

        // 名前の無い値（組み合わせなど）は数値で書く。名前は、定義された値だけ
        if (_fields.ContainsKey(number))
        {
            writer.WriteStringValue(_fields[number].Name);
        }
        else
        {
            writer.WriteNumberValue(number);
        }
    }

    private long NumberFor(FieldInfo field, long current)
    {
        if (_serverVersion is null)
        {
            return current;
        }

        // サーバーの版より後に変わったもののうち、いちばん早い変更の前の数値が、その版での数値
        var before = field.GetCustomAttributes<ValueBeforeAttribute>()
            .Where(a => VersionGuard.Normalize(_serverVersion) < a.Version)
            .OrderBy(a => a.Version)
            .FirstOrDefault();
        return before?.Value ?? current;
    }
}
