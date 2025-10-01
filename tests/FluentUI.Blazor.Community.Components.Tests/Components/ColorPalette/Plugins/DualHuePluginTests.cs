using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class DualHuePluginTests
{
    [Fact]
    public void Name_ShouldReturnDualHue()
    {
        var plugin = new DualHuePlugin("#FF0000");
        Assert.Equal("DualHue", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new DualHuePlugin("#00FF00");
        var options = new GenerationOptions();
        Assert.Throws<ArgumentException>(() => plugin.Generate("invalid", 4, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new DualHuePlugin("#00FF00");
        var options = new GenerationOptions();
        var result = plugin.Generate("#0000FF", 6, options);
        Assert.Equal(6, result.Count);
    }
}
