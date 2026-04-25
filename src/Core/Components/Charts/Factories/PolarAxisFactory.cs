using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class PolarAxisFactory : IAxisFactory<PolarSerieOptions>
{
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var polarSeries = series
            .Where(s => s is PolarSerie { IsVisible: true })
            .Cast<PolarSerie>()
            .ToList();

        if (polarSeries.Count == 0)
        {
            return (EmptyAxis(), EmptyAxis());
        }

        var categories = polarSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Category)
            .Distinct()
            .ToList();

        if (sort)
        {
            categories.Sort(StringComparer.Ordinal);
        }

        var values = polarSeries
            .SelectMany(s => s.Items)
            .Where(i => i.IsVisible)
            .Select(i => i.Value)
            .ToList();

        var max = values.Max();
        return (
            CreateAngleAxis(categories),
            CreateRadiusAxis(max)
        );
    }

    private static ChartAxis CreateAngleAxis(List<string> categories)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Polar,
            Minimum = 0,
            Maximum = categories.Count,
            Labels = categories,
            Map = index => index * (2 * Math.PI / categories.Count)
        };
    }

    private static ChartAxis CreateRadiusAxis(double max)
    {
        return new ChartAxis
        {
            AxisType = ChartAxisType.Polar,
            DataMinimum = 0,
            DataMaximum = max,
            Map = value =>
            {
                var t = value / max;

                return t;
            }
        };
    }

    private static ChartAxis EmptyAxis() => new() { Map = _ => 0 };
}
