using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class AnalogousPluginTests
{
    [Fact]
    public void Name_ShouldReturnAnalogous()
    {
        var plugin = new AnalogousPlugin();
        Assert.Equal("Analogous", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new AnalogousPlugin();
        var options = new GenerationOptions();
        Assert.Throws<ArgumentException>(() => plugin.Generate("invalid", 5, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new AnalogousPlugin();
        var options = new GenerationOptions();
        var result = plugin.Generate("#3366FF", 6, options);
        Assert.Equal(6, result.Count);
    }
}
