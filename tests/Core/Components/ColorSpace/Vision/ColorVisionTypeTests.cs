using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ColorVisionTypeTests
{
    [Fact]
    public void ColorVisionType_HasExpectedValues()
    {
        var values = Enum.GetValues<ColorVisionType>();

        Assert.Equal(9, values.Length);
        Assert.Contains(ColorVisionType.Normal, values);
        Assert.Contains(ColorVisionType.Protanopia, values);
        Assert.Contains(ColorVisionType.Deuteranopia, values);
        Assert.Contains(ColorVisionType.Tritanopia, values);
        Assert.Contains(ColorVisionType.Achromatopsia, values);
    }
}
