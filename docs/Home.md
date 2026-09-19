# Aquavit VR ドキュメント

**Aquavit VR** は、VR 向けの Web UI フレームワークです。UI は DOM / CSS で記述し、描画には [FloatSoda](https://github.com/sumx21t-3310/FloatSoda) の LayerTree を使います。.NET 10 / C# で実装します。

このページはドキュメント全体の入り口です。各ページは相互にリンクしています。

## ページ一覧

| ページ | 内容 | 対象読者 |
|---|---|---|
| [GettingStarted](GettingStarted.md) | 環境構築・ビルド・サンプルの実行 | 利用者 / コントリビュータ |
| [Architecture](Architecture.md) | FloatSoda との共有境界・依存ルール・リポジトリ構成 | コントリビュータ |
| [ReferencePolicy](ReferencePolicy.md) | 実装で参照してよい資料とコードの範囲(ライセンス) | コントリビュータ |

## どこから読むか

- **まず動かしたい** → [GettingStarted](GettingStarted.md)
- **設計を理解したい / コントリビュートしたい** → [Architecture](Architecture.md) → [ReferencePolicy](ReferencePolicy.md)

## 全体像

Aquavit VR は FloatSoda の Web 向け画面層ではなく、独立したフレームワークです。FloatSoda と共有するのは LayerTree 以下だけです。

```text
FloatSoda
Widget / Element / RenderObject
                │
                ▼
        FloatSoda Engine
     LayerTree / Compositor
                ▲
                │
          Aquavit VR
    DOM / CSS / Layout / Paint
```

詳細は [Architecture](Architecture.md) を参照してください。

## ロードマップ(Phase)

開発は Phase 単位で進めます。Phase は「フレームワークとして何ができる段階か」を表す到達点であり、NuGet のバージョン番号とは連動しません。各 Phase のスコープは [GitHub マイルストーン](https://github.com/sumx21t-3310/Aquavit-VR/milestones) を参照してください。

| Phase | 内容 | Issue | 状況 |
|---|---|---|---|
| Phase 0 - Engine Boundary | FloatSoda Engine のフレームワーク独立性を実証する(POC) | [#1](https://github.com/sumx21t-3310/Aquavit-VR/issues/1) | 進行中 |
| Phase 1 - Static Web | 静的な HTML / CSS を LayerTree へ変換して描画する | [#2](https://github.com/sumx21t-3310/Aquavit-VR/issues/2) | 未着手 |
| Phase 2 - Dynamic DOM | DOM の変更と入力イベントを描画へ反映する | [#3](https://github.com/sumx21t-3310/Aquavit-VR/issues/3) | 未着手 |
| Phase 3 - JavaScript Web Runtime | JavaScript から DOM とイベントを操作する | [#4](https://github.com/sumx21t-3310/Aquavit-VR/issues/4) | 未着手 |
| Phase 4 - Framework Integration | 既存の Web フレームワークを Aquavit VR 上で動かす | [#5](https://github.com/sumx21t-3310/Aquavit-VR/issues/5) | 未着手 |

## 検証の方法

Phase 0(POC)が完了するまでは、`samples/` のサンプルを実行し、出力された PNG を目視で検証します。PNG を期待値として固定するテストは、Phase 1 の [#15](https://github.com/sumx21t-3310/Aquavit-VR/issues/15) で追加します。

## 実装状況サマリ

| 領域 | 状況 |
|---|---|
| FloatSoda Framework を使わない LayerTree の構築(`samples/Aquavit.Samples.LayerTree`) | サンプルで確認済み |
| FloatSoda と同じ compositor / rasterizer を使った PNG 出力([#8](https://github.com/sumx21t-3310/Aquavit-VR/issues/8)) | 未実装 |
| 依存ルールの自動検査([#6](https://github.com/sumx21t-3310/Aquavit-VR/issues/6)) | 未実装 |
| DOM / CSS / Layout / Paint | 未実装 |

## リポジトリ構成

| パス | 役割 |
|---|---|
| `src/Aquavit` | フレームワーク本体。依存する FloatSoda のパッケージは `FloatSoda.Rendering` のみ |
| `samples/` | 検証用のサンプル。SteamVR なしで実行可能 |
| `tests/` | xunit テスト |
| `docs/` | このドキュメント |
