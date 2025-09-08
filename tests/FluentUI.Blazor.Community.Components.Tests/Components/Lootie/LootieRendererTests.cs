using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lottie;

public class LottieRendererTests
{
    [Fact]
    public void LottieRenderer_ShouldContainAllExpectedValues()
    {
        Assert.Equal(0, (int)LottieRenderer.Svg);
        Assert.Equal(1, (int)LottieRenderer.Canvas);
        Assert.Equal(2, (int)LottieRenderer.Html);
    }

    [Theory]
    [InlineData(LottieRenderer.Svg, "Svg")]
    [InlineData(LottieRenderer.Canvas, "Canvas")]
    [InlineData(LottieRenderer.Html, "Html")]
    public void LottieRenderer_ToString_ReturnsExpectedString(LottieRenderer renderer, string expected)
    {
        Assert.Equal(expected, renderer.ToString());
    }

    [Theory]
    [InlineData(0, LottieRenderer.Svg)]
    [InlineData(1, LottieRenderer.Canvas)]
    [InlineData(2, LottieRenderer.Html)]
    public void LottieRenderer_CastFromInt_ReturnsExpectedEnum(int value, LottieRenderer expected)
    {
        Assert.Equal(expected, (LottieRenderer)value);
    }
}
