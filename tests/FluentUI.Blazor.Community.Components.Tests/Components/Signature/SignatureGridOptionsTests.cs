using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureGridOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new SignatureGridOptions();

        Assert.Equal(GridDisplayMode.Lines, options.DisplayMode);
        Assert.Equal(20.0, options.CellSize);
        Assert.Equal("#cccccc", options.Color);
        Assert.Equal(0.5, options.Opacity);
        Assert.Equal(5, options.BoldEvery);
        Assert.Equal(1.0, options.StrokeWidth);
        Assert.Null(options.DashArray);
        Assert.False(options.ShowAxes);
        Assert.Equal("transparent", options.BackgroundColor);
        Assert.Equal(0.0, options.Margin);
        Assert.Equal(1.5, options.PointRadius);
    }

    [Fact]
    public void Properties_Can_Be_Set_And_Retrieved()
    {
        var options = new SignatureGridOptions
        {
            DisplayMode = GridDisplayMode.Dots,
            CellSize = 10.0,
            Color = "#000000",
            Opacity = 0.8,
            BoldEvery = 3,
            StrokeWidth = 2.0,
            DashArray = "2,2",
            ShowAxes = true,
            BackgroundColor = "#ffffff",
            Margin = 5.0,
            PointRadius = 2.5
        };

        Assert.Equal(GridDisplayMode.Dots, options.DisplayMode);
        Assert.Equal(10.0, options.CellSize);
        Assert.Equal("#000000", options.Color);
        Assert.Equal(0.8, options.Opacity);
        Assert.Equal(3, options.BoldEvery);
        Assert.Equal(2.0, options.StrokeWidth);
        Assert.Equal("2,2", options.DashArray);
        Assert.True(options.ShowAxes);
        Assert.Equal("#ffffff", options.BackgroundColor);
        Assert.Equal(5.0, options.Margin);
        Assert.Equal(2.5, options.PointRadius);
    }

    [Fact]
    public void Reset_Sets_Properties_To_Defaults()
    {
        var options = new SignatureGridOptions
        {
            DisplayMode = GridDisplayMode.Dots,
            CellSize = 10.0,
            Color = "#000000",
            Opacity = 0.8,
            BoldEvery = 3,
            StrokeWidth = 2.0,
            DashArray = "2,2",
            ShowAxes = true,
            BackgroundColor = "#ffffff",
            Margin = 5.0,
            PointRadius = 2.5
        };

        options.Reset();

        Assert.Equal(GridDisplayMode.Lines, options.DisplayMode);
        Assert.Equal(20.0, options.CellSize);
        Assert.Equal("#cccccc", options.Color);
        Assert.Equal(0.5, options.Opacity);
        Assert.Equal(5, options.BoldEvery);
        Assert.Equal(1.0, options.StrokeWidth);
        Assert.Null(options.DashArray);
        Assert.False(options.ShowAxes);
        Assert.Equal("transparent", options.BackgroundColor);
        Assert.Equal(0.0, options.Margin);
        Assert.Equal(1.5, options.PointRadius);
    }
}
