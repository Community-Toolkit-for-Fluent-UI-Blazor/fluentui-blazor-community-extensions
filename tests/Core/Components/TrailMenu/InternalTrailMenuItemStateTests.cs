using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class InternalTrailMenuItemStateTests
{
    [Fact]
    public void Id_IsPrefixedAndImmutable()
    {
        var item = new InternalTrailMenuItem();

        item.Id = "item";
        var first = item.Id;

        item.Id = "other";

        Assert.Equal($"{TrailMenuUtils.Prefix}item", first);
        Assert.Equal(first, item.Id);
    }

    [Fact]
    public void Items_DefaultToEmpty()
    {
        var item = new InternalTrailMenuItem();

        Assert.Empty(item.Items);
    }

    [Fact]
    public void ParentAndNext_AreSettable()
    {
        var parent = new InternalTrailMenuItem { Label = "Parent" };
        var child = new InternalTrailMenuItem { Label = "Child", Parent = parent };

        parent.Next = child;

        Assert.Same(parent, child.Parent);
        Assert.Same(child, parent.Next);
    }
}
