using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class StackedCategoryLineAxisFactory : IAxisFactory<CategoryLineOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var stacked = series
            .Where(s => s.IsVisible &&
                        (s.ChartType == ChartType.StackedArea))
            .OfType<Series.LineSerie>()
            .ToList();

        if (stacked.Count == 0)
        {
            return (
                XAxis: CreateEmptyCategoryAxis(plotArea),
                YAxis: CreateEmptyNumericAxis(plotArea)
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

        var xAxis = CreateCategoryAxis(categories, plotArea);
        var yAxis = CreateNumericAxis(0, max, plotArea);

        return (xAxis, yAxis);
    }

    private static ChartAxis CreateCategoryAxis(List<string> categories, ChartRect plot)
    {
        var start = plot.X;
        var end = plot.X + plot.Width;
        var step = categories.Count > 1 ? (end - start) / (categories.Count - 1) : 0;

        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = categories.Count - 1,
            Labels = categories,
            Map = index => start + index * step
        };
    }

    private static ChartAxis CreateNumericAxis(double min, double max, ChartRect plot)
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

    private static ChartAxis CreateEmptyCategoryAxis(ChartRect plot) => CreateCategoryAxis([], plot);

    private static ChartAxis CreateEmptyNumericAxis(ChartRect plot) => CreateNumericAxis(0, 1, plot);
}
