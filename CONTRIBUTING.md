# Contributing to Aquavit VR

Aquavit VR は現在 **開発初期(Phase 0)** です。API は予告なく破壊的に変更されることがあります。この点をご理解のうえでコントリビューションをお願いします。

このドキュメントは、開発とコントリビューションの規約を定めます。人間のコントリビュータと、作業するコーディングエージェントの両方に等しく適用されます。本文中の **必須** は MUST、**推奨** は SHOULD の強さで読んでください。

関連する規約は次の場所にあります。同じ規約を二重に書かず、参照してください。

| 目的 | 置き場所 |
|---|---|
| コードレビューの判断基準とテスト観点 | [REVIEW.md](REVIEW.md) |
| 参照してよい資料とコードの範囲(ライセンス) | [docs/ReferencePolicy.md](docs/ReferencePolicy.md) |
| API 設計の規約 | [docs/APIDesign.md](docs/APIDesign.md) |
| ドキュメントコメントの規約 | [docs/DocumentationComments.md](docs/DocumentationComments.md) |
| エージェントの入口・実行方針 | [AGENTS.md](AGENTS.md) |

このドキュメントは [FloatSoda の CONTRIBUTING.md](https://github.com/sumx21t-3310/FloatSoda/blob/main/CONTRIBUTING.md) を土台にしています。

---

## 最初に読むもの

**実装を始める前に [docs/ReferencePolicy.md](docs/ReferencePolicy.md) を読んでください**(必須)。Aquavit VR は MIT ライセンスで配布するため、WebF、Kraken、Blink、WebKit、Servo、Gecko のソースコードを参照せずに開発しています。これらのコードは、Issue と PR にも貼らないでください。

---

## 開発環境のセットアップ

- .NET 10 / C# 14 SDK
- SteamVR は現在のサンプルでは不要です

```bash
# ソリューション全体のビルド
dotnet build Aquavit.slnx

# 全テストの実行
dotnet test Aquavit.slnx

# サンプルの実行(PNG を書き出す)
dotnet run --project samples/Aquavit.Samples.LayerTree -- layer-tree.png
```

---

## Issue とラベル

### Issue が必須な変更

次の変更は、着手前に対応する Issue を立ててください(必須)。

- public API の追加・変更・削除
- observable behavior の変更
- アーキテクチャ・依存関係・プロジェクト構成に関する設計判断
- Web 標準との挙動の差異に関する判断
- breaking change を伴う変更

typo の修正、明白なドキュメント修正、CI・管理ファイルの局所的な保守のような、設計判断を伴わない小変更に限り、Issue を省略できます。迷ったら Issue を立ててください。

### タイトルとラベル

Issue のタイトルは「何をする Issue か」の記述に集中させ、種別・領域などの分類は GitHub Labels で表現します(必須)。`feat(DOM):` のような Conventional Commit 風の prefix はタイトルに付けません。

ラベルは3つの軸で構成します。ラベルの一覧と説明は[リポジトリの Labels ページ](https://github.com/sumx21t-3310/Aquavit-VR/labels)にあります。

| 軸 | ラベル | 付け方 |
|---|---|---|
| 種別 | `feature` / `bug` / `design` / `refactor` / `performance` / `documentation` / `tracking` / `release` / `maintenance` | 原則1つ付ける(必須) |
| 領域 | `area:core` / `area:dom` / `area:style` / `area:layout` / `area:paint` / `area:engine` / `area:input` / `area:runtime` / `area:framework` / `area:tooling` | 主要な領域を1つ付ける。2つの領域にまたがる Issue には複数付けてよい |
| 補助属性 | `breaking-change` / `spec-divergence` / `needs-device-test` / `good-first-issue` / `help-wanted` | 必要なものだけ付ける |

**種別ラベルは、実装手段ではなく Issue の主目的で1つ選びます**(必須)。

- Web 標準や docs の契約から外れた挙動を、契約どおりに戻す → `bug`
- 新しい利用者向け機能のために内部構造も変える → `feature`
- observable behavior を変えずに内部構造だけを整理する → `refactor`
- 選定や方針の決定が主目的である → `design`
- 複数の Issue を束ねて進捗を管理する → `tracking`(領域ラベルは付けない)
- テストの基盤や結合テストを追加する → `feature` と `area:tooling`

**領域ラベルはプロジェクト単位ではなくドメイン単位で選びます。** たとえば Style の解決は、コードが物理的に `src/Aquavit` にあっても `area:style` です。

優先度ラベル(`priority:*`)は導入しません。実施時期は Milestone(Phase)、作業順序は Issue の依存関係で管理し、同じ意味を持つ管理軸を複数作りません。

---

## ブランチ・PRフロー

- `main` へは直接 push せず、必ず PR 経由でマージしてください(必須)。
- PR を出す前に、CI(`.github/workflows/ci.yml`)と同じ手順をローカルで実行し、パスすることを確認してください(必須)。

```bash
dotnet build Aquavit.slnx --configuration Release -warnaserror
dotnet test Aquavit.slnx --configuration Release --no-build
```

> **実装状況** — `main` にはブランチ保護を設定しています。マージには PR と、CI(`build-and-test`)の成功が必要です。承認は必須にしていません。force push とブランチの削除は禁止しています。保護は管理者には適用していないため、オーナーも規約として PR 経由で進めます。

バグ報告・機能要望は `.github/ISSUE_TEMPLATE/` のテンプレートを使って Issue を立ててください。PR の本文は [`.github/PULL_REQUEST_TEMPLATE.md`](.github/PULL_REQUEST_TEMPLATE.md) に沿って記入してください。

### ブランチ名

人間が切る場合もエージェントが切る場合も、同じ規則を使います(必須)。

```text
<issue-number>-<primary-area>-<slug>   基本形
<issue-number>-<slug>                  領域を1つに定めにくいリポジトリ横断・管理系 Issue
<slug>                                 Issue を省略できる小変更(→ Issue が必須な変更)
```

- Issue 番号を先頭に置きます。「Issue #7 の作業ブランチを探す」操作を前方一致検索で済ませるためです
- `<primary-area>` は Issue の `area:*` ラベルのうち主要な領域1つです。Issue 側に領域ラベルが複数あっても、ブランチ名には1つだけ選びます
- `<slug>` は **lowercase ASCII + ハイフン**で書きます
- `feature` / `bug` などの種別と、`breaking-change` などの補助属性はブランチ名に入れません。分類は Issue の Labels で表します

例:

- `7-engine-build-layer-tree`
- `12-layout-box-model-and-block`
- `33-architecture-test`(管理系 Issue のため領域を省略)
- `fix-readme-typo`(Issue を省略できる小変更)

**作業した AI エージェントやツールの名前をブランチ名に含めないでください**(必須)。`codex/`、`claude/`、`agent/` のようなプレフィックスは使いません。**ブランチ名は「誰が変更したか」ではなく「何を変更するか」を表すもの**です。

### コミットメッセージ

Conventional Commits の接頭辞(`feat` / `fix` / `docs` / `refactor` / `test` / `chore` など)に続けて、件名を日本語で書きます。必要に応じて `feat(layout):` のようにスコープを付けます。

```text
feat(layout): Block 要素を縦方向に配置する
docs: ReferencePolicy に litehtml を追加する
```

### PR のスコープ

**Issue や PR の目的に不要な変更を混ぜないでください**(必須)。作業中に関連箇所を見つけても、次の変更を同じ PR に含めないでください。

- 無関係なリファクタリング
- 無関係なリネーム
- 無関係なクリーンアップ
- 無関係な依存関係の追加・削除

必要なものは別 Issue / PR として扱います。レビュー範囲が膨らむと、本来の変更の正しさを判断できなくなるためです。

Issue の目的を成立させるために不可避な前提変更(prerequisite change)は、次を**すべて**満たす場合に限り同じ PR に含めてかまいません。

- その Issue の完了に直接必要である
- 単独では別の利用者価値・設計目的を持たない
- 変更範囲と理由を PR 本文から追跡できる

独立した設計判断や別の利用者価値を持つ変更は、同じ箇所を触る場合でも別 Issue / PR にします。

**public API または observable behavior を変更する場合は、breaking change の有無を明示的に判断し、PR 本文に書いてください**(必須)。「無い」と判断した場合も、その旨を書きます。判断基準は [docs/APIDesign.md](docs/APIDesign.md) の「バージョニングと後方互換性」にあります。**開発初期であることは判定を省略する理由になりません。** breaking と判定したうえで、いま受け入れるかを別に判断します。breaking change を伴う Issue には `breaking-change` ラベルを付けてください。

### FloatSoda 側の変更が必要な場合

Aquavit VR は FloatSoda を NuGet パッケージとして参照します(→ [docs/Architecture.md](docs/Architecture.md))。FloatSoda 側の変更が必要になった場合は、FloatSoda のリポジトリに Issue と PR を出し、リリースされたバージョンを Aquavit VR から参照します。Aquavit VR 側の PR では、参照するバージョンを上げる変更だけを行います。

---

## テスト方針

テストは xunit を使用しています。

- `tests/Aquavit.Test` — `src/Aquavit` のテスト

**何を検証するか(テスト観点)は [REVIEW.md](REVIEW.md) の「5. テスト」にあります。** 実装を変更するときも、その観点で漏れを確認してください(必須)。Phase 0 が終わるまでは、描画の変更を `samples/` のサンプルが書き出す PNG でも確認します。

```bash
# 特定のテストのみ実行する例
dotnet test tests/Aquavit.Test --filter "FullyQualifiedName~EngineReferenceTest"
```

### テストの命名

テストメソッド名は **`対象メンバー名_条件_期待結果`** の形式で、条件と期待結果を**日本語**で書きます。クラス名とメンバー名の部分は英語のままにします。

```csharp
[Fact]
public void Children_Aquavit経由でLayerを追加_HasChildrenがtrue() { ... }

[Fact]
public void Parse_閉じタグが無い_仕様どおりに要素を閉じる() { ... }
```

| 要素 | 言語 | 理由 |
|---|---|---|
| テストクラス名(`EngineReferenceTest`) | 英語 | テスト対象の型名のミラー。ファイル名と `--filter` に揃える |
| メソッド名の先頭(対象メンバー名) | 英語 | API をリネームするとき「この API のテストはどこか」を grep で辿れるようにする |
| 条件・期待結果 | 日本語 | `SameSize_ReturnsZero` のような英語圧縮では仕様の粒度が落ちる。日本語なら同じ長さで正確に書ける |
| ヘルパーメソッド・ローカル変数 | 英語 | 通常のコーディング規約に従う |

補足:

- `[Fact(DisplayName = "…")]` は**使いません**。メソッド名と説明の二重管理になり、片方だけ更新される事故を招くためです。
- Web 標準に由来する機能のテストケースは、仕様の該当する節と [web-platform-tests](https://github.com/web-platform-tests/wpt)(BSD-3-Clause)から取ります(→ [REVIEW.md](REVIEW.md))。

---

## namespace とディレクトリ

**C# の namespace と、プロジェクトルート以下の物理ディレクトリ構造を一致させます**(必須)。

```text
src/Aquavit/Dom/Element.cs            →   namespace Aquavit.Dom;
tests/Aquavit.Test/Dom/ElementTest.cs →   namespace Aquavit.Test.Dom;
```

- **namespace を移動する場合は、対応するディレクトリへファイルも同じ変更で移動してください**(必須)。
- **ディレクトリだけ、または namespace だけを変更して不一致を作らないでください**(必須)。
- 例外が必要な場合は、その理由を明示してください。

`samples/` はこの規則の適用例で、加えてディレクトリ名・プロジェクト名も一致させます。

ファイルスコープ namespace(`namespace X;`)は `.editorconfig` の `IDE0161` でビルド時に強制されます。1つのファイルには1つの型を置きます(`SA1402`)。

---

## コーディング規約

API 設計の規約は **[docs/APIDesign.md](docs/APIDesign.md)**、ドキュメントコメントの規約は **[docs/DocumentationComments.md](docs/DocumentationComments.md)** を参照してください。

コードスタイルは、StyleCop.Analyzers、Roslynator、.NET SDK のコードスタイル規則によってビルド時に検査されます。有効なルールと、対象外にしたルールの理由は [.editorconfig](.editorconfig) にあります。

- **ビルドの警告は 0 件を保ちます**(必須)。CI は `-warnaserror` でビルドします。
- アナライザーのルールは opt-in で運用します。ルールを有効にするときは、目的を `.editorconfig` のコメントに書いてください。
- 警告を `#pragma warning disable` や `NoWarn` で抑える場合は、理由をコメントに書いてください。

---

## エージェント向けファイル

- **[AGENTS.md](AGENTS.md) は英語で書きます**(必須)。全エージェントが毎セッション読む入口ファイルであり、英語のほうが指示への追従性が高く、トークン量も少なく済むためです。
- **それ以外のエージェント向けファイルは日本語で書きます**(必須)。オーナー自身も読み書きするファイルであり、主要なコーディングエージェントは日本語を問題なく読みます。識別子・型名・ファイルパス・コマンド・コードは英語のままにします。
- プロジェクトの指示は `AGENTS.md` に書きます。`CLAUDE.md` は `AGENTS.md` を取り込むだけのスタブです。
- agent skill を追加する場合は、FloatSoda と同じく `.agents/skills/` を置き場所にします。

---

## サンプルを追加する場合

手順は [docs/GettingStarted.md](docs/GettingStarted.md#サンプルを追加する) にあります。ディレクトリ名、プロジェクト名、namespace を `Aquavit.Samples.<名前>` で一致させてください。

---

## ドキュメントを更新する場合

- `docs/` の文書は日本語で書きます。ページの先頭に `← [Home](Home.md)` を置き、新しいページは [docs/Home.md](docs/Home.md) のページ一覧に追加してください。
- 未実装や予定の内容を書く場合は、`> **実装状況** — ` で始まる注記で明示してください。
- 挙動を変更した PR では、該当する docs と XML ドキュメントコメントを同じ PR で更新してください(必須)。

---

## PRを出す前のチェックリスト

- [ ] [docs/ReferencePolicy.md](docs/ReferencePolicy.md) の「開かないもの」を参照していない
- [ ] 他の実装を参照した場合、プロジェクト名とライセンスを PR 本文に書いた
- [ ] `dotnet build Aquavit.slnx --configuration Release -warnaserror` が通る
- [ ] [REVIEW.md](REVIEW.md) のテスト観点に沿って、テストを追加・更新した
- [ ] `dotnet test Aquavit.slnx` が通る
- [ ] 描画に影響する変更の場合、サンプルを実行して PNG を確認した
- [ ] 追加した型とメンバーに XML ドキュメントコメントを付けた
- [ ] breaking change の有無を判断し、PR 本文に書いた
- [ ] 挙動の変更に合わせて docs を更新した
