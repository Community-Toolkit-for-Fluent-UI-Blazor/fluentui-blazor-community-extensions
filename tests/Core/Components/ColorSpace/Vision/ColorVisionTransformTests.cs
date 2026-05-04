using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ColorVisionTransformTests
{
    [Fact]
    public void ColorVisionTransform_ReturnsOriginalForNormal()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var color = new Srgb8(10, 20, 30);

        var result = ColorVisionTransform.Apply(color, ColorVisionType.Normal, space);

        Assert.Equal(color, result);
    }

    [Fact]
    public void ColorVisionTransform_TransformsForDeficiency()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var color = new Srgb8(120, 80, 40);

        var result = ColorVisionTransform.Apply(color, ColorVisionType.Deuteranopia, space);

        Assert.NotEqual(color, result);
    }

    [Fact]
    public void ColorVisionTransform_Achromatopsia_ProducesGrayscale()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var color = new Srgb8(200, 100, 50);

        var result = ColorVisionTransform.Apply(color, ColorVisionType.Achromatopsia, space);

        Assert.Equal(result.R, result.G);
        Assert.Equal(result.G, result.B);
    }
}
