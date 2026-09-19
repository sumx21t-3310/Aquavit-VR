using FloatSoda.Rendering.Layers;

namespace Aquavit.Test;

public class EngineReferenceTest
{
    [Fact]
    public void Children_LayerAddedThroughAquavitReference_HasChildrenIsTrue()
    {
        var root = new ContainerLayer();
        root.Children.Add(new ContainerLayer());

        Assert.True(root.HasChildren);
    }
}
