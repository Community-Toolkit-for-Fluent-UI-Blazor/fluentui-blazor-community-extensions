using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class GridLayoutTests
{
    [Fact]
    public void Update_SetsCorrectOffsets()
    {
        var layout = new GridLayout { Columns = 3, CellWidth = 50, CellHeight = 40 };
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
    .Invoke(layout, [4, 9, element]);

        Assert.Equal(50, element.OffsetXState.EndValue);
        Assert.Equal(40, element.OffsetYState.EndValue);
    }
}
