using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Components.Charts.Themes;

/// <summary>
/// Represents a strategy for animating chart elements based on specific triggers and effects.
/// </summary>
public sealed record ChartAnimationStrategy
{
    /// <summary>
    /// Gets the event that triggers the chart animation.
    /// </summary>
    /// <remarks>Use this property to specify when the chart animation should start, such as on load or on
    /// user interaction. The available trigger options are defined by the ChartAnimationTrigger enumeration.</remarks>
    public ChartAnimationTrigger Trigger { get; init; }

    /// <summary>
    /// Gets the animation effect to apply to the chart elements.
    /// </summary>
    /// <remarks>Use this property to specify how chart elements are animated when they are rendered or
    /// updated. The available effects are defined by the ChartAnimationEffect enumeration.</remarks>
    public ChartAnimationEffect Effect { get; init; }

    /// <summary>
    /// Gets the animation settings applied to the chart item.
    /// </summary>
    /// <remarks>Use this property to configure how the chart item animates when it is rendered or updated.
    /// The animation settings determine the visual transition effects for the chart item.</remarks>
    public ChartItemAnimation Animation { get; init; } = new();

    /// <summary>
    /// Gets a chart animation strategy that disables all animations.
    /// </summary>
    /// <remarks>Use this property to specify that no animation should be applied to chart elements. This is
    /// useful when animations are not desired or need to be explicitly turned off.</remarks>
    public static ChartAnimationStrategy None { get; } = new()
    {
        Trigger = ChartAnimationTrigger.None,
        Effect = ChartAnimationEffect.None,
        Animation = new ChartItemAnimation()
    };

    /// <summary>
    /// Gets the default animation strategy used for the initial appearance of chart elements.
    /// </summary>
    /// <remarks>The default strategy applies a fade effect with a 400-millisecond duration and an ease-out
    /// easing function when chart elements first appear. This property provides a convenient preset for consistent
    /// initial chart animations.</remarks>
    public static ChartAnimationStrategy DefaultInitial { get; } = new()
    {
        Trigger = ChartAnimationTrigger.InitialAppear,
        Effect = ChartAnimationEffect.Fade,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(500),
            Easing = SvgEasing.EaseOut
        }
    };

    /// <summary>
    /// Gets the default animation strategy used for updating chart elements from their previous state.
    /// </summary>
    /// <remarks>This strategy applies a sweep effect with a duration of 350 milliseconds and uses an
    /// ease-in-out easing function when chart data is updated. It is intended to provide a visually smooth transition
    /// for chart updates.</remarks>
    public static ChartAnimationStrategy DefaultUpdate { get; } = new()
    {
        Trigger = ChartAnimationTrigger.UpdateFromPrevious,
        Effect = ChartAnimationEffect.Sweep,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(350),
            Easing = SvgEasing.EaseInOut
        }
    };

    /// <summary>
    /// Gets the default animation strategy applied when a chart item is hovered in.
    /// </summary>
    /// <remarks>This strategy uses a scaling effect with a duration of 120 milliseconds and an ease-out
    /// easing function. It is intended to provide a consistent and visually appealing hover-in animation for chart
    /// components.</remarks>
    public static ChartAnimationStrategy DefaultHoverIn { get; } = new()
    {
        Trigger = ChartAnimationTrigger.HoverIn,
        Effect = ChartAnimationEffect.Scale,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(120),
            Easing = SvgEasing.EaseOut
        }
    };

    /// <summary>
    /// Gets the default animation strategy applied when a hover-out event occurs on a chart element.
    /// </summary>
    /// <remarks>This strategy uses a scaling effect with a short duration and an ease-in easing function. It
    /// is intended to provide a consistent and visually smooth transition when the user's pointer leaves a chart
    /// item.</remarks>
    public static ChartAnimationStrategy DefaultHoverOut { get; } = new()
    {
        Trigger = ChartAnimationTrigger.HoverOut,
        Effect = ChartAnimationEffect.Scale,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(120),
            Easing = SvgEasing.EaseIn
        }
    };

    /// <summary>
    /// Gets the default animation strategy used when selecting items in a chart.
    /// </summary>
    /// <remarks>This strategy applies a bounce effect with a duration of 200 milliseconds and an ease-out
    /// bounce easing function when an item is selected. Use this property to provide a consistent selection animation
    /// across chart components.</remarks>
    public static ChartAnimationStrategy DefaultSelectIn { get; } = new()
    {
        Trigger = ChartAnimationTrigger.SelectIn,
        Effect = ChartAnimationEffect.Bounce,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(200),
            Easing = SvgEasing.EaseOutBounce
        }
    };

    /// <summary>
    /// Gets the default animation strategy applied when a chart item is deselected.
    /// </summary>
    /// <remarks>This strategy uses a fade effect with a duration of 150 milliseconds and an ease-in easing
    /// function. It is intended to provide a consistent visual transition when items are unselected in a
    /// chart.</remarks>
    public static ChartAnimationStrategy DefaultSelectOut { get; } = new()
    {
        Trigger = ChartAnimationTrigger.SelectOut,
        Effect = ChartAnimationEffect.Fade,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(150),
            Easing = SvgEasing.EaseIn
        }
    };

    /// <summary>
    /// Gets the default animation strategy applied when a press-in event occurs on a chart element.
    /// </summary>
    public static ChartAnimationStrategy DefaultPressIn => new()
    {
        Trigger = ChartAnimationTrigger.PressIn,
        Effect = ChartAnimationEffect.Scale,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(80),
            Easing = SvgEasing.EaseOut
        }
    };

    /// <summary>
    /// Gets the default animation strategy for the press-out interaction.
    /// </summary>
    public static ChartAnimationStrategy DefaultPressOut => new()
    {
        Trigger = ChartAnimationTrigger.PressOut,
        Effect = ChartAnimationEffect.Scale,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(120),
            Easing = SvgEasing.EaseOutBack
        }
    };

    /// <summary>
    /// Gets the default chart animation strategy that applies a fade effect when animation is enabled.
    /// </summary>
    /// <remarks>The default strategy uses a fade effect with a duration of 200 milliseconds and an ease-out
    /// easing function. This can be used as a baseline for enabling chart animations with consistent behavior across
    /// components.</remarks>
    public static ChartAnimationStrategy DefaultEnable => new()
    {
        Trigger = ChartAnimationTrigger.Enable,
        Effect = ChartAnimationEffect.Fade,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(200),
            Easing = SvgEasing.EaseOut
        }
    };

    /// <summary>
    /// Gets the default animation strategy used when chart animations are disabled.
    /// </summary>
    /// <remarks>This strategy applies a fade effect with a short duration and ease-in easing when chart
    /// animations are turned off. Use this property to provide a consistent visual transition when disabling chart
    /// animations across charts.</remarks>
    public static ChartAnimationStrategy DefaultDisable => new()
    {
        Trigger = ChartAnimationTrigger.Disable,
        Effect = ChartAnimationEffect.Fade,
        Animation = new ChartItemAnimation
        {
            Duration = TimeSpan.FromMilliseconds(200),
            Easing = SvgEasing.EaseIn
        }
    };
}
