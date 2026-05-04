using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Maths;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class XyzTests
{
    [Fact]
    public void Xyz_Constructor_AssignsValues()
    {
        var xyz = new Xyz(0.1, 0.2, 0.3);

        Assert.Equal(0.1, xyz.X, 12);
        Assert.Equal(0.2, xyz.Y, 12);
        Assert.Equal(0.3, xyz.Z, 12);
    }

    [Fact]
    public void Xyz_EqualityAndHashCode()
    {
        var left = new Xyz(0.1, 0.2, 0.3);
        var right = new Xyz(0.1, 0.2, 0.3);
        var other = new Xyz(0.1, 0.25, 0.3);

        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Contains("X=", left.ToString());
    }

    [Fact]
    public void Xyz_ToXYZ_UsesSrgbConversionWhenRequested()
    {
        var matrix = Matrix3x3.Identity;
        var color = new Srgb8(255, 0, 0);

        var xyz = Xyz.ToXYZ(color, matrix, 2.2, true);

        Assert.True(xyz.X > 0);
        Assert.Equal(0, xyz.Y, 12);
        Assert.Equal(0, xyz.Z, 12);
    }

    [Fact]
    public void Xyz_ToXYZ_UsesGammaWhenNotSrgb()
    {
        var matrix = Matrix3x3.Identity;
        var color = new Srgb8(255, 0, 0);

        var xyz = Xyz.ToXYZ(color, matrix, 2.0, false);

        Assert.Equal(1.0, xyz.X, 12);
        Assert.Equal(0, xyz.Y, 12);
        Assert.Equal(0, xyz.Z, 12);
    }
}
