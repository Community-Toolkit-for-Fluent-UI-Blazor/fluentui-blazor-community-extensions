using System;
using FluentUI.Blazor.Community;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

public class EffectBuilderTests
{
    public static TheoryData<double> InvalidNonNegativeValues =>
        new() { -1, double.NaN, double.PositiveInfinity, double.NegativeInfinity };

    public static TheoryData<double> InvalidUnitIntervalValues =>
        new() { -0.1, 1.1, double.NaN, double.PositiveInfinity, double.NegativeInfinity };

    [Fact]
    public void Build_WhenEmpty_ReturnsNull()
    {
        var builder = new EffectBuilder();

        Assert.Null(builder.Build());
    }

    [Fact]
    public void Build_ReturnsCombinedEffects()
    {
        var builder = new EffectBuilder()
            .AddBlur(4)
            .AddBrightness(1.2)
            .AddHueRotate(90)
            .AddDropShadow(new CssLength(1), new CssLength(2), new CssLength(3), new RgbaColor(0, 0, 0, 0.5));

        var result = builder.Build();

        Assert.Equal("blur(4px) brightness(1.2) hue-rotate(90deg) drop-shadow(1px 2px 3px rgba(0,0,0,0.5))", result);
    }

    [Theory]
    [MemberData(nameof(InvalidNonNegativeValues))]
    public void AddBlur_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddBlur(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidNonNegativeValues))]
    public void AddBrightness_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddBrightness(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidNonNegativeValues))]
    public void AddContrast_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddContrast(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidUnitIntervalValues))]
    public void AddGrayscale_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddGrayscale(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidUnitIntervalValues))]
    public void AddInvert_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddInvert(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidUnitIntervalValues))]
    public void AddOpacity_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddOpacity(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidNonNegativeValues))]
    public void AddSaturate_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddSaturate(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [MemberData(nameof(InvalidUnitIntervalValues))]
    public void AddSepia_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddSepia(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddHueRotate_InvalidAngle_Throws(double angle)
    {
        var builder = new EffectBuilder();

        var exception = Assert.Throws<ArgumentException>(() => builder.AddHueRotate(angle));

        Assert.Equal("angle", exception.ParamName);
    }
}
