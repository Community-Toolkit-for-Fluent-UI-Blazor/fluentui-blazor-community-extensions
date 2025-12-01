using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;

public class StackLayoutTests : TestBase
{
    [Fact]
    public void Update_Horizontal_SetsOffsetX()
    {
        var cut = RenderComponent<StackLayout>(p => p.Add(x => x.Orientation, Orientation.Horizontal)
        .Add(p => p.Spacing, 10));
        var layout = cut.Instance;
        var element = new AnimatedElement();

        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
?.Invoke(layout, new object[] { 3, 5, element });

        Assert.Equal(30, element.OffsetXState!.EndValue);
        Assert.Null(element.OffsetYState);
    }

    [Fact]
    public void Update_Vertical_SetsOffsetY()
    {
        var cut = RenderComponent<StackLayout>(p => p.Add(x => x.Orientation, Orientation.Vertical)
        .Add(p => p.Spacing, 15));
        var layout = cut.Instance;
        var element = new AnimatedElement();
        layout.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
?.Invoke(layout, new object[] { 2, 5, element });

        Assert.Equal(30, element.OffsetYState!.EndValue);
        Assert.Null(element.OffsetXState);
    }
}
