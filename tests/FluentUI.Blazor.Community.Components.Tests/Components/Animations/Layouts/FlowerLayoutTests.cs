using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class FlowerLayoutTests
{
    [Fact]
    public void FlowerLayout_Update_AssignsCorrectOffsetsAndRotation()
    {
        var layout = new FlowerLayout();
        layout.SetDimensions(120, 90);
        var element = new AnimatedElement { Rotation = 10 };

        layout.ApplyLayout([element]);
        var expectedRadius = Math.Min(120, 90) / 3;
        var expectedAngle = 0;
        var expectedRadians = expectedAngle * Math.PI / 180;
        var expectedX = 60 + expectedRadius * Math.Cos(expectedRadians);
        var expectedY = 45 + expectedRadius * Math.Sin(expectedRadians);

        Assert.Equal(expectedX, element.OffsetXState.EndValue, 5);
        Assert.Equal(expectedY, element.OffsetYState.EndValue, 5);
        Assert.Equal(expectedAngle, element.RotationState.EndValue, 5);
    }
}
