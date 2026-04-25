using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class ScatterAxesFactory : IAxisFactory<CategoryLineOptions>
{
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var scatterSeries = series
            .Where(s => s.IsVisible && s.ChartType == ChartType.Scatter)
            .OfType<Series.LineSerie>()
            .ToList();

        if (scatterSeries.Count == 0)
        {
            return (
                XAxis: CreateEmptyCategoryAxis(plotArea),
                YAxis: CreateEmptyNumericAxis(plotArea)
            );
        }

        var categories = scatterSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Category)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var values = scatterSeries
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
