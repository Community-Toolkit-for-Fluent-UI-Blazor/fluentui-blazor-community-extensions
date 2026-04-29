using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal sealed class XYAxesFactory : IAxisFactory<XYSeriesOptions>
{
    /// <inheritdoc />
    public (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(
        bool sort,
        IEnumerable<ChartSerie> series,
        ChartRect plotArea)
    {
        var xySeries = series
            .Where(s => s.IsVisible)
            .OfType<XYSerie>()
            .ToList();

        if (xySeries.Count == 0)
        {
            return (CreateEmptyNumericAxis(), CreateEmptyNumericAxis());
        }

        var items = xySeries.SelectMany(s => s.Items).ToList();
        var xMin = items.Min(i => i.X);
        var xMax = items.Max(i => i.X);

        var yMin = items.Min(i => i.Y);
        var yMax = items.Max(i => i.Y);

        if (xMin == xMax)
        {
            xMin -= 1;
            xMax += 1;
        }

        if (yMin == yMax)
        {
            yMin -= 1;
            yMax += 1;
        }

        if (xMin > 0)
        {
            xMin = 0;
        }

        if (yMin > 0)
        {
            yMin = 0;
        }

        if (xMax < 0)
        {
            xMax = 0;
        }

        if (yMax < 0)
        {
            yMax = 0;
        }

        var xAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = xMin,
            DataMaximum = xMax
        };

        var yAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = yMin,
            DataMaximum = yMax
        };

        return (xAxis, yAxis);
    }

    private static ChartAxis CreateEmptyNumericAxis() =>
        new()
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = 0,
            DataMaximum = 1
        };
}

