namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration for a motion transition, including duration, delay, and easing parameters.
/// </summary>
/// <remarks>Use this class to define the timing and easing characteristics of an animation or transition. The
/// properties allow customization of how the motion behaves, such as how long it takes, when it starts, and the
/// interpolation function used. This class is typically used to standardize motion effects across UI
/// components.</remarks>
public sealed class MotionTransition
{
    /// <summary>
    /// Gets or sets the duration of the operation or animation.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// Gets or sets the amount of time to wait before the operation is executed.
    /// </summary>
    public TimeSpan? Delay { get; set; }

    /// <summary>
    /// Gets or sets the easing function used to interpolate values during animations.
    /// </summary>
    /// <remarks>The selected easing function determines the rate of change for the animation, affecting how
    /// the animated value accelerates or decelerates over time. Choose an appropriate function to achieve the desired
    /// animation effect.</remarks>
    public EasingFunction Function { get; set; } = EasingFunction.Cubic;

    /// <summary>
    /// Gets or sets the easing mode that determines how the interpolation is applied to the animation.
    /// </summary>
    /// <remarks>The easing mode specifies whether the easing function is applied at the start, end, or both
    /// ends of the animation. Changing this property affects the direction and behavior of the easing
    /// calculation.</remarks>
    public EasingMode Mode { get; set; } = EasingMode.Out;

    /// <summary>
    /// Creates a new instance of the MotionCurve class using the current duration, function, and mode values.
    /// </summary>
    /// <returns>A MotionCurve object initialized with the current duration, function, and mode.</returns>
    public MotionCurve ToCurve() => new(Duration, Function, Mode);
}
