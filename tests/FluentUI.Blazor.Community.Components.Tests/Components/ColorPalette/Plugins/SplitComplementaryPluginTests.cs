using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;

public class SplitComplementaryPluginTests
{
    [Fact]
    public void Name_ShouldReturnSplitComplementary()
    {
        // Arrange
        var plugin = new SplitComplementaryPlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("SplitComplementary", name);
    }

    [Fact]
    public void Generate_ShouldReturnPalette_WithValidBaseColor()
    {
        // Arrange
        var plugin = new SplitComplementaryPlugin();
        var baseColor = "#3498db"; 
        var steps = 9;
        var options = new GenerationOptions();

        // Act
        var palette = plugin.Generate(baseColor, steps, options);

        // Assert
        Assert.NotNull(palette);
        Assert.Equal(9, palette.Count);
        Assert.All(palette, color => Assert.False(string.IsNullOrWhiteSpace(color)));
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WithInvalidBaseColor()
    {
        // Arrange
        var plugin = new SplitComplementaryPlugin();
        var invalidColor = "not-a-color";
        var steps = 6;
        var options = new GenerationOptions();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => plugin.Generate(invalidColor, steps, options));
    }

    [Fact]
    public void Generate_ShouldDistributeStepsAcrossThreeGradients()
    {
        // Arrange
        var plugin = new SplitComplementaryPlugin();
        var baseColor = "#ff0000";
        var steps = 12;
        var options = new GenerationOptions();

        // Act
        var palette = plugin.Generate(baseColor, steps, options);

        // Assert
        Assert.Equal(12, palette.Count);
    }
}
