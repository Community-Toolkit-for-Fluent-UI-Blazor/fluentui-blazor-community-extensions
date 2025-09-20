using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class HelixLayoutTests
{
    [Fact]
    public void Update_SetsOffsetsAndRotation()
    {
        var layout = new HelixLayout();
        layout.SetDimensions(100, 100);
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
         .Invoke(layout, [2, 5, element]);

        Assert.Equal(70, element.OffsetXState.EndValue);
        Assert.InRange(element.OffsetYState.EndValue, 80, 90);
        Assert.InRange(element.RotationState.EndValue, 55, 65);
    }
}
