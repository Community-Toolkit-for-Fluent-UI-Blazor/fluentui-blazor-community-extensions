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

        var axis = context.XAxis!;
        var axisMin = axis.Minimum;
        var axisMax = axis.Maximum;

        double MapX(double value)
        {
            if (axisMin == axisMax)
            {
                return plot.X;
            }

            var t = (value - axisMin) / (axisMax - axisMin);
            return plot.X + t * plot.Width;
        }

        var bars = new List<ChartBar>(categoryCount);

        for (var categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++)
        {
            var value = serie.Items[categoryIndex].Value;
            var total = 0.0;

            if (serie.IsFull)
            {
                for (var s = 0; s < allSeries.Count; s++)
                {
                    total += allSeries[s].Items[categoryIndex].Value;
                }

                value = total == 0 ? 0 : value / total;
            }

            var stackedValue = 0.0;

            for (var s = 0; s < serieIndex; s++)
            {
                var prev = allSeries[s].Items[categoryIndex].Value;

                if (serie.IsFull)
                {
                    prev = total == 0 ? 0 : prev / total;
                }

                stackedValue += prev;
            }

            var x0 = MapX(stackedValue);
            var x1 = MapX(stackedValue + value);
            var bandTop = plot.Y + categoryIndex * bandHeight + barOffset;
            var y = bandTop + barHeight / 2.0;

            bars.Add(new ChartBar
            {
                Id = serie.Items[categoryIndex].Id ?? $"bar-{serie.Name}-{categoryIndex}",
                X = Math.Min(x0, x1),
                Y = y,
                Width = Math.Abs(x1 - x0),
                Height = barHeight,
                Value = value,
                CategoryIndex = categoryIndex
            });
        }

        return bars;
    }
}
