using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Animations;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations;

public class AnimationGroupTests : TestBase
{
    public AnimationGroupTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void AddElement_ShouldAddAnimatedElement()
    {
        var cut = RenderComponent<AnimationGroup>(p =>
        {
            p.AddChildContent<AnimationItem>();
        });

        Assert.Single(cut.Instance.AnimatedElementGroup.AnimatedElements);
    }

    [Fact]
    public void RemoveElement_ShouldRemoveAnimatedElement()
    {
        var cut = RenderComponent<AnimationGroup>(p =>
        {
            p.AddChildContent<AnimationItem>();
        });

        var item = cut.FindComponent<AnimationItem>();
        item.Instance.Dispose();

        Assert.Empty(cut.Instance.AnimatedElementGroup.AnimatedElements);
    }

    [Fact]
    public void RemoveLayout_ShouldSetLayoutStrategyToNull()
    {
        var group = new AnimationGroup();
        var layoutMock = new Mock<ILayoutStrategy>();
        group.SetLayout(layoutMock.Object);

        group.RemoveLayout();

        Assert.Null(group.AnimatedElementGroup.LayoutStrategy);
    }

    [Fact]
    public void SetLayout_ShouldSetLayoutStrategy()
    {
        var group = new AnimationGroup();
        var layoutMock = new Mock<ILayoutStrategy>();

        group.SetLayout(layoutMock.Object);

        Assert.Equal(layoutMock.Object, group.AnimatedElementGroup.LayoutStrategy);
    }

    [Fact]
    public void ApplyLayout_ShouldCallApplyLayoutOnGroup()
    {
        var cut = RenderComponent<AnimationGroup>(p =>
        {
        });

        var newLayout = new StackedRotatingLayout();
        cut.Instance.ApplyLayout(newLayout);
        Assert.Equal(newLayout, cut.Instance.AnimatedElementGroup.LayoutStrategy);
    }

    [Fact]
    public void ApplyLayout_DoesNothingOnGroup_WhenGroupHasLayout()
    {
        var cut = RenderComponent<AnimationGroup>(p =>
        {
            p.Add(x => x.Layout, builder =>
            {
                builder.OpenComponent(0, typeof(StackedRotatingLayout));
                builder.CloseComponent();
            });
        });

        var newLayout = new BindStackLayout();
        cut.Instance.ApplyLayout(newLayout);
        Assert.NotNull(cut.Instance.AnimatedElementGroup.LayoutStrategy);
        Assert.Equal(typeof(StackedRotatingLayout), cut.Instance.AnimatedElementGroup.LayoutStrategy.GetType());
    }

    [Fact]
    public void ApplyStartTime_ShouldCallApplyStartTimeOnLayoutStrategy()
    {
        var group = new AnimationGroup();
        var layoutMock = new Mock<ILayoutStrategy>();
        group.SetLayout(layoutMock.Object);
        var now = DateTime.Now;
        group.ApplyStartTime(now);

        layoutMock.Verify(l => l.ApplyStartTime(now), Times.Once);
    }

    [Fact]
    public void SetMaxDisplayedItems_ShouldSetMaxDisplayedItemsOnGroup()
    {
        var cut = RenderComponent<AnimationGroup>(p =>
        {
            p.Add(x => x.MaxDisplayedItems, 5);
        });

        cut.Instance.SetMaxDisplayedItems(10);
        Assert.Equal(5, cut.Instance.MaxDisplayedItems);
    }

    [Fact]
    public async Task Dispose_ShouldCallParentRemoveGroup_AndSuppressFinalize()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.AddChildContent<AnimationGroup>()
        );

        var group = cut.FindComponent<AnimationGroup>();
        Assert.NotNull(group);

        group.Instance.Dispose();

        var fieldInstance = typeof(FluentCxAnimation).GetField("_animationEngine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(cut.Instance) as AnimationEngine;

        var groupsField = typeof(AnimationEngine).GetField("_groups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var groups = groupsField?.GetValue(fieldInstance) as List<AnimatedElementGroup>;

        Assert.NotNull(groups);
        Assert.Empty(groups);
    }
}
