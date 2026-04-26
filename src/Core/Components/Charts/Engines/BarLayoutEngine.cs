using FluentUI.Blazor.Community.Components.Charts.Drawing;
using BS = FluentUI.Blazor.Community.Components.Charts.Series.BarSerie;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the layout engine responsible for calculating the positions and
///  dimensions of bars in a bar chart series based on the provided chart
///  context and series data.
/// </summary>
internal static class BarLayoutEngine
{
    /// <summary>
    /// Calculates the layout and positioning of bar for a single data series within a bar chart, based on the
    /// provided chart context and value range.
    /// </summary>
    /// <remarks>The method ensures that bars from multiple series are positioned side by side within each
    /// category band. The height of each bar is adjusted based on the series options and the total number of series.
    /// The horizontal position and width of each bar are scaled according to the specified value range and the plot
    /// area in the chart context.</remarks>
    /// <param name="serie">The data series for which to compute bar positions and sizes. Must not be null and should contain the items
    /// to be rendered as bars.</param>
    /// <param name="allSeries">A read-only list of all data series included in the chart. Used to determine the number of series for layout
    /// calculations. Must not be null.</param>
    /// <param name="minValue">The minimum value to use for scaling the horizontal axis. Typically represents the lowest data value across all
    /// series.</param>
    /// <param name="maxValue">The maximum value to use for scaling the horizontal axis. Typically represents the highest data value across all
    /// series.</param>
    /// <param name="serieIndex">The zero-based index of the current series within the allSeries list. Determines the horizontal offset of the
    /// columns for this series.</param>
    /// <param name="context">The chart context containing layout information such as the plot area dimensions. Must not be null.</param>
    /// <param name="options">The chart options containing configuration settings for bar rendering, including bar height and spacing. Must not be null.</param>
    /// <returns>A read-only list of ChartColumn objects representing the calculated position, size, and value for each column in
    /// the specified data series.</returns>
    public static IReadOnlyList<ChartBar> Layout(
        BS serie,
        IReadOnlyList<BS> allSeries,
        double minValue,
        double maxValue,
        int serieIndex,
        ChartContext context,
        CO options)
    {
        var plot = context.PlotArea;
        var categoryCount = serie.Items.Count;
        var seriesCount = allSeries.Count;
        var bandHeight = plot.Height / categoryCount;
        var rawBarHeight = bandHeight / seriesCount;
        var barHeight = rawBarHeight * (options.DefaultBarOptions.BarHeight);
        var barOffset = (rawBarHeight - barHeight) / 2.0;
        var axisMin = Math.Min(0, minValue);
        var axisMax = Math.Max(0, maxValue);

        double MapX(double value)
        {
            if (axisMax == axisMin)
            {
                return plot.X;
            }

            var t = (value - axisMin) / (axisMax - axisMin);

            return plot.X + t * plot.Width;
        }

        var bars = new List<ChartBar>(categoryCount);

        for (var categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++)
        {
            var item = serie.Items[categoryIndex];
            var bandTop = plot.Y +
                          categoryIndex * bandHeight +
                          serieIndex * rawBarHeight +
                          barOffset;

            var y = bandTop + barHeight / 2.0;
            var x0 = MapX(0);
            var x1 = MapX(item.Value);
            var x = Math.Min(x0, x1);
            var width = Math.Abs(x1 - x0);

            bars.Add(new ChartBar
            {
                Id = item.Id ?? $"bar-{serie.Name}-{categoryIndex}",
                X = x,
                Y = y,
                Width = width,
                Height = barHeight,
                Value = item.Value,
                CategoryIndex = categoryIndex
            });
        }

        return bars;
    }
}
