# VehicleVision.PleasanterTools.DotNetClient

Pleasanter の標準 Web API を呼ぶ、**.NET 向けのクライアントライブラリ**です（NuGet パッケージ `VehicleVision.PleasanterTools.DotNetClient`）。

> **開発中です。** API ごとのメソッドとモデルは、Pleasanter 1.3.0.0〜1.5.9.0 のソースから抽出した API 面の表をもとに生成しています。
> **実機の Pleasanter で確かめたのは、最新版（1.5.9.1、2026-11-02）の主要な口だけです**（[契約テスト](_documents/契約テスト.md)）。それ以外の版・口は未確認で、通常の単体テストは通信のフェイクだけです。範囲と方針は [アーキテクチャ方針](_documents/アーキテクチャ方針.md) にあります。

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
- **`Retry-After`**: 応答の HTTP ヘッダ `Retry-After`（429・503 などで付く）を `PleasanterResponse.RetryAfter`（`TimeSpan?`）で読めます。再送するか・いつ送るかは、これまでどおり呼び出し側が決めます
  - 秒指定も HTTP-date も読みます。日時は応答を受け取った時点の現在時刻との差で、負の秒・過去の日時は `TimeSpan.Zero` です
  - ヘッダが無い・読めない値・応答が返らなかった（`Unknown`）ときは `null` です。**上限の丸めはしません**（待たせる上限は呼び出し側で決めます）
  - ステータスには依らず、ヘッダがあれば読みます（3xx・200 でも付いていれば入ります）。応答の本文の `StatusCode` 由来の失敗でも、見るのは HTTP ヘッダだけです。`StatusCode`・`ErrorKind` の意味は変わりません
  - 一覧のような型付きの応答（`PleasanterResponse<T>`）とファイルの応答（`PleasanterBinaryResponse`）にも入ります
- **`BaseUrl` は通信の前に検査します。** API キーが本文に載るため、次を満たさない URL は `ArgumentException` で拒否します（例外の文言に API キーも URL も出しません）
  - `https` であること。`http` はループバック（`localhost`・`127.0.0.1`・`[::1]`）だけ使えます
  - 資格情報（`user:pass@`）・クエリ（`?`）・フラグメント（`#`）を含まないこと（パス `https://example.com/pleasanter` は使えます）
  - **検証用の環境（コンテナ間の通信など）だけ**、`PleasanterOptions.AllowInsecureHttp = true`（環境変数 `PLEASANTER_ALLOW_INSECURE_HTTP=true`）で、ループバック以外の `http` も使えます。既定は無効です。**有効にすると API キーが平文で流れるので、本番では有効にしません。** 資格情報・クエリ・フラグメント・`http`／`https` 以外のスキームは、有効にしても拒否します
- 生成の対象外の口や、モデルに無い項目は `client.PostJsonAsync(path, body)` で呼べます

## HttpClient の作り方（リダイレクトを辿らない）

API キーはリクエストの本文に載ります。**既定の `HttpClient` は 3xx を自動で辿り、307・308 では本文（API キー）をリダイレクト先のホストへそのまま再送します。**
クライアントは渡された `HttpClient` の設定を変えられないので、`PleasanterHttp` で作った辿らない `HttpClient`（`AllowAutoRedirect = false`、Cookie 無し）を使ってください。

```csharp
// 単純な用途: 1 つ作って使い回す
using var http = PleasanterHttp.CreateClient();
var client = new PleasanterApiClient(http, options);
```

IHttpClientFactory（`Microsoft.Extensions.Http`）を使うときは、主ハンドラーを差し替えます。

```csharp
services.AddHttpClient("pleasanter")
    .ConfigurePrimaryHttpMessageHandler(PleasanterHttp.CreateHandler);
services.AddTransient(sp => new PleasanterApiClient(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("pleasanter"),
    sp.GetRequiredService<IPleasanterOptionsProvider>()));
```

（この登録例は、ビルドと単体テストでは確かめていません。`Microsoft.Extensions.Http` は本ライブラリの依存ではありません）

- 3xx を受けたときは再送せず、`StatusCode` が 3xx、`ErrorKind` が `Permanent` の結果で返ります。`BaseUrl` が正しい最終の URL か（https の入口か）を見直してください
- HTTP が 200 でも、応答の本文の `StatusCode` が 200 以外なら、その値で `ErrorKind` を分類して失敗として返します（`IsSuccess` は `false`）。`StatusCode` は HTTP のまま、本文の値は `ApplicationStatusCode` で読めます。本文に `StatusCode` が無い応答は成功のままです
- 別のハンドラーを使う場合も、`HttpClientHandler.AllowAutoRedirect = false`（`SocketsHttpHandler` なら `AllowAutoRedirect = false`）にしてください

## Pleasanter の版

接続先の版で使えない機能（後の版で増えたエンドポイントやメンバー）を呼ぶと、送る前に `PleasanterVersionNotSupportedException` になります。

- 版は、最初の呼び出しの前に `api/utility/GetLicenseInfo` へ問い合わせて取ります。**取れるのは Pleasanter 1.4.21.0 以降**です
- 問い合わせの結果は、接続先（`BaseUrl`）ごとに**プロセスで共通に**覚えます。クライアントを作り直しても（DI の `AddTransient` でも）毎回は問い合わせません。取れなかったとき（到達不能・認証失敗・5xx など）は、`ServerVersionRetryInterval`（既定 1 分）のあいだ問い合わせず、版は不明として扱います。1 回の問い合わせは `ServerVersionTimeout`（既定 10 秒）で打ち切ります
- 問い合わせの通信を避けたいときは、`ServerVersion` を指定するか、`DetectServerVersion = false` にします
- 1.3.x〜1.4.20.0 では、`PleasanterOptions.ServerVersion` に版を書いてください（例: `ServerVersion = new Version(1, 4, 6, 0)`）
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
| C | [株式会社ピー・エム・シー ITソリューション事業部の商用ライセンス](LICENSE-COMMERCIAL.md) | AGPL の義務を負わずに利用したいとき（個別契約。法人に限らず、誰でも契約できます） |

SPDX 表記: `LGPL-3.0-or-later OR AGPL-3.0-or-later OR LicenseRef-PMC-Commercial`

A は、法人の種類ではなく利用の目的で決めます（個人の仕事での利用や公的機関は A ではありません）。判断の目安と、迷うときの問い合わせは [LICENSING.md](LICENSING.md) を参照してください。
**Pleasanter 本体（AGPL v3）を取り込まないことが、B・C の提供の前提です。** 依存も寛容ライセンスに限っています。
依存のライセンスは [NOTICE](NOTICE) にあります。開発環境と運用は [開発ガイド](_documents/開発ガイド.md) と [開発運用](_documents/開発運用.md) にあります。
貢献を取り込む前に [CLA](CLA.md) への署名が必要です（3 層で提供するため）。手続きは [貢献のしかた](CONTRIBUTING.md) を参照してください。
