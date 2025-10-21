using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class HeartLayoutTests
{
    [Fact]
    public void Update_SetsOffsetsOnHeartCurve()
    {
        var layout = new HeartLayout();
        layout.SetDimensions(200, 200);
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    .Invoke(layout, [0, 10, element]);

        // Vérifie que les offsets sont calculés et non nuls
        Assert.Equal(element.OffsetXState.EndValue, 100);
        Assert.InRange(element.OffsetYState.EndValue, 65, 67);
    }
}
