# API 面の抽出

Pleasanter の版ごとの API 面（エンドポイント・リクエストと応答のモデル・列挙値・既定値）を、ソースから機械的に抽出し、版の差分表を作る仕組み。Issue #18（フェーズ 1）。
どこを読んでいるかの根拠は [調査メモ: API 定義の場所](調査メモ_API定義の場所.md) にある。

- **ソースからの抽出であり、実機の Pleasanter では確かめていない。** 実機での確認は、フェーズ 3（版ごとの契約テスト）で行う
- 結果: [`api-surface/history.md`](../api-surface/history.md)（人が読む一覧）、`api-surface/history.json`（機械可読）、`api-surface/latest.json`（最新の版の全体）

## Pleasanter のコードの扱い（ライセンス）

- ツールは、Pleasanter のソースを**実行時にデータとして読むだけ**で、プロジェクト参照・コピー・リンクはしない。ビルドにも配布物（NuGet パッケージ）にも入らない（`IsPackable=false`）
- 出力に入れるのは、**識別子・型名・パス・属性名・リテラルの値**という事実だけ。ソースの文章（コメント）やコードの断片は入れない。初期化子がリテラルでも識別子の連なりでもないときは、中身を書かずに `{"unextracted": true}` とする
- Pleasanter のソースは、リポジトリの外の一時ディレクトリへ取得する（`_reference` のサブモジュールは触らない）

## 使い方（再現の手順）

```bash
# 1) ビルド
dotnet build -c Release

TOOL=tools/VehicleVision.PleasanterTools.DotNetClient.ApiSurface/bin/Release/net10.0/VehicleVision.PleasanterTools.DotNetClient.ApiSurface.dll
WORK=<リポジトリの外の短いパス>   # 例: C:/Users/<名前>/AppData/Local/Temp/plref

# 2) タグと、抽出に要るファイルの本体だけを取得する（ネットワークを使う。2026-10-21 時点で 66 タグ・約 13 MB・数十秒）
dotnet $TOOL prepare --repo $WORK --min 1.4.0.0

# 3) 全タグを抽出し、api-surface/ へ書く（ネットワークを使わない。約 3 分）
dotnet $TOOL run --repo $WORK --out-dir api-surface

# 1 つの版だけ（タグ、または展開済みのディレクトリ）
dotnet $TOOL extract --repo $WORK --tag Pleasanter_1.5.9.0 --out <ファイル>
dotnet $TOOL extract --source-dir _reference/Implem.Pleasanter --out <ファイル>
```

- `prepare` は、`git init` した作業リポジトリへ、`Pleasanter_*` のタグ（`--min` 以上）を**履歴 1 件・ファイル本体なし**（`--depth 1 --filter=blob:none`）で取得し、
  `Implem.Pleasanter`・`Implem.Libraries` の `*.cs` と `App_Data/Parameters/Api.json` の本体だけを、ID を指定してまとめて取得する
- `run`・`extract` は `GIT_NO_LAZY_FETCH=1` で読む。本体が足りなければ、ネットワークへ出ずに失敗する（`prepare` を先に実行する）
- `run` に `--snapshots <ディレクトリ>` を付けると、タグごとの全体（1 版あたり約 0.6 MB、66 版で約 40 MB）も書く。リポジトリには入れない
- 同じ入力からは同じ出力になる（日時を入れない・並びを序数順にそろえる）。2 回実行して一致することを確かめている
- Windows の長いパスの都合で、作業リポジトリと `--snapshots` はリポジトリの外の短いパスに置く

## 設計

### 解析の方法: Roslyn の構文解析を選んだ理由

C# の構文解析に **Roslyn（`Microsoft.CodeAnalysis.CSharp`、MIT）** を使う。正規表現にしなかった理由:

- 必要な情報が、属性だけでなく**メソッドの本体の中**にある（`RequestDataString.Deserialize<T>()`・`ApiResults.Get(response: new { … })`・`QueryStrings.Bool("…")`・`context.Authenticated`）。
  1 ファイルが 2 万行を超えるもの（`SiteUtilities.cs`）もあり、波かっこの対応・文字列・コメント・ラムダを正規表現で正しく区切るのは難しい
- 型の書き方（`Dictionary<string, decimal?>`・`Sqls.TableTypes`・入れ子の型・継承・partial）を、構文木なら取り違えずに扱える
- MIT で、依存の方針（寛容ライセンスのみ）に合う。**意味解析（コンパイル）は使わない**（Pleasanter のビルドや参照アセンブリを持ち込まないため）

正規表現は、**解析するファイルを絞るためだけ**に使う（「`class X`」を含むファイル、「`M(`」を含むファイル）。結果は構文木で確かめる。

