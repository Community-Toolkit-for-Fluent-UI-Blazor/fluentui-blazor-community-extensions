using System.Collections.Generic;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuLayoutEngineTests
{
    [Fact]
    public void Compute_AllItemsVisibleWhenSpaceAllows()
    {
        var (items, cache) = CreateItemsWithSizes(("a", 10), ("b", 20), ("c", 30));
        var visible = new List<ITrailMenuItem>();
        var overflow = new List<ITrailMenuItem>();

        TrailMenuLayoutEngine.Compute(items, cache, containerWidth: 100, visible, overflow);

        Assert.Equal(items, visible);
        Assert.Empty(overflow);
    }

    [Fact]
    public void Compute_PlacesItemsInOverflowWhenNeeded()
    {
        var (items, cache) = CreateItemsWithSizes(("a", 10), ("b", 20), ("c", 30));
        var visible = new List<ITrailMenuItem>();
        var overflow = new List<ITrailMenuItem>();

        TrailMenuLayoutEngine.Compute(items, cache, containerWidth: 50, visible, overflow);

        Assert.Single(visible);
        Assert.Equal(items[0], visible[0]);
        Assert.Equal([items[1], items[2]], overflow);
    }

    private static (IReadOnlyList<ITrailMenuItem> Items, TrailMenuCache Cache) CreateItemsWithSizes(params (string id, double size)[] values)
    {
        var cache = new TrailMenuCache();
        var items = new List<ITrailMenuItem>();

        foreach (var (id, size) in values)
        {
            var item = new InternalTrailMenuItem { Id = id, Label = id };
            items.Add(item);
            cache.Set(item.Id!, size);
        }

        return (items, cache);
    }
}
