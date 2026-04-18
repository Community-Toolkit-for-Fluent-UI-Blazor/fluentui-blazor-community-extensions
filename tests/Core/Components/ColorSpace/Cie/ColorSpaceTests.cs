using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Cie;

public class ColorSpaceTests
{
    [Fact]
    public void ColorSpace_Create_UsesProfileAndDefaultAdaptation()
    {
        var colorSpace = FluentUI.Blazor.Community.Components.ColorSpace.Cie.ColorSpace.Create(IccProfileName.Srgb);

        Assert.Equal(IccProfileName.Srgb, colorSpace.Profile.Name);
        Assert.Equal(ChromaticAdaptationMethod.Bradford, colorSpace.Adaptation);
        Assert.NotNull(colorSpace.WorkingSpace);
    }

    [Fact]
    public void ColorSpace_ToXyz_ReturnsZeroForBlack()
    {
        var colorSpace = FluentUI.Blazor.Community.Components.ColorSpace.Cie.ColorSpace.Create(IccProfileName.Srgb);
        var xyz = colorSpace.ToXyz(new Srgb8(0, 0, 0));

        Assert.Equal(0, xyz.X, 12);
        Assert.Equal(0, xyz.Y, 12);
        Assert.Equal(0, xyz.Z, 12);
    }

    [Fact]
    public void ColorSpace_Roundtrip_Srgb8_ReturnsCloseToOriginal()
    {
        var colorSpace = FluentUI.Blazor.Community.Components.ColorSpace.Cie.ColorSpace.Create(IccProfileName.Srgb);
        var original = new Srgb8(100, 150, 200);

        var xyz = colorSpace.ToXyz(original);
        var roundtrip = colorSpace.ToSrgb8(xyz);

        Assert.InRange(Math.Abs(roundtrip.R - original.R), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.G - original.G), 0, 1);
        Assert.InRange(Math.Abs(roundtrip.B - original.B), 0, 1);
        Assert.Equal(original.A, roundtrip.A);
    }

    [Fact]
    public void ColorSpace_SimulateVision_NormalReturnsOriginal()
    {
        var colorSpace = FluentUI.Blazor.Community.Components.ColorSpace.Cie.ColorSpace.Create(IccProfileName.Srgb);
        var original = new Srgb8(12, 200, 140);

        var simulated = colorSpace.SimulateVision(original, ColorVisionType.Normal);

        Assert.Equal(original, simulated);
    }

    [Fact]
    public void ColorSpace_SimulateVision_ChangesColorForDeficiency()
    {
        var colorSpace = FluentUI.Blazor.Community.Components.ColorSpace.Cie.ColorSpace.Create(IccProfileName.Srgb);
        var original = new Srgb8(255, 0, 0);

        var simulated = colorSpace.SimulateVision(original, ColorVisionType.Protanopia);

        Assert.NotEqual(original, simulated);
    }
}
