using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class CmykTests
{
    [Fact]
    public void Cmyk_Constructor_ClampsValues()
    {
        var cmyk = new Cmyk(-1, 2, 0.5, 1.5, -0.2);

        Assert.Equal(0, cmyk.C, 12);
        Assert.Equal(1, cmyk.M, 12);
        Assert.Equal(0.5, cmyk.Y, 12);
        Assert.Equal(1, cmyk.K, 12);
        Assert.Equal(0, cmyk.A, 12);
    }

    [Fact]
    public void Cmyk_FromRgbLinear_HandlesBlack()
    {
        var rgb = new RgbLinear(0, 0, 0);
        var cmyk = Cmyk.FromRgbLinear(rgb);

        Assert.Equal(0, cmyk.C, 12);
        Assert.Equal(0, cmyk.M, 12);
        Assert.Equal(0, cmyk.Y, 12);
        Assert.Equal(1, cmyk.K, 12);
    }

    [Fact]
    public void Cmyk_FromRgbLinear_RoundtripPreservesValues()
    {
        var rgb = new RgbLinear(0.2, 0.4, 0.6);
        var cmyk = Cmyk.FromRgbLinear(rgb, 0.5);

        var roundtrip = cmyk.ToRgbLinear();

        Assert.InRange(Math.Abs(roundtrip.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - rgb.B), 0, 1e-12);
        Assert.Equal(0.5, cmyk.A, 12);
    }

    [Fact]
    public void Cmyk_EqualityAndHashCode()
    {
        var left = new Cmyk(0.1, 0.2, 0.3, 0.4, 0.5);
        var right = new Cmyk(0.1, 0.2, 0.3, 0.4, 0.5);
        var other = new Cmyk(0.2, 0.2, 0.3, 0.4, 0.5);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
