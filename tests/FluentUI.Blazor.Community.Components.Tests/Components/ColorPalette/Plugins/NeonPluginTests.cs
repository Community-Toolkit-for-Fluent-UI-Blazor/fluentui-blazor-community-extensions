using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class NeonPluginTests
{
    [Fact]
    public void Name_ShouldReturnNeon()
    {
        // Arrange
        var plugin = new NeonPlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("Neon", name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        // Arrange
        var plugin = new NeonPlugin();
        var invalidColor = "not-a-color";
        var options = new GenerationOptions();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => plugin.Generate(invalidColor, 5, options));
    }

    [Fact]
    public void Generate_ShouldReturnCorrectNumberOfColors()
    {
        // Arrange
        var plugin = new NeonPlugin();
        var baseColor = "#FF00FF";
        var steps = 6;
        var options = new GenerationOptions();

        // Act
        var result = plugin.Generate(baseColor, steps, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);
    }
}
