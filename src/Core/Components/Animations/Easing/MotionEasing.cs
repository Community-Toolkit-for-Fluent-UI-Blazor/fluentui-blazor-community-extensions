namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static methods for calculating eased values using various mathematical easing functions and modes. Use this
/// class to generate interpolated values for animations or transitions based on different easing behaviors.
/// </summary>
/// <remarks>Easing functions are commonly used in UI animation and graphics to create smooth, natural
/// transitions. The class supports multiple easing types, such as linear, back, bounce, circular, cubic, elastic,
/// exponential, quadratic, quartic, and quintic, each with configurable modes (In, Out, InOut). All methods operate on
/// normalized time values and return results suitable for use in animation progress calculations.</remarks>
public static class MotionEasing
{
    /// <summary>
    /// Calculates the eased value for a given time parameter using the specified easing function and mode.
    /// </summary>
    /// <remarks>The time parameter is automatically clamped to the range [0, 1]. Use this method to obtain
    /// interpolated values for animations or transitions based on different easing behaviors.</remarks>
    /// <param name="function">The easing function to apply. Determines the mathematical formula used for interpolation.</param>
    /// <param name="mode">The easing mode to use. Specifies how the easing function is applied, such as in, out, or in-out.</param>
    /// <param name="t">The normalized time parameter, ranging from 0.0 to 1.0, representing the progress of the animation.</param>
    /// <returns>A double value representing the result of the easing calculation for the specified time parameter.</returns>
    public static double Evaluate(EasingFunction function, EasingMode mode, double t)
    {
        t = Clamp01(t);

        return function switch
        {
            EasingFunction.Linear => LinearEasing.Ease(t),
            EasingFunction.Back => Back(t, mode),
            EasingFunction.Bounce => Bounce(t, mode),
            EasingFunction.Circular => Circular(t, mode),
            EasingFunction.Cubic => Cubic(t, mode),
            EasingFunction.Elastic => Elastic(t, mode),
            EasingFunction.Exponential => Exponential(t, mode),
            EasingFunction.Quadratic => Quadratic(t, mode),
            EasingFunction.Quartic => Quartic(t, mode),
            EasingFunction.Quintic => Quintic(t, mode),
            _ => t
        };
    }

