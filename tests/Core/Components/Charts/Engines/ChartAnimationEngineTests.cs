using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartAnimationEngineTests
{
    private sealed class TestAnimatable : ISvgAnimatable<TestAnimatable>
    {
        public int AnimateCount { get; private set; }
        public int AnimateTransformCount { get; private set; }

        public TestAnimatable AddAnimate(string attribute, Action<SvgAnimateBuilder> configure)
        {
            AnimateCount++;
            configure(new SvgAnimateBuilder(new SvgBuilder(), new SvgAnimate()));
            return this;
        }

        public TestAnimatable WithOpacity(double value) => this;

        public TestAnimatable WithAttribute(string name, string value) => this;

        public TestAnimatable AddAnimateTransform(string type, Action<SvgAnimateTransformBuilder> configure)
        {
            AnimateTransformCount++;
            configure(new SvgAnimateTransformBuilder(new SvgBuilder(), new SvgAnimateTransform()));
            return this;
        }

        public TestAnimatable AddAnimateMotion(Action<SvgAnimateMotionBuilder> configure)
        {
            configure(new SvgAnimateMotionBuilder(new SvgBuilder(), new SvgAnimateMotion()));
            return this;
        }

        public TestAnimatable WithTransform(string transform) => this;

        public TestAnimatable Close() => this;
    }

    [Fact]
    public void ChartAnimationEngine_Apply_IgnoresWhenDisabled()
    {
        var animatable = new TestAnimatable();
        var payload = new PiePayload
        {
            Id = "id",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            Value = 1,
            LabelPosition = new ChartPoint(0, 0),
            StartAngle = 0,
            EndAngle = 90,
            MidAngle = 45,
            IsMultiDonutSlice = false,
            Trigger = ChartAnimationTrigger.InitialAppear,
            AnimationEnabled = true,
            Animation = new ChartItemAnimation()
        };

        var strategies = new ChartAnimationStrategies { AnimationsEnabled = false };

        ChartAnimationEngine<TestAnimatable>.Apply(animatable, payload, strategies);

        Assert.Equal(0, animatable.AnimateCount);
    }

    [Fact]
    public void ChartAnimationEngine_Apply_UsesStrategy()
    {
        var animatable = new TestAnimatable();
        var payload = new PiePayload
        {
            Id = "id",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            Value = 1,
            LabelPosition = new ChartPoint(0, 0),
            StartAngle = 0,
            EndAngle = 90,
            MidAngle = 45,
            IsMultiDonutSlice = false,
            Trigger = ChartAnimationTrigger.InitialAppear,
            AnimationEnabled = true,
            Animation = new ChartItemAnimation()
        };

        var strategies = new ChartAnimationStrategies { InitialAppear = ChartAnimationStrategy.DefaultInitial };

        ChartAnimationEngine<TestAnimatable>.Apply(animatable, payload, strategies);

        Assert.True(animatable.AnimateCount > 0 || animatable.AnimateTransformCount > 0);
    }
}
