using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.PathBar;

public class PathBarBuilderTests
{
    private class TestPathBarItem : IPathBarItem
    {
        public string? Label { get; set; }
        public IPathBarItem Parent { get; set; }
        public string? Id { get; set; }
        public Icon? Icon { get; }
        public IEnumerable<IPathBarItem> Items { get; set; } = new List<IPathBarItem>();
    }

    [Fact]
    public void Remove_MultipleIds_RemovesItems()
    {
        var parent = new TestPathBarItem { Id = "parent" };
        var child1 = new TestPathBarItem { Id = "path-bar-item-child1", Parent = parent };
        var child2 = new TestPathBarItem { Id = "path-bar-item-child2", Parent = parent };
        parent.Items = new[] { child1, child2 };

        PathBarItemBuilder.Remove(parent, new[] { "child1", "child2" });

        Assert.Empty(parent.Items);
    }

    [Fact]
    public void From_BuildsPathBarItems()
    {
        var entry = FileManagerEntry<NoFileEntryData>.CreateEntry([0,1,2], "Root", 3);
        entry.Id = "id1";
        var items = PathBarItemBuilder.From([entry]).ToList();

        Assert.Single(items);
        Assert.Equal($"{PathBarItemBuilder.Prefix}id1", items[0].Id);
        Assert.Equal("Root", items[0].Label);
    }

    [Fact]
    public void Merge_AddsItemsToRoot()
    {
        var root = new TestPathBarItem { Id = "root" };
        var item1 = new TestPathBarItem { Id = "a" };
        var item2 = new TestPathBarItem { Id = "b" };
        root.Items = new[] { item1 };

        PathBarItemBuilder.Merge(root, [item2]);

        Assert.Contains(item1, root.Items);
        Assert.Contains(item2, root.Items);
    }

    [Fact]
    public void Find_ReturnsCorrectItem()
    {
        var child = new TestPathBarItem { Id = "path-bar-item-foo" };
        var root = new TestPathBarItem { Id = "root", Items = [child] };

        var found = PathBarItemBuilder.Find(root.Items, "foo", true);

        Assert.Equal(child, found);
    }

    [Fact]
    public void GetAllParts_ReturnsMatchingSegments()
    {
        var child = new TestPathBarItem { Label = "child" };
        var root = new TestPathBarItem { Label = "root", Items = [child] };

        var result = PathBarItemBuilder.GetAllParts(root, ["root", "child"]).ToList();

        Assert.Single(result);
        Assert.Equal(child, result[0]);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("foo", "path-bar-item-foo")]
    [InlineData("path-bar-item-foo", "path-bar-item-foo")]
    public void GetIdentifier_WorksAsExpected(string? input, string? expected)
    {
        var result = PathBarItemBuilder.GetIdentifier(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpdateLabel_UpdatesCorrectItem()
    {
        var child = new TestPathBarItem { Id = "path-bar-item-foo", Label = "old" };
        var root = new TestPathBarItem { Items = [child] };

        PathBarItemBuilder.UpdateLabel(root, "foo", "new");

        Assert.Equal("new", child.Label);
    }

    [Fact]
    public void GetPath_ReturnsFullPath()
    {
        var root = new TestPathBarItem { Label = "root" };
        var child = new TestPathBarItem { Label = "child", Parent = root };
        var leaf = new TestPathBarItem { Label = "leaf", Parent = child };

        var path = PathBarItemBuilder.GetPath(leaf);

        var expected = $"root{Path.DirectorySeparatorChar}child{Path.DirectorySeparatorChar}leaf";
        Assert.Equal(expected, path);
    }
}