    /// <summary>
    /// Calculates a 'Back' easing value for the specified normalized time and easing mode.
    /// </summary>
    /// <remarks>The 'Back' easing function creates an animation effect where the value slightly overshoots
    /// its target before settling. Use this method to apply 'Back' easing to animations for a more dynamic
    /// transition.</remarks>
    /// <param name="t">The normalized time value, typically in the range [0, 1], representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and style of the easing calculation. Must be a valid value of the
    /// EasingMode enumeration.</param>
    /// <returns>A double representing the eased value at the specified time and mode. The result is typically between 0 and 1,
    /// depending on the easing mode and input.</returns>
    private static double Back(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => BackEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => BackEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => BackEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates a bounce easing value for the specified normalized time and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply a bounce effect to animations, such as UI transitions, by
    /// transforming linear progress into a non-linear, bouncing motion. The result depends on the selected easing mode:
    /// In, Out, or InOut.</remarks>
    /// <param name="t">The normalized time value, typically between 0 and 1, representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and style of the bounce effect. Must be a valid value of the
    /// EasingMode enumeration.</param>
    /// <returns>A double representing the eased value at the specified time and mode. The value is typically between 0 and 1.</returns>
    private static double Bounce(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => BounceEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => BounceEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => BounceEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates the circular easing value for a given normalized time and easing mode.
    /// </summary>
    /// <remarks>Circular easing produces a transition that accelerates or decelerates following a circular
    /// function, creating a smooth and natural animation effect. Use the In mode for acceleration, Out for
    /// deceleration, and InOut for a combination of both.</remarks>
    /// <param name="t">The normalized time value, typically in the range [0, 1], representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and style of the easing calculation. Supported values are In, Out,
    /// and InOut.</param>
    /// <returns>A double representing the eased value based on the specified time and mode. The result is typically in the range
    /// [0, 1].</returns>
    private static double Circular(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => CircularEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => CircularEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => CircularEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates a cubic easing value for the specified progress and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply cubic easing to animations or transitions. The result varies based
    /// on the selected easing mode: 'In' accelerates from zero, 'Out' decelerates to zero, and 'InOut' combines both
    /// effects.</remarks>
    /// <param name="t">The progress value to evaluate, typically in the range [0, 1].</param>
    /// <param name="mode">The easing mode that determines the direction and style of the cubic easing calculation.</param>
    /// <returns>A double representing the eased value corresponding to the input progress and mode.</returns>
    private static double Cubic(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => CubicEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => CubicEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => CubicEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates an elastic easing value for the specified normalized time and easing mode.
    /// </summary>
    /// <remarks>Elastic easing produces an effect where the animation overshoots its target and oscillates
    /// before settling. Use this method to create dynamic, spring-like transitions in animations.</remarks>
    /// <param name="t">The normalized time value, typically between 0 and 1, representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and behavior of the elastic easing function.</param>
    /// <returns>A double representing the eased value at the specified time and mode.</returns>
    private static double Elastic(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => ElasticEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => ElasticEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => ElasticEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates an exponential easing value for the specified normalized time and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply exponential easing to animations or transitions. The result varies
    /// based on the selected easing mode: In accelerates from zero, Out decelerates to zero, and InOut combines both
    /// effects.</remarks>
    /// <param name="t">The normalized time value, typically in the range [0, 1], representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and style of the exponential easing. Supported values are In, Out,
    /// and InOut.</param>
    /// <returns>A double representing the eased value corresponding to the input time and mode.</returns>
    private static double Exponential(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => ExponentialEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => ExponentialEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => ExponentialEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates the quadratic easing value for the specified progress and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply quadratic easing to animations or transitions. The result depends on
    /// the selected easing mode: In, Out, or InOut. If an unsupported mode is provided, the method returns the input
    /// value unchanged.</remarks>
    /// <param name="t">The progress value, typically in the range [0, 1], representing the normalized time or position within the
    /// animation.</param>
    /// <param name="mode">The easing mode that determines the direction and behavior of the quadratic easing calculation.</param>
    /// <returns>A double representing the eased value based on the quadratic function and the specified mode.</returns>
    private static double Quadratic(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => QuadraticEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => QuadraticEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => QuadraticEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates a quartic easing value for the specified normalized time and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply quartic easing to animations or transitions. The result depends on
    /// the selected easing mode: In, Out, or InOut. If an unsupported mode is provided, the method returns the input
    /// value unchanged.</remarks>
    /// <param name="t">The normalized time value, typically in the range [0, 1], representing the progress of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and behavior of the quartic easing calculation.</param>
    /// <returns>A double value representing the eased progress based on the quartic function and the specified mode.</returns>
    private static double Quartic(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => QuarticEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => QuarticEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => QuarticEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Calculates a quintic easing value for the specified progress and easing mode.
    /// </summary>
    /// <remarks>Use this method to apply smooth quintic transitions to animations or interpolations. The
    /// result depends on the selected easing mode: In, Out, or InOut.</remarks>
    /// <param name="t">The progress value, typically between 0 and 1, representing the normalized time of the animation.</param>
    /// <param name="mode">The easing mode that determines the direction and style of the quintic easing calculation.</param>
    /// <returns>A double representing the eased value based on the quintic function and the specified mode.</returns>
    private static double Quintic(double t, EasingMode mode) => mode switch
    {
        EasingMode.In => QuinticEasing.EaseIn(t, 0, 1, 1),
        EasingMode.Out => QuinticEasing.EaseOut(t, 0, 1, 1),
        EasingMode.InOut => QuinticEasing.EaseInOut(t, 0, 1, 1),
        _ => t
    };

    /// <summary>
    /// Constrains a double-precision value to the range between 0.0 and 1.0, inclusive.
    /// </summary>
    /// <param name="t">The value to be clamped. Values less than 0.0 are returned as 0.0; values greater than 1.0 are returned as 1.0.</param>
    /// <returns>A double value between 0.0 and 1.0, inclusive, representing the clamped result.</returns>
    private static double Clamp01(double t)
    {
        return Math.Clamp(t, 0.0, 1.0);
    }
}
