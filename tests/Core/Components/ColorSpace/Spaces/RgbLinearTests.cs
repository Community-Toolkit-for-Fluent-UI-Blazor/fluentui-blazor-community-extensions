using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class RgbLinearTests
{
    [Fact]
    public void RgbLinear_Constructor_ClampsValues()
    {
        var rgb = new RgbLinear(-0.1, 1.2, 0.5);

        Assert.Equal(0, rgb.R, 12);
        Assert.Equal(1, rgb.G, 12);
        Assert.Equal(0.5, rgb.B, 12);
    }

    [Fact]
    public void RgbLinear_FromSrgb8_RoundtripPreservesColor()
    {
        var srgb = new Srgb8(10, 20, 30);
        var linear = RgbLinear.FromSrgb8(srgb);
        var roundtrip = linear.ToSrgb8();

        Assert.InRange(Math.Abs(roundtrip.R - srgb.R), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.G - srgb.G), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.B - srgb.B), 0, 1);
    }

    [Fact]
    public void RgbLinear_Operators_WorkAsExpected()
    {
        var a = new RgbLinear(0.2, 0.3, 0.4);
        var b = new RgbLinear(0.1, 0.2, 0.3);

        var sum = a + b;
        var scaled = a * 2;
        var scaledReverse = 2 * b;

        Assert.Equal(0.3, sum.R, 12);
        Assert.Equal(0.5, sum.G, 12);
        Assert.Equal(0.7, sum.B, 12);
        Assert.Equal(0.4, scaled.R, 12);
        Assert.Equal(0.6, scaled.G, 12);
        Assert.Equal(0.8, scaled.B, 12);
        Assert.Equal(0.2, scaledReverse.R, 12);
        Assert.Equal(0.4, scaledReverse.G, 12);
        Assert.Equal(0.6, scaledReverse.B, 12);
    }

    [Fact]
    public void RgbLinear_EqualityAndHashCode()
    {
        var left = new RgbLinear(0.1, 0.2, 0.3);
        var right = new RgbLinear(0.1, 0.2, 0.3);
        var other = new RgbLinear(0.1, 0.25, 0.3);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
