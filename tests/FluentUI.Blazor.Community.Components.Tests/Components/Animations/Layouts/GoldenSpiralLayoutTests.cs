using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class GoldenSpiralLayoutTests
{
    [Fact]
    public void GoldenSpiralLayout_Update_AssignsOffsetsAndRotationInSpiral()
    {
        var layout = new GoldenSpiralLayout();
        layout.SetDimensions(200, 100);
        var element = new AnimatedElement();

        layout.ApplyLayout([element]);
        var scale = Math.Min(200, 100) / 10;
        var centerX = 100;
        var centerY = 50;
        var angle = 0 * 0.5;
        var radius = scale * Math.Pow(1.61803398875, angle / (2 * Math.PI));
        var x = centerX + radius * Math.Cos(angle);
        var y = centerY + radius * Math.Sin(angle);

        Assert.Equal(x, element.OffsetXState.EndValue, 5);
        Assert.Equal(y, element.OffsetYState.EndValue, 5);
        Assert.Equal(0, element.RotationState.EndValue, 5);
    }
}
