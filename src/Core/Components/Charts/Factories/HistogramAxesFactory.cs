using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Factories;

internal static class HistogramAxesFactory
{
    public static (ChartAxis XAxis, ChartAxis YAxis) CreateAxes(HistogramModel model)
    {
        var xAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = model.Min,
            DataMaximum = model.Max
        };

        var maxCount = 0;

        foreach (var c in model.Counts)
        {
            if (c > maxCount)
            {
                maxCount = c;
            }
        }

        var yAxis = new ChartAxis
        {
            AxisType = ChartAxisType.Numeric,
            DataMinimum = 0,
            DataMaximum = maxCount
        };

        return (xAxis, yAxis);
    }
}
