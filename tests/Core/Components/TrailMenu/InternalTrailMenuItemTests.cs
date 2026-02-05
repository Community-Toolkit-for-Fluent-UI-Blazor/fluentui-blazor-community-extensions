using System.Linq;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class InternalTrailMenuItemTests
{
    [Fact]
    public void Id_SetOnceUsesPrefixedValue()
    {
        var item = new InternalTrailMenuItem();

        item.Id = "first";
        item.Id = "second";

        Assert.Equal(TrailMenuUtils.GetIdentifier("first"), item.Id);
    }

    [Fact]
    public void Items_AssignParentsToChildren()
    {
        var parent = new InternalTrailMenuItem { Id = "parent", Label = "Parent" };
        var child1 = new InternalTrailMenuItem { Id = "child1", Label = "Child 1" };
        var child2 = new InternalTrailMenuItem { Id = "child2", Label = "Child 2" };

        parent.Items = new[] { child1, child2 };

        Assert.Same(parent, child1.Parent);
        Assert.Same(parent, child2.Parent);
        Assert.Equal(2, parent.Items.Count());
    }
}
