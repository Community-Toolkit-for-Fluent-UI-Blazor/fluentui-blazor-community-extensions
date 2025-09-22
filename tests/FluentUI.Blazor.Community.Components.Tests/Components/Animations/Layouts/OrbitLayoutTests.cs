using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class OrbitLayoutTests
{
    [Fact]
    public void Update_SetsCorrectOffsets()
    {
        var layout = new OrbitLayout { Radius = 50, CenterX = 10, CenterY = 20 };
        var element = new AnimatedElement();
        layout.ApplyLayout([element]);

        Assert.Equal(60, element.OffsetXState.EndValue, 5);
        Assert.Equal(20, element.OffsetYState.EndValue, 5);
    }
}
