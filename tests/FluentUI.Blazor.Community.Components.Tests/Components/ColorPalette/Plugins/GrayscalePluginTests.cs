using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class GrayscalePluginTests
{
    [Fact]
    public void Name_ShouldReturnGreyscale()
    {
        var plugin = new GrayscalePlugin();
        Assert.Equal("Greyscale", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new GrayscalePlugin();
        var options = new GenerationOptions();
        var result = plugin.Generate("#000000", 5, options);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void Generate_ShouldReturnBlackAndWhiteForTwoSteps()
    {
        var plugin = new GrayscalePlugin();
        var options = new GenerationOptions();
        var result = plugin.Generate("#000000", 2, options);
        Assert.Equal("#000000", result[0]);
        Assert.Equal("#ffffff", result[1], ignoreCase: true);
    }
}
