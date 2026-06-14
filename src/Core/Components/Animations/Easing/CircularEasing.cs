namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides easing functions based on a circular mathematical formula.
/// </summary>
/// <remarks>Circular easing functions create smooth transitions that mimic the motion of an object following a
/// circular path. These functions are commonly used in animations to achieve natural-looking acceleration and
/// deceleration effects.</remarks>
public static class CircularEasing
{
    /// <summary>
    /// Calculates the eased-in value for a given time using a circular easing function.
    /// </summary>
    /// <param name="t">The current time, or position, within the easing duration. Must be between 0 and <paramref name="d"/>.</param>
    /// <param name="b">The starting value of the property being eased.</param>
    /// <param name="c">The total change in the value of the property being eased.</param>
    /// <param name="d">The total duration of the easing operation. Must be greater than 0.</param>
    /// <returns>The calculated eased-in value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        -c * ((double)Math.Sqrt(1 - (t /= d) * t) - 1) + b;

    /// <summary>
    /// Applies an ease-out interpolation to calculate a value based on the specified parameters.
    /// </summary>
    /// <remarks>The ease-out interpolation starts quickly and decelerates toward the end, creating a smooth
    /// transition. Ensure that <paramref name="d"/> is greater than zero to avoid division by zero.</remarks>
    /// <param name="t">The current time or position, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        c * (double)Math.Sqrt(1 - (t = t / d - 1) * t) + b;

    /// <summary>
    /// Calculates an eased value using an "ease-in-out" interpolation, which provides a smooth transition  that
    /// accelerates at the beginning and decelerates at the end.
    /// </summary>
    /// <remarks>This method is commonly used in animations or transitions to create a natural acceleration
    /// and deceleration effect. The input <paramref name="t"/> is normalized relative to the duration <paramref
    /// name="d"/> to calculate the eased value.</remarks>
    /// <param name="t">The current time or position within the easing duration. Must be between 0 and <paramref name="d"/>.</param>
    /// <param name="b">The starting value of the property being eased.</param>
    /// <param name="c">The total change in the value of the property being eased.</param>
    /// <param name="d">The total duration of the easing operation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>, based on the "ease-in-out" easing function.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if ((t /= d / 2) < 1)
        {
            return -c / 2 * ((double)Math.Sqrt(1 - t * t) - 1) + b;
        }

        return c / 2 * ((double)Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
    }
}
