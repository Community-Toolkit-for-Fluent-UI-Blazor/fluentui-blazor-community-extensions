using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

/// <summary>
/// Represents a factory responsible for creating the X and Y axes for
///  a column chart based on the provided chart options, data series, and plot area configuration.
/// </summary>
internal sealed class ColumnAxisFactory : IAxisFactory<ColumnSerieOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var columnSeries = series
            .Where(s => s.IsVisible && s.ChartType == ChartType.Column)
            .OfType<Series.ColumnSerie>()
            .ToList();

        if (columnSeries.Count == 0)
        {
            return (
                XAxis: CreateEmptyCategoryAxis(plotArea),
                YAxis: CreateEmptyNumericAxis(plotArea)
            );
        }

        var categories = columnSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Category)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var values = columnSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Value)
            .ToList();

        var min = values.Min();
        var max = values.Max();

        if (min > 0)
        {
            min = 0;
        }

        if (max < 0)
        {
            max = 0;
        }

        if (min == max)
        {
            min -= 1;
            max += 1;
        }

        var xAxis = CreateCategoryAxis(categories, plotArea);
        var yAxis = CreateNumericAxis(min, max, plotArea);

        return (xAxis, yAxis);
    }

    /// <summary>
    /// Creates a category axis for a chart based on the specified categories and plot area.
    /// </summary>
    /// <remarks>The returned axis maps each category index to a position within the specified plot area. If
    /// only one category is provided, all positions map to the plot area's starting coordinate.</remarks>
    /// <param name="labels">The list of labels to be represented on the axis. The number of categories determines the axis range and
    /// tick positions.</param>
    /// <param name="plot">The plot area that defines the horizontal position and width for the axis mapping.</param>
    /// <returns>A ChartAxis configured as a category axis, with positions mapped according to the plot area and the number of
    /// categories.</returns>
    private static ChartAxis CreateCategoryAxis(
        List<string> labels,
        ChartRect plot)
    {
        var start = plot.X;
        var end = plot.X + plot.Width;
        var step = labels.Count > 1
            ? (end - start) / (labels.Count - 1)
            : 0;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = labels.Count - 1,
            Map = index => start + index * step,
            Labels = labels,
        };
    }

    /// <summary>
    /// Creates a numeric axis for a chart based on the specified minimum and maximum values and plot area.
    /// </summary>
    /// <param name="min">The minimum value of the numeric axis.</param>
    /// <param name="max">The maximum value of the numeric axis.</param>
    /// <param name="plot">The plot area that defines the vertical position and height for the axis mapping.</param>
    /// <returns>Returns a new ChartAxis instance representing a numeric axis with the specified range and plot area.</returns>
    private static ChartAxis CreateNumericAxis(
        double min,
        double max,
        ChartRect plot)
    {
        var top = plot.Y;
        var bottom = plot.Y + plot.Height;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = min,
            DataMaximum = max,
            Map = value =>
            {
                var t = (value - min) / (max - min);
                return bottom - t * (bottom - top);
            }
        };
    }

    /// <summary>
    /// Creates a new empty category axis for the specified plot area.
    /// </summary>
    /// <param name="plot">The plot area to which the empty category axis will be associated.</param>
    /// <returns>A new instance of a category axis with no categories, associated with the specified plot area.</returns>
    private static ChartAxis CreateEmptyCategoryAxis(ChartRect plot) => CreateCategoryAxis([], plot);

    /// <summary>
    /// Creates a numeric chart axis with a default range from 0 to 1 for the specified plot area.
    /// </summary>
    /// <param name="plot">The plot area to which the numeric axis will be added.</param>
    /// <returns>A new ChartAxis instance representing a numeric axis with a range from 0 to 1.</returns>
    private static ChartAxis CreateEmptyNumericAxis(ChartRect plot) => CreateNumericAxis(0, 1, plot);
}

