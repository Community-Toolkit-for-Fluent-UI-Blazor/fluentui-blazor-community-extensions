using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Icc;

public class IccProfileTests
{
    [Fact]
    public void IccProfile_Equality_WorksForSameValues()
    {
        var left = IccProfiles.Get(IccProfileName.Srgb);
        var right = IccProfiles.Get(IccProfileName.Srgb);
        var other = IccProfiles.Get(IccProfileName.Adobe);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.False(left.Equals(new object()));
    }

    [Fact]
    public void IccProfile_GetHashCode_MatchesForEqualValues()
    {
        var left = IccProfiles.Get(IccProfileName.Srgb);
        var right = IccProfiles.Get(IccProfileName.Srgb);

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void IccProfile_ToString_ContainsExpectedLabels()
    {
        var profile = IccProfiles.Get(IccProfileName.Srgb);

        var text = profile.ToString();

        Assert.Contains("Gamma:", text);
        Assert.Contains("StandardIlluminant:", text);
        Assert.Contains("Matrix:", text);
    }

    [Fact]
    public void IccProfile_ThrowsForInvalidGamma()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new IccProfile(
                IccProfileName.Srgb,
                0,
                IlluminantName.D65,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1);
        });
    }

    [Fact]
    public void IccProfile_ThrowsForInvalidIlluminant()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new IccProfile(
                IccProfileName.Srgb,
                2.2,
                (IlluminantName)99,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1);
        });
    }

    [Theory]
    [InlineData(0, 1, 1, 1, 1, 1, 1, 1, 1)]
    [InlineData(1, 0, 1, 1, 1, 1, 1, 1, 1)]
    [InlineData(1, 1, 0, 1, 1, 1, 1, 1, 1)]
    public void IccProfile_ThrowsForZeroMatrixValues(
        double m11,
        double m12,
        double m13,
        double m21,
        double m22,
        double m23,
        double m31,
        double m32,
        double m33)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new IccProfile(
                IccProfileName.Srgb,
                2.2,
                IlluminantName.D65,
                m11,
                m12,
                m13,
                m21,
                m22,
                m23,
                m31,
                m32,
                m33);
        });
    }
}
