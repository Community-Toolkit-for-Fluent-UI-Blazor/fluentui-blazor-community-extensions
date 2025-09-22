using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class GalaxyLayoutTest
{
    [Fact]
    public void GalaxyLayout_Update_AssignsOffsetsAndRotationBasedOnArmsAndTurns()
    {
        var layout = new GalaxyLayout { Arms = 2, Spread = 1, RotationPerTurn = 180, Turns = 1 };
        layout.SetDimensions(100, 100);
        var element = new AnimatedElement();

        layout.ApplyLayout([element]);
        var centerX = 50;
        var centerY = 50;
        var t = 0.0;
        var baseAngle = 0.0;
        var angle = baseAngle + t * 1 * 180;
        var radius = 1 * t * 50;
        var rad = angle * Math.PI / 180;

        Assert.Equal(centerX + radius * Math.Cos(rad), element.OffsetXState.EndValue, 5);
        Assert.Equal(centerY + radius * Math.Sin(rad), element.OffsetYState.EndValue, 5);
        Assert.Equal(angle, element.RotationState.EndValue, 5);
    }
}
