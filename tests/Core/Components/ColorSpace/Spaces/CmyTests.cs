using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class CmyTests
{
    [Fact]
    public void Cmy_Constructor_ClampsValues()
    {
        var cmy = new Cmy(-1, 2, 0.5, -0.2);

        Assert.Equal(0, cmy.C, 12);
        Assert.Equal(1, cmy.M, 12);
        Assert.Equal(0.5, cmy.Y, 12);
        Assert.Equal(0, cmy.A, 12);
    }

    [Fact]
    public void Cmy_FromRgbLinear_RoundtripPreservesValues()
    {
        var rgb = new RgbLinear(0.2, 0.4, 0.6);
        var cmy = Cmy.FromRgbLinear(rgb, 0.75);

        var roundtrip = cmy.ToRgbLinear();

        Assert.Equal(0.8, cmy.C, 12);
        Assert.Equal(0.6, cmy.M, 12);
        Assert.Equal(0.4, cmy.Y, 12);
        Assert.Equal(0.75, cmy.A, 12);
        Assert.Equal(rgb.R, roundtrip.R, 12);
        Assert.Equal(rgb.G, roundtrip.G, 12);
        Assert.Equal(rgb.B, roundtrip.B, 12);
    }

    [Fact]
    public void Cmy_EqualityAndHashCode()
    {
        var left = new Cmy(0.1, 0.2, 0.3, 0.4);
        var right = new Cmy(0.1, 0.2, 0.3, 0.4);
        var other = new Cmy(0.2, 0.2, 0.3, 0.4);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
