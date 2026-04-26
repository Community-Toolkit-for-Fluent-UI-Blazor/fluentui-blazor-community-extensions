using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CS = FluentUI.Blazor.Community.Components.Charts.Series.ColumnSerie;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the layout engine responsible for calculating
///  the positions and dimensions of columns in a column chart series.
/// </summary>
internal static class ColumnLayoutEngine
{
    /// <summary>
    /// Calculates the layout and positioning of columns for a single data series within a column chart, based on the
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
    /// <param name="minValue">The minimum value to use for scaling the vertical axis. Typically represents the lowest data value across all
    /// series.</param>
    /// <param name="maxValue">The maximum value to use for scaling the vertical axis. Typically represents the highest data value across all
    /// series.</param>
    /// <param name="serieIndex">The zero-based index of the current series within the allSeries list. Determines the horizontal offset of the
    /// columns for this series.</param>
    /// <param name="options">The chart options containing configuration settings for column layout, such as column width and spacing. Must not be null.</param>
    /// <param name="context">The chart context containing layout information such as the plot area dimensions. Must not be null.</param>
    /// <returns>A read-only list of ChartColumn objects representing the calculated position, size, and value for each column in
    /// the specified data series.</returns>
    public static IReadOnlyList<ChartColumn> Layout(
        CS serie,
        IReadOnlyList<CS> allSeries,
        double minValue,
        double maxValue,
        int serieIndex,
        ChartContext context,
        CO options)
    {
        var columnOptions = options.DefaultColumnOptions;
        var plot = context.PlotArea;
        var categoryCount = serie.Items.Count;
        var seriesCount = allSeries.Count;
        var bandWidth = plot.Width / categoryCount;
        var rawColumnWidth = bandWidth / seriesCount;
        var columnWidth = rawColumnWidth * columnOptions.ColumnWidth;
        var columnOffset = (rawColumnWidth - columnWidth) / 2.0;
        var axisMin = Math.Min(0, minValue);
        var axisMax = Math.Max(0, maxValue);

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
            var x = plot.X +
                    categoryIndex * bandWidth +
                    serieIndex * rawColumnWidth +
                    columnOffset +
                    columnWidth / 2.0;

            var y0 = MapY(0);
            var y1 = MapY(item.Value);
            var y = Math.Min(y0, y1);
            var height = Math.Abs(y1 - y0);

            columns.Add(new ChartColumn
            {
                Id = item.Id ?? $"col-{Guid.NewGuid()}",
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

