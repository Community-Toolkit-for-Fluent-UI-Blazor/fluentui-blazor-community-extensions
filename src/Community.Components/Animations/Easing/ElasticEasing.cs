namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides easing functions that produce an elastic motion effect, commonly used in animations.
/// </summary>
/// <remarks>Elastic easing functions create a motion effect that resembles a spring oscillating back and forth
/// before settling. These functions are useful for creating dynamic and visually appealing animations.  The class
/// includes three easing modes: <list type="bullet"> <item> <term>EaseIn</term> <description>Starts the motion slowly,
/// then accelerates with an elastic effect.</description> </item> <item> <term>EaseOut</term> <description>Starts the
/// motion quickly, then decelerates with an elastic effect.</description> </item> <item> <term>EaseInOut</term>
/// <description>Combines the effects of <c>EaseIn</c> and <c>EaseOut</c>, starting and ending with an elastic
/// effect.</description> </item> </list></remarks>
public static class ElasticEasing
{
    /// <summary>
    /// Represents a constant multiplier used in calculations.
    /// </summary>
    /// <remarks>The value is derived from multiplying 0.3 by 1.5.</remarks>
    private const double m = 0.3 * 1.5;

    /// <summary>
    /// Calculates the eased-in value for a given time, using an elastic easing function.
    /// </summary>
    /// <remarks>This method uses an elastic easing function, which produces an oscillating effect that
    /// gradually decreases in amplitude as the animation progresses.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the property being animated.</param>
    /// <param name="c">The total change in the value of the property being animated.</param>
    /// <param name="d">The total duration of the animation.</param>
    /// <returns>The calculated eased-in value at the specified time <paramref name="t"/>.</returns>
    public static double EaseIn(double t, double b, double c, double d)
    {
        if (t == 0)
        {
            return b;
        }

        if ((t /= d) == 1)
        {
            return b + c;
        }

        var p = d * 0.3;
        var a = c;
        var s = p / 4;

        return -(a * Math.Pow(2, 10 * (t -= 1)) *
                 Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
    }

    /// <summary>
    /// Calculates the eased-out value of a parameter over time using an elastic easing function.
    /// </summary>
    /// <remarks>This method implements an elastic easing-out function, which starts quickly and then
    /// decelerates  with oscillations as it approaches the target value. It is commonly used in animations to create  a
    /// spring-like effect.</remarks>
    /// <param name="t">The current time, in the range [0, <paramref name="d"/>].</param>
    /// <param name="b">The starting value of the parameter.</param>
    /// <param name="c">The total change in the parameter's value.</param>
    /// <param name="d">The total duration of the easing operation.</param>
    /// <returns>The calculated value at the given time <paramref name="t"/>.</returns>
    public static double EaseOut(double t, double b, double c, double d)
    {
        if (t == 0)
        {
            return b;
        }

        if ((t /= d) == 1)
        {
            return b + c;
        }

        var p = d * 0.3;
        var a = c;
        var s = p / 4;

        return a * Math.Pow(2, -10 * t) *
                   Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    }

    /// <summary>
    /// Applies an easing function that combines both "ease-in" and "ease-out" effects to calculate a value  based on
    /// the progression of time.
    /// </summary>
    /// <remarks>This method uses a sinusoidal easing function to create a smooth transition effect. The
    /// "ease-in" effect  accelerates the value at the beginning, while the "ease-out" effect decelerates it toward the
    /// end.</remarks>
    /// <param name="t">The current time or position in the animation, where 0 represents the start and <paramref name="d"/> represents
    /// the end.</param>
    /// <param name="b">The starting value of the property being animated.</param>
    /// <param name="c">The total change in the value of the property being animated.</param>
    /// <param name="d">The total duration of the animation.</param>
    /// <returns>The calculated value of the property at the given time <paramref name="t"/>.</returns>
    public static double EaseInOut(double t, double b, double c, double d)
    {
        if (t == 0)
        {
            return b;
        }

        if ((t /= d / 2) == 2)
        {
            return b + c;
        }

        var p = d * m;
        var a = c;
        var s = p / 4;

        if (t < 1)
        {
            return -0.5f * (a * Math.Pow(2, 10 * (t -= 1)) *
                            Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }

        return a * Math.Pow(2, -10 * (t -= 1)) *
               Math.Sin((t * d - s) * (2 * Math.PI) / p) * 0.5f + c + b;
    }
}
