using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class HistogramLayoutEngine
{
    public static IReadOnlyList<HistogramBar> Layout(ChartContext context)
    {
        var model = context.HistogramModel!;
        var counts = model.Counts;
        var binEdges = model.BinEdges;
        var binCount = counts.Length;

        if (binCount == 0)
        {
            return [];
        }

        var bars = new HistogramBar[binCount];
        var xAxis = context.XAxis!;
        var yAxis = context.YAxis!;
        var baseline = yAxis.Map(0);

        for (var b = 0; b < binCount; b++)
        {
            var x1Value = binEdges[b];
            var x2Value = binEdges[b + 1];
            var count = counts[b];
            var x1 = xAxis.Map(x1Value);
            var x2 = xAxis.Map(x2Value);
            var y2 = yAxis.Map(count);
            var y1 = baseline;

            bars[b] = new HistogramBar
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                Count = count
            };
        }

        return bars;
    }
}

