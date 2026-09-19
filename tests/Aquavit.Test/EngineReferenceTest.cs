using FloatSoda.Rendering.Layers;

namespace Aquavit.Test;

public class EngineReferenceTest
{
    [Fact]
    public void Children_Aquavit経由でLayerを追加_HasChildrenがtrue()
    {
        var root = new ContainerLayer();
        root.Children.Add(new ContainerLayer());

        Assert.True(root.HasChildren);
    }
}
