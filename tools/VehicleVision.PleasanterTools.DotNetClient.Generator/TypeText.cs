namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>
/// history.json に書かれたとおりの型（例: <c>Dictionary&lt;string,List&lt;int?&gt;&gt;?</c>・<c>int?[]</c>）を読んだ木。
/// </summary>
/// <param name="Name">名前（<c>Permissions.Types</c> のように修飾を含む）。</param>
/// <param name="Arguments">型引数。</param>
/// <param name="Nullable">名前の直後の <c>?</c>。</param>
/// <param name="ArrayRanks">配列の <c>[]</c> の数。</param>
internal sealed record TypeText(string Name, IReadOnlyList<TypeText> Arguments, bool Nullable, int ArrayRanks)
{
    public static TypeText Parse(string text)
    {
        var position = 0;
        var result = ParseType(text.Replace(" ", string.Empty, StringComparison.Ordinal), ref position);
        if (position != text.Replace(" ", string.Empty, StringComparison.Ordinal).Length)
        {
            throw new GenerationException($"型 {text} を読めません。");
        }

        return result;
    }

    /// <summary><c>?</c> を除いた形（版ごとの型の違いが、null 可否だけかを比べるため）。</summary>
    public string Shape() => Name
        + (Arguments.Count > 0 ? "<" + string.Join(",", Arguments.Select(a => a.Shape())) + ">" : string.Empty)
        + string.Concat(Enumerable.Repeat("[]", ArrayRanks));

    private static TypeText ParseType(string s, ref int i)
    {
        var start = i;
        while (i < s.Length && (char.IsLetterOrDigit(s[i]) || s[i] is '_' or '.'))
        {
            i++;
        }

        if (i == start)
        {
            throw new GenerationException($"型 {s} の {i} 文字目を読めません。");
        }

        var name = s[start..i];
        var args = new List<TypeText>();
        if (i < s.Length && s[i] == '<')
        {
            i++;
            while (true)
            {
                args.Add(ParseType(s, ref i));
                if (s[i] == ',')
                {
                    i++;
                    continue;
                }

                if (s[i] == '>')
                {
                    i++;
                    break;
                }

                throw new GenerationException($"型 {s} の {i} 文字目を読めません。");
            }
        }

        var nullable = false;
        var ranks = 0;
        while (i < s.Length)
        {
            if (s[i] == '?')
            {
                // 配列の後ろの ? は配列そのものの null 可否。生成ではいちばん外側を必ず null 可にするので、要素の ? だけを残す
                nullable |= ranks == 0;
                i++;
            }
            else if (s[i] == '[' && i + 1 < s.Length && s[i + 1] == ']')
            {
                ranks++;
                i += 2;
            }
            else
            {
                break;
            }
        }

        return new TypeText(name, args, nullable, ranks);
    }
}
