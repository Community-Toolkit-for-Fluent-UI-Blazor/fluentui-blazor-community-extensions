using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class HsbTests
{
    [Fact]
    public void Hsb_Constructor_ClampsAndNormalizes()
    {
        var hsb = new Hsb(725, -1, 2, -0.5);

        Assert.Equal(5, hsb.H, 12);
        Assert.Equal(0, hsb.S, 12);
        Assert.Equal(1, hsb.B, 12);
        Assert.Equal(0, hsb.A, 12);
    }

    [Fact]
    public void Hsb_FromSrgb8_RoundtripPreservesColor()
    {
        var color = new Srgb8(220, 120, 40, 64);
        var hsb = Hsb.FromSrgb8(color);

        var roundtrip = hsb.ToSrgb8();

        Assert.InRange(Math.Abs(roundtrip.R - color.R), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.G - color.G), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.B - color.B), 0, 1);
        Assert.Equal(byte.MaxValue, roundtrip.A);
    }

    [Fact]
    public void Hsb_EqualityAndHashCode()
    {
        var left = new Hsb(100, 0.2, 0.3, 0.4);
        var right = new Hsb(100, 0.2, 0.3, 0.4);
        var other = new Hsb(120, 0.2, 0.3, 0.4);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
