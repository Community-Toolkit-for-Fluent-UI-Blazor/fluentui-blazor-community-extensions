namespace FluentUI.Blazor.Community.Components.Charts.Helpers;

/// <summary>
/// Represents a helper class for generating "nice" numeric tick values for chart axes.
/// </summary>
internal static class NumericTickGenerator
{
    /// <summary>
    /// Generates a sequence of "nice" numeric tick values between the specified minimum and maximum.
    /// </summary>
    /// <param name="min">The minimum data value.</param>
    /// <param name="max">The maximum data value.</param>
    /// <param name="maxTicks">The approximate maximum number of ticks to generate.</param>
    /// <returns>A read-only list of tick values.</returns>
    public static IReadOnlyList<double> GenerateNice(double min, double max, int maxTicks = 6)
    {
        var ticks = new List<double>();

        if (double.IsNaN(min) || double.IsNaN(max) ||
            double.IsInfinity(min) || double.IsInfinity(max))
        {
            return ticks;
        }

        if (min == max)
        {
            // Expand a degenerate range
            min -= 1;
            max += 1;
        }

        if (max < min)
        {
            (min, max) = (max, min);
        }

        var range = NiceNumber(max - min, round: false);
        var step = NiceNumber(range / (maxTicks - 1), round: true);

        var tickMin = Math.Floor(min / step) * step;
        var tickMax = Math.Ceiling(max / step) * step;
        var guard = 0;

        for (var v = tickMin; v <= tickMax + step * 0.5; v += step)
        {
            ticks.Add(v);

            if (++guard > 1000)
            {
                break;
            }
        }

        return ticks;
    }

    /// <summary>
    /// Generates a sequence of evenly spaced numeric tick values between the specified minimum and maximum.
    /// </summary>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="count">The number of tick values to generate.</param>
    /// <returns>A read-only list of evenly spaced tick values.</returns>
    public static IReadOnlyList<double> GenerateExactTicks(double min, double max, int count)
    {
        var list = new List<double>();

        if (count < 2)
        {
            count = 2;
        }

        var step = (max - min) / (count - 1);

        for (var i = 0; i < count; i++)
        {
            list.Add(min + i * step);
        }

        return list;
    }

    /// <summary>
    /// Returns a "nice" number approximately equal to range.
    /// Rounds the number if round is true, otherwise takes the ceiling.
    /// </summary>
    private static double NiceNumber(double range, bool round)
    {
        if (range <= 0 || double.IsNaN(range) || double.IsInfinity(range))
        {
            return 1;
        }

        var exponent = Math.Floor(Math.Log10(range));
        var fraction = range / Math.Pow(10, exponent);

        double niceFraction;

        if (round)
        {
            if (fraction < 1.5)
            {
                niceFraction = 1;
            }
            else if (fraction < 3)
            {
                niceFraction = 2;
            }
            else if (fraction < 7)
            {
                niceFraction = 5;
            }
            else
            {
                niceFraction = 10;
            }
        }
        else
        {
            if (fraction <= 1)
            {
                niceFraction = 1;
            }
            else if (fraction <= 2)
            {
                niceFraction = 2;
            }
            else if (fraction <= 5)
            {
                niceFraction = 5;
            }
            else
            {
                niceFraction = 10;
            }
        }

        return niceFraction * Math.Pow(10, exponent);
    }
}
