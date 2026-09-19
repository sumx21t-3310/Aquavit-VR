# Code Review Guidelines

このドキュメントは、Aquavit VR のコードレビューで**何を、どの優先度で見るか**を定めます。人間のレビュアーと、レビューを行うコーディングエージェントの両方が対象です。テストの観点も本ドキュメントに記載します。コードを実装する際にも、同じ観点でテストの漏れを確認してください。

役割分担は次のとおりです。

| 目的 | 置き場所 |
|---|---|
| エージェントの入口、Engine 境界のルール、ビルドと検証のコマンド | [AGENTS.md](AGENTS.md) |
| 参照してよい資料とコードの範囲 | [docs/ReferencePolicy.md](docs/ReferencePolicy.md) |
| FloatSoda との共有境界と依存ルールの説明 | [docs/Architecture.md](docs/Architecture.md) |
| Phase と Issue の対応 | [docs/Home.md](docs/Home.md) |

本文中の **必須** は MUST、**推奨** は SHOULD の強さで読んでください。

レビューの出力は**日本語**で書きます(→ [AGENTS.md](AGENTS.md) の Output Language)。コード・識別子・型名・ファイルパスは原文のままにします。

このガイドは [FloatSoda の REVIEW.md](https://github.com/sumx21t-3310/FloatSoda/blob/main/REVIEW.md) を土台としています。FloatSoda の Widget / Element / RenderObject に固有の章は含めず、Aquavit VR の UI モデル(DOM / CSS / Layout / Paint)に合わせて書き換えています。

---

## 1. 重要度の順序

レビューでの指摘は、概ね次の順に重要です。上位の問題を差し置いて、下位の指摘を並べないでください。

1. **licensing / engine boundary** — 「開かないもの」に由来するコードが入っていないか、FloatSoda との共有境界を越えていないか(→ 4章)
2. **behavioral correctness** — 仕様どおりに動くか
3. **tree lifecycle / state transitions** — DOM の追加・削除・差し替えと状態遷移が壊れていないか(Phase 2 以降)
4. **incremental update correctness** — Style / Layout / Paint の dirty 伝播と再計算の範囲が正しいか(Phase 2 以降)
5. **public API consistency** — 既存の API と一貫しているか
6. **test coverage** — behavioral contract を押さえたテストがあるか
7. **documentation consistency** — docs / XML ドキュメントコメントと実際の挙動が一致しているか
8. **performance** — **concrete impact のあるもののみ**
9. **style / maintainability**

1 を最上位に置くのは、混入したあとで取り除くコストが最も高いからです。ライセンスの問題は、公開済みの履歴とパッケージにも残ります。

---

## 2. finding の基準

- **concrete failure mode を説明できる問題を優先します**(必須)。「どの入力・状態で、何が壊れるか」を書けない指摘は、書けるようになるまで優先度を下げてください。
- **subjective な好みだけの指摘をしません**(必須)。規約・契約・失敗する条件のいずれにも紐づかない「私ならこう書く」という意見は指摘ではありません。
- **hypothetical な問題を過剰に報告しません**(必須)。「将来こう使われたら壊れるかもしれない」という問題は、その使い方が実際に到達可能であることを示せる場合だけ挙げてください。
- **Issue のスコープ外の改善要求を、安易に blocking にしません**(必須)。気づいた点は「別 Issue 向け」と明示し、非 blocking で伝えてください(→ 7章)。
- **既存コードがそうなっているという理由だけで、正しい仕様と判断しません**(必須)。次章の優先順位で確認してください。

blocking にできるのは、重要度 1〜6 に該当し、かつ concrete failure mode を説明できる指摘です。重要度 7〜9 の指摘は、原則として非 blocking の提案として扱います。ただし、重要度 1 のうちライセンスに関する指摘は、failure mode の説明を待たずに blocking とします。

---

## 3. 仕様の優先順位

仕様が食い違ったときは、**上から順に**確認します(必須)。

1. **Aquavit VR で明示的に定義された設計判断と対象範囲** — `docs/` 各ページの明記、該当する Phase の Issue に書かれた完了条件と対象範囲
2. **Web 標準** — DOM と HTML は WHATWG、CSS は W3C の仕様。挙動の確認には MDN も使えます。1 に該当する記述がない場合は、Web 標準に従います
3. **既存の Aquavit VR 実装は根拠になりません** — 実装がそうなっていることは、それが正しいことを意味しません

**古い Issue や既存実装だけを根拠に、新しい挙動を決めないでください**(必須)。Issue が書かれた時点の前提が今も成り立つかを確認します。

**WebF のソースコードは、仕様の根拠としても参照しません**(必須)。開いてよい参照先と開かない参照先は [docs/ReferencePolicy.md](docs/ReferencePolicy.md) にあります。

Aquavit VR は Web 標準の全体を実装しません。対象範囲は Phase ごとに、必要になった機能から広げます。**対象範囲の外にある機能が未実装であることは、指摘の対象にしません。** 対象範囲の中で Web 標準と挙動が異なる場合は、その差異が `docs/` か Issue に理由付きで書かれているかを確認します。

---

## 4. Aquavit VR 固有の確認項目

該当する変更がなければ、その項目は飛ばしてかまいません。

### ライセンス

参照先の区分は [docs/ReferencePolicy.md](docs/ReferencePolicy.md) にあります。

- **「開かないもの」に由来するコードが入っていないか**(必須)。PR の説明、コメント、コミットメッセージに、WebF、Kraken、Blink、WebKit、Servo、Gecko のソースを参照した形跡がある場合は blocking とし、オーナーに確認します。
- **他の実装を参照した PR で、プロジェクト名とライセンスが PR から追跡できるか**(必須)。参照先が「開いてよいもの」の表にあることを確認します。
- **コードを移植した PR で、`THIRD_PARTY_NOTICES.md` に元のプロジェクト、ライセンス、著作権表示が記録されているか**(必須)。
- **新しい依存パッケージのライセンスが MIT での配布と両立するか**(必須)。

### Engine 境界

- **`src/Aquavit` が参照する FloatSoda のパッケージが `FloatSoda.Rendering` だけか**(必須)。`.csproj` の `PackageReference` と `ProjectReference` の差分を確認します。
- **FloatSoda の `Widget` / `Element` / `RenderObject` / `BuildContext` / `Key` に依存していないか**(必須)。
- FloatSoda 側の変更が必要な PR では、FloatSoda のリポジトリでの変更とリリースが先に済んでいるかを確認します。

> **実装状況** — 依存ルールの自動検査は [#6](https://github.com/sumx21t-3310/Aquavit-VR/issues/6) で追加します。それまでは、この節をレビューで確認します。

### Layer

- **`ILayer` を実装する型を追加した場合、`Clone()` が新しく持たせたフィールドをコピーしているか**(必須)。FloatSoda は LayerTree を `Clone()` してレンダースレッドへ渡します。`Clone()` 後に `SKPicture`、子 Layer のリスト、`SKPaint` などの**可変オブジェクトを共有していないか**を確認してください。この対応が漏れるとデータレースになり、テストではほとんど検出できません。

---

## 5. テスト

### テスト観点

**何を検証するか**の規約です。実装を変更するときも、レビューするときも、この観点で漏れを確認します。

実装変更では、次のうち**その変更に該当するものをすべて考慮してください**(必須)。該当しない観点まで機械的に埋める必要はありません。

| 観点 | 内容 | 該当する時期 |
|---|---|---|
| representative normal behavior | 代表的な正常系。その API が普通に使われる形 | 現在 |
| boundary / degenerate inputs | 境界値と退化ケース。ゼロサイズ、空の子リスト、子が1つ、空文字列、空の HTML など | 現在 |
| invalid inputs | 不正入力。負数・非有限値・null が、握り潰されず契約どおりに扱われるか。HTML / CSS の構文エラーは、Web 標準が定める回復の挙動に従うか | 現在 |
| regression test | 再発防止。下記参照 | 現在 |
| state transitions | 状態遷移。属性やスタイルの変更の前後、イベントの配送の成立・不成立 | Phase 2 以降([#16](https://github.com/sumx21t-3310/Aquavit-VR/issues/16)、[#20](https://github.com/sumx21t-3310/Aquavit-VR/issues/20)) |
| tree lifecycle | DOM ツリーのライフサイクル。ノードの追加・削除・移動・差し替え | Phase 2 以降([#16](https://github.com/sumx21t-3310/Aquavit-VR/issues/16)) |
| incremental behavior | 差分更新。dirty 化の条件と、Style / Layout / Paint の再計算の範囲 | Phase 2 以降([#17](https://github.com/sumx21t-3310/Aquavit-VR/issues/17)、[#18](https://github.com/sumx21t-3310/Aquavit-VR/issues/18)) |

原則として、**テストは実装詳細をミラーせず、observable behavior・behavioral contract・invariant を検証します**(必須)。private な内部状態や呼び出し回数を確かめるテストは、リファクタリングで壊れる一方で挙動の誤りを捕まえられないため、避けてください。

**バグ修正では regression test を原則必須とします**(必須)。元のバグを再現するテストを書き、**修正前に失敗し、修正後に成功する**ことを確認してください。修正を戻すと失敗するかが判定基準です。

Web 標準に由来する機能では、**Aquavit VR 独自にテストケースを想像するだけで済ませないでください**(必須)。仕様の該当する節を確認し、仕様が明示している退化ケースと回復の挙動を検証します。参照した仕様の節は、PR から追跡できるようにします(推奨)。

### PNG による検証

- Phase 0(POC)が終わるまでは、描画の変更を `samples/` のサンプルの PNG で検証します。**描画に影響する PR では、実行したサンプルと、PNG で確認した内容が PR に書かれているか**(必須)を確認します。
- PNG を期待値として固定するテストは [#15](https://github.com/sumx21t-3310/Aquavit-VR/issues/15) で追加します。追加後は、期待値の PNG を更新する PR で、**更新の理由が PR に書かれているか**(必須)を確認します。理由のない期待値の更新は、テストを通すための書き換えと区別できません。

### レビューで確認すること

- 観点の抜けを見ます。**該当するのに無い観点を指摘します**(必須)。
- テストが observable behavior / behavioral contract / invariant を検証しているか(必須)。リファクタリングで壊れるものの挙動は正しい、というテストは指摘対象です。
- バグ修正に regression test があるか(必須)。修正コードと同時に読み、「このテストは修正前に失敗したはずか」を判断してください。
- テスト命名が `対象メンバー名_条件_期待結果` に従っているか(FloatSoda と同じ規約)。**既存テストの一括リネームは求めません。**

---

## 6. namespace / ディレクトリ

C# の namespace と、プロジェクトルート以下の物理ディレクトリ構造を一致させます(FloatSoda と同じ規約)。レビューでは、**namespace とディレクトリの片方だけが変わっていないか**を確認します(必須)。

---

## 7. scope discipline

- Issue / PR の目的に**不要な変更が混ざっていないか**(必須) — unrelated refactoring / rename / cleanup / 依存の追加・削除。混ざっている場合は、別 PR への分離を求めます。
- public API または observable behavior を変更する PR で、**breaking change の有無が明示的に判断され、PR 本文に書かれているか**(必須)。
- レビュアー自身も、スコープ外の改善は「別 Issue 向け」と明示して伝えます(必須)。
