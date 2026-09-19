using FloatSoda.Rendering.Layers;
using SkiaSharp;

// FloatSoda Framework を経由せずに LayerTree を組み、PNG へ書き出す POC サンプルです。
const int width = 512;
const int height = 512;

var outputPath = Path.GetFullPath(args.Length > 0 ? args[0] : "layer-tree.png");

var root = new ContainerLayer();
root.Children.Add(new PictureLayer { Picture = RecordRect(new SKRect(0, 0, width, height), SKColors.White) });

var clip = new ClipRectLayer(new SKRect(64, 64, 448, 448));
var transform = new TransformLayer { Transform = SKMatrix.CreateRotationDegrees(15, width / 2f, height / 2f) };
var opacity = new OpacityLayer { Alpha = 160 };

opacity.Children.Add(new PictureLayer { Picture = RecordRect(new SKRect(96, 96, 416, 416), SKColors.SteelBlue) });
transform.Children.Add(opacity);
clip.Children.Add(transform);
root.Children.Add(clip);

using var surface = SKSurface.Create(new SKImageInfo(width, height));
var context = LayerContext.Create(surface);

root.Layout(context);
root.Paint(context);

using var image = surface.Snapshot();
using var data = image.Encode(SKEncodedImageFormat.Png, 100);
using (var stream = File.Create(outputPath))
{
    data.SaveTo(stream);
}

Console.WriteLine(outputPath);

static SKPicture RecordRect(SKRect rect, SKColor color)
{
    using var recorder = new SKPictureRecorder();
    using var paint = new SKPaint { Color = color, IsAntialias = true };

    recorder.BeginRecording(rect).DrawRect(rect, paint);

    return recorder.EndRecording();
}
