# VehicleVision.PleasanterTools.DotNetClient

Pleasanter の標準 Web API を呼ぶ、**.NET 向けのクライアントライブラリ**です（NuGet パッケージ `VehicleVision.PleasanterTools.DotNetClient`）。

> **開発中です。** API ごとのメソッドとモデルは、Pleasanter 1.4.0.0〜1.5.9.0 のソースから抽出した API 面の表をもとに生成しています。
> **実機の Pleasanter では、まだ確かめていません**（単体テストは通信のフェイクだけです）。範囲と方針は [アーキテクチャ方針](_documents/アーキテクチャ方針.md) にあります。

## 使い方

```csharp
using VehicleVision.PleasanterTools.DotNetClient;
using VehicleVision.PleasanterTools.DotNetClient.Models;

var options = new PleasanterOptions
{
    BaseUrl = "https://pleasanter.example.com",
    ApiKey = Environment.GetEnvironmentVariable("PLEASANTER_API_KEY")!,
};
var client = new PleasanterApiClient(new HttpClient(), options);

// レコードを作る（記録テーブルなら IssueApiModel、期限付きテーブルなら ResultApiModel など）
var created = await client.Items.CreateAsync(siteId, new IssueApiModel { Title = "件名", ClassA = "値" });

// レコードを取り出す。Response の中身は型付けしていないので、読む型を選ぶ
var list = await client.Items.GetAsync(siteId, new Api { View = new View { ColumnFilterHash = new() { ["Status"] = "[100]" } } });
var records = await client.DeserializeAsync<List<IssueApiModel>>(list.Value?.Response?.Data);
```

API キーは、環境変数や Git 管理外の設定から読んでください。コードや設定ファイルへ書かないでください。

- 結果は `PleasanterResponse` で返し、失敗は例外にせず `ErrorKind` で分類します（クライアントは再試行しません）
- 生成の対象外の口や、モデルに無い項目は `client.PostJsonAsync(path, body)` で呼べます

## Pleasanter の版

接続先の版で使えない機能（後の版で増えたエンドポイントやメンバー）を呼ぶと、送る前に `PleasanterVersionNotSupportedException` になります。

- 版は、最初の呼び出しの前に `api/utility/GetLicenseInfo` へ問い合わせて取ります。**取れるのは Pleasanter 1.4.21.0 以降**です
- 1.4.0.0〜1.4.20.0 では、`PleasanterOptions.ServerVersion` に版を書いてください（例: `ServerVersion = new Version(1, 4, 6, 0)`）
- 版が分からないときは、既定では拒否せずに送ります（`UnknownServerVersion = UnknownServerVersionBehavior.Reject` で止められます）
- 名前が変わった項目（添付ファイルの `Extension`。1.4.7.0 より前は `Extention`）は、新しい名前に統一し、旧い版へは旧い名前で送ります

詳しくは [クライアントの生成と版の判定](_documents/クライアントの生成と版の判定.md) にあります。

## 方針

- **Pleasanter 本体のコードは取り込みません。** 本体は AGPL v3 で、取り込むと LGPL・商用ライセンスで提供できなくなります。生成に使うのは、ソースから抽出した識別子・型名・パス・版という事実だけです
- クライアントは再試行しません。応答を分類して返し、再送するかは呼び出し側が決めます

## ライセンス

**3 層**で提供します。詳細は [LICENSING.md](LICENSING.md)。

| | ライセンス | 想定する利用者・利用 |
| --- | --- | --- |
| A | [LGPL-3.0-or-later](LICENSE)（LGPL が参照する GPL は [COPYING](COPYING)） | 個人・非営利団体・教育機関が、営利事業に使わない場合 |
| B | [AGPL-3.0-or-later](LICENSE-AGPL) | 営利事業に使う場合（企業・営利団体・個人の営利利用・公的機関）に、AGPL の義務を守って利用するとき |
| C | [株式会社ピー・エム・シー ITソリューション事業部の商用ライセンス](LICENSE-COMMERCIAL.md) | 営利事業に使う場合（B と同じ）に、AGPL の義務を負わずに利用したいとき（個別契約） |

SPDX 表記: `LGPL-3.0-or-later OR AGPL-3.0-or-later OR LicenseRef-PMC-Commercial`

A は、法人の種類ではなく利用の目的で決めます（個人の仕事での利用や公的機関は A ではありません）。判断の目安と、迷うときの問い合わせは [LICENSING.md](LICENSING.md) を参照してください。
**Pleasanter 本体（AGPL v3）を取り込まないことが、B・C の提供の前提です。** 依存も寛容ライセンスに限っています。
依存のライセンスは [NOTICE](NOTICE) にあります。開発環境と運用は [開発ガイド](_documents/開発ガイド.md) と [開発運用](_documents/開発運用.md) にあります。
貢献を取り込む前に [CLA](CLA.md) への署名が必要です（3 層で提供するため）。手続きは [貢献のしかた](CONTRIBUTING.md) を参照してください。
