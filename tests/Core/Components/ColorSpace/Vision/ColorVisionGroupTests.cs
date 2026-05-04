using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ColorVisionGroupTests
{
    [Fact]
    public void ColorVisionGroup_HasExpectedValues()
    {
        Assert.Equal(ColorVisionGroup.Normal, ColorVisionMapper.Decompose(ColorVisionType.Normal).Group);
        Assert.Equal(ColorVisionGroup.Protan, ColorVisionMapper.Decompose(ColorVisionType.Protanopia).Group);
        Assert.Equal(ColorVisionGroup.Deutan, ColorVisionMapper.Decompose(ColorVisionType.Deuteranopia).Group);
        Assert.Equal(ColorVisionGroup.Tritan, ColorVisionMapper.Decompose(ColorVisionType.Tritanopia).Group);
        Assert.Equal(ColorVisionGroup.Achroma, ColorVisionMapper.Decompose(ColorVisionType.Achromatopsia).Group);
    }
}
