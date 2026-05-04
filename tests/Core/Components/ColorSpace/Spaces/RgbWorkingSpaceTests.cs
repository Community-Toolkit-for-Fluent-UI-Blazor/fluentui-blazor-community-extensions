using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class RgbWorkingSpaceTests
{
    [Fact]
    public void RgbWorkingSpace_Create_UsesProfileAndAdaptation()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb, ChromaticAdaptationMethod.VonKries);

        Assert.Equal(IccProfileName.Srgb, space.Profile.Name);
        Assert.Equal(ChromaticAdaptationMethod.VonKries, space.Adaptation);
        Assert.NotEqual(default, space.RgbToXyzMatrix);
        Assert.NotEqual(default, space.XyzToRgbMatrix);
        Assert.NotEqual(default, space.WhitePointSource);
        Assert.NotEqual(default, space.WhitePointDestination);
    }

    [Fact]
    public void RgbWorkingSpace_Create_FromProfileUsesDefaultAdaptation()
    {
        var profile = IccProfiles.Get(IccProfileName.Srgb);
        var space = RgbWorkingSpace.Create(profile);

        Assert.Equal(ChromaticAdaptationMethod.Bradford, space.Adaptation);
        Assert.Equal(profile.Name, space.Profile.Name);
    }
}
