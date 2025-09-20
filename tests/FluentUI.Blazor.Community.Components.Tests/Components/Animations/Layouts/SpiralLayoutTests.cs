using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class SpiralLayoutTests
{
    [Fact]
    public void Update_SetsSpiralOffsetsAndRotation()
    {
        var layout = new SpiralLayout { RadiusStep = 15, AngleStep = 45 };
        var element = new AnimatedElement();
            layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, new object[] { 3, 10, element });

        var angle = 3 * 45 * Math.PI / 180;
        var radius = 3 * 15;
        var expectedX = radius * Math.Cos(angle);
        var expectedY = radius * Math.Sin(angle);
        var expectedRotation = MathHelper.ToDegrees(angle);

        Assert.Equal(expectedX, element.OffsetXState.EndValue, 5);
        Assert.Equal(expectedY, element.OffsetYState.EndValue, 5);
        Assert.Equal(expectedRotation, element.RotationState.EndValue, 5);
    }
}
