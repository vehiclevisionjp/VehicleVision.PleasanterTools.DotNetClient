# VehicleVision.PleasanterTools.DotNetClient

Pleasanter の標準 Web API を呼ぶ、**.NET 向けのクライアントライブラリ**です（NuGet パッケージ `VehicleVision.PleasanterTools.DotNetClient`）。

> **開発中です。** いまあるのは最小のクライアント（接続設定と、API の呼び出し・エラーの分類）だけです。範囲と方針は [アーキテクチャ方針](_documents/アーキテクチャ方針.md) にあります。

## 使い方

```csharp
var options = new PleasanterOptions
{
    BaseUrl = "https://pleasanter.example.com",
    ApiKey = Environment.GetEnvironmentVariable("PLEASANTER_API_KEY")!,
};
var client = new PleasanterApiClient(new HttpClient(), options);
```

API キーは、環境変数や Git 管理外の設定から読んでください。コードや設定ファイルへ書かないでください。

## 方針

- **Pleasanter 本体のコードは取り込みません。** 本体は AGPL v3 で、取り込むと LGPL で配布できなくなります
- クライアントは再試行しません。応答を分類して返し、再送するかは呼び出し側が決めます

## ライセンス

[LGPL-3.0-or-later](LICENSE) です（LGPL が参照する GPL の本文は [COPYING](COPYING)）。
このライブラリを使うアプリのライセンスは、LGPL に縛られません。このライブラリ自体を改変して配布するときは、その改変を LGPL で公開してください。詳しくは [ライセンス](_documents/ライセンス.md) を参照してください。
依存のライセンスは [NOTICE](NOTICE) にあります。開発環境と運用は [開発ガイド](_documents/開発ガイド.md) と [開発運用](_documents/開発運用.md) にあります。
貢献を取り込む前に [CLA（草案）](CLA.md) への署名が必要です。
将来の再許諾に備えるためのもので、現在の LGPL の提供条件は変わりません。手続きは [貢献のしかた](CONTRIBUTING.md) を参照してください。
