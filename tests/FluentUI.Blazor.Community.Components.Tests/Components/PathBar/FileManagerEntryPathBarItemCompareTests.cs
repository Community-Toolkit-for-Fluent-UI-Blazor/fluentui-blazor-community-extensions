using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.PathBar;

public class PathBarItemMock : IPathBarItem
{
    public PathBarItemMock(string? id, string? label)
    {
        Id = id;
        Label = label;
    }

    public string? Id { get; }

    public string? Label { get; set; }

    public IPathBarItem Parent { get; }

    public Icon? Icon { get; }

    public IEnumerable<IPathBarItem> Items { get; set; } = [];
}

public class FileManagerEntryPathBarItemComparerTests
{
    private readonly PathBarItemComparer _comparer = PathBarItemComparer.Default;

    [Fact]
    public void Compare_BothNull_ReturnsZero()
    {
        Assert.Equal(0, _comparer.Compare(null, null));
    }

    [Fact]
    public void Compare_XNull_YNotNull_ReturnsOne()
    {
        var y = new PathBarItemMock(Identifier.NewId(), "Test label");
        Assert.Equal(1, _comparer.Compare(null, y));
    }

    [Fact]
    public void Compare_XNotNull_YNull_ReturnsMinusOne()
    {
        var x = new PathBarItemMock(Identifier.NewId(), "Test Label");
        Assert.Equal(-1, _comparer.Compare(x, null));
    }

    [Fact]
    public void Compare_SameId_ReturnsZero()
    {
        var id = Identifier.NewId();
        var label = "Test Label";

        var x = new PathBarItemMock(id, label);
        var y = new PathBarItemMock(id, label);
        Assert.Equal(0, _comparer.Compare(x, y));
    }

    [Fact]
    public void Compare_DifferentIds_ReturnsCompareResult()
    {
        var x = new PathBarItemMock("1", "Test Label");
        var y = new PathBarItemMock("2", "Test label");
        var result = _comparer.Compare(x, y);
        Assert.True(result < 0 || result > 0);
        Assert.Equal(x.Id.CompareTo(y.Id), result);
    }
}
