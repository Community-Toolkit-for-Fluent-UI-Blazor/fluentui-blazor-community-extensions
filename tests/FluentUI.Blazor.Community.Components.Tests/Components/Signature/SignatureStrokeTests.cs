using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureStrokeTests
{
    [Fact]
    public void Default_Constructor_Should_Initialize_Properties()
    {
        var stroke = new SignatureStroke();

        Assert.NotNull(stroke.Points);
        Assert.Empty(stroke.Points);
        Assert.Equal(3f, stroke.Width);
        Assert.Equal("#000000", stroke.Color);
        Assert.Equal(1f, stroke.Opacity);
        Assert.True(stroke.Smooth);
        Assert.False(stroke.Eraser);
        Assert.Equal(SignatureLineStyle.Solid, stroke.LineStyle);
    }

    [Fact]
    public void Properties_Should_Be_Settable()
    {
        var stroke = new SignatureStroke
        {
            Points = [new PointF(1, 2), new PointF(3, 4)],
            Width = 5f,
            Color = "#FF0000",
            Opacity = 0.5f,
            Smooth = false,
            Eraser = true,
            LineStyle = SignatureLineStyle.Dashed
        };

        Assert.Equal(2, stroke.Points.Count);
        Assert.Equal(new PointF(1, 2), stroke.Points[0]);
        Assert.Equal(5f, stroke.Width);
        Assert.Equal("#FF0000", stroke.Color);
        Assert.Equal(0.5f, stroke.Opacity);
        Assert.False(stroke.Smooth);
        Assert.True(stroke.Eraser);
        Assert.Equal(SignatureLineStyle.Dashed, stroke.LineStyle);
    }
}
