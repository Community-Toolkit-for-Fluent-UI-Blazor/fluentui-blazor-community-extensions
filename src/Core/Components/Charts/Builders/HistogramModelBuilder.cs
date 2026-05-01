using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class HistogramModelBuilder
{
    public static HistogramModel Build(
        IReadOnlyList<ValueItem> items,
        HistogramOptions options)
    {
        var min = double.MaxValue;
        var max = double.MinValue;

        for (var i = 0; i < items.Count; i++)
        {
            var v = items[i].Value;

            min = Math.Min(min, v);
            max = Math.Max(max, v);
        }

        if (min == max)
        {
            min -= 1;
            max += 1;
        }

        var binCount = options.BinCount;
        var range = max - min;
        var binWidth = range / binCount;
        var counts = new int[binCount];
        var edges = new double[binCount + 1];

        for (var i = 0; i <= binCount; i++)
        {
            edges[i] = min + i * binWidth;
        }

        for (var i = 0; i < items.Count; i++)
        {
            var v = items[i].Value;
            var bin = (int)((v - min) / binWidth);

            if (bin == binCount)
            {
                bin--;
            }

            counts[bin]++;
        }

        return new HistogramModel
        {
            Min = min,
            Max = max,
            BinWidth = binWidth,
            Counts = counts,
            BinEdges = edges
        };
    }
}

