using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class BubbleLayoutEngine
{
    public static IReadOnlyList<BubblePoint> Layout(
        Series.LineSerie serie,
        ChartContext context)
    {
        var items = CategoryAxisEngine.Sort(serie.Items, serie.Options);
        var count = items.Count;

        var radiusMin = (serie.Options as BubbleSerieOptions)?.MinRadius ?? 4;
        var radiusMax = (serie.Options as BubbleSerieOptions)?.MaxRadius ?? 30;
        var bubbleValues = items.Select(i => i.Value).ToList();

        var minVal = bubbleValues.Min();
        var maxVal = bubbleValues.Max();

        var range = maxVal - minVal;

        if (range <= 0)
        {
            range = 1;
        }

        var points = new List<BubblePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];

            var x = context.XAxis!.Map(i);
            var y = context.YAxis!.Map(item.Value);
            var raw = item.Value;
            var t = (raw - minVal) / range;
            var radius = radiusMin + t * (radiusMax - radiusMin);

            points.Add(new BubblePoint
            {
                Id = item.Id ?? $"bubble-point-{Guid.NewGuid()}",
                X = x,
                Y = y,
                Radius = radius,
                CategoryIndex = i,
                Value = item.Value,
                BubbleValue = raw
            });
        }

        return points;
    }
}

