using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class CascadeLayoutTests
{
    [Fact]
    public void CascadeLayout_Update_SetsOffsetsCorrectly()
    {
        var layout = new CascadeLayout
        {
            OffsetXStep = 10,
            OffsetYStep = 20
        };

        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(layout, [2, 5, element]);

        Assert.Equal(20, element.OffsetXState.EndValue);
        Assert.Equal(40, element.OffsetYState.EndValue);
    }
}
