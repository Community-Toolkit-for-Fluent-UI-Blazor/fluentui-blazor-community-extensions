using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette.Plugins;
public class DesaturatePluginTests
{
    [Fact]
    public void Name_ShouldBeDesaturate()
    {
        var plugin = new DesaturatePlugin();
        Assert.Equal("Desaturate", plugin.Name);
    }

    [Fact]
    public void Generate_ShouldThrowArgumentException_WhenBaseColorIsInvalid()
    {
        var plugin = new DesaturatePlugin();
        var options = new GenerationOptions();

        Assert.Throws<ArgumentException>(() =>
            plugin.Generate("not-a-color", 5, options)
        );
    }

    [Theory]
    [InlineData("#FF0000", 0.0)] // amount = 0, couleur inchangée
    [InlineData("#FF0000", 1.0)] // amount = 1, couleur désaturée à fond
    [InlineData("#00FF00", 0.5)] // amount = 0.5, désaturation partielle
    public void Generate_ShouldReturnGradient_WithExpectedCount(string baseColor, double amount)
    {
        var plugin = new DesaturatePlugin(amount);
        var options = new GenerationOptions();
        int steps = 4;

        var result = plugin.Generate(baseColor, steps, options);

        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);
        foreach (var color in result)
        {
            Assert.False(string.IsNullOrWhiteSpace(color));
        }
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(2.0)]
    public void Generate_ShouldClampAmount_BetweenZeroAndOne(double amount)
    {
        var plugin = new DesaturatePlugin(amount);
        var options = new GenerationOptions();
        int steps = 3;

        // Doit fonctionner sans exception et retourner le bon nombre de couleurs
        var result = plugin.Generate("#123456", steps, options);

        Assert.NotNull(result);
        Assert.Equal(steps, result.Count);
    }
}
