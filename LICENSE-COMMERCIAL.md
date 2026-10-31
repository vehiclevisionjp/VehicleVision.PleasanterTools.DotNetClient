# 商用ライセンス（LicenseRef-PMC-Commercial）

対象ソフトウェアは **VehicleVision.PleasanterTools.DotNetClient**

Copyright (C) PMC Co., Ltd. IT Solution Div.

本書で「当社」とは、株式会社ピー・エム・シー ITソリューション事業部を指します。

本文書は、[`LICENSING.md`](LICENSING.md) の選択肢 **C（商用ライセンス）** の概要です。
**正式な条件は個別に締結する契約書によります。** 本文書は契約書ではありません。

## 概要

商用ライセンスは、**GNU AGPL v3 の義務を負わずに**本ソフトウェアを利用するための
選択肢です。契約できる相手は法人に限りません。次を想定しています。

- 改変版のソースコードを公開せずに利用する（ネットワーク経由で提供する場合を含む）
- proprietary な製品・システムへ組み込む
- 保守・サポート・SLA を契約に含める

営利事業に使わない個人・非営利団体・教育機関は、無償で LGPL（選択肢 A）を選べます。範囲の詳細は [`LICENSING.md`](LICENSING.md) が定めます。

## 3 層の違い

| | LGPL v3（A） | AGPL v3（B） | 商用ライセンス（C） |
| --- | --- | --- | --- |
| 対象 | 営利事業に使わない個人・非営利団体・教育機関 | 営利事業に使う場合・公的機関 | AGPL の義務を負わずに利用したい場合（法人に限らない） |
| 費用 | 無償 | 無償 | 個別契約 |
| ソースコード開示義務 | 改変部分を配布する場合 | **あり**（改変して配布またはネットワーク提供する場合を含む。結合した著作物の配布は全体が対象） | なし |
| proprietary 製品への組み込み | 可（LGPL の条件を守る場合） | 不可 | 可 |
| 保守・サポート | なし | なし | 契約による |

## 対象範囲

本リポジトリのうち、**当社が著作権を持つ部分**が対象です。次は対象外です。

- サードパーティの OSS と部品（[`NOTICE`](NOTICE) 参照）。各ライセンスに従ってください
  <!-- license:begin sni-scope=off -->
  <!-- license:end sni-scope -->
  <!-- license:begin reference-scope=on -->
- [`_reference/`](_reference/README.md) 配下（Pleasanter 本体。AGPL v3）。
  **参照専用であり、本ソフトウェアの一部ではありません**
  <!-- license:end reference-scope -->

<!-- license:begin core-terms=off -->
<!-- license:end core-terms -->

<!-- license:begin sni-terms=off -->
<!-- license:end sni-terms -->

## お問い合わせ

株式会社ピー・エム・シー ITソリューション事業部

お問い合わせは、次のページの連絡先からお願いします: [https://www.vehiclevision.jp/division/](https://www.vehiclevision.jp/division/)
