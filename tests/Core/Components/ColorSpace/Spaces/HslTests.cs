using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class HslTests
{
    [Fact]
    public void Hsl_Constructor_ClampsAndNormalizes()
    {
        var hsl = new Hsl(420, -1, 2, -0.5);

        Assert.Equal(60, hsl.H, 12);
        Assert.Equal(0, hsl.S, 12);
        Assert.Equal(1, hsl.L, 12);
        Assert.Equal(0, hsl.A, 12);
    }

    [Fact]
    public void Hsl_FromSrgb8_RoundtripPreservesColor()
    {
        var color = new Srgb8(120, 200, 80, 128);
        var hsl = Hsl.FromSrgb8(color);

        var roundtrip = hsl.ToSrgb8();

        Assert.InRange(Math.Abs(roundtrip.R - color.R), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.G - color.G), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.B - color.B), 0, 1);
        Assert.Equal(byte.MaxValue, roundtrip.A);
    }

    [Fact]
    public void Hsl_EqualityAndHashCode()
    {
        var left = new Hsl(30, 0.4, 0.5, 0.6);
        var right = new Hsl(30, 0.4, 0.5, 0.6);
        var other = new Hsl(40, 0.4, 0.5, 0.6);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