### 抽出の規則

| 項目 | 規則 |
| --- | --- |
| エンドポイント | `Implem.Pleasanter/Controllers/` の下の `…Controller` クラスの公開メソッドのうち、`[Http*]` か `[Route]` があるもの。属性ルーティング（クラスかメソッドにテンプレートがある）と、名前が `Api_` で始まる旧来のコントローラだけを対象にし、画面用の規約ルーティングのコントローラは除く。`[NonAction]` は除く |
| パス | クラスの `[Route]` とメソッドのテンプレートをつなぎ、`[controller]`・`[action]` を置き換える。綴りは元のまま。旧来のコントローラは `null`（`Startup.cs` のどの規約に当たるかは推定しない。規約そのものは `conventionalRoutes` に記録する） |
| 照合のキー | `POST api/items/{id}/get` のように「メソッド 小文字のパス」。旧来は `POST legacy:Api_Items/Get` |
| 領域 | パスの先頭で `api`・`scim`、旧来は `legacy` |
| 認証 | `[AllowAnonymous]`・`[Authorize]` の有無と、コントローラのメソッドの中の `context.Authenticated`・`context.HasPrivilege` の参照の有無 |
| レート制限 | `[EnableRateLimiting("…")]`（メソッドの指定がクラスより優先） |
| 入力の手がかり | コントローラの中の `Request.Body`・`Request.Form`・`Request.Form["…"]`・`Request.Form.Files`・`Request.Headers["…"]`・`…QueryStrings.X("…")`・`…Forms.X("…")`、`Parameters.*` の参照（`gates`） |
| リクエストの型 | アクションから呼び出しを辿り、`….RequestDataString.Deserialize<T>()` の `T` を集める。`[FromBody]` の型も加える |
| 応答 | 辿ったメソッドの中の `ApiResults.X(…)` の `X`、`ApiResults.Get(response: new { … })` のメンバー名、辿ったメソッドの戻り値の型のうち名前が `ApiModel` で終わるもの |
| モデル | リクエスト・応答の型と `ApiResponse` を起点に、公開のインスタンスのプロパティ・フィールドの型を辿る。`[NonSerialized]`・`[JsonIgnore]`・`[IgnoreDataMember]` のメンバーは記録するが、その型は辿らない |
| メンバー | 名前・プロパティかフィールドか・書かれたとおりの型・参照先の型・null 可否・既定値・読み取り専用か・属性 |
| 既定値 | `App_Data/Parameters/Api.json` の値 |

呼び出しの辿り方（`ExtractorOptions` で変えられる）:

- 深さ 0（コントローラ）: 同じクラスの補助メソッドは名前に関係なく辿る。`…Utilities` の静的メソッドも辿る
- 深さ 1 以降: 名前に `Api` を含むメソッドだけを辿る（`GetByApi`・`ApplyCreateByApi` など）
- 受け手が型名なら、その型のメソッド。`new T(…)` なら `T`。受け手がローカル変数などで型が分からないときは、**同じディレクトリで同名のメソッドを持つ型**を候補にする
- `ApiResults`・`ApiResponses` の中へは辿らない。深さの上限は 6

### 限界（取れないもの・推定を含むもの）

- **応答の匿名型のメンバーの型は未抽出。** 名前だけを取る。`Data` の要素の型は、`response.modelTypes`（辿ったメソッドの戻り値の型）を手がかりにする
- **リクエストの型は「辿った処理が解釈する型」の和集合。** `api/items/{id}/create` はレコードの種類で振り分けられるため、`SiteApiModel`・`IssueApiModel`・`ResultApiModel`・`WikiApiModel` がすべて並ぶ。
  また、コントローラが内部で組み立てた本文を解釈するもの（`api/binaries/upload` の `UpdateByApi`）も含まれる
- **名前の解決は推定。** 単純名が同じ型が複数あるときは、入れ子の関係・名前空間・using で決め、決まらなければ `ambiguousTypes` に残す（1.4.0.0〜1.5.9.0 では 0 件）。
  ソースに無い型は辿らず、`unresolvedTypes` に残す（`string`・`List`・`Dictionary`・`DateTime` などのよく使う組み込みの型は数えない。1.4.0.0〜1.5.9.0 では 0 件）
