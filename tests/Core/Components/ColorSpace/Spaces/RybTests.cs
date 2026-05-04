using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class RybTests
{
    [Fact]
    public void Ryb_Constructor_ClampsValues()
    {
        var ryb = new Ryb(-1, 2, 0.5, -0.5);

        Assert.Equal(0, ryb.R, 12);
        Assert.Equal(1, ryb.Y, 12);
        Assert.Equal(0.5, ryb.B, 12);
        Assert.Equal(0, ryb.A, 12);
    }

    [Fact]
    public void Ryb_FromSrgb8_RoundtripPreservesColor()
    {
        var color = new Srgb8(200, 100, 50, 128);
        var ryb = Ryb.FromSrgb8(color);

        var roundtrip = ryb.ToSrgb8();

        Assert.InRange(Math.Abs(roundtrip.R - color.R), 0, 2);
        Assert.InRange(Math.Abs(roundtrip.G - color.G), 0, 2);
        Assert.InRange(Math.Abs(roundtrip.B - color.B), 0, 2);
        Assert.Equal(byte.MaxValue, roundtrip.A);
    }

    [Fact]
    public void Ryb_EqualityAndHashCode()
    {
        var left = new Ryb(0.1, 0.2, 0.3, 0.4);
        var right = new Ryb(0.1, 0.2, 0.3, 0.4);
        var other = new Ryb(0.1, 0.25, 0.3, 0.4);

        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
