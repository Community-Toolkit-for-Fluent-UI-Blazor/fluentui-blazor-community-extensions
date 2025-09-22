namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for performing exponential easing calculations, commonly used in animations to create smooth
/// transitions between values.
/// </summary>
/// <remarks>Exponential easing functions are used to interpolate values over time, producing transitions that
/// start or end with a rapid change in value. These methods are useful in scenarios such as animating UI elements or
/// simulating motion effects.  The class includes three easing modes: <list type="bullet"> <item> <term>EaseIn</term>
/// <description>Starts the transition slowly and accelerates towards the end.</description> </item> <item>
/// <term>EaseOut</term> <description>Starts the transition quickly and decelerates towards the end.</description>
/// </item> <item> <term>EaseInOut</term> <description>Combines EaseIn and EaseOut, starting and ending slowly with
/// acceleration in the middle.</description> </item> </list></remarks>
public static class ExponentialEasing
{
    /// <summary>
    /// Calculates the eased-in value for a given time using an exponential easing function.
    /// </summary>
    /// <remarks>This method uses an exponential easing function, which starts slowly and accelerates  as time
    /// progresses. If <paramref name="t"/> is 0, the method returns <paramref name="b"/>.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the property being eased.</param>
    /// <param name="c">The total change in the value of the property being eased.</param>
    /// <param name="d">The total duration of the easing, in the same units as <paramref name="t"/>.</param>
    /// <returns>The calculated eased-in value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        (t == 0) ? b : c * (double)Math.Pow(2, 10 * (t / d - 1)) + b;

    /// <summary>
    /// Applies an exponential easing-out function to interpolate a value over time.
    /// </summary>
    /// <remarks>This method implements an exponential easing-out function, which starts quickly and
    /// decelerates toward the end of the duration. If <paramref name="t"/> equals <paramref name="d"/>, the method
    /// returns <paramref name="b"/> + <paramref name="c"/>.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation. Must be greater than 0.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d) =>
        (t == d) ? b + c : c * (-(double)Math.Pow(2, -10 * t / d) + 1) + b;

    /// <summary>
    /// Calculates an eased value using an exponential easing function with both "ease-in" and "ease-out" behavior.
    /// </summary>
    /// <remarks>This method applies an exponential easing function that accelerates at the beginning
    /// ("ease-in") and decelerates at the end ("ease-out"). The easing effect is symmetric, with the midpoint of the
    /// animation being the transition between acceleration and deceleration.</remarks>
    /// <param name="t">The current time or position within the animation, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the property being animated.</param>
    /// <param name="c">The total change in the value of the property being animated.</param>
    /// <param name="d">The total duration of the animation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if (t == 0)
        {
            return b;
        }

        if (t == d)
        {
            return b + c;
        }

        if ((t /= d / 2) < 1)
        {
            return c / 2 * (double)Math.Pow(2, 10 * (t - 1)) + b;
        }

        return c / 2 * (-(double)Math.Pow(2, -10 * --t) + 2) + b;
    }
}
