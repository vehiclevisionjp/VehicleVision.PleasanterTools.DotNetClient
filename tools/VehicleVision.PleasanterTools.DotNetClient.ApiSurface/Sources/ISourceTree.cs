namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

/// <summary>
/// 抽出の入力になる、ある版のソースツリー（読み取り専用）。パスは区切りを <c>/</c> にした相対パス。
/// </summary>
internal interface ISourceTree
{
    /// <summary>版の名前（タグ名やディレクトリ名）。</summary>
    string Label { get; }

    /// <summary>コミットの ID。分からなければ null。</summary>
    string? Revision { get; }

    /// <summary>ツリーにあるファイルのパス（並びは序数順）。</summary>
    IReadOnlyList<string> Files { get; }

    /// <summary>内容が同じなら同じになるキー（解析結果のキャッシュに使う）。</summary>
    string ContentKey(string path);

    /// <summary>ファイルを UTF-8 の文字列として読む。無ければ null。</summary>
    string? Read(string path);
}
