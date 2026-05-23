using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuUtilsTests
{
    [Fact]
    public void Remove_RemovesItemFromParent()
    {
        var root = CreateItem("root", "Root");
        var child1 = CreateItem("child1", "Child 1");
        var child2 = CreateItem("child2", "Child 2");
        root.Items = new List<ITrailMenuItem>([child1, child2]);

        TrailMenuUtils.Remove(root, ["child1"]);

        var items = root.Items.ToList();
        Assert.DoesNotContain(child1, items);
        Assert.Contains(child2, items);
    }

    [Fact]
    public void Merge_AddsUniqueItems()
    {
        var root = CreateItem("root", "Root");
        var item1 = CreateItem("item1", "Item 1");
        var item2 = CreateItem("item2", "Item 2");
        var item3 = CreateItem("item3", "Item 3");
        root.Items = new[] { item1, item2 };

        TrailMenuUtils.Merge(root, [item2, item3]);

        var items = root.Items.ToList();
        Assert.Equal(3, items.Count);
        Assert.Contains(item1, items);
        Assert.Contains(item2, items);
        Assert.Contains(item3, items);
    }

    [Theory]
    [InlineData("trail-menu-item-test", "test")]
    [InlineData("TEST", "trail-menu-item-test")]
    [InlineData("trail-menu-item-Test", "test")]
    public void IsSame_IgnoresPrefixAndCase(string left, string right)
    {
        Assert.True(TrailMenuUtils.IsSame(left, right));
    }

    [Fact]
    public void Find_ReturnsNestedItem()
    {
        var grandChild = CreateItem("grand", "Grand");
        var child = CreateItem("child", "Child", grandChild);
        var root = CreateItem("root", "Root", child);

        var result = TrailMenuUtils.Find(root.Items, "grand", removePrefix: true);

        Assert.Same(grandChild, result);
    }

    [Fact]
    public void GetIdentifier_AddsPrefixWhenMissing()
    {
        var result = TrailMenuUtils.GetIdentifier("node");

        Assert.Equal("trail-menu-item-node", result);
    }

    [Fact]
    public void UpdateLabel_UpdatesMatchingItem()
    {
        var child = CreateItem("child", "Old");
        var root = CreateItem("root", "Root", child);

        TrailMenuUtils.UpdateLabel(root, "child", "New");

        Assert.Equal("New", child.Label);
    }

    [Fact]
    public void GetPath_ReturnsFullPath()
    {
        var grandChild = CreateItem("grand", "Grand");
        var child = CreateItem("child", "Child", grandChild);
        var root = CreateItem("root", "Root", child);

        var result = TrailMenuUtils.GetPath(grandChild);

        var expected = string.Join(Path.DirectorySeparatorChar, "Root", "Child", "Grand");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseSegments_SplitsPath()
    {
        var path = $"Root{Path.DirectorySeparatorChar} Child {Path.DirectorySeparatorChar}Grand";

        var segments = TrailMenuUtils.ParseSegments(path);

        Assert.Equal(["Root", "Child", "Grand"], segments);
    }

    private static InternalTrailMenuItem CreateItem(string id, string label, params InternalTrailMenuItem[] children)
    {
        var item = new InternalTrailMenuItem
        {
            Id = id,
            Label = label
        };

        if (children.Length > 0)
        {
            item.Items = children;
        }

        return item;
    }
}
