using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Components.Charts.Engines;

internal static class RoseLayoutEngine
{
    public static IReadOnlyList<RoseSegment> Layout(PolarSerie serie, ChartContext ctx)
    {
        var items = serie.Items;
        var count = items.Count;

        if (count == 0)
        {
            return [];
        }

        var cx = ctx.PlotArea.X + ctx.PlotArea.Width / 2.0;
        var cy = ctx.PlotArea.Y + ctx.PlotArea.Height / 2.0;

        var maxRadius = Math.Min(ctx.PlotArea.Width, ctx.PlotArea.Height) / 2.0;
        var maxValue = items.Max(i => Math.Abs(i.Value));
        var angleStep = (Math.PI * 2.0) / count;
        var segments = new List<RoseSegment>(count);

        if (maxValue <= 0)
        {
            maxValue = 1;
        }

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var value = Math.Abs(item.Value);

            var startAngle = i * angleStep;
            var endAngle = startAngle + angleStep;
            var outerRadius = (value / maxValue) * maxRadius;

            segments.Add(new RoseSegment
            {
                Index = i,
                CenterX = cx,
                CenterY = cy,
                StartAngle = startAngle,
                EndAngle = endAngle,
                InnerRadius = 0,
                OuterRadius = outerRadius,
                Value = value,
                Category = item.Name
            });
        }

        return segments;
    }
}

