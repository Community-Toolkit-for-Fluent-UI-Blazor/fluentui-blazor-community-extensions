namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides cubic easing functions for animations, including easing in, easing out, and easing in and out.
/// </summary>
/// <remarks>Cubic easing functions are commonly used in animations to create smooth transitions. These functions
/// calculate the interpolated value based on the cubic easing formula, which determines the rate of change over time.
/// The methods in this class allow for acceleration, deceleration, or a combination of both.</remarks>
public static class CubicEasing
{
    /// <summary>
    /// Calculates the eased-in interpolation value for a given time, using a cubic easing function.
    /// </summary>
    /// <remarks>This method uses a cubic easing-in function, which starts the motion slowly and accelerates
    /// as time progresses.</remarks>
    /// <param name="t">The current time, or position, within the easing duration. Must be between 0 and <paramref name="d"/>.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the easing operation. Must be greater than 0.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        c * (t /= d) * t * t + b;

    /// <summary>
    /// Applies an ease-out cubic interpolation to calculate a value based on the given time and range.
    /// </summary>
    /// <param name="t">The current time or position, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the range.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        c * ((t = t / d - 1) * t * t + 1) + b;

    /// <summary>
    /// Calculates an eased value using a cubic ease-in-out interpolation.
    /// </summary>
    /// <param name="t">The current time or position, where 0 represents the start and <paramref name="d"/> represents the end.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t + b;
        }

        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }
}
