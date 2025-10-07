namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides easing functions that implement a "back" motion, where the animation overshoots its target and then returns
/// to it. These functions are commonly used in animations to create a spring-like effect.
/// </summary>
public static class BackEasing
{
    /// <summary>
    /// Calculates the eased-in interpolation of a value using a back easing function.
    /// </summary>
    /// <param name="t">The current time or position, where 0 represents the start and <paramref name="d"/> represents the end.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <param name="s">The overshoot amount, which controls the extent of the "back" effect. Defaults to 1.70158.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d, double s = 1.70158f) =>
        c * (t /= d) * t * ((s + 1) * t - s) + b;

    /// <summary>
    /// Applies an "ease-out" interpolation to calculate a value based on the specified parameters.
    /// </summary>
    /// <remarks>This method implements a "back" easing function with an ease-out behavior, where the
    /// interpolation starts quickly and decelerates toward the end. The optional <paramref name="s"/> parameter allows
    /// customization of the overshoot effect, with higher values producing a more pronounced overshoot.</remarks>
    /// <param name="t">The current time or position, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <param name="s">An optional overshoot parameter that controls the intensity of the easing effect. Defaults to 1.70158.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d, double s = 1.70158f) =>
        c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;

    /// <summary>
    /// Applies an easing function that combines both "ease-in" and "ease-out" effects to calculate an interpolated
    /// value.
    /// </summary>
    /// <remarks>This method uses a combination of "ease-in" and "ease-out" easing, making it suitable for
    /// animations that require a smooth acceleration at the start and deceleration at the end. The optional parameter
    /// <paramref name="s"/> allows customization of the overshoot effect, where higher values result in a more
    /// pronounced easing curve.</remarks>
    /// <param name="t">The current time or position in the animation, where 0 represents the start and <paramref name="d"/> represents
    /// the end.</param>
    /// <param name="b">The starting value of the property being animated.</param>
    /// <param name="c">The total change in the value of the property being animated.</param>
    /// <param name="d">The total duration of the animation.</param>
    /// <param name="s">An optional overshoot parameter that controls the intensity of the easing effect. Defaults to 1.70158.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d, double s = 1.70158f)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * (t * t * (((s *= 1.525f) + 1) * t - s)) + b;
        }

        return c / 2 * ((t -= 2) * t * (((s *= 1.525f) + 1) * t + s) + 2) + b;
    }
}
