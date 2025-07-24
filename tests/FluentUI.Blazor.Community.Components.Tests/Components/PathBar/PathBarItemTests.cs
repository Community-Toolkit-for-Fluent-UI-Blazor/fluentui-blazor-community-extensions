using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class PathBarItemTests
    : TestBase
{
    [Fact]
    public void PathBarItem_Default()
    {
        PathBarItem instance = new();

        Assert.Empty(instance.Items);
        Assert.Null(instance.Label);
        Assert.Null(instance.Id);
        Assert.Null(instance.Icon);
        Assert.Null(instance.Parent);
    }

    [Theory]
    [InlineData("Home")]
    [InlineData("Visual Studio")]
    [InlineData("FluentUI Extensions")]
    public void PathBarItem_Label(string label)
    {
        PathBarItem instance = new()
        {
            Label = label
        };

        Assert.Equal(label, instance.Label);
    }

    [Theory]
    [InlineData("01234")]
    [InlineData("045678")]
    [InlineData("bgddf-deedfsf-00fsdfsd-deazea")]
    public void PathBarItem_Id(string id)
    {
        PathBarItem instance = new()
        {
            Id = id
        };

        Assert.Equal(id, instance.Id);
    }

    [Fact]
    public void PathBarItem_Icon()
    {
        var icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Desktop();
        var instance = new PathBarItem()
        {
            Icon = icon
        };

        Assert.NotNull(instance.Icon);
        Assert.Equal(icon, instance.Icon);
    }

    [Fact]
    public void PathBarItem_WithParent()
    {
        var child = new PathBarItem();

        var parent = new PathBarItem()
        {
            Items = [child]
        };

        Assert.NotNull(child.Parent);
        Assert.Equal(parent, child.Parent);
    }

    [Fact]
    public void PathBarItem_WithChildren()
    {
        var child1 = new PathBarItem();
        var child2 = new PathBarItem();

        var parent = new PathBarItem()
        {
            Items = [child1, child2]
        };

        Assert.NotEmpty(parent.Items);
        Assert.Equal(2, parent.Items.Count());
        Assert.Equal(child1, parent.Items.ElementAt(0));
        Assert.Equal(child2, parent.Items.ElementAt(1));
    }

    [Fact]
    public void Find_ReturnsCorrectItem()
    {
        var root = new PathBarItem { Label = "Root", Id = "root" };
        var child = new PathBarItem { Label = "Child", Id = "child" };
        var grandChild = new PathBarItem { Label = "GrandChild", Id = "grandchild" };
        child.Items = [grandChild];
        root.Items = [child];

        var found = PathBarItem.Find([root], "grandchild");
        Assert.Equal(grandChild, found);

        var notFound = PathBarItem.Find([root], "notfound");
        Assert.Null(notFound);
    }

    [Fact]
    public void GetPath_ReturnsCorrectPath()
    {
        var root = new PathBarItem { Label = "Root", Id = "root" };
        var child = new PathBarItem { Label = "Child", Id = "child" };
        var grandChild = new PathBarItem { Label = "GrandChild", Id = "grandchild" };
        child.Items = [grandChild];
        root.Items = [child];

        var expected = string.Join(Path.DirectorySeparatorChar, new[] { "Root", "Child", "GrandChild" });
        var actual = PathBarItem.GetPath(grandChild);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetPath_Null_ReturnsNull()
    {
        Assert.Null(PathBarItem.GetPath(null));
    }
}
