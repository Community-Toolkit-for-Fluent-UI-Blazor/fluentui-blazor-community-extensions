using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class AccessibilitySafePluginTests
{
    [Fact]
    public void Name_ShouldReturnAccessibilitySafe()
    {
        // Arrange
        var plugin = new AccessibilitySafePlugin();

        // Act
        var name = plugin.Name;

        // Assert
        Assert.Equal("AccessibilitySafe", name);
    }

    [Theory]
    [InlineData(0, new string[] { })]
    [InlineData(1, new[] { "#66C2A5" })]
    [InlineData(3, new[] { "#66C2A5", "#FC8D62", "#8DA0CB" })]
    [InlineData(8, new[] { "#66C2A5", "#FC8D62", "#8DA0CB", "#E78AC3", "#A6D854", "#FFD92F", "#E5C494", "#B3B3B3" })]
    [InlineData(10, new[] { "#66C2A5", "#FC8D62", "#8DA0CB", "#E78AC3", "#A6D854", "#FFD92F", "#E5C494", "#B3B3B3" })]
    public void Generate_ShouldReturnExpectedColors(int steps, string[] expected)
    {
        // Arrange
        var plugin = new AccessibilitySafePlugin();
        var options = new GenerationOptions();

        // Act
        var result = plugin.Generate("#000000", steps, options);

        // Assert
        Assert.Equal(expected, result);
    }
}
