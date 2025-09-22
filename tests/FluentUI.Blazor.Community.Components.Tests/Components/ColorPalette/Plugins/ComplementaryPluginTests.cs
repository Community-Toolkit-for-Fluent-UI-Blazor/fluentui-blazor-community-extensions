using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class ComplementaryPluginTests
{
    [Fact]
    public void Name_ShouldReturnComplementary()
    {
        var plugin = new ComplementaryPlugin();
        Assert.Equal("Complementary", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new ComplementaryPlugin();
        var options = new GenerationOptions();
        Assert.Throws<ArgumentException>(() => plugin.Generate("invalid", 5, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        var plugin = new ComplementaryPlugin();
        var options = new GenerationOptions();
        var result = plugin.Generate("#3366FF", 4, options);
        Assert.Equal(4, result.Count);
    }
}
