← [Home](Home.md)

# Target Users

Aquavit VR が想定する利用者と、動作を保証する範囲を定めます。何を実装し、何を実装しないかを判断するときの基準です。

> **実装状況** — このページは到達目標です。現在の Aquavit VR は、HTML / CSS / JavaScript をまだ読み込めません。保証の範囲に到達するのは、Phase 4([#5](https://github.com/sumx21t-3310/Aquavit-VR/issues/5))の完了時を見込んでいます。

## 想定する利用者

Web のフロントエンド開発に慣れた開発者です。React で UI を書き、普段どおりにビルドし、その成果物を VR のオーバーレイとして表示します。

この利用者は、Aquavit VR のドキュメントを読んで書き方を学びません。すでに知っている Web の書き方で書きます。このため、利用者との契約は Aquavit VR のドキュメントではなく、**「ブラウザならこう動く」という利用者の前提**です。

FloatSoda の想定する利用者(コードを AI に書かせる VRChatter など)とは異なります。FloatSoda は独自の API を見つけやすく、誤用しにくくすることを重視します。Aquavit VR は、利用者がすでに持っている前提を裏切らないことを重視します。

## 開発の流れ

利用者は、普段の Web 開発と同じように**ブラウザで開発します**。開発サーバー、保存した変更の即時反映、ブラウザの開発者ツールは、利用者の手元の環境のものをそのまま使います。仕上がった UI をビルドし、配布物を Aquavit VR に読み込ませて、VR で確認します。

```text
ブラウザで開発する  →  ビルドする  →  Aquavit VR で配布物を読み込む  →  VR で確認する
```

この流れは、「ブラウザで動く使い方が、Aquavit VR でも同じように動く」という保証を前提にしています。ブラウザと挙動が異なると、利用者は VR の中で原因を調べることになり、そこにはブラウザの開発者ツールがありません。保証の範囲でブラウザと同じように動くことが、開発体験の土台です。

**VR の中で表示を見ながらコードを書き換える開発は、Aquavit VR の対象に含めません。** この開発体験は FloatSoda が担います。FloatSoda は .NET のホットリロード(`dotnet watch`)に対応しており、コードの変更を検知して Widget ツリーを再ビルドします。

| 開発のしかた | 選ぶもの |
|---|---|
| React と Web の書き方で UI を作り、ブラウザで開発する | Aquavit VR |
| C# で UI を作り、VR の中で表示を見ながら書き換える | FloatSoda |

どちらも、同じ FloatSoda の描画エンジンで VR のオーバーレイを表示します。

## 入力

Aquavit VR が受け取るのは、**ビルド済みの配布物**(`dist/` に出力された HTML、CSS、バンドル済みの JavaScript)です。

- JavaScript は、バンドラーがまとめ、minify したコードです。React 本体もその中に含まれます。
- Aquavit VR は、配布物がどの Web API を使うかを選べません。
- UI ライブラリが DOM を直接操作するコードも、React のコードと区別せずに実行します。

配布物を読み込む仕組みは [#31](https://github.com/sumx21t-3310/Aquavit-VR/issues/31) で整備します。

## 動作を保証する範囲

次の組み合わせで作った UI が動くことを保証します。**これ以外の組み合わせは保証しません。**

| 対象 | 内容 | 確認したバージョン |
|---|---|---|
| React + DaisyUI | React で書いた UI に、DaisyUI のコンポーネントを使う。DaisyUI は Tailwind CSS のプラグインで、JavaScript を含まない | React 19.3 / DaisyUI 5.7 / Tailwind CSS 4.3 |
| Bootstrap | Bootstrap の CSS と、`bootstrap.js`(Popper を含む)を使う | Bootstrap 5.3 / Popper 2.11 |

バージョンは 2026-09-20 時点の最新です。いずれも MIT ライセンスです。

「動く」の基準は、ブラウザでの表示と挙動です。ブラウザで動く使い方が、Aquavit VR でも同じように動くことを指します。ブラウザでも動かない使い方は、保証の対象に含めません。たとえば、Bootstrap の公式ドキュメントは、Bootstrap の JavaScript と React が同じ DOM 要素を書き換えると不具合が起きると説明しています。この組み合わせは、Aquavit VR でも保証しません。

### この範囲を選んだ理由

- 対象が公開されており、コンポーネントを数えられる。公式ドキュメントのコンポーネント一覧を、そのまま検証の題材にできる。
- 正解をブラウザで確かめられる。
- 性質の異なる2つを含む。DaisyUI は JavaScript を持たず、新しい CSS の機能に依存する。`bootstrap.js` は CSS が保守的で、JavaScript から DOM を直接操作する。

「Web 標準をどこまで実装するか」という問いには終わりがありません。保証の範囲を具体的なライブラリで定めることで、実装する機能を「この範囲を動かすのに必要か」で判断できます。

## この想定が設計に与えている影響

| 影響を受ける箇所 | 内容 |
|---|---|
| CSS の対象範囲([#11](https://github.com/sumx21t-3310/Aquavit-VR/issues/11) 以降) | Tailwind CSS 4 は、対応するブラウザの下限を Chrome 111 / Safari 16.4 / Firefox 128 としており、CSS 変数に強く依存します。DaisyUI を動かすには、この世代の CSS の機能が必要です。Phase 1 は最小限の CSS から始め、この水準へ向けて広げます |
| JavaScript Runtime の選定([#22](https://github.com/sumx21t-3310/Aquavit-VR/issues/22)) | バンドラーが出力した、minify 済みの現行の JavaScript を実行できることが条件に加わります |
| JavaScript Binding([#23](https://github.com/sumx21t-3310/Aquavit-VR/issues/23)、[#24](https://github.com/sumx21t-3310/Aquavit-VR/issues/24)) | React が必要とする DOM API に加えて、`bootstrap.js` と Popper が使う DOM API が対象になります |
| Frame Scheduler([#18](https://github.com/sumx21t-3310/Aquavit-VR/issues/18)) | 要素の位置を計算するライブラリは、JavaScript の実行の途中で要素の矩形を読み取ります。フレーム単位で更新をまとめる設計に加えて、Layout をその場で確定させる手段が必要になる見込みです |
| 配布物の読み込み([#31](https://github.com/sumx21t-3310/Aquavit-VR/issues/31)) | 開発はブラウザで行うため、Aquavit VR に開発サーバーへの接続や、変更の即時反映の仕組みは必要ありません。#31 の受け入れ条件にある「開発時と配布時の読み込み処理を分けること」は、この前提で内容を見直します |
| 互換性プロファイル([#28](https://github.com/sumx21t-3310/Aquavit-VR/issues/28)) | 対象のフレームワークは React、互換性の範囲はこのページの「動作を保証する範囲」です |

## 未決定の項目

次の項目は決まっていません。該当する Issue に着手するまでに決めます。

| 項目 | 内容 |
|---|---|
| 到達の順序 | 保証の範囲へ、どの順で到達するか。候補は「React + DaisyUI → Bootstrap の CSS → `bootstrap.js`」の順 |
| 文字入力 | `input` や `textarea` への文字入力(キャレット、選択範囲、IME、VR でのキーボード)を保証に含めるか。表示と、クリックで完結する操作だけを先に保証する案がある |
| 保証の範囲の外の挙動 | 未対応の API が呼ばれたときに、API の名前を示して失敗するか、何も返さずに処理を続けるか |
| 検証の方法 | 保証をどう検証するか。候補は、同じ配布物をブラウザと Aquavit VR の両方で動かして、DOM の状態と PNG を比較する差分テスト |
| 開発者への出力 | JavaScript の `console` の出力と、未対応の API が呼ばれたことの警告を、開発者の手元へどう返すか。VR の中にはブラウザの開発者ツールが無いため、出力と警告が原因を調べる手掛かりになる |
| 配布物の再読み込み | 配布物が更新されたときに、Aquavit VR が自動で読み込み直すか。利用者がビルドを監視モードで動かしている場合に、VR での確認が速くなる |
| バージョンの追従 | 対象のライブラリが更新されたときに、保証するバージョンをどう更新するか |
| viewport | メディアクエリが参照する viewport の大きさを、オーバーレイの描画先からどう決めるか |

## 関連ページ

- [Home](Home.md) — ロードマップ(Phase)
- [ReferencePolicy](ReferencePolicy.md) — 参照してよい資料とコードの範囲
- [Architecture](Architecture.md) — FloatSoda との共有境界
