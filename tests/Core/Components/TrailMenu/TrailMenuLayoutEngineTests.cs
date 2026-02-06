using System.Collections.Generic;
using System.Linq;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuLayoutEngineTests
{
    [Fact]
    public void Compute_NoItems_ClearsLists()
    {
        var cache = new TrailMenuCache();
        var visible = new List<ITrailMenuItem> { CreateItem("visible") };
        var overflow = new List<ITrailMenuItem> { CreateItem("overflow") };

        TrailMenuLayoutEngine.Compute([], cache, 100, visible, overflow);

        Assert.Empty(visible);
        Assert.Empty(overflow);
    }

    [Fact]
    public void Compute_AllItemsFit_LeavesOverflowEmpty()
    {
        var items = new List<ITrailMenuItem>
        {
            CreateItem("item1"),
            CreateItem("item2"),
            CreateItem("item3")
        };
        var cache = new TrailMenuCache();
        cache.Set(items[0].Id!, 50);
        cache.Set(items[1].Id!, 60);
        cache.Set(items[2].Id!, 70);

        var visible = new List<ITrailMenuItem>();
        var overflow = new List<ITrailMenuItem>();

        TrailMenuLayoutEngine.Compute(items, cache, 220, visible, overflow);

        Assert.Equal(items, visible);
        Assert.Empty(overflow);
    }

    [Fact]
    public void Compute_WhenItemsOverflow_PutsAllButLastInOverflow()
    {
        var items = new List<ITrailMenuItem>
        {
            CreateItem("item1"),
            CreateItem("item2"),
            CreateItem("item3")
        };
        var cache = new TrailMenuCache();
        cache.Set(items[0].Id!, 50);
        cache.Set(items[1].Id!, 60);
        cache.Set(items[2].Id!, 70);

        var visible = new List<ITrailMenuItem>();
        var overflow = new List<ITrailMenuItem>();

        TrailMenuLayoutEngine.Compute(items, cache, 200, visible, overflow);

        Assert.Equal([items[2]], visible);
        Assert.Equal(items.Take(2).ToList(), overflow);
    }

    private static InternalTrailMenuItem CreateItem(string id) => new()
    {
        Id = id,
        Label = id
    };
}
