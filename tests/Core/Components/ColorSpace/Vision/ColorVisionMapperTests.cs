using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ColorVisionMapperTests
{
    [Theory]
    [InlineData(ColorVisionType.Normal, 0.0)]
    [InlineData(ColorVisionType.Protanopia, 1.0)]
    [InlineData(ColorVisionType.Protanomaly, 0.5)]
    [InlineData(ColorVisionType.Deuteranopia, 1.0)]
    [InlineData(ColorVisionType.Deuteranomaly, 0.5)]
    [InlineData(ColorVisionType.Tritanopia, 1.0)]
    [InlineData(ColorVisionType.Tritanomaly, 0.5)]
    [InlineData(ColorVisionType.Achromatopsia, 1.0)]
    [InlineData(ColorVisionType.Achromatomaly, 0.5)]
    public void ColorVisionMapper_Decompose_ReturnsExpectedSeverity(ColorVisionType type, double severity)
    {
        var (actualGroup, actualSeverity) = ColorVisionMapper.Decompose(type);

        Assert.Equal(severity, actualSeverity, 12);
        if (type == ColorVisionType.Normal)
        {
            Assert.Equal(ColorVisionGroup.Normal, actualGroup);
        }
        else
        {
            Assert.NotEqual(ColorVisionGroup.Normal, actualGroup);
        }
    }

    [Fact]
    public void ColorVisionMapper_Decompose_ReturnsNormalForUnknown()
    {
        var (actualGroup, actualSeverity) = ColorVisionMapper.Decompose((ColorVisionType)0);

        Assert.Equal(ColorVisionGroup.Normal, actualGroup);
        Assert.Equal(0.0, actualSeverity, 12);
    }
}
