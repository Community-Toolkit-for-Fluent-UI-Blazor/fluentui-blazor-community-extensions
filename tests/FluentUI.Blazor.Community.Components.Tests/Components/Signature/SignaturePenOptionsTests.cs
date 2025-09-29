using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignaturePenOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new SignaturePenOptions();

        Assert.Equal("#000000", options.Color);
        Assert.Equal(2.0, options.BaseWidth);
        Assert.Null(options.DashArray);
        Assert.True(options.PressureEnabled);
        Assert.True(options.Smoothing);
        Assert.Equal(1.0, options.Opacity);
        Assert.Equal(LineCap.Round, options.LineCap);
        Assert.Equal(LineJoin.Round, options.LineJoin);
        Assert.NotNull(options.Shadow);
    }

    [Fact]
    public void Properties_Can_Be_Set_And_Retrieved()
    {
        var options = new SignaturePenOptions
        {
            Color = "red",
            BaseWidth = 5.5,
            DashArray = "5,2",
            PressureEnabled = false,
            Smoothing = false,
            Opacity = 0.5,
            LineCap = LineCap.Butt,
            LineJoin = LineJoin.Bevel
        };

        Assert.Equal("red", options.Color);
        Assert.Equal(5.5, options.BaseWidth);
        Assert.Equal("5,2", options.DashArray);
        Assert.False(options.PressureEnabled);
        Assert.False(options.Smoothing);
        Assert.Equal(0.5, options.Opacity);
        Assert.Equal(LineCap.Butt, options.LineCap);
        Assert.Equal(LineJoin.Bevel, options.LineJoin);
    }

    [Fact]
    public void Reset_Sets_Properties_To_Default()
    {
        var options = new SignaturePenOptions
        {
            Color = "blue",
            BaseWidth = 10.0,
            DashArray = "1,1",
            PressureEnabled = false,
            Smoothing = false,
            Opacity = 0.2,
            LineCap = LineCap.Square,
            LineJoin = LineJoin.Miter
        };

        options.Shadow.Enabled = true;
        options.Shadow.Color = "red";
        options.Shadow.Blur = 10;
        options.Shadow.OffsetX = 5;
        options.Shadow.OffsetY = 5;

        options.Reset();

        Assert.Equal("#000000", options.Color);
        Assert.Equal(2.0, options.BaseWidth);
        Assert.Null(options.DashArray);
        Assert.True(options.PressureEnabled);
        Assert.True(options.Smoothing);
        Assert.Equal(1.0, options.Opacity);
        Assert.Equal(LineCap.Round, options.LineCap);
        Assert.Equal(LineJoin.Round, options.LineJoin);

        Assert.False(options.Shadow.Enabled);
        Assert.Equal("rgba(0,0,0,0.3)", options.Shadow.Color);
        Assert.Equal(2, options.Shadow.Blur);
        Assert.Equal(1, options.Shadow.OffsetX);
        Assert.Equal(1, options.Shadow.OffsetY);
    }
}
