# _reference（参考専用・AGPL）

Pleasanter 本体（**GNU AGPL v3**）は、AI（Claude・Copilot・Codex）が
API 仕様・スキーマ・挙動を事実確認するためだけに読む参照ソース。
本リポジトリでは追跡せず、必要なときに上流の `main` の先頭を取得する。
CI は参照ソースを使わず、checkout の `submodules: false` も維持する。

- 上流: <https://github.com/Implem/Implem.Pleasanter>
- 既定の取得先: リポジトリ直下の `_reference/Implem.Pleasanter/`（Git 管理外）
- 共有取得先: 環境変数 `PLEASANTER_REFERENCE_DIR`（その場所を直接読む）
- この `README.md` は引き続き Git で追跡する

## 厳守事項

- **コードをコピー・流用・リンク・プロジェクト参照しない。**
  本ライブラリは標準 Web API をネットワーク越しに利用する独立したクライアントとして実装する
- **本ライブラリは LGPL＋AGPL＋商用の 3 層。** AGPL のコードを取り込むと LGPL と商用ライセンスで提供できなくなる
- ビルド対象・NuGet パッケージ・ソリューションへ含めない。Git メタデータも配布しない
- 取得した本体の作業ツリーを編集しない。本リポジトリへのコミット対象でもない
- 実機未検証の結果を動作確認済みと書かない。実際の JSON の日時書式・未設定値・権限は実機で確認する

API 面のタグ別抽出は、従来どおり別の一時取得先をデータとして読む。
生成に使うのは識別子・型名・パス・版などの事実のみで、本体の文章やコードは取り込まない。
このオンデマンド取得とは別の仕組みとして維持する（[API 面の抽出](../_documents/API面の抽出.md)）。

## 取得と更新

事実確認を始める前に毎回実行する。Git と bash が必要で、Windows では Git Bash を使う。

```bash
bash scripts/sync-reference.sh

# worktree 間で共有する場合（絶対パスを推奨）
export PLEASANTER_REFERENCE_DIR='D:\references\Implem.Pleasanter'
bash scripts/sync-reference.sh
```

PowerShell からも環境変数を設定して bash を起動できる。

```powershell
$env:PLEASANTER_REFERENCE_DIR = 'D:\references\Implem.Pleasanter'
bash scripts/sync-reference.sh
```

未取得なら `main` を `git clone --depth 1` で浅く取得する。
既存なら `git fetch --depth 1 origin main` と `git checkout --detach FETCH_HEAD` で更新する。
終了時に Path・短い SHA・コミット日時・Status・完全な FullSHA・取得日時（UTC）・Version を出す。
Version が読めない場合は警告と `Version=unknown` を出し、版を推測しない。

変更済み・ステージ済み・未追跡ファイルがある作業ツリー、上流 URL が異なる origin、
別リポジトリや非空の取得先では停止する。通信失敗は失敗コードで終了し、既存の HEAD とファイルを維持する。
新規 clone は一時取得先で成功してから配置する。並行取得はロックで止める。
異常終了で `.sync-lock` が残った場合は、同期が実行中でないと確認してから空ディレクトリを取り除く。

`PLEASANTER_REFERENCE_DIR` 指定時は取得先の案内だけを出し、ローカルへのリンクは作らない。
相対パスはリポジトリ直下を基準にする。共有先の更新中に他の AI が読まないよう調整する。

## 事実の記録と再現性

2026-10-26 の Template の決定に基づき、サブモジュール・日次更新 workflow・追従 Issue / PR は廃止する。
AI の参照専用で CI は使わないため、ポインタ更新 PR や参照更新用 bot の迂回権限は不要。
既存の日次更新用ブランチ・Draft PR・Issue の整理は別作業で行う。

再現性は gitlink ではなく、文書に残す**完全なコミット SHA・ソースへの固定リンク・参照日**で保つ。
現在の取得先が同じ SHA とは限らないので、過去の記述を最新ソースの根拠として扱わない。
再確認していない過去の事実の SHA や参照日を、自動で最新の値へ置き換えない。
`main` はリリース前の変更も含む。Version だけでリリース済みとは判断せず、
必要なら上流の `Pleasanter_*` タグとの対応も確認する。

確認例（2026-10-27 参照、SHA `664c833b144d9da9b6bb58e60f7e48eb01141e58`）:

- [Context.cs][context] は API リクエスト本文の `ApiKey` を読み、ユーザー解決へ渡す
- [BinariesController.cs][binaries] の upload は `Authorization` ヘッダーからキーを取り出す
- [Implem.Pleasanter.csproj][project] の Version は `1.5.9.1`、TargetFramework は `net10.0`

実機の応答・実運用の権限は未検証であり、このソース確認だけでは保証しない。

取得仕様の一次情報（2026-10-27 参照、承認済み Template 手順の参照元を継承）:

- [git clone](https://git-scm.com/docs/git-clone)（浅い clone とブランチ指定）
- [git fetch](https://git-scm.com/docs/git-fetch)（`--depth` と `FETCH_HEAD`）
- [承認済みの配布手順][distribution]（Template SHA `3312f003fbe9827090e20bc643800824bdf30e22`）

[context]: https://github.com/Implem/Implem.Pleasanter/blob/664c833b144d9da9b6bb58e60f7e48eb01141e58/Implem.Pleasanter/Libraries/Requests/Context.cs#L497-L504
[binaries]: https://github.com/Implem/Implem.Pleasanter/blob/664c833b144d9da9b6bb58e60f7e48eb01141e58/Implem.Pleasanter/Controllers/Api/BinariesController.cs#L92-L103
[project]: https://github.com/Implem/Implem.Pleasanter/blob/664c833b144d9da9b6bb58e60f7e48eb01141e58/Implem.Pleasanter/Implem.Pleasanter.csproj#L3-L10
[distribution]: https://github.com/vehiclevisionjp/VehicleVision.PleasanterTools.Template/blob/3312f003fbe9827090e20bc643800824bdf30e22/_documents/参照ソースのオンデマンド取得への配布手順.md
