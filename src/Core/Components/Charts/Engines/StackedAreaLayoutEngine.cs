using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Provides layout calculations for stacked area charts.
/// </summary>
internal static class StackedAreaLayoutEngine
{
    /// <summary>
    /// Computes the layout geometry for a category line series, generating path and point data with optional stacking
    /// support.
    /// </summary>
    /// <param name="serie">The category line series to layout.</param>
    /// <param name="allSeries">Collection of all series in the chart, used for calculating stacked values.</param>
    /// <param name="serieIndex">Zero-based index of the serie within the allSeries collection.</param>
    /// <param name="context">Chart context containing axis mapping information.</param>
    /// <returns>A tuple containing the generated line path and a read-only list of positioned line points.</returns>
    public static (LinePath TopPath, LinePath BottomPath, IReadOnlyList<LinePoint> Points) Layout(
        CategoryLineSerie serie,
        IReadOnlyList<CategoryLineSerie> allSeries,
        int serieIndex,
        ChartContext context)
    {
        var items = CategoryAxisEngine.Sort(serie.Items, serie.Options);
        var count = items.Count;
        var topPoints = new List<ChartPoint>(count);
        var bottomPoints = new List<ChartPoint>(count);
        var pointPayloads = new List<LinePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var value = item.Value;
            var total = 0.0;

            if (serie.IsFull)
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

                if (serie.IsFull)
                {
                    prev = total == 0 ? 0 : prev / total;
                }

                bottomStack += prev;
            }

            var topStack = bottomStack + value;
            var x = context.XAxis!.Map(i);
            var yTop = context.YAxis!.Map(topStack);
            var yBottom = context.YAxis!.Map(bottomStack);

            topPoints.Add(new ChartPoint(x, yTop));
            bottomPoints.Add(new ChartPoint(x, yBottom));

            pointPayloads.Add(new LinePoint
            {
                Id = item.Id ?? $"stacked-point-{Guid.NewGuid()}",
                X = x,
                Y = yTop,
                CategoryIndex = i,
                Value = topStack
            });
        }

        var topPath = new LinePath
        {
            Id = serie.Id ?? $"stacked-area-top-{Guid.NewGuid()}",
            Points = topPoints,
            Smooth = serie.Options?.Smooth ?? false
        };

        var bottomPath = new LinePath
        {
            Id = serie.Id != null ? $"{serie.Id}-bottom" : $"stacked-area-bottom-{Guid.NewGuid()}",
            Points = bottomPoints,
            Smooth = serie.Options?.Smooth ?? false
        };

        return (topPath, bottomPath, pointPayloads);
    }
}
