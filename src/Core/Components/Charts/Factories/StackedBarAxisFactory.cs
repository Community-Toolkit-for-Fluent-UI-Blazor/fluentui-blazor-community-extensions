using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

/// <summary>
/// Represents a factory for creating axes for stacked bar chart series.
/// </summary>
internal sealed class StackedBarAxisFactory : IAxisFactory<BarSerieOptions>
{
    /// <summary>
    /// Creates X and Y axes for a stacked bar chart based on the provided series data.
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
        var stacked = series
            .Where(s => s.IsVisible && s.ChartType == ChartType.StackedBar)
            .OfType<Series.BarSerie>()
            .ToList();

        if (stacked.Count == 0)
        {
            return (
                XAxis: CreateEmptyNumericAxis(plotArea),
                YAxis: CreateEmptyCategoryAxis(plotArea)
            );
        }

        var sums = new Dictionary<string, double>(StringComparer.Ordinal);
        var max = 0.0;

        foreach (var serie in stacked)
        {
            foreach (var item in serie.Items)
            {
                if (!item.IsVisible)
                {
                    continue;
                }

                var category = item.Category;
                var value = item.Value;

                if (sums.TryGetValue(category, out var current))
                {
                    current += value;
                    sums[category] = current;

                    if (current > max)
                    {
                        max = current;
                    }
                }
                else
                {
                    sums[category] = value;

                    if (value > max)
                    {
                        max = value;
                    }
                }
            }
        }

        var categories = sums.Keys.ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        if (max == 0)
        {
            max = 1;
        }

        var xAxis = CreateNumericAxis(0, max, plotArea);
        var yAxis = CreateCategoryAxis(categories, plotArea);

        return (xAxis, yAxis);
    }

    /// <summary>
    /// Creates a category axis with evenly distributed labels mapped to vertical positions within the plot area.
    /// </summary>
    /// <param name="categories">The category labels to display on the axis.</param>
    /// <param name="plot">The plot area rectangle defining the vertical positioning boundaries.</param>
    /// <returns>A <see cref="ChartAxis"/> configured for category display with calculated label positions.</returns>
    private static ChartAxis CreateCategoryAxis(List<string> categories, ChartRect plot)
    {
        var start = plot.Y;
        var end = plot.Y + plot.Height;
        var step = categories.Count > 1 ? (end - start) / (categories.Count - 1) : 0;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = categories.Count - 1,
            Map = index => start + index * step,
            Labels = categories
        };
    }

    /// <summary>
    /// Creates a numeric chart axis with a linear mapping function that transforms data values to pixel coordinates
    /// within the specified plot area.
    /// </summary>
    /// <param name="min">The minimum value of the axis range.</param>
    /// <param name="max">The maximum value of the axis range.</param>
    /// <param name="plot">The rectangular plot area defining the axis bounds.</param>
    /// <returns>A configured chart axis with numeric type and linear value-to-pixel mapping.</returns>
    private static ChartAxis CreateNumericAxis(double min, double max, ChartRect plot)
    {
        var left = plot.X;
        var right = plot.X + plot.Width;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = min,
            DataMaximum = max,
            Map = value =>
            {
                var t = (value - min) / (max - min);
                return left + t * (right - left);
            }
        };
    }

    /// <summary>
    /// Creates a category axis with no categories.
    /// </summary>
    /// <param name="plot">The chart plot rectangle.</param>
    /// <returns>A category axis with no categories.</returns>
    private static ChartAxis CreateEmptyCategoryAxis(ChartRect plot) => CreateCategoryAxis([], plot);

    /// <summary>
    /// Creates a numeric axis with a default range of 0 to 1.
    /// </summary>
    /// <param name="plot">The chart rectangle defining the plot area.</param>
    /// <returns>A numeric chart axis with a default range.</returns>
    private static ChartAxis CreateEmptyNumericAxis(ChartRect plot) => CreateNumericAxis(0, 1, plot);
}
