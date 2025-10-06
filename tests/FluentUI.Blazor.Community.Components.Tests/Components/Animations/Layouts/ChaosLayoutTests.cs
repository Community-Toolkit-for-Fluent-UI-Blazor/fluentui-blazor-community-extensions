using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class ChaosLayoutTests
{
    [Fact]
    public void ChaosLayout_Update_SetsOffsetsCorrectly()
    {
        var layout = new ChaosLayout
        {
            Spread = 100
        };

        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(layout, [0, 1, element]);

        Assert.InRange(element.OffsetXState.EndValue, -50, 50);
        Assert.InRange(element.OffsetYState.EndValue, -50, 50);
        Assert.InRange(element.RotationState.EndValue, 0, 360);
        Assert.InRange(element.ScaleXState.EndValue, 0.5, 1.5);
        Assert.InRange(element.ScaleYState.EndValue, 0.5, 1.5);
        Assert.InRange(element.OpacityState.EndValue, 0.5, 1.0);
    }
}
