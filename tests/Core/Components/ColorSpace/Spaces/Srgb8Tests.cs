using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class Srgb8Tests
{
    [Fact]
    public void Srgb8_ConstructorsAndFactories_AssignValues()
    {
        var color = new Srgb8(10, 20, 30, 40);
        var rgb = Srgb8.FromRgb(10, 20, 30);
        var argb = Srgb8.FromArgb(40, 10, 20, 30);

        Assert.Equal(10, color.R);
        Assert.Equal(20, color.G);
        Assert.Equal(30, color.B);
        Assert.Equal(40, color.A);
        Assert.Equal(rgb, new Srgb8(10, 20, 30));
        Assert.Equal(argb, color);
    }

    [Fact]
    public void Srgb8_EqualityAndHashCode()
    {
        var left = new Srgb8(1, 2, 3, 4);
        var right = new Srgb8(1, 2, 3, 4);
        var other = new Srgb8(1, 2, 4, 4);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Equal("#01020304", left.ToString());
    }

    [Fact]
    public void Srgb8_Clamp_ClampsChannels()
    {
        var color = new Srgb8(255, 0, 255, 255);

        var clamped = color.Clamp();

        Assert.Equal(color, clamped);
    }

    [Fact]
    public void Srgb8_WithAlpha_AssignsNewAlpha()
    {
        var color = new Srgb8(10, 20, 30, 40);

        var updated = color.WithAlpha(200);

        Assert.Equal(200, updated.A);
        Assert.Equal(10, updated.R);
        Assert.Equal(20, updated.G);
        Assert.Equal(30, updated.B);
    }

    [Fact]
    public void Srgb8_LightenAndDarken_AdjustLightness()
    {
        var color = new Srgb8(100, 100, 100, 200);

        var lighter = color.Lighten(50);
        var darker = color.Darken(50);

        Assert.NotEqual(color, lighter);
        Assert.NotEqual(color, darker);
        Assert.Equal(byte.MaxValue, lighter.A);
        Assert.Equal(byte.MaxValue, darker.A);
    }

    [Fact]
    public void Srgb8_Blend_InterpolatesChannels()
    {
        var color = new Srgb8(0, 0, 0, 0);
        var other = new Srgb8(100, 200, 255, 255);

        var blended = color.Blend(other, 50);

        Assert.Equal(50, blended.R);
        Assert.Equal(100, blended.G);
        Assert.Equal(127, blended.B);
        Assert.Equal(127, blended.A);
    }

    [Fact]
    public void Srgb8_Invert_InvertsChannels()
    {
        var color = new Srgb8(10, 20, 30, 40);
        var inverted = color.Invert();

        Assert.Equal(245, inverted.R);
        Assert.Equal(235, inverted.G);
        Assert.Equal(225, inverted.B);
        Assert.Equal(40, inverted.A);
    }

    [Fact]
    public void Srgb8_Contrast_SelectsBasedOnThreshold()
    {
        var color = new Srgb8(250, 250, 250);
        var light = new Srgb8(255, 255, 255);
        var dark = new Srgb8(0, 0, 0);

        var contrast = color.Contrast(light, dark, 0.5);

        Assert.Equal(dark, contrast);
    }

    [Fact]
    public void Srgb8_Parse_ParsesHexRgbAndRgba()
    {
        Assert.Equal(new Srgb8(255, 0, 0), Srgb8.Parse("#ff0000"));
        Assert.Equal(new Srgb8(255, 0, 0, 136), Srgb8.Parse("#f008"));
    }

    [Fact]
    public void Srgb8_Parse_ParsesRgbFunctionAndNamedColor()
    {
        Assert.Equal(new Srgb8(255, 0, 0), Srgb8.Parse("rgb(255,0,0)"));
        Assert.Equal(new Srgb8(255, 0, 0, 128), Srgb8.Parse("rgb(255,0,0,0.5)"));
        Assert.Equal(new Srgb8(255, 0, 0), Srgb8.Parse("red"));
    }

    [Fact]
    public void Srgb8_Parse_ThrowsForInvalidInput()
    {
        Assert.Throws<ArgumentException>(() => Srgb8.Parse(" "));
        Assert.Throws<FormatException>(() => Srgb8.Parse("not-a-color"));
    }

    [Fact]
    public void Srgb8_TryParse_HandlesInputs()
    {
        Assert.True(Srgb8.TryParse("#00ff00", out var green));
        Assert.Equal(new Srgb8(0, 255, 0), green);

        Assert.True(Srgb8.TryParse("rgb(100, 200, 50)", out var rgb));
        Assert.Equal(new Srgb8(100, 200, 50), rgb);

        Assert.True(Srgb8.TryParse("blue", out var named));
        Assert.Equal(new Srgb8(0, 0, 255), named);

        Assert.False(Srgb8.TryParse("invalid", out _));
        Assert.False(Srgb8.TryParse(null, out _));
    }

    [Fact]
    public void Srgb8_ToString_ReturnsHex()
    {
        var color = new Srgb8(255, 170, 0, 255);

        Assert.Equal("#FFAA00FF", color.ToString());
    }
}
