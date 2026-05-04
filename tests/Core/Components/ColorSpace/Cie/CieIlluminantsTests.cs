using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class CieIlluminantsTests
{
    [Fact]
    public void CieIlluminants_Get_ReturnsExpectedIlluminant()
    {
        var illuminant = CieIlluminants.Get(IlluminantName.D65);

        Assert.Equal(0.31271, illuminant.X2, 5);
        Assert.Equal(0.32902, illuminant.Y2, 5);
        Assert.Equal(0.31382, illuminant.X10, 5);
        Assert.Equal(0.33100, illuminant.Y10, 5);
        Assert.Equal(6504, illuminant.Kelvin, 0);
    }

    [Fact]
    public void CieIlluminants_Get_ThrowsForUnknownIlluminant()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CieIlluminants.Get((IlluminantName)99));
    }
}
