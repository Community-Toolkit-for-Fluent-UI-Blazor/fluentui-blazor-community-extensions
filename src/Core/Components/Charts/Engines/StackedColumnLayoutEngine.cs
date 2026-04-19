using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CS = FluentUI.Blazor.Community.Components.Charts.Series.ColumnSerie;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the layout engine responsible for calculating
///  the positions and dimensions of columns in a stacked column chart series.
/// </summary>
internal static class StackedColumnLayoutEngine
{
    /// <summary>
    /// Calculates the layout and positioning of columns for a single data series within a stacked column chart, based on the
    /// provided chart context and value range.
    /// </summary>
    /// <remarks>The method ensures that columns from multiple series are positioned side by side within each
    /// category band. The width of each column is adjusted based on the series options and the total number of series.
    /// The vertical position and height of each column are scaled according to the specified value range and the plot
    /// area in the chart context.</remarks>
    /// <param name="serie">The data series for which to compute column positions and sizes. Must not be null and should contain the items
    /// to be rendered as columns.</param>
    /// <param name="allSeries">A read-only list of all data series included in the chart. Used to determine the number of series for layout
    /// calculations. Must not be null.</param>
    /// <param name="serieIndex">The zero-based index of the current series within the allSeries list. Determines the horizontal offset of the
    /// columns for this series.</param>
    /// <param name="context">The chart context containing layout information such as the plot area dimensions. Must not be null.</param>
    /// <returns>A read-only list of ChartColumn objects representing the calculated position, size, and value for each column in
    /// the specified data series.</returns>
    public static IReadOnlyList<ChartColumn> Layout(
        CS serie,
        IReadOnlyList<CS> allSeries,
        int serieIndex,
        ChartContext context)
    {
        var plot = context.PlotArea;
        var categoryCount = serie.Items.Count;
        var bandWidth = plot.Width / categoryCount;
        var rawColumnWidth = bandWidth;
        var columnWidth = rawColumnWidth * (serie.Options?.ColumnWidth ?? 1.0);
        var columnOffset = (rawColumnWidth - columnWidth) / 2.0;
        var axis = context.YAxis;
        var axisMin = Math.Min(0, axis?.Minimum ?? 0);
        var axisMax = Math.Max(0, axis?.Maximum ?? 1);

        double MapY(double value)
        {
            if (axisMax == axisMin)
            {
                return plot.Y + plot.Height;
            }

            var t = (value - axisMin) / (axisMax - axisMin);

            return plot.Y + plot.Height - t * plot.Height;
        }

        var columns = new List<ChartColumn>(categoryCount);

        for (var categoryIndex = 0; categoryIndex < categoryCount; categoryIndex++)
        {
            var item = serie.Items[categoryIndex];
            var stackedValue = 0.0;

            for (var s = 0; s < serieIndex; s++)
            {
                stackedValue += allSeries[s].Items[categoryIndex].Value;
            }

            var x = plot.X +
                    categoryIndex * bandWidth +
                    columnOffset +
                    columnWidth / 2.0;

            var y0 = MapY(stackedValue);
            var y1 = MapY(stackedValue + item.Value);
            var y = Math.Min(y0, y1);
            var height = Math.Abs(y1 - y0);

            columns.Add(new ChartColumn
            {
                Id = item.Id ?? $"col-{serie.Name}-{categoryIndex}",
                X = x,
                Y = y,
                Width = columnWidth,
                Height = height,
                Value = item.Value,
                CategoryIndex = categoryIndex
            });
        }

        return columns;
    }
}

