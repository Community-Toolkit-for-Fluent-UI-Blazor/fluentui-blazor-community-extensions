using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class ShadowOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new ShadowOptions();

        Assert.False(options.Enabled);
        Assert.Equal("rgba(0,0,0,0.3)", options.Color);
        Assert.Equal(2.0, options.Blur);
        Assert.Equal(1.0, options.OffsetX);
        Assert.Equal(1.0, options.OffsetY);
    }

    [Fact]
    public void Properties_Can_Be_Set_And_Retrieved()
    {
        var options = new ShadowOptions
        {
            Enabled = true,
            Color = "#FF0000",
            Blur = 5.5,
            OffsetX = -3.2,
            OffsetY = 7.8
        };

        Assert.True(options.Enabled);
        Assert.Equal("#FF0000", options.Color);
        Assert.Equal(5.5, options.Blur);
        Assert.Equal(-3.2, options.OffsetX);
        Assert.Equal(7.8, options.OffsetY);
    }

    [Fact]
    public void Reset_Sets_Properties_To_Defaults()
    {
        var options = new ShadowOptions
        {
            Enabled = true,
            Color = "#123456",
            Blur = 10.0,
            OffsetX = 4.0,
            OffsetY = -2.0
        };

        options.Reset();

        Assert.False(options.Enabled);
        Assert.Equal("rgba(0,0,0,0.3)", options.Color);
        Assert.Equal(2.0, options.Blur);
        Assert.Equal(1.0, options.OffsetX);
        Assert.Equal(1.0, options.OffsetY);
    }
}
