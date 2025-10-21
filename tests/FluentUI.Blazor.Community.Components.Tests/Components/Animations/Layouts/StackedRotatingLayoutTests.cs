using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class StackedRotatingLayoutTests
{
    [Fact]
    public void Update_SetsRotationAndOpacity()
    {
        var layout = new StackedRotatingLayout();
        var element = new AnimatedElement();
        int count = 4;

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, [2, count, element]);

        Assert.Equal(180.0, element.RotationState.EndValue, 2);
        Assert.Equal(1.0 - (2.0 / (count + 1)), element.OpacityState.EndValue, 4);
    }
}

