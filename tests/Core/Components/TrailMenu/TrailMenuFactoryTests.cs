using System;
using System.Linq;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuFactoryTests
{
    [Fact]
    public void Create_BuildsRootItem()
    {
        var icon = TrailMenuIcons.HomeIcon;
        var builder = TrailMenuFactory.Create("root", "Root", icon);

        var root = builder.Build();

        Assert.Equal("Root", root.Label);
        Assert.Equal(TrailMenuUtils.GetIdentifier("root"), root.Id);
        Assert.Same(icon, root.Icon);
    }

    [Fact]
    public void Builder_AddsItemsToRoot()
    {
        var builder = TrailMenuFactory.Create("root", "Root")
            .Add("child1", "Child 1")
            .Root()
            .Add("child2", "Child 2");

        var root = builder.Build();
        var children = root.Items.ToList();

        Assert.Equal(2, children.Count);
        Assert.Equal(["Child 1", "Child 2"], children.Select(c => c.Label));
    }

    [Fact]
    public void AddSibling_ThrowsWhenAtRoot()
    {
        var builder = TrailMenuFactory.Create("root", "Root");

        Assert.Throws<InvalidOperationException>(() => builder.AddSibling("sibling", "Sibling"));
    }
}
