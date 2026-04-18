using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Provides animation logic for SVG chart elements by applying animation effects based on chart animation triggers and
/// strategies.
/// </summary>
/// <remarks>This static class centralizes the application of chart-related animations, mapping animation triggers
/// and effects to their corresponding actions. It is intended for internal use within the charting components to ensure
/// consistent animation behavior across different chart elements. The class supports a variety of animation scenarios,
/// such as initial appearance, hover, selection, press, enable, and disable states, and applies the appropriate SVG
/// animation based on the provided payload and chart strategies.</remarks>
/// <typeparam name="TBuilder">The type of the builder used to construct or modify SVG elements that support animation.</typeparam>
internal static class ChartAnimationEngine<TBuilder>
{
    /// <summary>
    /// Provides a mapping between chart animation triggers and their corresponding actions to apply animations to SVG
    /// elements.
    /// </summary>
    /// <remarks>Each entry associates a specific ChartAnimationTrigger with an action that applies the
    /// appropriate animation logic to an animatable SVG element using the provided animation payload. This dictionary
    /// enables efficient dispatch of animation behaviors based on user interaction or chart state changes.</remarks>
    private static readonly Dictionary<ChartAnimationTrigger, Action<ISvgAnimatable<TBuilder>, IAnimatedPayload>> TriggerActions =
        new(EqualityComparer<ChartAnimationTrigger>.Default)
        {
            [ChartAnimationTrigger.InitialAppear] = ApplyInitialAppear,
            [ChartAnimationTrigger.HoverIn] = ApplyHoverIn,
            [ChartAnimationTrigger.HoverOut] = ApplyHoverOut,
            [ChartAnimationTrigger.PressIn] = ApplyPressIn,
            [ChartAnimationTrigger.PressOut] = ApplyPressOut,
            [ChartAnimationTrigger.SelectIn] = ApplySelectIn,
            [ChartAnimationTrigger.SelectOut] = ApplySelectOut,
            [ChartAnimationTrigger.Enable] = ApplyEnable,
            [ChartAnimationTrigger.Disable] = ApplyDisable,
            [ChartAnimationTrigger.Remove] = ApplyRemove,
            [ChartAnimationTrigger.UpdateFromPrevious] = ApplyUpdate
        };

    /// <summary>
    /// Provides a mapping between chart animation effects and their corresponding actions to apply those effects to
    /// animatable SVG elements.
    /// </summary>
    /// <remarks>This dictionary enables efficient lookup and execution of animation logic based on the
    /// specified chart animation effect. Each entry associates a ChartAnimationEffect value with an action that applies
    /// the effect to an ISvgAnimatable&lt;Builder&gt; instance using the provided animation payload.</remarks>
    private static readonly Dictionary<ChartAnimationEffect, Action<ISvgAnimatable<TBuilder>, IAnimatedPayload>> EffectActions =
        new(EqualityComparer<ChartAnimationEffect>.Default)
        {
            [ChartAnimationEffect.Fade] = ApplyFade,
            [ChartAnimationEffect.Scale] = ApplyScale,
            [ChartAnimationEffect.Slide] = ApplySlide,
            [ChartAnimationEffect.Sweep] = ApplySweep,
            [ChartAnimationEffect.Bounce] = ApplyBounce
        };

    /// <summary>
    /// Applies the appropriate animation to the given SVG element based on the provided animation payload.
    /// </summary>
    /// <param name="element">The SVG element to which the animation should be applied.</param>
    /// <param name="payload">The payload containing animation details such as the trigger, effect, duration, and delay.</param>
    /// <param name="strategies">The chart strategies containing animation settings for different triggers. Cannot be null.</param>
    public static void Apply(
        ISvgAnimatable<TBuilder> element,
        IAnimatedPayload payload,
        Themes.ChartAnimationStrategies strategies)
    {
        if (!strategies.AnimationsEnabled)
        {
            return;
        }

        var strategy = payload.Trigger switch
        {
            ChartAnimationTrigger.InitialAppear => strategies.InitialAppear,
            ChartAnimationTrigger.UpdateFromPrevious => strategies.Update,
            ChartAnimationTrigger.Remove => strategies.Remove,
            ChartAnimationTrigger.HoverIn => strategies.HoverIn,
            ChartAnimationTrigger.HoverOut => strategies.HoverOut,
            ChartAnimationTrigger.SelectIn => strategies.SelectIn,
            ChartAnimationTrigger.SelectOut => strategies.SelectOut,
            ChartAnimationTrigger.PressIn => strategies.PressIn,
            ChartAnimationTrigger.PressOut => strategies.PressOut,
            ChartAnimationTrigger.Enable => strategies.Enable,
            ChartAnimationTrigger.Disable => strategies.Disable,
            _ => null
        };

        if (strategy is null)
        {
            return;
        }

        payload.AnimationEnabled = true;
        payload.Animation = strategy.Animation;
        payload.Effect = strategy.Effect;
        payload.Trigger = strategy.Trigger;

        Apply(element, payload);
    }

