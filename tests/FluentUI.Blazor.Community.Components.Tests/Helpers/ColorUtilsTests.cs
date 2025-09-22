using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Helpers;
public class ColorUtilsTests
{
    [Theory]
    [InlineData("#FFF", true)]
    [InlineData("#ffffff", true)]
    [InlineData("123456", true)]
    [InlineData("#123", true)]
    [InlineData("abc", true)]
    [InlineData("#abcd", false)]
    [InlineData("#12", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidHex_WorksAsExpected(string? input, bool expected)
    {
        Assert.Equal(expected, ColorUtils.IsValidHex(input));
    }

    [Theory]
    [InlineData("#FFF", true)]
    [InlineData("red", true)] // suppose que CssColorNamesUtils.TryGetHex("red", out _) == true
    [InlineData("unknowncolor", false)]
    [InlineData("#123456", true)]
    [InlineData(null, false)]
    public void IsValidHexOrCssName_WorksAsExpected(string? input, bool expected)
    {
        Assert.Equal(expected, ColorUtils.IsValidHexOrCssName(input));
    }

    [Theory]
    [InlineData("#FFF", "#FFFFFF")]
    [InlineData("red", "#FF0000")] // suppose que CssColorNamesUtils.TryGetHex("red", out var hex) => "#FF0000"
    [InlineData("invalid", "#000000")]
    [InlineData(null, "#000000")]
    [InlineData("", "#000000")]
    public void NormalizeToHex_WorksAsExpected(string? input, string expected)
    {
        Assert.Equal(expected, ColorUtils.NormalizeToHex(input));
    }

    [Theory]
    [InlineData("#123", "#112233")]
    [InlineData("123", "#112233")]
    [InlineData("#abcdef", "#ABCDEF")]
    [InlineData("abcdef", "#ABCDEF")]
    [InlineData(null, "#000000")]
    [InlineData("", "#000000")]
    public void NormalizeHex_WorksAsExpected(string? input, string expected)
    {
        Assert.Equal(expected, ColorUtils.NormalizeHex(input));
    }

    [Fact]
    public void HexToRgb_WorksAsExpected()
    {
        var rgb = ColorUtils.HexToRgb("#FF0000");
        Assert.Equal((255, 0, 0), rgb);

        rgb = ColorUtils.HexToRgb("#00FF00");
        Assert.Equal((0, 255, 0), rgb);

        rgb = ColorUtils.HexToRgb("#0000FF");
        Assert.Equal((0, 0, 255), rgb);
    }

    [Fact]
    public void HexToRgbString_WorksAsExpected()
    {
        Assert.Equal("255,0,0", ColorUtils.HexToRgbString("#FF0000"));
        Assert.Equal("0,255,0", ColorUtils.HexToRgbString("#00FF00"));
        Assert.Equal("0,0,255", ColorUtils.HexToRgbString("#0000FF"));
    }

    [Fact]
    public void RgbToHex_WorksAsExpected()
    {
        Assert.Equal("#FF0000", ColorUtils.RgbToHex(255, 0, 0));
        Assert.Equal("#00FF00", ColorUtils.RgbToHex(0, 255, 0));
        Assert.Equal("#0000FF", ColorUtils.RgbToHex(0, 0, 255));
    }

    [Fact]
    public void HexToHsl_And_HslToHex_AreInverse()
    {
        var (H, S, L) = ColorUtils.HexToHsl("#FF0000");
        var hex = ColorUtils.HslToHex(H, S, L);
        Assert.Equal("#FF0000", hex, ignoreCase: true);
    }

    [Fact]
    public void RgbToHsl_And_HslToRgb_AreInverse()
    {
        var (H, S, L) = ColorUtils.RgbToHsl(255, 0, 0);
        var rgb = ColorUtils.HslToRgb(H, S, L);
        Assert.Equal((255, 0, 0), rgb);
    }

    [Fact]
    public void GenerateRandomHex_GeneratesUniqueColors()
    {
        var colors = ColorUtils.GenerateRandomHex(10);
        Assert.Equal(10, colors.Count);
        Assert.All(colors, c => Assert.Matches(@"^#[0-9A-F]{6}$", c));
        Assert.Equal(colors.Count, new HashSet<string>(colors, StringComparer.OrdinalIgnoreCase).Count);
    }

    [Fact]
    public void GenerateGradient_Shades_Works()
    {
        var gradient = ColorUtils.GenerateGradient("#FF0000", 5, GradientStrategy.Shades);
        Assert.Equal(5, gradient.Count);
        Assert.All(gradient, c => Assert.StartsWith("#", c));
    }

    [Fact]
    public void GenerateCustomGradient_Works()
    {
        var gradient = ColorUtils.GenerateCustomGradient("#FF0000", "#0000FF", 5);
        Assert.Equal(5, gradient.Count);
        Assert.StartsWith("#", gradient[0]);
        Assert.StartsWith("#", gradient[^1]);
    }

    [Fact]
    public void GenerateScheme_Complementary_Works()
    {
        var scheme = ColorUtils.GenerateScheme("#FF0000", ColorPaletteMode.Complementary, 6);
        Assert.True(scheme.Count > 0);
        Assert.All(scheme, c => Assert.StartsWith("#", c));
    }

    [Theory]
    [InlineData(-0.5, 0.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(0.5, 0.5)]
    [InlineData(1.0, 1.0)]
    [InlineData(1.5, 1.0)]
    public void Clamp01_Works(double input, double expected)
    {
        Assert.Equal(expected, ColorUtils.Clamp01(input), 5);
    }
}
