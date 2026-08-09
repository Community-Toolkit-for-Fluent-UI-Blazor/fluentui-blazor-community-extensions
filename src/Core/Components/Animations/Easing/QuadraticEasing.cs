namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a quadratic easing function for animations.
/// </summary>
public static class QuadraticEasing
{
    /// <summary>
    /// Calculates the eased-in value for a given time, using a quadratic easing function.
    /// </summary>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the property being eased.</param>
    /// <param name="c">The total change in the value of the property being eased.</param>
    /// <param name="d">The total duration of the easing operation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        c * (t /= d) * t + b;

    /// <summary>
    /// Applies an ease-out interpolation to calculate a value based on the specified parameters.
    /// </summary>
    /// <remarks>The ease-out interpolation starts quickly and decelerates toward the end of the duration.
    /// Ensure that <paramref name="d"/> is not zero to avoid division by zero errors.</remarks>
    /// <param name="t">The current time or position, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        -c * (t /= d) * (t - 2) + b;

    /// <summary>
    /// Applies an ease-in-out quadratic interpolation to calculate a value based on the progression of time.
    /// </summary>
    /// <remarks>This method uses a quadratic easing function to smoothly transition between the starting and
    /// ending values. The easing effect accelerates at the beginning, decelerates in the middle, and accelerates again
    /// toward the end.</remarks>
    /// <param name="t">The current time or position within the duration, where 0 represents the start and <paramref name="d"/>
    /// represents the end.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t + b;
        }

        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    }
}
