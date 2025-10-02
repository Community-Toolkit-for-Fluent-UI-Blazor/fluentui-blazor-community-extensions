using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureWatermarkOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new SignatureWatermarkOptions();

        Assert.Equal("Signature", options.Text);
        Assert.Null(options.ImageUrl);
        Assert.Equal(0.1, options.Opacity);
        Assert.Equal("sans-serif", options.FontFamily);
        Assert.Equal(48, options.FontSize);
        Assert.Equal("bold", options.FontWeight);
        Assert.Equal("#000000", options.Color);
        Assert.Equal(-30, options.Rotation);
        Assert.Equal(new Point(50, 50), options.Position);
        Assert.False(options.Repeat);
        Assert.Equal(WatermarkVerticalAlignment.Middle, options.TextAlign);
        Assert.Equal(0, options.LetterSpacing);
    }

    [Fact]
    public void Properties_Can_Be_Set_And_Retrieved()
    {
        var options = new SignatureWatermarkOptions
        {
            Text = "Test",
            ImageUrl = "http://image",
            Opacity = 0.5,
            FontFamily = "Arial",
            FontSize = 24,
            FontWeight = "normal",
            Color = "#FF0000",
            Rotation = 15,
            Position = new Point(10, 20),
            Repeat = true,
            TextAlign = WatermarkVerticalAlignment.End,
            LetterSpacing = 2.5
        };

        Assert.Equal("Test", options.Text);
        Assert.Equal("http://image", options.ImageUrl);
        Assert.Equal(0.5, options.Opacity);
        Assert.Equal("Arial", options.FontFamily);
        Assert.Equal(24, options.FontSize);
        Assert.Equal("normal", options.FontWeight);
        Assert.Equal("#FF0000", options.Color);
        Assert.Equal(15, options.Rotation);
        Assert.Equal(new Point(10, 20), options.Position);
        Assert.True(options.Repeat);
        Assert.Equal(WatermarkVerticalAlignment.End, options.TextAlign);
        Assert.Equal(2.5, options.LetterSpacing);
    }

    [Fact]
    public void Reset_Sets_Properties_To_Default()
    {
        var options = new SignatureWatermarkOptions
        {
            Text = "Test",
            ImageUrl = "http://image",
            Opacity = 0.5,
            FontFamily = "Arial",
            FontSize = 24,
            FontWeight = "normal",
            Color = "#FF0000",
            Rotation = 15,
            Position = new Point(10, 20),
            Repeat = true,
            TextAlign = WatermarkVerticalAlignment.End,
            LetterSpacing = 2.5
        };

        options.Reset();

        Assert.Equal("Signature", options.Text);
        Assert.Null(options.ImageUrl);
        Assert.Equal(0.1, options.Opacity);
        Assert.Equal("sans-serif", options.FontFamily);
        Assert.Equal(48, options.FontSize);
        Assert.Equal("bold", options.FontWeight);
        Assert.Equal("#000000", options.Color);
        Assert.Equal(-30, options.Rotation);
        Assert.Equal(new Point(50, 50), options.Position);
        Assert.False(options.Repeat);
        Assert.Equal(WatermarkVerticalAlignment.Middle, options.TextAlign);
        Assert.Equal(0, options.LetterSpacing);
    }
}
