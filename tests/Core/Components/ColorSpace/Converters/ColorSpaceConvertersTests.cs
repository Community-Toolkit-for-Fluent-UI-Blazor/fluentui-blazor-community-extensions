using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using FluentUI.Blazor.Community.Components.Maths;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Converters;

public class ColorSpaceConvertersTests
{
    [Fact]
    public void ColorSpaceConverters_HslRoundtrip()
    {
        var rgb = new RgbLinear(0.2, 0.4, 0.6);
        var hsl = ColorSpaceConverters.ToHsl(rgb, 0.7);
        var roundtrip = ColorSpaceConverters.FromHsl(hsl);

        Assert.InRange(Math.Abs(roundtrip.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - rgb.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_HsvRoundtrip()
    {
        var rgb = new RgbLinear(0.3, 0.5, 0.1);
        var hsv = ColorSpaceConverters.ToHsv(rgb, 0.4);
        var roundtrip = ColorSpaceConverters.FromHsv(hsv);

        Assert.InRange(Math.Abs(roundtrip.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - rgb.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_HsbRoundtrip()
    {
        var rgb = new RgbLinear(0.3, 0.2, 0.8);
        var hsb = ColorSpaceConverters.ToHsb(rgb, 0.5);
        var roundtrip = ColorSpaceConverters.FromHsb(hsb);

        Assert.InRange(Math.Abs(roundtrip.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - rgb.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_RybRoundtrip()
    {
        var rgb = new RgbLinear(0.7, 0.2, 0.1);
        var ryb = ColorSpaceConverters.ToRyb(rgb, 0.8);
        var roundtrip = ColorSpaceConverters.FromRyb(ryb);

        Assert.InRange(Math.Abs(roundtrip.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - rgb.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_ToLinearAndToSrgb8()
    {
        var color = new Srgb8(128, 64, 32, 128);
        var linear = ColorSpaceConverters.ToLinear(color);
        var srgb = ColorSpaceConverters.ToSrgb8(linear, color.A);

        Assert.InRange(Math.Abs(srgb.R - color.R), 0, 1);
        Assert.InRange(Math.Abs(srgb.G - color.G), 0, 1);
        Assert.InRange(Math.Abs(srgb.B - color.B), 0, 1);
        Assert.Equal(color.A, srgb.A);
    }

    [Fact]
    public void ColorSpaceConverters_SrgbToLinearAndLinearToSrgb()
    {
        var linear = ColorSpaceConverters.SrgbToLinear(0.5);
        var srgb = ColorSpaceConverters.LinearToSrgb(linear);

        Assert.InRange(srgb, (byte)120, (byte)130);
    }

    [Fact]
    public void ColorSpaceConverters_InverseGamma_UsesSrgbPath()
    {
        var value = ColorSpaceConverters.InverseGamma(2.4, 0.5);

        Assert.Equal(ColorSpaceConverters.SrgbToLinear(0.5), value, 12);
    }

    [Fact]
    public void ColorSpaceConverters_XyzConversion_Roundtrip()
    {
        var matrix = Matrix3x3.Identity;
        var rgb = new RgbLinear(0.2, 0.4, 0.6);
        var xyz = ColorSpaceConverters.ToXyz(rgb, matrix);
        var linear = ColorSpaceConverters.ToLinear(xyz, matrix);

        Assert.InRange(Math.Abs(linear.R - rgb.R), 0, 1e-12);
        Assert.InRange(Math.Abs(linear.G - rgb.G), 0, 1e-12);
        Assert.InRange(Math.Abs(linear.B - rgb.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_XyYConversions()
    {
        var xyz = new Xyz(0.2, 0.3, 0.4);

        var xyy = ColorSpaceConverters.ToXyY(xyz);
        var roundtrip = ColorSpaceConverters.ToXyz(xyy);

        Assert.InRange(Math.Abs(roundtrip.X - xyz.X), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.Y - xyz.Y), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.Z - xyz.Z), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_XyzFromSrgb_UsesGamma()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var xyz = ColorSpaceConverters.ToXyz(new Srgb8(255, 0, 0), space);

        Assert.True(xyz.X > 0);
        Assert.Equal(0.32428683076499337, xyz.Y, 15);
        Assert.Equal(0.16366291900300001, xyz.Z, 12);
    }

    [Fact]
    public void ColorSpaceConverters_OklabRoundtrip()
    {
        var xyz = new Xyz(0.5, 0.4, 0.3);
        var lab = ColorSpaceConverters.ToOklab(xyz);
        var roundtrip = ColorSpaceConverters.FromOklab(lab);

        Assert.InRange(Math.Abs(roundtrip.X - xyz.X), 0, 1e-7);
        Assert.InRange(Math.Abs(roundtrip.Y - xyz.Y), 0, 1e-7);
        Assert.InRange(Math.Abs(roundtrip.Z - xyz.Z), 0, 1e-7);
    }

    [Fact]
    public void ColorSpaceConverters_OklchRoundtrip()
    {
        var lab = new Oklab(0.7, 0.1, -0.2);
        var lch = ColorSpaceConverters.ToOklch(lab);
        var roundtrip = ColorSpaceConverters.FromOklch(lch);

        Assert.InRange(Math.Abs(roundtrip.L - lab.L), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.A - lab.A), 0, 1e-12);
        Assert.InRange(Math.Abs(roundtrip.B - lab.B), 0, 1e-12);
    }

    [Fact]
    public void ColorSpaceConverters_FromXyzToSrgb8_ClampsChannels()
    {
        var matrix = Matrix3x3.Identity;
        var xyz = new Xyz(2, 2, 2);

        var srgb = ColorSpaceConverters.FromXyzToSrgb8(xyz, matrix, 2.4);

        Assert.Equal(byte.MaxValue, srgb.R);
        Assert.Equal(byte.MaxValue, srgb.G);
        Assert.Equal(byte.MaxValue, srgb.B);
        Assert.Equal(byte.MaxValue, srgb.A);
    }

    [Fact]
    public void ColorSpaceConverters_ModifyColor_AdjustsColor()
    {
        var space = RgbWorkingSpace.Create(IccProfileName.Srgb);
        var color = new Srgb8(200, 150, 100);

        var modified = ColorSpaceConverters.ModifyColor(color, true, space, ColorVisionType.Normal);

        Assert.NotEqual(color, modified);
    }
}
