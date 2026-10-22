using System.Text;

namespace VehicleVision.PleasanterTools.DotNetClient.Generator;

/// <summary>字下げ付きで C# を書く。改行は LF にそろえる（同じ入力から同じ出力にするため）。</summary>
internal sealed class CodeWriter
{
    private readonly StringBuilder _text = new();
    private int _indent;

    public CodeWriter Line(string line = "")
    {
        // 生成器のソースが CRLF で取り出されても（raw 文字列の改行）、出力は LF にする
        line = line.TrimEnd('\r');
        if (line.Length > 0)
        {
            _text.Append(' ', _indent * 4).Append(line);
        }

        _text.Append('\n');
        return this;
    }

    public CodeWriter Open(string line)
    {
        Line(line);
        Line("{");
        _indent++;
        return this;
    }

    public CodeWriter Close(string suffix = "")
    {
        _indent--;
        Line("}" + suffix);
        return this;
    }

    /// <summary>XML ドキュメントの文字（<c>&lt;</c>・<c>&gt;</c>・<c>&amp;</c>）を逃がす。</summary>
    public static string Xml(string text) => text
        .Replace("&", "&amp;", StringComparison.Ordinal)
        .Replace("<", "&lt;", StringComparison.Ordinal)
        .Replace(">", "&gt;", StringComparison.Ordinal);

    /// <summary>C# の文字列リテラルにする。</summary>
    public static string Literal(string text) => "\"" + text
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("\"", "\\\"", StringComparison.Ordinal) + "\"";

    public override string ToString() => _text.ToString();
}
