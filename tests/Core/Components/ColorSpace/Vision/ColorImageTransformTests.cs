using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ColorImageTransformTests
{
    [Fact]
    public void ColorImageTransform_ByteArray_ThrowsForInvalidLength()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);

        Assert.Throws<ArgumentException>(() => ColorImageTransform.TransformImage(new byte[3], ColorVisionType.Protanopia, space));
    }

    [Fact]
    public void ColorImageTransform_ByteArray_ReturnsOriginalForNormal()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var pixels = new byte[] { 10, 20, 30, 255 };

        var result = ColorImageTransform.TransformImage(pixels, ColorVisionType.Normal, space);

        Assert.Same(pixels, result);
    }

    [Fact]
    public void ColorImageTransform_ByteArray_TransformsPixels()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var pixels = new byte[] { 255, 0, 0, 255, 0, 255, 0, 255 };

        var result = ColorImageTransform.TransformImage(pixels, ColorVisionType.Protanopia, space);

        Assert.NotSame(pixels, result);
        Assert.Equal(pixels.Length, result.Length);
    }

    [Fact]
    public void ColorImageTransform_IntArray_ReturnsOriginalForNormal()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var pixels = new[] { unchecked((int)0xFF112233), unchecked((int)0xFF445566) };

        var result = ColorImageTransform.TransformImage(pixels, ColorVisionType.Normal, space);

        Assert.Same(pixels, result);
    }

    [Fact]
    public void ColorImageTransform_IntArray_TransformsPixels()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var pixels = new[] { unchecked((int)0xFF112233), unchecked((int)0xFF445566) };

        var result = ColorImageTransform.TransformImage(pixels, ColorVisionType.Tritanopia, space);

        Assert.NotSame(pixels, result);
        Assert.Equal(pixels.Length, result.Length);
    }
}
