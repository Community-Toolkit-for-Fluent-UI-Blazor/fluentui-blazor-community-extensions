using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class PastelPluginTests
{
    [Fact]
    public void Name_ShouldReturnPastel()
    {
        // Arrange
        var plugin = new PastelPlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("Pastel", name);
    }

    [Fact]
    public void Generate_InvalidBaseColor_ThrowsArgumentException()
    {
        // Arrange
        var plugin = new PastelPlugin();
        var invalidColor = "not-a-color";
        var options = new GenerationOptions();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            plugin.Generate(invalidColor, 5, options)
        );
        Assert.Contains("Base color must be a valid hex color code.", ex.Message);
    }

    [Fact]
    public void Generate_ValidBaseColor_CallsColorUtilsAndReturnsGradient()
    {
        // Arrange
        var plugin = new PastelPlugin();
        var baseColor = "#ff0000";
        var options = new GenerationOptions();
        var steps = 3;

        var result = plugin.Generate(baseColor, steps, options);

        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);

        foreach (var color in result)
        {
            Assert.False(string.IsNullOrWhiteSpace(color));
        }
    }
}
