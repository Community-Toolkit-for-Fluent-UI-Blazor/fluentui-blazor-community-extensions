using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;
public class ColorPaletteEasingTests
{
    [Fact]
    public void Enum_Contains_All_Expected_Values()
    {
        Assert.Equal(0, (int)ColorPaletteEasing.Linear);
        Assert.Equal(1, (int)ColorPaletteEasing.ExponentialIn);
        Assert.Equal(2, (int)ColorPaletteEasing.ExponentialOut);
        Assert.Equal(3, (int)ColorPaletteEasing.Sine);
    }

    [Theory]
    [InlineData(ColorPaletteEasing.Linear)]
    [InlineData(ColorPaletteEasing.ExponentialIn)]
    [InlineData(ColorPaletteEasing.ExponentialOut)]
    [InlineData(ColorPaletteEasing.Sine)]
    public void Enum_Can_Be_Assigned(ColorPaletteEasing easing)
    {
        var options = new GenerationOptions { Easing = easing };
        Assert.Equal(easing, options.Easing);
    }
}
