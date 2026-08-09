namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides easing functions that simulate a bouncing motion for animations.
/// </summary>
/// <remarks>This class includes static methods for calculating bounce easing values, which are commonly used in
/// animations to create effects that mimic the behavior of a bouncing object. The easing functions include: <list
/// type="bullet"> <item><term>EaseOut</term>: Starts quickly and decelerates, simulating a bounce at the end.</item>
/// <item><term>EaseIn</term>: Starts slowly and accelerates, simulating a bounce at the beginning.</item>
/// <item><term>EaseInOut</term>: Combines the effects of EaseIn and EaseOut, simulating a bounce at both the beginning
/// and end.</item> </list> These methods are useful for creating visually appealing animations in user interfaces,
/// games, or other graphical applications.</remarks>
public static class BounceEasing
{
    /// <summary>
    /// Applies an easing-out function to interpolate a value over time, creating a decelerating motion.
    /// </summary>
    /// <remarks>This method uses a piecewise easing-out function based on the "bounce" easing equation.  It
    /// is commonly used in animations to create a natural deceleration effect.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d)
    {
        if ((t /= d) < (1 / 2.75f))
        {
            return c * (7.5625f * t * t) + b;
        }
        else if (t < (2 / 2.75f))
        {
            return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f) + b;
        }
        else if (t < (2.5f / 2.75f))
        {
            return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f) + b;
        }
        else
        {
            return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f) + b;
        }
    }

    /// <summary>
    /// Calculates the eased-in value for a given time, using an easing function.
    /// </summary>
    /// <remarks>This method applies an easing-in effect, which starts the interpolation slowly and
    /// accelerates as it progresses. The easing behavior is determined by the complementary <see cref="EaseOut"/>
    /// function.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the interpolation.</param>
    /// <returns>The interpolated value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d) =>
        c - EaseOut(d - t, 0, c, d) + b;

    /// <summary>
    /// Calculates an eased value using a combination of ease-in and ease-out interpolation.
    /// </summary>
    /// <remarks>This method combines ease-in and ease-out easing functions to create a smooth transition. The
    /// first half of the duration applies an ease-in effect, while the second half applies an ease-out
    /// effect.</remarks>
    /// <param name="t">The current time or position within the easing duration. Must be between 0 and <paramref name="d"/>.</param>
    /// <param name="b">The starting value of the interpolation.</param>
    /// <param name="c">The total change in value over the duration.</param>
    /// <param name="d">The total duration of the easing operation.</param>
    /// <returns>The interpolated value at the given time <paramref name="t"/>, smoothly transitioning between ease-in and
    /// ease-out phases.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if (t < d / 2)
        {
            return EaseIn(t * 2, 0, c, d) * 0.5f + b;
        }

        return EaseOut(t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b;
    }
}
