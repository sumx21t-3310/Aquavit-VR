← [Home](Home.md)

# Reference Policy

Aquavit VR を実装するときに、どの資料とコードを参照してよいかを定めます。人間のコントリビューターと、コーディングエージェントの両方が対象です。

## 目的

Aquavit VR は MIT ライセンスで配布します。GPL などの copyleft ライセンスのコードを写したり移植したりすると、その部分を MIT で配布できなくなります。プログラミング言語を C# に変えた移植も、元のコードの派生物です。

このポリシーの目的は、**参照していないことを、後から示せる状態を保つこと**です。そのために、参照先を「開いてよいもの」と「開かないもの」の2つに分けます。「読むのはよいが移植は禁止」という中間の区分は設けません。コーディングエージェントは読んだコードに似た出力を生成しやすく、写していないことを示す手段が無いためです。

著作権が保護するのはコード、その構造、コメント、テストケースなどの表現です。仕様、アルゴリズムの考え方、観測できる挙動は保護の対象ではありません。このため、開かない実装についても、設計を説明した文書は読めます。

## 開いてよいもの

| 参照先 | ライセンス | 用途 |
|---|---|---|
| WHATWG の仕様(DOM、HTML)、W3C の CSS 仕様、MDN | 仕様 | 挙動を決める第一の情報源 |
| [web-platform-tests](https://github.com/web-platform-tests/wpt) | BSD-3-Clause | テストケースの出どころ |
| [FloatSoda](https://github.com/sumx21t-3310/FloatSoda) | MIT | LayerTree と描画 |
| [Flutter](https://github.com/flutter/flutter) | BSD-3-Clause | FloatSoda が参考にしている描画とレイアウトの設計 |
| [Ladybird](https://github.com/LadybirdBrowser/ladybird) | BSD-2-Clause | ブラウザエンジン全体の実装例 |
| [litehtml](https://github.com/litehtml/litehtml) | BSD-3-Clause | 軽量な HTML / CSS レンダラーの実装例 |
| [Yoga](https://github.com/facebook/yoga)、[Taffy](https://github.com/DioxusLabs/taffy) | MIT | Flex Layout の実装例 |
| [AngleSharp](https://github.com/AngleSharp/AngleSharp)、[AngleSharp.Css](https://github.com/AngleSharp/AngleSharp.Css)、[ExCSS](https://github.com/TylerBrinks/ExCSS) | MIT | C# の HTML / CSS パーサー |

ライセンスは 2026-09-20 に、各リポジトリの GitHub 上の表示で確認しました。web-platform-tests は `LICENSE.md` の本文で確認しました。

開いてよい参照先でも、次を守ります。

- 開くファイルのヘッダーにライセンスの記載がある場合は、そのファイルのライセンスを確認します。リポジトリの表示と異なるライセンスのファイルは開きません。
- コードを移植した場合は、元のプロジェクト、ライセンス、著作権表示を `THIRD_PARTY_NOTICES.md` に記録します。このファイルは、最初に移植した PR で作ります。
- 参照したプロジェクトとライセンスを、PR の本文に書きます。

## 開かないもの

次の実装は、ソースコード、テスト、Issue、Pull Request、リポジトリ内のドキュメントを開きません。記事やチャットに引用されたコード片も対象です。

| 参照先 | 理由 |
|---|---|
| [WebF](https://github.com/openwebf/webf) とその fork | GPL-3.0 |
| [Kraken](https://github.com/openkraken/kraken) | Apache-2.0 ですが、WebF の前身で同じ系譜のコードです。WebF を参照していないという説明が弱くなるため、開きません |
| Chromium の Blink(`third_party/blink`) | リポジトリの表示は BSD-3-Clause ですが、DOM と Layout のファイルは LGPL です。2026-09-20 に `core/dom/node.cc` と `core/layout/layout_box.cc` のヘッダーで確認しました |
| WebKit | LGPL と BSD のファイルが混在しています |
| Servo、Gecko | MPL-2.0。ファイル単位の copyleft です |

Issue と Pull Request を含めるのは、コード片と差分が議論に混ざるためです。

これらの実装についても、次の資料は読めます。

- WebF の公式サイトにある製品情報(何ができるか、どのフレームワークが動くか)
- Chromium、WebKit、Servo、Gecko の設計ドキュメントと公式ブログ。コードの引用が中心のページは開きません。

実装を読んで確かめたいときは、Ladybird と web-platform-tests を使います。

## 表に無い参照先

上の2つの表に無い実装は、**開く前にオーナーに確認します**。確認では次を調べます。

1. リポジトリのライセンスが MIT、BSD、Apache-2.0 のいずれかであること
2. 参照したいファイルのヘッダーが、リポジトリの表示と同じライセンスであること
3. 「開かないもの」の表にある実装の fork や移植ではないこと

3つとも満たす参照先は、「開いてよいもの」の表に確認日とともに追加します。

## 接触の記録

「開かないもの」に意図せず触れた場合は、隠さずに記録します。触れた日、触れた範囲、その後に書いたコードとの関係を、オーナーに報告してください。記録があれば、影響する範囲を特定して書き直せます。

| 日付 | 内容 |
|---|---|
| 2026-09-20 | このポリシーを作るために、エージェント(Claude Code)が GitHub API で WebF のリポジトリ情報と `LICENSE` の本文を取得しました。ソース、テスト、Issue は開いていません。同日、Blink の 2 ファイルについて、ライセンスを確認するために先頭のヘッダー部分を取得しました。ヘッダーより後ろのコードは表示していません。 |

> **実装状況** — このポリシーは文書で運用しています。URL やコマンドを機械的に遮断する仕組みは入れていません。エージェントが「開かないもの」に触れかけた事例が出た時点で、追加を検討します。
