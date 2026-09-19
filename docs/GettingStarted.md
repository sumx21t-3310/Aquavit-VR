← [Home](Home.md)

# Getting Started

リポジトリをビルドし、サンプルを実行して PNG を出力するまでの手順です。

## 必要なもの

| 項目 | 内容 |
|---|---|
| .NET SDK | 10.0 以降 |
| SteamVR | 現在のサンプルでは不要です |

## ビルドとテスト

リポジトリのルートで実行します。

```bash
dotnet build Aquavit.slnx
```

```bash
dotnet test Aquavit.slnx
```

## サンプルを実行する

`Aquavit.Samples.LayerTree` は、FloatSoda Framework を使わずに LayerTree を組み立て、PNG に書き出します。

```bash
dotnet run --project samples/Aquavit.Samples.LayerTree -- layer-tree.png
```

引数には出力先のパスを指定します。省略した場合は、カレントディレクトリの `layer-tree.png` に書き出されます。実行すると、出力されたファイルの絶対パスが表示されます。

出力される画像は 512 × 512 ピクセルです。白い背景の上に、15度回転した半透明の青い四角形が描かれます。四角形の角は、クリップ範囲(64〜448)の外側で切り取られます。

## サンプルを追加する

Phase 0(POC)が終わるまでは、サンプルの出力を目で確認して検証します(→ [Home](Home.md#検証の方法))。新しい検証を追加するときは、次の手順で進めます。

1. `samples/Aquavit.Samples.<名前>/` にコンソールアプリのプロジェクトを作ります。
2. `src/Aquavit/Aquavit.csproj` を `ProjectReference` で参照します。
3. `Aquavit.slnx` の `/samples/` フォルダにプロジェクトを追加します。
