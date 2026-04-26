using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

/// <summary>
/// Represents a factory for creating axes for 100% stacked bar chart series.
/// </summary>
internal sealed class Stacked100BarAxisFactory : IAxisFactory<BarSerieOptions>
{
    /// <summary>
    /// Creates X and Y axes for a 100% stacked bar chart based on the provided series data.
    /// </summary>
    /// <param name="sort">Whether to sort categories alphabetically.</param>
    /// <param name="series">The collection of chart series to analyze.</param>
    /// <param name="plotArea">The rectangular area for plotting the chart.</param>
    /// <returns>A tuple containing the numeric X-axis and categorical Y-axis.</returns>
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var bars = series
            .Where(s => s.IsVisible)
            .OfType<Series.BarSerie>()
            .ToList();

        if (bars.Count == 0)
        {
            return (
                XAxis: CreateNumericAxis(0, 1),
                YAxis: CreateCategoryAxis([])
            );
        }

        var categories = bars
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Name)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var xAxis = CreateNumericAxis(0, 1);
        var yAxis = CreateCategoryAxis(categories);

        return (xAxis, yAxis);
    }

    private static ChartAxis CreateCategoryAxis(List<string> categories)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = Math.Max(0, categories.Count - 1),
            Labels = categories
        };
    }

    private static ChartAxis CreateNumericAxis(double min, double max)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = min,
            DataMaximum = max
        };
    }
}
