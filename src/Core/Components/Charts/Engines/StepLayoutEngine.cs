using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class StepLayoutEngine
{
    public static (LinePath Path, IReadOnlyList<LinePoint> Points) Layout(
        Series.LineSerie serie,
        ChartContext context,
        CO options)
    {
        var items = CategoryAxisEngine.Sort(serie.Items, options.Step);
        var count = items.Count;

        var stepPoints = new List<ChartPoint>(count * 2 - 1);
        var payloadPoints = new List<LinePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var x = context.XAxis!.Map(i);
            var y = context.YAxis!.Map(item.Value);

            stepPoints.Add(new ChartPoint(x, y));

            if (i < count - 1)
            {
                var xNext = context.XAxis!.Map(i + 1);
                stepPoints.Add(new ChartPoint(xNext, y));
            }

            payloadPoints.Add(new LinePoint
            {
                Id = item.Id ?? $"step-point-{Guid.NewGuid()}",
                X = x,
                Y = y,
                CategoryIndex = i,
                Value = item.Value
            });
        }

        var path = new LinePath
        {
            Id = serie.Id ?? $"step-path-{Guid.NewGuid()}",
            Points = stepPoints,
            Smooth = false
        };

        return (path, payloadPoints);
    }
}

