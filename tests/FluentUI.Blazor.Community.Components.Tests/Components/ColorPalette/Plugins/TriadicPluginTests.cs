using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;

public class TriadicPluginTests
{
    [Fact]
    public void Name_ShouldReturn_Triadic()
    {
        // Arrange
        var plugin = new TriadicPlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("Triadic", name);
    }

    [Fact]
    public void Generate_ShouldReturn_CorrectNumberOfColors()
    {
        // Arrange
        var plugin = new TriadicPlugin();
        var baseColor = "#3366FF";
        var steps = 9;
        var options = new GenerationOptions();

        // Act
        var result = plugin.Generate(baseColor, steps, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);
    }

    [Fact]
    public void Generate_ShouldDistributeColorsAcrossTriad()
    {
        // Arrange
        var plugin = new TriadicPlugin();
        var baseColor = "#3366FF";
        var steps = 6;
        var options = new GenerationOptions();

        // Act
        var result = plugin.Generate(baseColor, steps, options);

        Assert.Equal(6, result.Count);
        Assert.NotEqual(result[0], result[2]);
        Assert.NotEqual(result[2], result[4]);
        Assert.NotEqual(result[0], result[4]);
    }

    [Fact]
    public void Generate_WithInvalidColor_ShouldThrow()
    {
        // Arrange
        var plugin = new TriadicPlugin();
        var invalidColor = "not-a-color";
        var steps = 6;
        var options = new GenerationOptions();

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => plugin.Generate(invalidColor, steps, options));
    }
}
