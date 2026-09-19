# Aquavit VR

A web UI framework for VR, powered by the FloatSoda engine.

Aquavit VR は、[FloatSoda](https://github.com/sumx21t-3310/FloatSoda) の描画エンジン(LayerTree 以下)を利用する独立した Web UI フレームワークです。
UI モデルは DOM / CSS / Layout / Paint で構成します。FloatSoda の Widget / Element / RenderObject は使用しません。

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

## Status

開発初期です。ロードマップは [Issues](https://github.com/sumx21t-3310/Aquavit-VR/issues) を参照してください。

## Documentation

ドキュメントの入り口は [docs/Home.md](docs/Home.md) です。

## Build

```bash
dotnet build Aquavit.slnx
```

```bash
dotnet test Aquavit.slnx
```

## License

MIT
