using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class CieIlluminantTests
{
    [Fact]
    public void CieIlluminant_Constructor_AssignsProperties()
    {
        var illuminant = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        Assert.Equal(0.4, illuminant.X2);
        Assert.Equal(0.3, illuminant.Y2);
        Assert.Equal(0.45, illuminant.X10);
        Assert.Equal(0.35, illuminant.Y10);
        Assert.Equal(5000, illuminant.Kelvin);
    }

    [Fact]
    public void CieIlluminant_Equality_WorksForSameValues()
    {
        var left = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);
        var right = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);
        var different = new CieIlluminant(0.41, 0.3, 0.45, 0.35, 5000);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(different));
        Assert.False(left.Equals(new object()));
    }

    [Fact]
    public void CieIlluminant_GetHashCode_MatchesForEqualValues()
    {
        var left = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);
        var right = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void CieIlluminant_ToString_ContainsExpectedLabels()
    {
        var illuminant = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        var text = illuminant.ToString();

        Assert.Contains("X2:", text);
        Assert.Contains("Y2:", text);
        Assert.Contains("X10:", text);
        Assert.Contains("Y10:", text);
        Assert.Contains("Kelvin:", text);
    }

    [Fact]
    public void CieIlluminant_ToXyz_UsesTwoDegreePointOfView()
    {
        var illuminant = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        var xyz = illuminant.ToXyz(IlluminantPointOfView.TwoDegrees);

        Assert.Equal(1.3333333333333333, xyz.X, 12);
        Assert.Equal(1.0, xyz.Y, 12);
        Assert.Equal(1.0, xyz.Z, 12);
    }

    [Fact]
    public void CieIlluminant_ToXyz_UsesTenDegreePointOfView()
    {
        var illuminant = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        var xyz = illuminant.ToXyz(IlluminantPointOfView.TenDegrees);

        Assert.Equal(1.2857142857142858, xyz.X, 12);
        Assert.Equal(1.0, xyz.Y, 12);
        Assert.Equal(0.5714285714285714, xyz.Z, 12);
    }

    [Fact]
    public void CieIlluminant_ToXyz_ThrowsForInvalidPointOfView()
    {
        var illuminant = new CieIlluminant(0.4, 0.3, 0.45, 0.35, 5000);

        Assert.Throws<ArgumentOutOfRangeException>(() => illuminant.ToXyz((IlluminantPointOfView)99));
    }
}
