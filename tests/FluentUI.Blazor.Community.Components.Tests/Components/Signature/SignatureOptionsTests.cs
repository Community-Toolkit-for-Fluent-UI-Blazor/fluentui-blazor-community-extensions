using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureOptionsTests
{

    [Fact]
    public void Default_Constructor_Sets_Default_Values()
    {
        var options = new SignatureOptions();

        Assert.Equal(2.0f, options.StrokeWidth);
        Assert.Equal("#000000", options.PenColor);
        Assert.Equal(1.0f, options.PenOpacity);
        Assert.Equal("#000000", options.ShadowColor);
        Assert.Equal(0.3f, options.ShadowOpacity);
        Assert.Equal(SignatureLineStyle.Solid, options.StrokeStyle);
        Assert.True(options.Smooth);
        Assert.False(options.UseShadow);
        Assert.False(options.UsePointerPressure);
        Assert.Equal(300, options.Width);
        Assert.Equal(150, options.Height);
        Assert.Equal("#FFFFFF", options.Background);
        Assert.True(options.ShowSeparatorLine);
        Assert.Equal(1.0f, options.SeparatorY);
        Assert.Equal("#0058E9", options.SeparatorLineColor);
        Assert.False(options.ShowGrid);
        Assert.Equal("#e0e0e0", options.GridColor);
        Assert.Equal(20.0f, options.GridSpacing);
        Assert.Equal(SignatureGridType.Lines, options.GridType);
        Assert.Equal(0.5f, options.GridOpacity);
        Assert.Null(options.WatermarkText);
        Assert.Equal(0.1f, options.WatermarkOpacity);
    }

    [Fact]
    public void Constructor_Sets_Custom_Values()
    {
        var options = new SignatureOptions(
            StrokeWidth: 5.0f,
            PenColor: "#FF0000",
            PenOpacity: 0.5f,
            ShadowColor: "#00FF00",
            ShadowOpacity: 0.8f,
            StrokeStyle: SignatureLineStyle.Dashed,
            Smooth: false,
            UseShadow: true,
            UsePointerPressure: true,
            Width: 500,
            Height: 200,
            Background: "#CCCCCC",
            ShowSeparatorLine: false,
            SeparatorY: 10.0f,
            SeparatorLineColor: "#123456",
            ShowGrid: true,
            GridColor: "#654321",
            GridSpacing: 10.0f,
            GridType: SignatureGridType.Dots,
            GridOpacity: 0.9f,
            WatermarkText: "Test",
            WatermarkOpacity: 0.7f
        );

        Assert.Equal(5.0f, options.StrokeWidth);
        Assert.Equal("#FF0000", options.PenColor);
        Assert.Equal(0.5f, options.PenOpacity);
        Assert.Equal("#00FF00", options.ShadowColor);
        Assert.Equal(0.8f, options.ShadowOpacity);
        Assert.Equal(SignatureLineStyle.Dashed, options.StrokeStyle);
        Assert.False(options.Smooth);
        Assert.True(options.UseShadow);
        Assert.True(options.UsePointerPressure);
        Assert.Equal(500, options.Width);
        Assert.Equal(200, options.Height);
        Assert.Equal("#CCCCCC", options.Background);
        Assert.False(options.ShowSeparatorLine);
        Assert.Equal(10.0f, options.SeparatorY);
        Assert.Equal("#123456", options.SeparatorLineColor);
        Assert.True(options.ShowGrid);
        Assert.Equal("#654321", options.GridColor);
        Assert.Equal(10.0f, options.GridSpacing);
        Assert.Equal(SignatureGridType.Dots, options.GridType);
        Assert.Equal(0.9f, options.GridOpacity);
        Assert.Equal("Test", options.WatermarkText);
        Assert.Equal(0.7f, options.WatermarkOpacity);
    }

    [Fact]
    public void Equality_Works_As_Expected()
    {
        var options1 = new SignatureOptions();
        var options2 = new SignatureOptions();

        Assert.Equal(options1, options2);
        Assert.True(options1 == options2);
        Assert.False(options1 != options2);
        Assert.Equal(options1.GetHashCode(), options2.GetHashCode());
    }

    [Fact]
    public void Deconstruct_Works_As_Expected()
    {
        var options = new SignatureOptions(
            StrokeWidth: 3.0f,
            PenColor: "#111111"
        );

        var (
            strokeWidth, penColor, penOpacity, shadowColor, shadowOpacity,
            strokeStyle, smooth, useShadow, usePointerPressure, width, height,
            background, showSeparatorLine, separatorY, separatorLineColor,
            showGrid, gridColor, gridSpacing, gridType, gridOpacity,
            watermarkText, watermarkOpacity
        ) = options;

        Assert.Equal(3.0f, strokeWidth);
        Assert.Equal("#111111", penColor);
        Assert.Equal(1.0f, penOpacity);
        Assert.Equal("#000000", shadowColor); 
        
    }
}
