namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of animation states, including start and end values, duration, easing functions, and interpolation
/// logic.
/// </summary>
/// <remarks>This class is designed to facilitate animations by defining the start and end values, the duration of
/// the animation,  and the easing function and mode used to interpolate between the values. The <see
/// cref="Interpolate"/> method calculates  the interpolated value at a specific point in time based on these
/// parameters.</remarks>
/// <typeparam name="T">The type of the values being interpolated. This type must support the interpolation strategy defined by the provided
/// interpolator.</typeparam>
public sealed class AnimationState<T>
{
    /// <summary>
    /// Gets or sets the initial value of the operation or process.
    /// </summary>
    public T StartValue { get; set; } = default!;

    /// <summary>
    /// Gets or sets the end value of the range or operation.
    /// </summary>
    public T EndValue { get; set; } = default!;

    /// <summary>
    /// Gets or sets the duration of the operation.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets the start time of the event.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the easing function used to interpolate values during animations.
    /// </summary>
    public EasingFunction EasingFunction { get; set; } = EasingFunction.Linear;

    /// <summary>
    /// Gets or sets the easing mode that determines the interpolation behavior of the animation.
    /// </summary>
    /// <remarks>The easing mode specifies how the animation interpolates between its start and end values. 
    /// Common modes include <see cref="EasingMode.In"/>, <see cref="EasingMode.Out"/>, and <see
    /// cref="EasingMode.InOut"/>.</remarks>
    public EasingMode EasingMode { get; set; } = EasingMode.InOut;

    /// <summary>
    /// Calculates the interpolated value at a specified point in time based on the elapsed duration, easing function,
    /// and interpolation strategy.
    /// </summary>
    /// <remarks>The interpolation progress is determined by the elapsed time since the start time, relative
    /// to the  total duration. The progress is then adjusted using the specified easing function and mode before  being
    /// passed to the interpolator.</remarks>
    /// <param name="now">The current time used to calculate the interpolation progress.</param>
    /// <param name="interpolator">An object that defines the interpolation strategy for transitioning  between the start and end values.</param>
    /// <returns>The interpolated value of type <typeparamref name="T"/> at the specified time.</returns>
    public T Interpolate(DateTime now, IInterpolator<T> interpolator)
    {
        var elapsed = (now - StartTime).TotalMilliseconds;
        var t = Math.Clamp(elapsed / Duration.TotalMilliseconds, 0, 1);
        var eased = ApplyEasing(EasingFunction, EasingMode, t);

        return interpolator.Lerp(StartValue, EndValue, eased);
    }

    /// <summary>
    /// Applies the specified easing function and mode to calculate the eased value for the given time.
    /// </summary>
    /// <param name="easingFunction">The easing function to use, such as <see cref="EasingFunction.Linear"/> or <see cref="EasingFunction.Bounce"/>.</param>
    /// <param name="easingMode">The easing mode to apply, such as <see cref="EasingMode.In"/>, <see cref="EasingMode.Out"/>, or <see
    /// cref="EasingMode.InOut"/>.</param>
    /// <param name="time">The normalized time value, typically between 0.0 and 1.0, representing the progress of the animation.</param>
    /// <returns>The eased value corresponding to the specified time, calculated using the provided easing function and mode.</returns>
    /// <exception cref="NotSupportedException">Thrown if the specified combination of <paramref name="easingFunction"/> and <paramref name="easingMode"/> is
    /// not supported.</exception>
    private static double ApplyEasing(
        EasingFunction easingFunction,
        EasingMode easingMode,
        double time)
    {
        return (easingFunction, easingMode) switch
        {
            (EasingFunction.Linear, _) => LinearEasing.Ease(time),
            (EasingFunction.Back, EasingMode.In) => BackEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Back, EasingMode.Out) => BackEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Back, EasingMode.InOut) => BackEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Bounce, EasingMode.In) => BounceEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Bounce, EasingMode.Out) => BounceEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Bounce, EasingMode.InOut) => BounceEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Circular, EasingMode.In) => CircularEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Circular, EasingMode.Out) => CircularEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Circular, EasingMode.InOut) => CircularEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Cubic, EasingMode.In) => CubicEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Cubic, EasingMode.Out) => CubicEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Cubic, EasingMode.InOut) => CubicEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Elastic, EasingMode.In) => ElasticEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Elastic, EasingMode.Out) => ElasticEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Elastic, EasingMode.InOut) => ElasticEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Exponential, EasingMode.In) => ExponentialEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Exponential, EasingMode.Out) => ExponentialEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Exponential, EasingMode.InOut) => ExponentialEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Quadratic, EasingMode.In) => QuadraticEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Quadratic, EasingMode.Out) => QuadraticEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Quadratic, EasingMode.InOut) => QuadraticEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Quartic, EasingMode.In) => QuarticEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Quartic, EasingMode.Out) => QuarticEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Quartic, EasingMode.InOut) => QuarticEasing.EaseInOut(time, 0, 1, 1),
            (EasingFunction.Quintic, EasingMode.In) => QuinticEasing.EaseIn(time, 0, 1, 1),
            (EasingFunction.Quintic, EasingMode.Out) => QuinticEasing.EaseOut(time, 0, 1, 1),
            (EasingFunction.Quintic, EasingMode.InOut) => QuinticEasing.EaseInOut(time, 0, 1, 1),
            _ => throw new NotSupportedException($"Easing function '{easingFunction}' with mode '{easingMode}' is not supported."),
        };
    }

    /// <summary>
    /// Applies the specified animation parameters to the current instance, updating its state accordingly.
    /// </summary>
    /// <param name="startValue">The starting value of the animation. Defaults to the type's default value if not specified.</param>
    /// <param name="endValue">The ending value of the animation. Defaults to the type's default value if not specified.</param>
    /// <param name="startTime">The start time of the animation.</param>
    /// <param name="duration">The duration of the animation.</param>
    internal void Apply(T startValue = default!,
                        T endValue = default!,
                        DateTime startTime = default,
                        TimeSpan duration = default)
    {
        StartValue = startValue;
        EndValue = endValue;
        Duration = duration;
        StartTime = startTime;
    }
}
