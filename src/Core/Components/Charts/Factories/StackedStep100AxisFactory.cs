using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class Stacked100StepAxisFactory : IAxisFactory<CategoryLineOptions>
{
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var steps = series
            .Where(s => s.IsVisible &&
                        (s.ChartType == ChartType.Stacked100Step))
            .OfType<Series.LineSerie>()
            .ToList();

        if (steps.Count == 0)
        {
            return (
                XAxis: CreateCategoryAxis([], plotArea),
                YAxis: CreateNumericAxis(0, 1, plotArea)
            );
        }

        var categories = steps
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Name)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var xAxis = CreateCategoryAxis(categories, plotArea);
        var yAxis = CreateNumericAxis(0, 1, plotArea);

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
            Maximum = Math.Max(0, categories.Count - 1),
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
}
