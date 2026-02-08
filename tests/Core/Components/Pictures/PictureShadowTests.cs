using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureShadowTests
{
    [Fact]
    public void ToCss_UsesDefaults()
    {
        var shadow = new Shadow();

        Assert.Equal("0px 0px 5px 0px rgba(0,0,0,0.5)", shadow.ToCss());
    }

    [Fact]
    public void ToCss_UsesCustomValues()
    {
        var shadow = new Shadow
        {
            OffsetX = new CssLength(1),
            OffsetY = new CssLength(2),
            BlurRadius = new CssLength(3),
            SpreadRadius = new CssLength(4),
            Color = new RgbaColor(10, 20, 30, 0.25)
        };

        Assert.Equal("1px 2px 3px 4px rgba(10,20,30,0.25)", shadow.ToCss());
    }
}
