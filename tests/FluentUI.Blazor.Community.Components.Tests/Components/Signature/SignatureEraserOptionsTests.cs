using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureEraserOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new SignatureEraserOptions();

        Assert.Equal(10.0, options.Size);
        Assert.Equal(EraserShape.Circle, options.Shape);
        Assert.Equal(1.0, options.Opacity);
        Assert.False(options.SoftEdges);
        Assert.Equal(Cursor.Crosshair, options.Cursor);
        Assert.False(options.PartialErase);
        Assert.Equal("crosshair", options.CursorCss);
    }

    [Fact]
    public void Properties_Can_Be_Set_And_Retrieved()
    {
        var options = new SignatureEraserOptions
        {
            Size = 25.5,
            Shape = EraserShape.Square,
            Opacity = 0.5,
            SoftEdges = true,
            Cursor = Cursor.Pointer,
            PartialErase = true
        };

        Assert.Equal(25.5, options.Size);
        Assert.Equal(EraserShape.Square, options.Shape);
        Assert.Equal(0.5, options.Opacity);
        Assert.True(options.SoftEdges);
        Assert.Equal(Cursor.Pointer, options.Cursor);
        Assert.True(options.PartialErase);
        Assert.Equal("pointer", options.CursorCss);
    }

    [Fact]
    public void CursorCss_Returns_Null_When_Cursor_Is_None()
    {
        var options = new SignatureEraserOptions
        {
            Cursor = Cursor.None
        };

        Assert.Null(options.CursorCss);
    }

    [Fact]
    public void CursorCss_Converts_Default_To_Auto()
    {
        var options = new SignatureEraserOptions
        {
            Cursor = Cursor.Default
        };

        Assert.Equal("auto", options.CursorCss);
    }

    [Fact]
    public void Reset_Sets_Properties_To_Default_Values()
    {
        var options = new SignatureEraserOptions
        {
            Size = 20,
            Shape = EraserShape.Square,
            Opacity = 0.3,
            SoftEdges = true,
            Cursor = Cursor.Pointer,
            PartialErase = true
        };

        options.Reset();

        Assert.Equal(10.0, options.Size);
        Assert.Equal(EraserShape.Circle, options.Shape);
        Assert.Equal(1.0, options.Opacity);
        Assert.False(options.SoftEdges);
        Assert.Equal(Cursor.Crosshair, options.Cursor);
        Assert.False(options.PartialErase);
        Assert.Equal("crosshair", options.CursorCss);
    }
}
