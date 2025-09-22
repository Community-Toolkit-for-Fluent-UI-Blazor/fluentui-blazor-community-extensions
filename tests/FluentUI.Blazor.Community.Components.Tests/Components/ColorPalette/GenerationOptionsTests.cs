using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;

public class GenerationOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new GenerationOptions();

        Assert.Equal(0, options.SaturationMin);
        Assert.Equal(1.0, options.SaturationMax);
        Assert.Equal(0.05, options.LightnessMin);
        Assert.Equal(0.95, options.LightnessMax);
        Assert.False(options.Reverse);
        Assert.Equal(ColorPaletteEasing.Linear, options.Easing);
    }

    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        var options = new GenerationOptions
        {
            SaturationMin = 0.2,
            SaturationMax = 0.8,
            LightnessMin = 0.1,
            LightnessMax = 0.9,
            Reverse = true,
            Easing = ColorPaletteEasing.ExponentialOut
        };

        Assert.Equal(0.2, options.SaturationMin);
        Assert.Equal(0.8, options.SaturationMax);
        Assert.Equal(0.1, options.LightnessMin);
        Assert.Equal(0.9, options.LightnessMax);
        Assert.True(options.Reverse);
        Assert.Equal(ColorPaletteEasing.ExponentialOut, options.Easing);
    }
}