    /// <summary>
    /// Applies an animation to the specified SVG animatable element using the provided animation payload if animation
    /// is enabled and a valid trigger is set.
    /// </summary>
    /// <remarks>No animation is applied if animation is disabled, the animation payload is null, or the
    /// trigger is set to None.</remarks>
    /// <param name="element">The SVG animatable element to which the animation will be applied.</param>
    /// <param name="payload">The animation payload containing animation settings, trigger, and enablement state. Animation is only applied if
    /// animation is enabled, a valid animation is present, and the trigger is not set to None.</param>
    private static void Apply(ISvgAnimatable<TBuilder> element, IAnimatedPayload payload)
    {
        if (!payload.AnimationEnabled ||
            payload.Animation is null ||
            payload.Trigger == ChartAnimationTrigger.None)
        {
            return;
        }

        if (TriggerActions.TryGetValue(payload.Trigger, out var action))
        {
            action(element, payload);
        }
    }

    /// <summary>
    /// Applies the initial appear animation effect to the given SVG element based on the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG element to which the animation should be applied.</param>
    /// <param name="payload">The payload containing animation details such as the trigger, effect, duration, and delay.</param>
    private static void ApplyInitialAppear(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        if (payload.Animation is null)
        {
            return;
        }

        if (EffectActions.TryGetValue(payload.Effect, out var effect))
        {
            effect(el, payload);
        }
    }

    /// <summary>
    /// Applies hover-in animation effects to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <remarks>If the payload represents a multi-donut slice in a pie chart, only the opacity animation is
    /// applied and the scale animation is skipped.</remarks>
    /// <param name="el">The SVG animatable element to which the hover-in animation will be applied.</param>
    /// <param name="payload">The animation payload containing animation parameters such as duration and delay.</param>
    private static void ApplyHoverIn(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("opacity", a => a.From(1)
                                       .To(0.7)
                                       .Duration(anim.Duration)
                                       .Begin(anim.Delay)
                                       .Fill());

        if (payload is PiePayload pp && pp.IsMultiDonutSlice)
        {
            return;
        }

        el.AddAnimateTransform("scale", a => a.From(1)
                                              .To(1.05)
                                              .Duration(anim.Duration)
                                              .Begin(anim.Delay)
                                              .WithAttribute("additive", "sum")
                                              .Fill());
    }

    /// <summary>
    /// Applies the hover-out animation to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the hover-out animation will be applied.</param>
    /// <param name="payload">The animation payload containing animation parameters such as duration and delay.</param>
    private static void ApplyHoverOut(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("opacity", a => a.From(0.7).To(1).Duration(anim.Duration).Begin(anim.Delay).Fill());

        if (payload is PiePayload pp && pp.IsMultiDonutSlice)
        {
            return;
        }

        el.AddAnimateTransform("scale", a => a.From(1.05)
                                         .To(1)
                                         .Duration(anim.Duration)
                                         .Begin(anim.Delay)
                                         .WithAttribute("additive", "sum")
                                         .Fill());
    }

