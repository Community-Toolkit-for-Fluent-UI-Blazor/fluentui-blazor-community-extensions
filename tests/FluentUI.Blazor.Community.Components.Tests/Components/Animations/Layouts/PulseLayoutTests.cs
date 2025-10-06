using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class PulseLayoutTests
{
    [Fact]
    public void Update_SetsScaleWithinExpectedRange()
    {
        var layout = new PulseLayout { BaseScale = 1.0, PulseScale = 0.2 };
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    .Invoke(layout, [0, 1, element]);

        Assert.InRange(element.ScaleXState.EndValue, 1.0, 1.2);
        Assert.InRange(element.ScaleYState.EndValue, 1.0, 1.2);
    }
}
