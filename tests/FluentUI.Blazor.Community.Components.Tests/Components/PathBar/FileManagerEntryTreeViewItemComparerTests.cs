using System;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.PathBar;

public class FileManagerEntryTreeViewItemComparerTests
{
    private sealed class TestTreeViewItem : ITreeViewItem
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<ITreeViewItem>? Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Icon? IconCollapsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Icon? IconExpanded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Disabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Expanded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<TreeViewItemExpandedEventArgs, Task>? OnExpandedAsync { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    [Fact]
    public void Compare_BothNull_ReturnsZero()
    {
        var comparer = FileManagerEntryTreeViewItemComparer.Default;
        Assert.Equal(0, comparer.Compare(null, null));
    }

    [Fact]
    public void Compare_XNull_YNotNull_ReturnsOne()
    {
        var comparer = FileManagerEntryTreeViewItemComparer.Default;
        var y = new TestTreeViewItem { Id = "A" };
        Assert.Equal(1, comparer.Compare(null, y));
    }

    [Fact]
    public void Compare_XNotNull_YNull_ReturnsMinusOne()
    {
        var comparer = FileManagerEntryTreeViewItemComparer.Default;
        var x = new TestTreeViewItem { Id = "A" };
        Assert.Equal(-1, comparer.Compare(x, null));
    }

    [Theory]
    [InlineData("A", "A", 0)]
    [InlineData("A", "B", -1)]
    [InlineData("B", "A", 1)]
    public void Compare_ById_ReturnsExpected(string id1, string id2, int expected)
    {
        var comparer = FileManagerEntryTreeViewItemComparer.Default;
        var x = new TestTreeViewItem { Id = id1 };
        var y = new TestTreeViewItem { Id = id2 };
        Assert.Equal(expected, comparer.Compare(x, y));
    }
}
