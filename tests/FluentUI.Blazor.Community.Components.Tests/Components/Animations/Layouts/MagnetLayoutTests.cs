using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class MagnetLayoutTests
{
    [Fact]
    public void Update_SetsOffsetsNearTarget()
    {
        var layout = new MagnetLayout { MagnetX = 100, MagnetY = 200 };
        var element = new AnimatedElement();
        layout.ApplyLayout([element]);

        Assert.InRange(element.OffsetXState.EndValue, 75, 125);
        Assert.InRange(element.OffsetYState.EndValue, 175, 225);
    }
}
