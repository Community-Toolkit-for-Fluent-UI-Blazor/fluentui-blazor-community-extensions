using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Icc;

public class IccProfilesTests
{
    [Theory]
    [InlineData(IccProfileName.Adobe, 2.2)]
    [InlineData(IccProfileName.Srgb, 2.2)]
    [InlineData(IccProfileName.AppleRgb, 1.8)]
    public void IccProfiles_Get_ReturnsProfileWithExpectedGamma(IccProfileName name, double expectedGamma)
    {
        var profile = IccProfiles.Get(name);

        Assert.Equal(name, profile.Name);
        Assert.Equal(expectedGamma, profile.Gamma, 3);
        Assert.True(Enum.IsDefined(profile.IlluminantName));
    }

    [Fact]
    public void IccProfiles_Get_ThrowsForUnknownProfile()
    {
        Assert.Throws<ArgumentException>(() => IccProfiles.Get((IccProfileName)99));
    }
}
