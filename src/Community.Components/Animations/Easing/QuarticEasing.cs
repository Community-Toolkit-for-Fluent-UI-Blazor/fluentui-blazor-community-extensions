namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides quartic easing functions for animations, which calculate intermediate values based on a quartic polynomial.
/// These functions are commonly used to create smooth transitions in animations.
/// </summary>
/// <remarks>The class includes three easing functions: <list type="bullet"> <item> <term>EaseIn</term>
/// <description>Starts the motion slowly and accelerates as it progresses.</description> </item> <item>
/// <term>EaseOut</term> <description>Starts the motion quickly and decelerates as it progresses.</description> </item>
/// <item> <term>EaseInOut</term> <description>Combines the behaviors of EaseIn and EaseOut, starting and ending slowly
/// while accelerating in the middle.</description> </item> </list> Each function takes the same parameters,
/// representing the current time, start value, change in value, and duration of the animation.</remarks>
public static class QuarticEasing
{
    /// <summary>
    /// Calculates the eased-in value for a given time using a quartic easing function.
    /// </summary>
    /// <remarks>This method applies a quartic easing-in function, which starts the motion slowly  and
    /// accelerates as time progresses. It is commonly used in animations to create  a smooth, natural-looking
    /// transition.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the easing operation. Must be greater than 0.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        c * (t /= d) * t * t * t + b;

    /// <summary>
    /// Applies an ease-out cubic interpolation to calculate a value based on the progression of time.
    /// </summary>
    /// <param name="t">The current time or position in the interpolation, where 0 represents the start and <paramref name="d"/>
    /// represents the end.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration of the interpolation.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        -c * ((t = t / d - 1) * t * t * t - 1) + b;

    /// <summary>
    /// Calculates an eased value using an ease-in-out quartic interpolation.
    /// </summary>
    /// <remarks>This method applies an ease-in-out quartic easing function, which accelerates at the
    /// beginning and decelerates at the end. The input <paramref name="t"/> is normalized relative to <paramref
    /// name="d"/> to calculate the eased value.</remarks>
    /// <param name="t">The current time or position within the interpolation, where 0 represents the start and <paramref name="d"/>
    /// represents the end.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration of the interpolation.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t * t + b;
        }

        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    }
}
