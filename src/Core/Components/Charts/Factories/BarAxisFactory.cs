using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

/// <summary>
/// Provides a factory for creating axes for bar chart series. This class generates the appropriate X and Y axes based
/// on the provided bar series options, series data, and plot area configuration.
/// </summary>
/// <remarks>This factory is intended for use with bar chart visualizations and ensures that axes are constructed
/// according to the visible data and chart options. It handles cases where no data is present by returning valid, empty
/// axes, and sorts categories if specified in the options. The factory is not intended to be used directly by
/// consumers; instead, it is used internally by chart rendering components to ensure consistent axis generation for bar
/// charts.</remarks>
internal sealed class BarAxisFactory : IAxisFactory<BarSerieOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var barSeries = series
            .Where(s => s.IsVisible && s.ChartType == ChartType.Bar)
            .OfType<Series.BarSerie>()
            .ToList();

        if (barSeries.Count == 0)
        {
            return (
                XAxis: CreateEmptyNumericAxis(),
                YAxis: CreateEmptyCategoryAxis(plotArea)
            );
        }

        var categories = barSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Category)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var values = barSeries
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

        var xAxis = CreateNumericAxis(min, max);
        var yAxis = CreateCategoryAxis(categories, plotArea);

        return (xAxis, yAxis);
    }

    /// <summary>
    /// Creates a category axis for a chart based on the specified categories and plot area.
    /// </summary>
    /// <remarks>The returned axis maps each category index to a position within the plot area. If only one
    /// category is provided, all indices map to the starting position.</remarks>
    /// <param name="categories">The list of category names to be represented on the axis. The number of categories determines the axis range and
    /// tick positions.</param>
    /// <param name="plot">The plot area that defines the vertical position and height for mapping category indices to chart coordinates.</param>
    /// <returns>A ChartAxis instance configured as a category axis, with axis range and mapping function set according to the
    /// provided categories and plot area.</returns>
    private static ChartAxis CreateCategoryAxis(
        List<string> categories,
        ChartRect plot)
    {
        var start = plot.Y;
        var end = plot.Y + plot.Height;
        var step = categories.Count > 1
            ? (end - start) / (categories.Count - 1)
            : 0;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = categories.Count - 1,
            Map = index => start + index * step,
            Labels = categories,
        };
    }

    /// <summary>
    /// Creates a numeric axis for a chart based on the specified minimum and maximum values and the provided plot area.
    /// </summary>
    /// <remarks>The returned axis maps numeric values linearly between the specified minimum and maximum
    /// values to the horizontal range of the plot area.</remarks>
    /// <returns>A ChartAxis instance configured as a numeric axis mapped to the specified plot area.</returns>
    private static ChartAxis CreateNumericAxis(
        double min,
        double max)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = min,
            DataMaximum = max
        };
    }

    /// <summary>
    /// Creates a new empty category axis for the specified plot area.
    /// </summary>
    /// <param name="plot">The plot area to which the empty category axis will be associated.</param>
    /// <returns>A new instance of a category axis with no categories, associated with the specified plot area.</returns>
    private static ChartAxis CreateEmptyCategoryAxis(ChartRect plot) => CreateCategoryAxis([], plot);

    /// <summary>
    /// Creates a numeric axis with a default range from 0 to 1 for the specified plot area.
    /// </summary>
    /// <returns>A new ChartAxis instance representing a numeric axis with a range from 0 to 1.</returns>
    private static ChartAxis CreateEmptyNumericAxis() => CreateNumericAxis(0, 1);
}
