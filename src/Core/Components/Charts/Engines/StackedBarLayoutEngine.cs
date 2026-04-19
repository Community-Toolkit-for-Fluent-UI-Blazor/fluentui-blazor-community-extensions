using FluentUI.Blazor.Community.Components.Charts.Drawing;
using BS = FluentUI.Blazor.Community.Components.Charts.Series.BarSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Provides layout calculations for stacked bar chart series by computing the position and dimensions of bars based on
/// cumulative values from preceding series.
/// </summary>
internal static class StackedBarLayoutEngine
{
    /// <summary>
    /// Calculates the layout positions and dimensions for horizontal bars in a stacked bar chart.
    /// </summary>
    /// <param name="serie">The series to calculate bar positions for.</param>
    /// <param name="allSeries">All series in the chart, used to calculate stacked offsets.</param>
    /// <param name="serieIndex">The index of the current series within all series.</param>
    /// <param name="context">The chart rendering context containing plot area dimensions.</param>
    /// <returns>A read-only list of positioned chart bars with calculated dimensions and coordinates.</returns>
    public static IReadOnlyList<ChartBar> Layout(
        BS serie,
        IReadOnlyList<BS> allSeries,
        int serieIndex,
        ChartContext context)
    {
        var plot = context.PlotArea;
        var categoryCount = serie.Items.Count;
        var bandHeight = plot.Height / categoryCount;
        var rawBarHeight = bandHeight;
        var barHeight = rawBarHeight * (serie.Options?.BarHeight ?? 1.0);
        var barOffset = (rawBarHeight - barHeight) / 2.0;
        var axis = context.XAxis;
        var minValue = axis?.Minimum ?? 0;
        var maxValue = axis?.Maximum ?? 1;

        double MapX(double value)
        {
            if (minValue == maxValue)
            {
                return plot.X;
            }

            var t = (value - minValue) / (maxValue - minValue);

            return plot.X + t * plot.Width;
        }

        var bars = new List<ChartBar>(categoryCount);

        for (var categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++)
        {
            var item = serie.Items[categoryIndex];
            var stackedValue = 0.0;

            for (var s = 0; s < serieIndex; s++)
            {
                stackedValue += allSeries[s].Items[categoryIndex].Value;
            }

            var bandTop = plot.Y + categoryIndex * bandHeight + barOffset;
            var y = bandTop + barHeight / 2.0;

            var x0 = MapX(stackedValue);
            var x1 = MapX(stackedValue + item.Value);

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
