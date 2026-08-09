namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides easing functions based on a quintic polynomial (t^5) for smooth animations.
/// </summary>
/// <remarks>Quintic easing functions are commonly used in animations to create smooth transitions. These
/// functions calculate intermediate values based on a time parameter, allowing for different easing effects such as
/// accelerating, decelerating, or a combination of both.</remarks>
public static class QuinticEasing
{
    /// <summary>
    /// Calculates the eased value for a given time using an "EaseIn" quintic easing function.
    /// </summary>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the property being eased.</param>
    /// <param name="c">The total change in the value of the property being eased.</param>
    /// <param name="d">The total duration of the easing operation.</param>
    /// <returns>The calculated eased value at the specified time.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        c * (t /= d) * t * t * t * t + b;

    /// <summary>
    /// Applies an easing-out function to interpolate a value over time, creating a decelerating motion.
    /// </summary>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        c * ((t = t / d - 1) * t * t * t * t + 1) + b;

    /// <summary>
    /// Applies an ease-in-out quintic interpolation to the given time value.
    /// </summary>
    /// <remarks>This method uses a quintic easing function to create a smooth transition that accelerates at
    /// the start,  decelerates at the end, and maintains a steady rate in the middle.</remarks>
    /// <param name="t">The current time, or position, within the animation. Must be between 0 and <paramref name="d"/>.</param>
    /// <param name="b">The starting value of the property being animated.</param>
    /// <param name="c">The total change in the value of the property being animated.</param>
    /// <param name="d">The total duration of the animation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t * t * t + b;
        }

        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }
}
