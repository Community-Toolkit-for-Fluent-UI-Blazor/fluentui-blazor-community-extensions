namespace FluentUI.Blazor.Community.Components.Tests.Effects;

public class EffectBuilderTests
{
    [Fact]
    public void AddBlur_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddBlur(5);
        Assert.Equal("blur(5px)", builder.Build());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddBlur_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddBlur(value));
    }

    [Fact]
    public void AddBrightness_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddBrightness(1.5);
        Assert.Equal("brightness(1.5)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddBrightness_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddBrightness(value));
    }

    [Fact]
    public void AddContrast_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddContrast(2);
        Assert.Equal("contrast(2)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddContrast_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddContrast(value));
    }

    [Fact]
    public void AddGrayscale_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddGrayscale(0.5);
        Assert.Equal("grayscale(0.5)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddGrayscale_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddGrayscale(value));
    }

    [Fact]
    public void AddHueRotate_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddHueRotate(90);
        Assert.Equal("hue-rotate(90deg)", builder.Build());
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddHueRotate_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddHueRotate(value));
    }

    [Fact]
    public void AddInvert_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddInvert(1);
        Assert.Equal("invert(1)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddInvert_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddInvert(value));
    }

    [Fact]
    public void AddOpacity_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddOpacity(0.7);
        Assert.Equal("opacity(0.7)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddOpacity_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddOpacity(value));
    }

    [Fact]
    public void AddSaturate_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddSaturate(2.5);
        Assert.Equal("saturate(2.5)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddSaturate_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddSaturate(value));
    }

    [Fact]
    public void AddSepia_ValidValue_AddsEffect()
    {
        var builder = new EffectBuilder();
        builder.AddSepia(1);
        Assert.Equal("sepia(1)", builder.Build());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void AddSepia_InvalidValue_Throws(double value)
    {
        var builder = new EffectBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddSepia(value));
    }

    [Fact]
    public void AddDropShadow_ValidValues_AddsEffect()
    {
        var builder = new EffectBuilder();
        var offsetX = new CssLength(2, LengthUnit.Pixels);
        var offsetY = new CssLength(3, LengthUnit.Pixels);
        var blurRadius = new CssLength(4, LengthUnit.Pixels);
        var color = new RgbaColor(255, 0, 0, 1);
        builder.AddDropShadow(offsetX, offsetY, blurRadius, color);
        Assert.Equal($"drop-shadow({offsetX} {offsetY} {blurRadius} {color})", builder.Build());
    }

    [Fact]
    public void Build_NoEffects_ReturnsNull()
    {
        var builder = new EffectBuilder();
        Assert.Null(builder.Build());
    }

    [Fact]
    public void Build_MultipleEffects_ConcatenatesEffects()
    {
        var builder = new EffectBuilder();
        builder.AddBlur(1).AddBrightness(2);
        Assert.Equal("blur(1px) brightness(2)", builder.Build());
    }
}
