using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;

public class MonochromaticPluginTests
{
    [Fact]
    public void Name_ShouldReturnMonochromatic()
    {
        var plugin = new MonochromaticPlugin();
        Assert.Equal("Monochromatic", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new MonochromaticPlugin();
        var options = new GenerationOptions();
        Assert.Throws<ArgumentException>(() => plugin.Generate("invalid", 5, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new MonochromaticPlugin();
        var options = new GenerationOptions
        {
            SaturationMin = 0.2,
            SaturationMax = 0.8,
            LightnessMin = 0.1,
            LightnessMax = 0.9,
            Reverse = false
        };
        var result = plugin.Generate("#3366FF", 4, options);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Generate_ShouldReverseColors_WhenReverseOptionIsTrue()
    {
        var plugin = new MonochromaticPlugin();
        var options = new GenerationOptions
        {
            SaturationMin = 0.2,
            SaturationMax = 0.8,
            LightnessMin = 0.1,
            LightnessMax = 0.9,
            Reverse = true
        };

        var normal = plugin.Generate("#3366FF", 3, new GenerationOptions
        {
            SaturationMin = 0.2,
            SaturationMax = 0.8,
            LightnessMin = 0.1,
            LightnessMax = 0.9,
            Reverse = false
        });

        var reversed = new List<string>(plugin.Generate("#3366FF", 3, options));
        reversed.Reverse();

        Assert.Equal(normal, reversed);
    }
}
