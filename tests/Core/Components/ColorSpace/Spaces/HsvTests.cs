using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class HsvTests
{
    [Fact]
    public void Hsv_Constructor_ClampsAndNormalizes()
    {
        var hsv = new Hsv(-30, -1, 2, 2);

        Assert.Equal(330, hsv.H, 12);
        Assert.Equal(0, hsv.S, 12);
        Assert.Equal(1, hsv.V, 12);
        Assert.Equal(1, hsv.A, 12);
    }

    [Fact]
    public void Hsv_FromSrgb8_RoundtripPreservesColor()
    {
        var color = new Srgb8(10, 100, 200, 100);
        var hsv = Hsv.FromSrgb8(color);

        var roundtrip = hsv.ToSrgb8();

        Assert.InRange(Math.Abs(roundtrip.R - color.R), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.G - color.G), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.B - color.B), 0, 1);
        Assert.Equal(byte.MaxValue, roundtrip.A);
    }

    [Fact]
    public void Hsv_EqualityAndHashCode()
    {
        var left = new Hsv(10, 0.2, 0.3, 0.4);
        var right = new Hsv(10, 0.2, 0.3, 0.4);
        var other = new Hsv(20, 0.2, 0.3, 0.4);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
