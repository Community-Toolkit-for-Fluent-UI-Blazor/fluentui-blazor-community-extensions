using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder class responsible for constructing the axes for a chart based on the provided series data.
/// </summary>
internal static class ChartAxesBuilder
{
    /// <summary>
    /// Configures the category (X) and numeric (Y) axes for the chart based on the provided series data.
    /// </summary>
    /// <remarks>The method assumes that all series contain the same number of items, and sets the X axis as a
    /// category axis indexed by data point position. The Y axis is configured to encompass the global minimum and
    /// maximum values across all series, including zero if necessary.</remarks>
    /// <param name="context">The chart context in which the axes will be created and assigned.</param>
    /// <param name="series">The collection of data series used to determine axis categories and value ranges. All series are expected to
    /// have the same number of data points.</param>
    internal static void BuildCategoryAxes(
        ChartContext context,
        IReadOnlyList<ChartSerie> series)
    {
        var plot = context.PlotArea;

        var count = series.Max(s => s.ItemsCount);

        context.XAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = count - 1,
            Map = i =>
            {
                var t = count <= 1 ? 0 : i / (double)(count - 1);

                return plot.X + t * plot.Width;
            }
        };

        var values = series.SelectMany(s => s.Values);
        var min = double.MaxValue;
        var max = double.MinValue;

        foreach (var value in values)
        {
            if (value < min)
            {
                min = value;
            }

            if (value > max)
            {
                max = value;
            }
        }

        min = Math.Min(0, min);
        max = Math.Max(0, max);

        context.YAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            Minimum = min,
            Maximum = max,
            Map = v =>
            {
                var t = (v - min) / (max - min);

                return plot.Y + plot.Height - t * plot.Height;
            }
        };
    }
}
