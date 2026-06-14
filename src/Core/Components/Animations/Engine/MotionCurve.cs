namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an animation curve that defines the timing and easing behavior for an animation sequence.
/// </summary>
/// <remarks>An animation curve specifies how the progress of an animation changes over time, using a combination
/// of duration, easing function, and easing mode. Use this type to configure custom animation transitions in UI
/// components or other time-based effects. The curve is immutable after creation. Thread safety is guaranteed for
/// reading properties.</remarks>
/// <param name="Duration">The total duration of the animation. Must be a positive TimeSpan.</param>
/// <param name="Function">The easing function that defines the rate of change of the animation over time.</param>
/// <param name="Mode">The easing mode that specifies how the easing function is applied (e.g., In, Out, InOut).</param>
public sealed record MotionCurve(TimeSpan Duration, EasingFunction Function, EasingMode Mode)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="elapsed"></param>
    /// <returns></returns>
    public double Evaluate(TimeSpan elapsed)
    {
        var u = elapsed.TotalMilliseconds / Duration.TotalMilliseconds;

        u = Math.Clamp(u, 0.0, 1.0);

        return MotionEasing.Evaluate(Function, Mode, u);
    }
}