    /// <summary>
    /// Applies a press-in animation effect to the specified SVG animatable element using the provided animation
    /// payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the press-in animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters to use for the press-in effect.</param>
    private static void ApplyPressIn(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").From("scale(1)").To("scale(0.95)").Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a press-out animation effect to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the press-out animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters to use for the press-out effect.</param>
    private static void ApplyPressOut(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").From("scale(0.95)").To("scale(1.05)").Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies selection animation effects to the specified SVG animatable element using the provided animation
    /// payload.
    /// </summary>
    /// <remarks>This method adds a stroke width and a scaling transform animation to visually indicate
    /// selection. The animation parameters are derived from the provided payload.</remarks>
    /// <param name="el">The SVG animatable element to which the selection animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplySelectIn(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("stroke-width", a => a.From(2).To(4).Duration(anim.Duration).Begin(anim.Delay).Fill());
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").Values("scale(1); scale(1.1); scale(1)").KeyTimes(0, 0.5, 1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a 'select out' animation to the specified SVG animatable element, modifying its stroke width over time.
    /// </summary>
    /// <remarks>This method configures the element to animate its stroke width from 4 to 2 using the timing
    /// specified in the animation payload.</remarks>
    /// <param name="el">The SVG animatable element to which the animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, including duration and delay.</param>
    private static void ApplySelectOut(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("stroke-width", a => a.From(4).To(2).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a fade-in animation to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the fade-in animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplyFade(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.WithOpacity(0);
        el.AddAnimate("opacity", a => a.From(0).To(1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a scaling animation to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the scaling animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplyScale(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").Values("scale(0); scale(1)").KeyTimes(0, 1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a slide-in animation to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the slide animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplySlide(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").Values("translate(0,10); translate(0,0)").KeyTimes(0, 1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a sweep animation to the specified SVG animatable element using the provided animation payload. This effect
    /// animates the stroke dash offset to create a sweeping motion along the path.
    /// </summary>
    /// <param name="el">The SVG animatable element to which the sweep animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplySweep(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.WithAttribute("pathLength", "1");
        el.AddAnimate("stroke-dashoffset", a => a.From(1).To(0).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a bounce animation to the specified SVG animatable element using the provided animation payload.
    /// </summary>
    /// <remarks>The bounce animation scales the element from zero to slightly larger than its original size,
    /// then returns it to its original scale. The animation parameters are determined by the values in the provided
    /// payload.</remarks>
    /// <param name="el">The SVG animatable element to which the bounce animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplyBounce(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;
        el.AddAnimate("transform", a => a.WithAttribute("additive", "sum").Values("scale(0); scale(1.1); scale(1)").KeyTimes(0, 0.7, 1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies an opacity animation to the specified animatable SVG element using the provided animation payload.
    /// </summary>
    /// <param name="el">The SVG element to which the opacity animation will be applied. Must implement the ISvgAnimatable interface.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay. Must not be null and must
    /// have a valid Animation property.</param>
    private static void ApplyEnable(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;

        el.AddAnimate("opacity", a => a.From(0.4).To(1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a disabled visual state to the specified SVG animatable element by animating its opacity.
    /// </summary>
    /// <remarks>This method reduces the element's opacity to visually indicate a disabled state. The
    /// animation parameters are determined by the provided payload.</remarks>
    /// <param name="el">The SVG animatable element to which the disabled state animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters such as duration and delay.</param>
    private static void ApplyDisable(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;

        el.AddAnimate("opacity", a => a.From(1).To(0.4).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies an opacity animation update to the specified animatable SVG element using the provided animation
    /// payload.
    /// </summary>
    /// <param name="el">The SVG element that supports animation and will receive the opacity animation.</param>
    /// <param name="payload">The animation payload containing the animation parameters to apply.</param>
    private static void ApplyUpdate(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;

        el.AddAnimate("opacity", a => a.From(0.5).To(1).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }

    /// <summary>
    /// Applies a removal animation to the specified SVG animatable element by animating its opacity from fully visible
    /// to fully transparent.
    /// </summary>
    /// <remarks>This method configures the element to fade out by animating its opacity from 1 to 0, using
    /// the duration and delay specified in the animation payload.</remarks>
    /// <param name="el">The SVG animatable element to which the removal animation will be applied.</param>
    /// <param name="payload">The animation payload containing the animation parameters, such as duration and delay.</param>
    private static void ApplyRemove(ISvgAnimatable<TBuilder> el, IAnimatedPayload payload)
    {
        var anim = payload.Animation!;

        el.AddAnimate("opacity", a => a.From(1).To(0).Duration(anim.Duration).Begin(anim.Delay).Fill());
    }
}
