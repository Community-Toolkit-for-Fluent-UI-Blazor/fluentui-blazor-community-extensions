using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerResizeEventArgsTests
{
    private static SchedulerItem<string> CreateItem(long id = 1, string title = "Item") =>
        new SchedulerItem<string>
        {
            Id = id,
            Title = title,
            Start = new DateTime(2025, 11, 30, 9, 0, 0),
            End = new DateTime(2025, 11, 30, 10, 0, 0)
        };

    [Fact]
    public void Constructor_Assigns_Properties()
    {
        var item = CreateItem();
        var args = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Top, 12.5f, 34.5f);

        Assert.Same(item, args.Item);
        Assert.Equal(ResizeDirection.Top, args.Direction);
        Assert.Equal(12.5f, args.X);
        Assert.Equal(34.5f, args.Y);
    }

    [Fact]
    public void Two_Args_With_Same_Values_Are_Equal()
    {
        var item = CreateItem();
        var a = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Left, 1f, 2f);
        var b = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Left, 1f, 2f);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Args_Differing_In_Item_Are_NotEqual()
    {
        var item1 = CreateItem(1, "A");
        var item2 = CreateItem(2, "B"); // different instance

        var a = new SchedulerResizeEventArgs<string>(item1, ResizeDirection.Right, 0f, 0f);
        var b = new SchedulerResizeEventArgs<string>(item2, ResizeDirection.Right, 0f, 0f);

        Assert.NotEqual(a, b);
        Assert.False(a == b);
    }

    [Fact]
    public void Args_Differing_In_DirectionOrCoordinates_Are_NotEqual()
    {
        var item = CreateItem();

        var baseArgs = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Bottom, 5f, 5f);
        var diffDir = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Top, 5f, 5f);
        var diffX = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Bottom, 6f, 5f);
        var diffY = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Bottom, 5f, 6f);

        Assert.NotEqual(baseArgs, diffDir);
        Assert.NotEqual(baseArgs, diffX);
        Assert.NotEqual(baseArgs, diffY);
    }

    [Fact]
    public void Deconstruct_Provides_All_Values()
    {
        var item = CreateItem();
        var args = new SchedulerResizeEventArgs<string>(item, ResizeDirection.Left, 7.5f, 8.5f);

        var (it, dir, x, y) = args;

        Assert.Same(item, it);
        Assert.Equal(ResizeDirection.Left, dir);
        Assert.Equal(7.5f, x);
        Assert.Equal(8.5f, y);
    }
}
