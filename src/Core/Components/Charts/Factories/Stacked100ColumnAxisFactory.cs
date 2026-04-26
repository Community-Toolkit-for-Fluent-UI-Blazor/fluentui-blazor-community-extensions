using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class Stacked100ColumnAxisFactory : IAxisFactory<ColumnSerieOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var columns = series
            .Where(s => s.IsVisible)
            .OfType<Series.ColumnSerie>()
            .ToList();

        if (columns.Count == 0)
        {
            return (
                XAxis: CreateCategoryAxis([]),
                YAxis: CreateNumericAxis(0, 1)
            );
        }

        var categories = columns
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Name)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var xAxis = CreateCategoryAxis(categories);
        var yAxis = CreateNumericAxis(0, 1);

        return (xAxis, yAxis);
    }

    private static ChartAxis CreateCategoryAxis(List<string> labels)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Category,
            Minimum = 0,
            Maximum = Math.Max(0, labels.Count - 1),
            Labels = labels
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
