using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class SpiralGalaxyLayoutTests
{
    [Fact]
    public void Update_SetsSpiralGalaxyOffsetsAndRotation()
    {
        var layout = new SpiralGalaxyLayout { SpiralFactor = 0.5 };
        layout.SetDimensions(100, 200);

        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    .Invoke(layout, new object[] { 2, 5, element });

        var theta = 2 * 0.3;
        var radius = 0.5 * Math.Exp(0.1 * theta);
        var centerX = 100 / 2;
        var centerY = 200 / 2;
        var expectedX = centerX + radius * Math.Cos(theta);
        var expectedY = centerY + radius * Math.Sin(theta);
        var expectedRotation = MathHelper.ToDegrees(theta);

        Assert.Equal(expectedX, element.OffsetXState.EndValue, 5);
        Assert.Equal(expectedY, element.OffsetYState.EndValue, 5);
        Assert.Equal(expectedRotation, element.RotationState.EndValue, 5);
    }
}
