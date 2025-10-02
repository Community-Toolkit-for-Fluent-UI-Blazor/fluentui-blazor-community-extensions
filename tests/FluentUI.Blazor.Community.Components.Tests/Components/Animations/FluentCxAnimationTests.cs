using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Animations;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations;
public class FluentCxAnimationTests : TestBase
{
    public FluentCxAnimationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Component_Rendered_With_Default_Parameters()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxAnimation>();

        // Assert
        Assert.NotNull(cut.Instance);
        Assert.Equal(400, cut.Instance.Width);
        Assert.Equal(400, cut.Instance.Height);
        Assert.Equal(10, cut.Instance.MaxDisplayedItems);
    }

    [Fact]
    public void AddElement_Registers_AnimatedElement()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.AddChildContent<AnimationItem>());

        var fieldInstance = typeof(FluentCxAnimation).GetField("_animationEngine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cut.Instance) as AnimationEngine;

        var elementsField = typeof(AnimationEngine).GetField("_elements", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var elements = elementsField.GetValue(fieldInstance) as List<AnimatedElement>;

        Assert.Single(elements);
    }

    [Fact]
    public void RemoveElement_Unregisters_AnimatedElement()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.AddChildContent<AnimationItem>());

        var item = cut.FindComponent<AnimationItem>();
        item.Instance.Dispose();

        var fieldInstance = typeof(FluentCxAnimation).GetField("_animationEngine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(cut.Instance) as AnimationEngine;

        var elementsField = typeof(AnimationEngine).GetField("_elements", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var elements = elementsField.GetValue(fieldInstance) as List<AnimatedElement>;

        Assert.Empty(elements);
    }

    [Fact]
    public void AddGroup_Registers_AnimatedElementGroup()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.AddChildContent<AnimationGroup>());

        var fieldInstance = typeof(FluentCxAnimation).GetField("_animationEngine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
           .GetValue(cut.Instance) as AnimationEngine;

        var groupsField = typeof(AnimationEngine).GetField("_groups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var groups = groupsField.GetValue(fieldInstance) as List<AnimatedElementGroup>;

        Assert.Single(groups);
    }

    [Fact]
    public void RemoveGroup_Unregisters_AnimatedElementGroup()
    {
        var cut = RenderComponent<FluentCxAnimation>(
           p => p.AddChildContent<AnimationGroup>());

        var group = cut.FindComponent<AnimationGroup>();
        group.Instance.Dispose();

        var fieldInstance = typeof(FluentCxAnimation).GetField("_animationEngine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
           .GetValue(cut.Instance) as AnimationEngine;

        var groupsField = typeof(AnimationEngine).GetField("_groups", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var groups = groupsField.GetValue(fieldInstance) as List<AnimatedElementGroup>;

        Assert.Empty(groups);
    }

    [Fact]
    public void SetLayout_Sets_layoutsStrategy_And_Dimensions()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.Add(x => x.Layout, builder =>
            {
                builder.OpenComponent(0, typeof(BindStackLayout));
                builder.CloseComponent();
            }));

        var layout = cut.Instance.GetType().GetField("_layouts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(cut.Instance) as List<AnimatedLayoutBase>;

        Assert.NotNull(layout);
        Assert.Single(layout);
        Assert.IsType<BindStackLayout>(layout[0]);
    }

    [Fact]
    public void RemoveLayout_Sets_layoutsStrategy_Null()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.Add(x => x.Layout, builder =>
            {
                builder.OpenComponent(0, typeof(BindStackLayout));
                builder.CloseComponent();
            }));

        var layouts = cut.Instance.GetType().GetField("_layouts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(cut.Instance) as List<AnimatedLayoutBase>;

        Assert.NotNull(layouts);
        Assert.Single(layouts);
        cut.Instance.RemoveLayout(layouts.First());

        Assert.Empty(cut.Instance.GetType().GetField("_layouts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(cut.Instance) as List<AnimatedLayoutBase>);
    }

    [Fact]
    public async Task OnLoopCompleted_Resets_layoutsStrategy_StartTime()
    {
        var cut = RenderComponent<FluentCxAnimation>(
            p => p.Add(x => x.Layout, builder =>
            {
                builder.OpenComponent(0, typeof(BindStackLayout));
                builder.CloseComponent();
            }));

        var layouts = cut.Instance.GetType().GetField("_layouts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(cut.Instance) as List<AnimatedLayoutBase>;
        var layout = layouts.First();
        await cut.Instance.OnLoopCompletedAsync();

        Assert.Equal(DateTime.Now, layout.StartTime, TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public async Task OnAnimationCompleted_MorphingLayout_NextLayout()
    {
        var cut = RenderComponent<FluentCxAnimation>(p => p.Add(x => x.Layout, builder =>
        {
            builder.OpenComponent<MorphingLayout>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
            {
                b.OpenComponent(0, typeof(BindStackLayout));
                b.CloseComponent();

                b.OpenComponent(1, typeof(StackedRotatingLayout));
                b.CloseComponent();
            }));

            builder.CloseComponent();
        }));

        await cut.Instance.OnAnimationCompletedAsync();
        var layout = cut.Instance.GetType().GetField("_layouts", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(cut.Instance);
        var _currentLayoutIndex = typeof(MorphingLayout).GetField("_currentLayoutIndex", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(layout);

        Assert.Equal(0, _currentLayoutIndex);
    }

    [Fact]
    public void OnAnimationFrame_Returns_AnimatedElements()
    {
        var cut = RenderComponent<FluentCxAnimation>();
        var result = cut.Instance.OnAnimationFrame();

        Assert.NotNull(result);
        Assert.IsType<List<JsonAnimatedElement>>(result);
    }
}
