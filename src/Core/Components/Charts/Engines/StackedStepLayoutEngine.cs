using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class StackedStepLayoutEngine
{
    public static (LinePath TopPath, LinePath BottomPath, IReadOnlyList<LinePoint> Points) Layout(
        Series.LineSerie serie,
        IReadOnlyList<Series.LineSerie> allSeries,
        int serieIndex,
        ChartContext context,
        CO options)
    {
        var serieOptions = options.CategoryLineOptions;
        var items = CategoryAxisEngine.Sort(serie.Items, serieOptions);
        var count = items.Count;
        var top = new List<ChartPoint>(count * 2 - 1);
        var bottom = new List<ChartPoint>(count * 2 - 1);
        var payloadPoints = new List<LinePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var value = item.Value;
            var total = 0.0;

            if (serie.LineType == LineChartType.Stacked100Step)
            {
                for (var s = 0; s < allSeries.Count; s++)
                {
                    total += allSeries[s].Items[i].Value;
                }

                value = total == 0 ? 0 : value / total;
            }

            var bottomStack = 0.0;

            for (var s = 0; s < serieIndex; s++)
            {
                var prev = allSeries[s].Items[i].Value;

                if (serie.LineType == LineChartType.Stacked100Step)
                {
                    prev = total == 0 ? 0 : prev / total;
                }

                bottomStack += prev;
            }

            var topStack = bottomStack + value;

            var x = context.XAxis!.Map(i);
            var yTop = context.YAxis!.Map(topStack);
            var yBottom = context.YAxis!.Map(bottomStack);

            top.Add(new ChartPoint(x, yTop));
            bottom.Add(new ChartPoint(x, yBottom));

            if (i < count - 1)
            {
                var xNext = context.XAxis!.Map(i + 1);
                top.Add(new ChartPoint(xNext, yTop));
                bottom.Add(new ChartPoint(xNext, yBottom));
            }

            payloadPoints.Add(new LinePoint
            {
                Id = item.Id ?? $"stacked-step-point-{Guid.NewGuid()}",
                X = x,
                Y = yTop,
                CategoryIndex = i,
                Value = topStack
            });
        }

        var topPath = new LinePath
        {
            Id = serie.Id ?? $"stacked-step-top-{Guid.NewGuid()}",
            Points = top,
            Smooth = false
        };

        var bottomPath = new LinePath
        {
            Id = serie.Id != null ? $"{serie.Id}-bottom-step" : $"stacked-step-bottom-{Guid.NewGuid()}",
            Points = bottom,
            Smooth = false
        };

        return (topPath, bottomPath, payloadPoints);
    }
}
