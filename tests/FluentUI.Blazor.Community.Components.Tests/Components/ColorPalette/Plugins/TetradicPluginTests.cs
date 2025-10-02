using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class TetradicPluginTests
{
    [Fact]
    public void Name_ShouldReturnTetradic()
    {
        var plugin = new TetradicPlugin();
        Assert.Equal("Tetradic", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_ForInvalidBaseColor()
    {
        var plugin = new TetradicPlugin();
        var options = new GenerationOptions();

        Assert.Throws<ArgumentException>(() =>
            plugin.Generate("not-a-color", 8, options));
    }

    [Theory]
    [InlineData("#FF0000", 4)]
    [InlineData("#00FF00", 8)]
    [InlineData("#0000FF", 12)]
    public void Generate_ShouldReturnCorrectNumberOfColors(string baseColor, int steps)
    {
        var plugin = new TetradicPlugin();
        var options = new GenerationOptions();

        var result = plugin.Generate(baseColor, steps, options);

        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);
    }

    [Fact]
    public void Generate_ShouldDistributeColorsAcrossTetrad()
    {
        var plugin = new TetradicPlugin();
        var options = new GenerationOptions();
        var baseColor = "#FF0000";
        var steps = 8;

        var result = plugin.Generate(baseColor, steps, options);

        Assert.Equal(steps, result.Count);
    }
}