- **null 可否**は、`?` 付き・値型（組み込みとソース内の enum・struct）・参照型（組み込みとソース内の class）から決める。Pleasanter は Nullable 参照型を無効にしているので、参照型は null を取りうる。どちらか分からないものは `null`（未判定）
- **認証の確認は、コントローラのメソッドの中だけを見る。** `…Utilities` の中での権限の確認（例: テナントの管理者か）は未抽出。フィルタ（`CheckApiContextAttributes`）の中身も未抽出（属性名だけ）
- **SCIM（`scim/v2/*`）は、エンドポイントとパラメータだけ。** 本文の型は、`…ByApi` の形を取らないため未抽出
- **MCP サーバ（1.5.2.0〜、`MapMcp`）は対象外。** 別の規格の口であり、コントローラの形をしていない
- 動的な値（`Startup.cs` の規約の既定値が変数のもの）は `?` とする

## 保存の形

| ファイル | 中身 | 大きさ（2026-10-21） |
| --- | --- | --- |
| `api-surface/latest.json` | 最新の版の全体（スキーマ `pleasanter-api-surface/1`） | 約 0.7 MB |
| `api-surface/history.json` | 全版の履歴（スキーマ `pleasanter-api-history/1`）。項目ごとに Since・Until・変更・値の版 | 約 1.4 MB |
| `api-surface/history.md` | 人が読む一覧（増減・変化した項目だけを表にし、変わらない項目は件数だけ） | 約 40 KB |

タグごとの全体（66 版で約 40 MB）は置かない。`history.json` の `revisions` を順にたどれば、どの版の値も復元できる。

### history.json の読み方（フェーズ 2 の入力）

- `tags`: 対象の版（古い順）。`Since`・`Until` はこの中の版を指す
- `items[].key`: `endpoint:POST api/items/{id}/get`・`type:<完全名>`・`member:<型の完全名>#<名前>`・`enum:<型の完全名>#<名前>`・`parameter:Api.<名前>`・`route:<名前>`
- `items[].since`: 最初に現れた版。**対象の最初の版（1.4.0.0）なら「その版以前から在る」**
- `items[].until`: 無くなった版（**その版からは無い**。排他）。最新の版に在れば無い
- `items[].intervals`: 途中で無くなって戻ったものだけ。区間の一覧
- `items[].changes`: 中身が変わった版と、変わった要素の名前（`type`・`nullable`・`rateLimiting` など）
- `items[].revisions`: 値の版。`since` から次の revision の手前まで有効
- エンドポイントの比較からは、`handlers`（辿ったメソッド）・`file`・ルートの属性（`Route`・`Http*`。パスとメソッドに表れている）を外している。内部の実装の変化を API の変化と取り違えないため

フェーズ 2 では、`since`・`until` から `Since`/`Until` 属性を作れる。名前の変更（例: `Attachment` の `Extention` → `Extension`、1.4.7.0）は、別の項目の廃止と導入として現れるので、対応づけは人が判断する。

## 検証

- 単体テスト: `tests/VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Tests`。架空の小さなソース（Pleasanter のコードではない）で、エンドポイント・パス・認証・入力・リクエストの型・応答・モデル・名前の解決・履歴・Markdown を確かめる
- ソースとの突き合わせ（2026-10-21）: `Pleasanter_1.4.0.0`・`Pleasanter_1.4.23.3`・`Pleasanter_1.5.9.0` で、次を手で確かめて一致した
  - エンドポイントの数（`Controllers/Api/`・`Api_*`・`Scim/` の `[Http*]` の数）: 52・60・83
  - `Libraries/Requests/Api.cs` のメンバー（1.4.0.0 に `PageSize` が無く、1.5.9.0 に `SsCache` がある）と、`ImportApi` のメンバー
  - `Libraries/DataTypes/Attachment.cs` の `Extention`（1.4.0.0）と `Extension`（1.4.23.3）
  - `Controllers/Api/ItemsController.cs` のアクション数（1.4.0.0 で 15）
  - `Extensions` の `HasPrivilege`（1.4.13.0 で無く、1.4.16.0 で有る）
  - `Models/Users/UserApiModel.cs` のメンバー数（1.4.23.3 で 54）と型・null 可否
  - `Models/Tenants/CreateTenantApiModel.cs` と `Libraries/Responses/ApiResponse.cs` のメンバー（1.5.9.0）
  - `IssueBulkUpsertApiModel` の `Data`・`Keys` の `?` の有無（1.4.6.0 で有り、1.4.7.0 で無い）
  - `api/items/{id}/delete` のリクエストの型が空であること（`IssueUtilities.DeleteByApi` に `Deserialize` が無い）
- ディレクトリからの抽出（`--source-dir`）とタグからの抽出が、同じ版で一致すること

## 更新するとき

新しい Pleasanter のタグが出たら、`prepare` と `run` をやり直し、`api-surface/` の差分を確かめてコミットする。抽出の規則に合わない形（ファイルの移動・新しい書き方）が出たら、[調査メモ](調査メモ_API定義の場所.md) を直してから `ExtractorOptions` を変える。
