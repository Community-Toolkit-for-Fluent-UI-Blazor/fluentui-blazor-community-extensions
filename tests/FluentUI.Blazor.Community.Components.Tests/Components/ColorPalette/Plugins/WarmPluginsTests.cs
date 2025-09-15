using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;

public class WarmPluginsTests
{
    [Fact]
    public void Name_ShouldReturnWarm()
    {
        // Arrange
        var plugin = new WarmPlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("Warm", name);
    }

    [Fact]
    public void Generate_ShouldCallColorUtilsWithCorrectParameters()
    {
        // Arrange
        var plugin = new WarmPlugin();
        var options = new GenerationOptions();
        var steps = 5;

        // Act
        var result = plugin.Generate("#000000", steps, options);

        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);

        foreach (var color in result)
        {
            Assert.StartsWith("#", color);
            Assert.Equal(7, color.Length);
        }

        Assert.Equal("#FF0000", result[0]);
        Assert.Equal("#FF3600", result[1]);
        Assert.Equal("#FF6C00", result[2]);
        Assert.Equal("#FFA100", result[3]);
        Assert.Equal("#FFD700", result[4]);
    }
}
