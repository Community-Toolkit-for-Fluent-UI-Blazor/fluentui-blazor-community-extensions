namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Defines all easing functions supported by SVG animations.
/// This enum is exhaustive and covers:
/// - CSS timing functions
/// - CSS cubic-bezier presets
/// - CSS steps()
/// - SMIL calcMode variants
/// - Complex easings (elastic, bounce, back)
/// - Fully custom cubic-bezier curves
/// </summary>
public enum SvgEasing
{
    /// <summary>
    /// Constant speed from start to end.
    /// </summary>
    Linear,

    /// <summary>
    /// Standard CSS "ease" curve (slow start, fast middle, slow end).
    /// </summary>
    Ease,

    /// <summary>
    /// Slow acceleration from the start.
    /// </summary>
    EaseIn,

    /// <summary>
    /// Fast start, then decelerates toward the end.
    /// </summary>
    EaseOut,

    /// <summary>
    /// Slow start, fast middle, slow end.
    /// </summary>
    EaseInOut,

    /// <summary>
    /// Very smooth acceleration using a sine curve.
    /// </summary>
    EaseInSine,

    /// <summary>
    /// Very smooth deceleration using a sine curve.
    /// </summary>
    EaseOutSine,

    /// <summary>
    /// Sine-based ease-in-out, extremely smooth.
    /// </summary>
    EaseInOutSine,

    /// <summary>
    /// Acceleration following a quadratic curve.
    /// </summary>
    EaseInQuad,

    /// <summary>
    /// Deceleration following a quadratic curve.
    /// </summary>
    EaseOutQuad,

    /// <summary>
    /// Quadratic ease-in-out.
    /// </summary>
    EaseInOutQuad,

    /// <summary>
    /// Acceleration following a cubic curve.
    /// </summary>
    EaseInCubic,

    /// <summary>
    /// Deceleration following a cubic curve.
    /// </summary>
    EaseOutCubic,

    /// <summary>
    /// Cubic ease-in-out.
    /// </summary>
    EaseInOutCubic,

    /// <summary>
    /// Strong acceleration using a quartic curve.
    /// </summary>
    EaseInQuart,

    /// <summary>
    /// Strong deceleration using a quartic curve.
    /// </summary>
    EaseOutQuart,

    /// <summary>
    /// Quartic ease-in-out.
    /// </summary>
    EaseInOutQuart,

    /// <summary>
    /// Very strong acceleration using a quintic curve.
    /// </summary>
    EaseInQuint,

    /// <summary>
    /// Very strong deceleration using a quintic curve.
    /// </summary>
    EaseOutQuint,

    /// <summary>
    /// Quintic ease-in-out.
    /// </summary>
    EaseInOutQuint,

    /// <summary>
    /// Extremely fast acceleration (exponential).
    /// </summary>
    EaseInExpo,

    /// <summary>
    /// Extremely fast deceleration (exponential).
    /// </summary>
    EaseOutExpo,

    /// <summary>
    /// Exponential ease-in-out.
    /// </summary>
    EaseInOutExpo,

    /// <summary>
    /// Acceleration following a circular curve.
    /// </summary>
    EaseInCirc,

    /// <summary>
    /// Deceleration following a circular curve.
    /// </summary>
    EaseOutCirc,

    /// <summary>
    /// Circular ease-in-out.
    /// </summary>
    EaseInOutCirc,

    /// <summary>
    /// Starts by moving slightly backward before accelerating forward.
    /// </summary>
    EaseInBack,

    /// <summary>
    /// Overshoots the target before settling.
    /// </summary>
    EaseOutBack,

    /// <summary>
    /// Backwards overshoot at start and end.
    /// </summary>
    EaseInOutBack,

    /// <summary>
    /// Elastic spring effect at the beginning.
    /// </summary>
    EaseInElastic,

    /// <summary>
    /// Elastic spring effect at the end.
    /// </summary>
    EaseOutElastic,

    /// <summary>
    /// Elastic spring effect at both start and end.
    /// </summary>
    EaseInOutElastic,

    /// <summary>
    /// Bounce effect at the beginning.
    /// </summary>
    EaseInBounce,

    /// <summary>
    /// Bounce effect at the end.
    /// </summary>
    EaseOutBounce,

    /// <summary>
    /// Bounce effect at both start and end.
    /// </summary>
    EaseInOutBounce,

    /// <summary>
    /// Equivalent to steps(1, start).
    /// </summary>
    StepStart,

    /// <summary>
    /// Equivalent to steps(1, end).
    /// </summary>
    StepEnd,

    /// <summary>
    /// Generic steps(n, position) easing. Requires parameters.
    /// </summary>
    Steps,

    /// <summary>
    /// Instant jumps between values (calcMode="discrete").
    /// </summary>
    Discrete,

    /// <summary>
    /// Evenly paced animation based on distance (calcMode="paced").
    /// </summary>
    Paced,

    /// <summary>
    /// Bezier spline interpolation (calcMode="spline"). Requires keySplines.
    /// </summary>
    Spline,

    /// <summary>
    /// Fully custom cubic-bezier curve. Requires (x1,y1,x2,y2).
    /// </summary>
    CustomCubicBezier
}
