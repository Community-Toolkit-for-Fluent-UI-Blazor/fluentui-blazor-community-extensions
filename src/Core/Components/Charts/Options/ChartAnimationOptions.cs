using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the configuration options for chart animations, including mode, duration, delay, easing, and optional
/// custom parameters.
/// </summary>
/// <remarks>Use this class to specify how chart animations should behave when rendering or updating chart
/// elements. The options allow customization of animation timing, easing behavior, and advanced parameters such as
/// custom cubic-bezier curves or step-based easing. Some properties are only relevant depending on the selected easing
/// mode.</remarks>
public sealed class ChartAnimationOptions
{
    /// <summary>
    /// Gets the animation effect to apply to the chart elements during rendering.
    /// </summary>
    public ChartAnimationEffect Effect { get; init; } = ChartAnimationEffect.Fade;

    /// <summary>
    /// Gets the duration of the operation or animation.
    /// </summary>
    public TimeSpan Duration { get; init; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Gets the delay interval before the associated operation is executed.
    /// </summary>
    public TimeSpan Delay { get; init; } = TimeSpan.Zero;

    /// <summary>
    /// Gets the easing function used to interpolate values during the SVG animation.
    /// </summary>
    /// <remarks>The easing function determines the rate of change of the animation over time. Different
    /// easing options can be used to create various animation effects, such as accelerating, decelerating, or
    /// both.</remarks>
    public SvgEasing Easing { get; init; } = SvgEasing.EaseInOut;

    /// <summary>
    /// Optional custom cubic-bezier curve (x1,y1,x2,y2).
    /// Used only when Easing = CustomCubicBezier.
    /// </summary>
    public (double x1, double y1, double x2, double y2)? CustomBezier { get; init; }

    /// <summary>
    /// Optional steps configuration.
    /// Used only when Easing = Steps.
    /// </summary>
    public (int count, bool start)? Steps { get; init; }
}
