using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class FanLayoutTests
{
    [Fact]
    public void FanLayout_Update_SetsFanPropertiesCorrectly()
    {
        var layout = new FanLayout
        {
            Radius = 100,
            AngleSpread = 90
        };
        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
             .Invoke(layout, [0, 3, element]);

        // Calculs attendus
        var startAngle = -45.0;
        var rad = Math.PI * startAngle / 180.0;
        Assert.Equal(100 * Math.Cos(rad), element.OffsetXState.EndValue, 5);
        Assert.Equal(100 * Math.Sin(rad), element.OffsetYState.EndValue, 5);
        Assert.Equal(startAngle, element.RotationState.EndValue, 5);
    }
}
