using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations.Layouts;

public class MorphingLayoutTests : TestContext
{
    public MorphingLayoutTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Add_AddsLayout_WhenNotPresent()
    {
        var layout = new MorphingLayout();
        var animatedLayout = new StackedRotatingLayout();
        layout.Add(animatedLayout);
        layout.Add(animatedLayout);

        var field = typeof(MorphingLayout)
            .GetField("_layouts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var layouts = field.GetValue(layout) as List<AnimatedLayoutBase>;
        Assert.Single(layouts);
    }

    [Fact]
    public void Remove_RemovesLayout_WhenPresent()
    {
        var layout = new MorphingLayout();
        var animatedLayout = new StackedRotatingLayout();
        layout.Add(animatedLayout);
        layout.Remove(animatedLayout);

        var field = typeof(MorphingLayout)
            .GetField("_layouts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var layouts = field.GetValue(layout) as List<AnimatedLayoutBase>;
        Assert.Empty(layouts);
    }

    [Fact]
    public void NextLayout_ReturnsFalse_WhenLessThanTwoLayouts()
    {
        var layout = new MorphingLayout();
        Assert.False(layout.NextLayout());
        layout.Add(new StackedRotatingLayout());
        Assert.False(layout.NextLayout());
    }

    [Fact]
    public void NextLayout_Loops_WhenParentLoopTrue()
    {
        var mockModule = JSInterop.SetupModule("./_content/FluentUI.Blazor.Community.Components/Components/Animations/FluentCxAnimation.razor.js");

        var cut = RenderComponent<FluentCxAnimation>(
            p => p.Add(x => x.Loop, true)
                  .Add(x => x.Layout, builder =>
            {
                builder.OpenComponent(0, typeof(MorphingLayout));
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<StackedRotatingLayout>(0);
                    b.CloseComponent();
                    b.OpenComponent<WaveLayout>(1);
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        // Avance jusqu'à la fin
        var layout = cut.FindComponent<MorphingLayout>().Instance;
        Assert.True(layout.NextLayout());
        Assert.True(layout.NextLayout());
    }

    [Fact]
    public void CreateState_ReturnsAnimationState_WithCorrectValues()
    {
        var layout = new MorphingLayout
        {
            Duration = TimeSpan.FromSeconds(1),
            EasingFunction = EasingFunction.Linear,
            EasingMode = EasingMode.In
        };
        var now = DateTime.Now;
        layout.ApplyStartTime(now);
        var state = layout.CreateState(10, 5);
        Assert.Equal(5, state.StartValue);
        Assert.Equal(10, state.EndValue);
        Assert.Equal(layout.Duration, state.Duration);
        Assert.Equal(now, state.StartTime);
        Assert.Equal(layout.EasingFunction, state.EasingFunction);
        Assert.Equal(layout.EasingMode, state.EasingMode);
    }

    [Fact]
    public void SetParametersAsync_Throws_WhenImmediateTrue()
    {
        var layout = new MorphingLayout();
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object>
        {
            { "Immediate", true }
        });
        Assert.Throws<NotSupportedException>(() => layout.SetParametersAsync(parameters).GetAwaiter().GetResult());
    }

    [Fact]
    public void SetImmediate_Throws_WhenTrue()
    {
        Assert.Throws<NotSupportedException>(() => MorphingLayout.SetImmediate(true));
    }

    [Fact]
    public void SetImmediate_DoesNotThrow_WhenFalse()
    {
        MorphingLayout.SetImmediate(false);
    }

    [Fact]
    public void ApplyStartTime_SetsStartTime_AndPropagates()
    {
        var layout = new MorphingLayout();
        var animatedLayout1 = new StackedRotatingLayout();
        var animatedLayout2 = new WaveLayout();
        layout.Add(animatedLayout1);
        layout.Add(animatedLayout2);
        layout.ApplyLayout([]);
        var now = DateTime.Now;
        layout.ApplyStartTime(now);

        Assert.Equal(now, animatedLayout1.StartTime);
        Assert.Equal(now, animatedLayout2.StartTime);
    }

    [Fact]
    public void SetDimensions_PropagatesToAllLayouts()
    {
        var layout = new MorphingLayout();
        var animatedLayout1 = new StackedRotatingLayout();
        var animatedLayout2 = new WaveLayout();
        layout.Add(animatedLayout1);
        layout.Add(animatedLayout2);
        layout.SetDimensions(100, 200);

        Assert.Equal(100, animatedLayout1.Width);
        Assert.Equal(200, animatedLayout1.Height);
        Assert.Equal(100, animatedLayout2.Width);
        Assert.Equal(200, animatedLayout2.Height);
    }

    [Fact]
    public void Dispose_DisposesAllLayouts_AndRemovesFromParent()
    {
        var layout = new MorphingLayout();
        var animatedLayout1 = new StackedRotatingLayout();
        var animatedLayout2 = new WaveLayout();
        layout.Add(animatedLayout1);
        layout.Add(animatedLayout2);
        layout.Dispose();

        var field = typeof(MorphingLayout)
           .GetField("_layouts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var layouts = field.GetValue(layout) as List<AnimatedLayoutBase>;
        Assert.Empty(layouts);
    }
}
