using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;

public class FloatLayoutTests
{
    [Fact]
    public void FloatLayout_Update_AssignsRandomOffsetsWithinDriftRange()
    {
        var layout = new FloatLayout { DriftRange = 50 };
        layout.SetDimensions(100, 100);
        var element = new AnimatedElement();

        layout.ApplyLayout([element]);

        Assert.InRange(element.OffsetXState.EndValue, -25, 25);
        Assert.InRange(element.OffsetYState.EndValue, -25, 25);
    }

}
