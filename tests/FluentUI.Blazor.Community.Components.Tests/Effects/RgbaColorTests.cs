namespace FluentUI.Blazor.Community.Components.Tests.Effects;

public class RgbaColorTests
{
    [Theory]
    [InlineData(0, 0, 0, 1.0, "rgba(0,0,0,1)")]
    [InlineData(255, 128, 64, 0.5, "rgba(255,128,64,0.5)")]
    [InlineData(10, 20, 30, 0.75, "rgba(10,20,30,0.75)")]
    [InlineData(255, 255, 255, 0.99, "rgba(255,255,255,0.99)")]
    public void ToString_ReturnsExpectedFormat(byte r, byte g, byte b, double a, string expected)
    {
        var color = new RgbaColor(r, g, b, a);
        Assert.Equal(expected, color.ToString());
    }

    [Fact]
    public void Constructor_DefaultAlpha_IsOpaque()
    {
        var color = new RgbaColor(1, 2, 3);
        Assert.Equal("rgba(1,2,3,1)", color.ToString());
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_InvalidAlpha_ThrowsArgumentException(double invalidAlpha)
    {
        Assert.Throws<ArgumentException>(() => new RgbaColor(1, 2, 3, invalidAlpha));
    }
}
