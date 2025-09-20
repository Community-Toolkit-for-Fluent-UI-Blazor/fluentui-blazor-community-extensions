using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;
public class StackLayoutTests
{
    [Fact]
    public void Update_Horizontal_SetsOffsetX()
    {
        var layout = new StackLayout { Orientation = Orientation.Horizontal, Spacing = 10 };
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, new object[] { 3, 5, element });

        Assert.Equal(30, element.OffsetXState.EndValue);
        Assert.Null(element.OffsetYState);
    }

    [Fact]
    public void Update_Vertical_SetsOffsetY()
    {
        var layout = new StackLayout { Orientation = Orientation.Vertical, Spacing = 15 };
        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
.Invoke(layout, new object[] { 2, 5, element });

        Assert.Equal(30, element.OffsetYState.EndValue);
        Assert.Null(element.OffsetXState);
    }
}
