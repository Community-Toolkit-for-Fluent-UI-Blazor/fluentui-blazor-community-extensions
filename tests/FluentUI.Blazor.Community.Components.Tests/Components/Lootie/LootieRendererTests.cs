using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lootie;

public class LootieRendererTests
{
    [Fact]
    public void LootieRenderer_ShouldContainAllExpectedValues()
    {
        Assert.Equal(0, (int)LootieRenderer.Svg);
        Assert.Equal(1, (int)LootieRenderer.Canvas);
        Assert.Equal(2, (int)LootieRenderer.Html);
    }

    [Theory]
    [InlineData(LootieRenderer.Svg, "Svg")]
    [InlineData(LootieRenderer.Canvas, "Canvas")]
    [InlineData(LootieRenderer.Html, "Html")]
    public void LootieRenderer_ToString_ReturnsExpectedString(LootieRenderer renderer, string expected)
    {
        Assert.Equal(expected, renderer.ToString());
    }

    [Theory]
    [InlineData(0, LootieRenderer.Svg)]
    [InlineData(1, LootieRenderer.Canvas)]
    [InlineData(2, LootieRenderer.Html)]
    public void LootieRenderer_CastFromInt_ReturnsExpectedEnum(int value, LootieRenderer expected)
    {
        Assert.Equal(expected, (LootieRenderer)value);
    }
}
