# 調査メモ: Pleasanter の API がソースのどこで定義されているか

Issue #18（フェーズ 1）の調査の記録。API 面を機械的に抽出するツール（[API 面の抽出](API面の抽出.md)）の前提になる。

- 出典: <https://github.com/Implem/Implem.Pleasanter> のタグ `Pleasanter_1.4.0.0`・`Pleasanter_1.4.23.3`・`Pleasanter_1.5.9.0` を中心に、`Pleasanter_1.4.0.0`〜`Pleasanter_1.5.9.0` の全 66 タグ
- 参照日: 2026-10-21
- 読んだのはソースだけで、**実機の Pleasanter では確かめていない**。パスは `Implem.Pleasanter/` からの相対で書く（`Implem.Libraries/` などは別に書く）

## 結論

- **API の骨格は 1.4.0.0 から 1.5.9.0 まで同じ構造**。API を定義するファイルの場所・名前の変更は無い。変化は「コントローラ・アクション・モデルのメンバーの追加」と「属性の追加」が中心
- そのため、1 つの抽出の規則で全タグを扱える（タグごとに規則を変える必要は無かった）

## 定義の場所

| 対象 | 場所 | 形 |
| --- | --- | --- |
| API のコントローラ | `Controllers/Api/*Controller.cs` | 属性ルーティング。クラスに `[Route("api/[controller]")]`、アクションは全部 `[HttpPost("…")]`。`OutgoingMailsController` だけ `[Route("api")]` と `{reference}/{id}/{controller}/Send` |
| 旧来の API のコントローラ | `Controllers/Api_{Depts,Groups,Items,Users}Controller.cs` | 属性ルーティングが無い。`Startup.cs` の `MapControllerRoute`（規約ルーティング）で届く。**どの URL で届くかは確かめていない** |
| 規約ルーティング | `Startup.cs` の `MapControllerRoute` | `Default`・`Others`・`Item`・`Binaries`・`BinariesUpload`、1.4.23.0 から `FormBinaries` |
| SCIM | `Controllers/Scim/*.cs`（1.5.8.0〜） | `scim/v2/Users` などで GET・POST・PUT・PATCH・DELETE。認証は `CheckScimContextAttributes` |
| MCP サーバ | `MCP/`（1.5.2.0〜）と `Startup.cs` の `MapMcp` | 設定 `McpServer.Enabled` のときだけ。MCP の規格の別の口で、今回の抽出の対象外 |
| API 共通の確認 | `Filters/CheckApiContextAttributes.cs` | コントローラに付く属性。IP 制限・不正な JSON・Token の確認などを行う |
| リクエストの共通部分 | `Libraries/Requests/Api.cs` | `ApiVersion`・`ApiKey`・`View`・`Offset`・`PageSize`（1.4.13.0〜）・`TableType`・`Token`・`SsCache`（1.5.6.0〜）など |
| リクエストの個別の型 | `Libraries/Requests/{ApiDeleteOption,ExportApi,ExtendedApi,ImportApi,RecordSelector,SessionApi}.cs`、`Models/*/*ApiModel.cs`、`Models/Shared/_BaseApiModel.cs`、`Models/Sites/ApiSiteSettings/*.cs` | 普通のクラス（自動実装プロパティ・公開フィールド・初期化子・継承・入れ子の列挙型） |
| 本文の解釈 | `Models/*/*Utilities.cs`・`Models/Items/ItemModel.cs` などの `…ByApi` | `context.RequestDataString.Deserialize<T>()`。`ItemModel` はレコードの種類（Sites・Issues・Results・Wikis）で、各 `…Utilities` へ振り分ける |
| API キー | `Libraries/Requests/Context.cs` | 本文を `Api` として読み、`ApiKey` を取る（[アーキテクチャ方針](アーキテクチャ方針.md) の決定と同じ出典）。`Binaries` の `upload` だけは `Authorization: Bearer` ヘッダから取る（`Controllers/Api/BinariesController.cs`） |
| 応答 | `Libraries/Responses/ApiResults.cs`・`ApiResponse.cs` | 一覧は `StatusCode`・`LimitPerDate`・`LimitRemaining`・`Response`（中身は匿名型。`Offset`・`PageSize`・`TotalCount`・`Data` など）。成功・失敗は `ApiResponse`（`Id`・`StatusCode`・`LimitPerDate`・`LimitRemaining`・`Message`） |
| API の既定値 | `App_Data/Parameters/Api.json`（型は `Implem.ParameterAccessor/Parts/Api.cs`） | `Version` 1.1・`Enabled`・`PageSize` 200・`LimitPerSite`・`Compatibility_1_3_12`。**全 66 タグで同じ内容** |
| 型の一部 | `Implem.Libraries/`（`Sqls.TableTypes` など） | API のモデルが参照する列挙型などがある |

## 認証の形

- API のコントローラは、ASP.NET の認証を `[AllowAnonymous]` で素通しにし、アクションの中で `context.Authenticated` を確かめて、だめなら `ApiResults.Unauthorized` を返す
- `Extensions` は 1.4.16.0 から `context.HasPrivilege`（特権ユーザー）も確かめる
- `Demo` の `Register` は認証を確かめず、設定（`Parameters.Service.DemoApi` など）で有効かを決める
- レート制限（`[EnableRateLimiting("Api")]`・`"ApiHeavy"`）は 1.5.6.0 から。`Tenants` には付いていない（1.5.9.0 時点）

## 版で増えたもの（主なもの）

詳細は [API 面の版差](../api-surface/history.md)。

| 版 | 内容 |
| --- | --- |
| 1.4.5.0 | `api/items/{id}/GetClosestSiteId` |
| 1.4.6.0 | `api/items/{id}/BulkUpsert` と `IssueBulkUpsertApiModel`・`ResultBulkUpsertApiModel` |
| 1.4.7.0 | 添付の `Attachment` のフィールド `Extention` が `Extension` に変わる（名前の変更） |
| 1.4.13.0 | `api/extensions/*`・`api/utility/GetLicenseInfo`、`Api` に `PageSize` |
| 1.5.6.0 | レート制限の属性、`Api` に `SsCache` |
| 1.5.7.0 | `api/tenants/*`（`Create`・`Get`・`{id}/Delete`・`{id}/Suspend`・`{id}/Resume`） |
| 1.5.8.0 | SCIM（`scim/v2/*`）、`UserApiModel` に `UiType` |
| 1.5.9.0 | `api/items/{id}/MoveSite` |

## 抽出するうえで難しい点

- **応答の多くが匿名型**（`new { Offset = …, Data = … }`）で、メンバーの型は構文から分からない。メンバー名だけを取る
- **リクエストの型は、コントローラでは分からない。** 処理を `…ByApi` の呼び出しで辿り、`RequestDataString.Deserialize<T>()` を集める必要がある。`ItemModel` は種類で振り分けるため、どの種類の型かは実行時に決まる
- 意味解析（コンパイル）はできない（Pleasanter のビルドを持ち込まない）ので、名前の解決は構文・名前空間・using からの推定になる
