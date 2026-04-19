using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Provides a factory for creating category (X) and numeric (Y) axes for category line charts based on the provided
/// chart options, series, and plot area.
/// </summary>
/// <remarks>This factory is intended for use with line charts that use categorical X axes and numeric Y axes. It
/// analyzes the visible series and their items to determine the set of categories and the numeric value range, ensuring
/// that axes are valid even when no data is present. The axes produced are suitable for rendering category line charts
/// within the specified plot area.</remarks>
internal sealed class CategoryLineAxisFactory : IAxisFactory<CategoryLineOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var lineSeries = series
            .Where(s => s.IsVisible && (s.ChartType == ChartType.CategoryLine || s.ChartType == ChartType.CategoryArea))
            .OfType<CategoryLineSerie>()
            .ToList();

        if (lineSeries.Count == 0)
        {
            return (
                XAxis: CreateEmptyCategoryAxis(plotArea),
                YAxis: CreateEmptyNumericAxis(plotArea)
            );
        }

        var categories = lineSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Category)
            .Distinct()
            .ToList();

        var values = lineSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Value)
            .ToList();

        var min = values.Min();
        var max = values.Max();

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
    /// only one category is provided, all indices map to the starting position.</remarks>
    /// <param name="categories">The list of category names to be represented on the axis. The number of categories determines the axis range and
    /// tick positions.</param>
    /// <param name="plot">The plot area that defines the horizontal position and width for the axis mapping.</param>
    /// <returns>A ChartAxis configured as a category axis, with axis range and mapping function corresponding to the provided
    /// categories and plot area.</returns>
    private static ChartAxis CreateCategoryAxis(
        List<string> categories,
        ChartRect plot)
    {
        var start = plot.X;
        var end = plot.X + plot.Width;
        var step = categories.Count > 1
            ? (end - start) / (categories.Count - 1)
            : 0;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = categories.Count - 1,
            Labels = categories,
            Map = index => start + index * step
        };
    }

    /// <summary>
    /// Creates a numeric axis for a chart based on the specified minimum and maximum values and the provided plot area.
    /// </summary>
    /// <remarks>The returned axis maps numeric values to vertical positions within the plot area, with higher
    /// values mapped toward the top of the area.</remarks>
    /// <param name="min">The minimum value represented on the axis.</param>
    /// <param name="max">The maximum value represented on the axis.</param>
    /// <param name="plot">The plot area used to determine the axis position and scaling.</param>
    /// <returns>A ChartAxis instance configured as a numeric axis with the specified range and mapping to the plot area.</returns>
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
