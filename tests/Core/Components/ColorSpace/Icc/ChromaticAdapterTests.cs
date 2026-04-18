using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Icc;

public class ChromaticAdapterTests
{
    [Fact]
    public void ChromaticAdapter_Adapt_ReturnsSameColorWhenWhitePointsEqual()
    {
        var whitePoint = CieIlluminants.Get(IlluminantName.D65).ToXyz(IlluminantPointOfView.TwoDegrees);
        var color = ColorSpaceConverters.ToXyz(new Xyy(0.3, 0.3, 1.0));

        var result = ChromaticAdapter.Adapt(
            0.3,
            0.3,
            1.0,
            whitePoint,
            whitePoint,
            ChromaticAdaptationMethod.XYZScaling);

        Assert.Equal(color.X, result.X, 12);
        Assert.Equal(color.Y, result.Y, 12);
        Assert.Equal(color.Z, result.Z, 12);
    }

    [Fact]
    public void ChromaticAdapter_Adapt_UsesDifferentMatricesPerMethod()
    {
        var whitePointSource = CieIlluminants.Get(IlluminantName.D50).ToXyz(IlluminantPointOfView.TwoDegrees);
        var whitePointDestination = CieIlluminants.Get(IlluminantName.D65).ToXyz(IlluminantPointOfView.TwoDegrees);

        var vonKries = ChromaticAdapter.Adapt(
            0.4,
            0.3,
            1.0,
            whitePointSource,
            whitePointDestination,
            ChromaticAdaptationMethod.VonKries);

        var bradford = ChromaticAdapter.Adapt(
            0.4,
            0.3,
            1.0,
            whitePointSource,
            whitePointDestination,
            ChromaticAdaptationMethod.Bradford);

        var delta = Math.Abs(vonKries.X - bradford.X) + Math.Abs(vonKries.Y - bradford.Y) + Math.Abs(vonKries.Z - bradford.Z);

        Assert.True(delta > 1e-6);
    }
}
