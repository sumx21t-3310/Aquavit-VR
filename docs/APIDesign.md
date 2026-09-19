← [Home](Home.md)

# API Design Guidelines

このドキュメントは、Aquavit VR の API を設計・実装する際の規約を定めます。[FloatSoda の APIDesign](https://github.com/sumx21t-3310/FloatSoda/blob/main/docs/APIDesign.md) のうち、C# の API 設計として共通に使える章を引き継いでいます。FloatSoda の Widget を前提にした章(コンポーネント API、プロパティ命名、ファクトリメソッド)は含めていません。

> **実装状況** — Aquavit VR の公開 API は、まだほとんどありません。このページは、Phase 0 と Phase 1 で最初の API を書くときに迷わないための、最小限の規約です。コード例の型名は説明用で、実在の API とは限りません。DOM の API を C# でどう命名するか(`appendChild` と `AppendChild` のどちらにするかなど)は未決定で、[#10](https://github.com/sumx21t-3310/Aquavit-VR/issues/10) で決めます。

## 1. 2種類の API を区別する

Aquavit VR の API は、由来によって2種類に分かれます。規約の当てはめ方が異なるため、最初にどちらの API かを判断します。

| 種類 | 例 | 設計の基準 |
|---|---|---|
| Web 標準に由来する API | DOM のノード、イベント、Style のプロパティ | Web 標準の意味と挙動に従う。C# の慣習より、Web 標準との対応を優先する |
| Aquavit VR 独自の API | 描画先の設定、LayerTree への変換、Frame Scheduler、ホストとの接続 | このページの規約に従う |

Web 標準に由来する API では、Web 標準と異なる挙動を作りません。差異が必要な場合は、理由を `docs/` か Issue に書き、Issue に `spec-divergence` ラベルを付けます(→ [REVIEW.md](../REVIEW.md) の「3. 仕様の優先順位」)。

## 2. null チェックは `is null` / `is not null` を使う

参照の null 判定には `==` / `!=` 演算子ではなく、パターンマッチの `is null` / `is not null` を使用します。

```csharp
// ✅ 推奨
if (layer is null) return;
if (layer is not null) root.Children.Add(layer);

// ❌ 避ける
if (layer == null) return;
if (layer != null) root.Children.Add(layer);
```

**理由:**

- `==` / `!=` はユーザー定義の演算子オーバーロードに解決される可能性があり、意図しない比較ロジックが実行される恐れがある。一方 `is null` は常に厳密な null 判定を評価するため、型に依存せず安全である。
- `is null` / `is not null` は、「null かどうかを判定する」という意図を明確に表現できる。
- 等値演算子をオーバーロードする `record` や `record struct`(7章・9章)を使うため、この違いが問題になりやすい。

> **補足:** 値型(`record struct` など)の比較や、null 以外の値との比較には従来どおり `==` / `!=` を使用します。本ルールは、**参照の null 判定**に限定した規約です。`== null` / `!= null` は、Roslynator の `RCS1248` がビルド時に検出します。

## 3. Aquavit VR 独自の通知には `event` を使う

Aquavit VR 独自の API では、マルチキャストデリゲート(`event`)で表現できる通知を、`AddListener` / `RemoveListener` のようなリスナーパターンで独自に実装せず、C# の `event` として公開します。

```csharp
// ✅ 推奨: event による通知
public class FrameScheduler
{
    public event Action? FrameCompleted;
}

// ❌ 避ける: リスナーパターンの独自実装
public interface IFrameListener { void OnFrameCompleted(); }

public class FrameScheduler
{
    public void AddListener(IFrameListener listener) { /* ... */ }
    public void RemoveListener(IFrameListener listener) { /* ... */ }
}
```

**理由:**

- C# ではマルチキャストデリゲートが、リスナーの一覧の管理と、通知中の購読解除の安全性(invocation list のスナップショット)を言語レベルで提供している。
- `+=` / `-=` による購読と解除は、C# 開発者にとって最も予測しやすい API である。
- 購読側にインターフェースの実装を強制せず、ラムダ式やメソッド参照を直接渡せる。

**DOM のイベントは、この規約の対象外です。** DOM の `addEventListener` / `removeEventListener` は Web 標準に由来する API であり、capture、bubbling、`once` などの意味を持ちます。C# の `event` では表現できないため、Web 標準のとおりに実装します([#20](https://github.com/sumx21t-3310/Aquavit-VR/issues/20)、[#24](https://github.com/sumx21t-3310/Aquavit-VR/issues/24))。

**補足:**

- 通知元の共通抽象が必要な場合は、通知元側のインターフェースに `event` を宣言する。
- `Dispose()` ではイベントフィールドに `null` を代入し、購読を破棄する。

## 4. 値と設定は不変にする

値オブジェクトと設定オブジェクトのプロパティは、原則として `init` アクセサを使用し、構築後の変更を禁止します。

```csharp
public record RenderTargetOptions
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    public double DevicePixelRatio { get; init; } = 1;
}
```

**DOM のノードは、この規約の対象外です。** DOM は Web 標準で変更できるツリーとして定義されており、子の追加と削除、属性とテキストの変更を受け付けます([#16](https://github.com/sumx21t-3310/Aquavit-VR/issues/16))。

| 対象 | 方針 |
|---|---|
| ジオメトリ、色、長さなどの値 | 不変(`readonly record struct`、7章) |
| オプション、設定、解決済みの Style | 不変(`record` と `init`) |
| DOM のノード | 変更できる(Web 標準のとおり) |
| Layout と Paint の結果 | 外部からは読み取り専用。再計算で新しい結果に置き換える |

## 5. デフォルト値の方針

- 設定オブジェクトのプロパティには**合理的なデフォルト値**を設定し、最小限の記述で使用できるようにする
- 妥当なデフォルトが存在しないプロパティは `required` キーワードで明示する
- Web 標準に由来する値のデフォルトは、Web 標準の初期値(initial value)に合わせる

4章の `RenderTargetOptions` では、`Width` と `Height` に妥当なデフォルトが無いため `required` とし、`DevicePixelRatio` にはデフォルトを設定しています。

## 6. バージョニングと後方互換性

### 6.1 非破壊的変更(マイナーバージョン)

- 新しい型、メンバー、プロパティの追加(デフォルト値あり)
- `required` でないプロパティのオプション化
- Web 標準の対象範囲の拡大(未対応だった機能への対応)

### 6.2 破壊的変更(メジャーバージョン)

- 型、メンバー、プロパティの削除やリネーム
- 型の変更
- `required` の追加
- **observable behavior の変更** — 既存の正しい利用コードから観測できる挙動の変化。DOM の API の挙動、Style の解決、Layout、Paint、Hit Testing、イベントの配送、更新が描画に反映されるタイミングが判定の観点となる。

observable behavior に関しては、**互換性の基準を文書化された契約**(本ドキュメント、`docs/`、Web 標準)とします。契約から逸脱していた挙動を本来の契約へ戻す修正は、挙動が変化しても破壊的変更ではなくバグ修正として扱います。反対に、契約どおりに動いていた挙動を変更する場合は破壊的変更となります。

### 6.3 廃止予定の API の扱い

廃止する API には `[Obsolete]` を付け、代わりに使う API と、削除を予定するバージョンをメッセージに書きます。

```csharp
/// <summary>描画先の幅を取得します。</summary>
[Obsolete("RenderTargetOptions.Width を使用してください。v1.0 で削除予定です。")]
public int TargetWidth { get; init; }
```

### 6.4 開発初期での判定と許容の分離

開発初期であることは、breaking change の**判定**を省略する理由になりません。判定と許容は分けて行います。

1. まず public API / observable behavior に対して、6.1 と 6.2 の基準で breaking change に該当するかを通常どおり判定する
2. そのうえで、開発初期としてその変更を現時点で受け入れるかを別途判断する

判定の結果は PR の本文に記載し(→ [CONTRIBUTING.md](../CONTRIBUTING.md))、breaking change を伴う Issue には `breaking-change` ラベルを付与します。

## 7. ジオメトリオブジェクトには `record struct` を使う

座標・サイズ・余白などのジオメトリ型は `readonly record struct` で定義します。値型のためヒープ割り当てが不要で、Layout の計算時のパフォーマンスに優れます。`record` の等値比較・分解・`with` 式も利用できます。

```csharp
// ✅ 推奨
public readonly record struct Size(double Width, double Height);
public readonly record struct Point(double X, double Y);
public readonly record struct Rect(double X, double Y, double Width, double Height);
public readonly record struct EdgeSizes(double Top, double Right, double Bottom, double Left);
```

`with` 式を使うと、既存の値から一部だけ変えた新しい値を簡潔に作れます。

```csharp
var padding = new EdgeSizes(8, 8, 8, 8);
var wider = padding with { Left = 16, Right = 16 };
```

**ジオメトリ型に `class` や通常の `struct` を使わない理由:**

| | `record struct` | `class` | `struct` |
|---|---|---|---|
| ヒープ割り当て | なし | あり | なし |
| 等値比較 | 値ベース(自動) | 参照ベース | 手動実装が必要 |
| `with` 式 | ✅ | ✅ | ❌ |
| 分解 (`Deconstruct`) | ✅ | 手動 | 手動 |

> **実装状況** — Aquavit VR のジオメトリ型は未実装です。FloatSoda のジオメトリ型(`FloatSoda.Abstractions`)を共有するか、Aquavit VR で独自に定義するかは、Engine の共有境界を決める [#6](https://github.com/sumx21t-3310/Aquavit-VR/issues/6) で判断します。現在の依存ルールでは、参照できる FloatSoda のパッケージは `FloatSoda.Rendering` だけです。

## 8. 実数は `double` を基本とし、Skia 型を公開 API に出さない

### 8.1 実数型の方針

公開 API に現れる実数(座標・サイズ・角度・比率など)には **`double`** を使います。`float` は SkiaSharp との境界でのみ使い、境界での変換はフレームワーク内部で行います。

```csharp
// ✅ 推奨: 公開 API は double
public readonly record struct Size(double Width, double Height);

// ❌ 避ける: 公開 API に float を露出
public readonly record struct Size(float Width, float Height);
```

**理由:**

- C# の浮動小数リテラルはデフォルトで `double` のため、接尾辞 `f` なしで書ける。
- Layout の計算や、角度から行列への変換といった合成計算は、`double` で保持するほうが誤差が蓄積しにくい。
- DOM の座標に関する API(`getBoundingClientRect` が返す `DOMRect` など)は、Web 標準で `double` として定義されている。

**性能に関する補足:** 値オブジェクトのサイズは倍になりますが、UI の用途では実害はありません。SIMD 化されたホットパスなど `float` が正当化される箇所は、公開 API ではなく内部実装に限定します。

### 8.2 Skia 型を公開 API に出さない

`SKCanvas` / `SKPicture` / `SKRect` / `SKColor` などの SkiaSharp 型は、DOM、Style、Layout の公開 API に露出させません。ジオメトリと色は Aquavit VR の型で表し、Skia 型への変換は Paint の段階で行います。

```csharp
// ✅ 推奨: Aquavit VR の型
public Rect BorderBox { get; }

// ❌ 避ける: Skia 型の直接露出
public SKRect BorderBox { get; }
```

**理由:**

- レンダリングのバックエンドを実装の詳細に保つため。FloatSoda も同じ境界を引いており、将来バックエンドが変わっても、DOM、Style、Layout の公開 API を破壊せずに済む。
- DOM と Layout だけを使う利用者とテストに、SkiaSharp への直接の依存を強制しない。

**LayerTree を受け渡す境界は、この規約の対象外です。** Aquavit VR の出力は FloatSoda の LayerTree(`FloatSoda.Rendering.Layers.ILayer`)であり、`ILayer` と `LayerContext` は Skia 型(`SKRect`、`SKCanvas`)を含みます。LayerTree を生成して返す API と、Paint の段階の API では、`FloatSoda.Rendering` の型をそのまま使います。

| 段階 | 公開 API に現れる型 |
|---|---|
| DOM / Style / Layout | Aquavit VR の型だけ |
| Paint / LayerTree への変換 | Aquavit VR の型と、`FloatSoda.Rendering` の型(Skia 型を含む) |

## 9. 共有される設定オブジェクトには `record` を使う

ツリー全体や複数の段階で共有する設定(描画先の設定、既定のフォント設定など)は、`record`(参照型)で定義します。`with` 式で一部だけ変えた設定を派生でき、等値比較によって再計算の要否を判定できます。

```csharp
var highDensity = options with { DevicePixelRatio = 2 };
```

**`record struct` ではなく `record`(参照型)を選ぶ理由:**

共有される設定は、多くの箇所から参照されます。値型にすると、受け渡しのたびにコピーが発生します。参照型の `record` にすると、`with` 式で変形したときだけ新しいインスタンスを生成し、変化のない箇所へは同じ参照を引き渡せます。

小さく、頻繁に生成される値(7章のジオメトリ型)には `record struct` を使い、共有される設定には `record` を使います。

## 10. ドキュメントコメント規約

XML ドキュメントコメントの規約は [DocumentationComments](DocumentationComments.md) を参照してください。

要点は次の通りです。

- アクセス修飾子を問わず、原則としてすべての型およびメンバーに記述する(`public` だけでなく `private` も対象)。
- ドキュメントコメントは正式な API Reference の原稿として扱い、役割・契約・副作用を完結させる。使い方・チュートリアルは `docs/` へ分離する。
- `<example>` は原則として使用しない。

## 関連ページ

- [DocumentationComments](DocumentationComments.md) — ドキュメントコメント規約
- [Architecture](Architecture.md) — FloatSoda との共有境界と依存ルール
- [Home](Home.md) — ドキュメント一覧
