# 貢献のしかた

Issue・PR・コミット・レビュー・ドキュメントは日本語で書いてください。
共通規約は [.github/copilot-instructions.md](.github/copilot-instructions.md)、
開発手順は [開発ガイド](_documents/開発ガイド.md) を参照してください。

本ライブラリは **3 層**（LGPL-3.0-or-later・AGPL-3.0-or-later・商用）で提供します
（[LICENSING.md](LICENSING.md)）。
**他人の著作物を AGPL・商用ライセンスで再頒布する権利は、その人から貰わない限りありません。**
署名のない貢献を取り込むと、**その部分だけ 3 層で提供できなくなります。**

## CLA へ署名する

**貢献は、取り込む前に [CLA.md](CLA.md) への署名が要ります。**
LGPL・AGPL・商用ライセンスのいずれでも提供できるよう、再許諾できる状態を保つためです。

1. [CLA.md](CLA.md) を読む
2. 「CLA への署名」の Issue を作り、同意する（Issue テンプレートあり）
3. 管理者が同意と許諾権限を確認し、[.github/cla/signed.yml](.github/cla/signed.yml) へ追記する
4. PR のチェック `cla` で確認する（必須化は初期配置後の別作業）

**会社の業務として貢献する場合は、法人用の署名も要ります。** 勤務先の承認を先に取ってください。
署名一覧は管理者が変更します。貢献者自身の PR で追記しても、その PR の判定には使いません。
一覧の変更を対象 base ブランチへ取り込んだ後、PR を更新してチェックを再実行してください。
未署名ならチェックが失敗し、案内コメントが付きます。

Dependabot・GitHub Actions・`my-repo-automation-bot[bot]` の自動更新 PR は対象外です。
管理者の確認手順と、必須化前の配置条件は [開発運用](_documents/開発運用.md) にあります。

## 作業の進め方

Issue を作り、`develop` から作業ブランチを切って実装・検証します。
PR の base は `develop`、本文には `Closes #N` を書きます。
作業中は Draft と `WIP:` を使い、レビュー可能になったら Draft を解除します。
