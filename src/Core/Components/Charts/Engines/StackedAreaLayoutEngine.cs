using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

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
    /// <param name="options">Chart options containing configuration settings.</param>
    /// <returns>A tuple containing the generated line path and a read-only list of positioned line points.</returns>
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
        var topPoints = new List<ChartPoint>(count);
        var bottomPoints = new List<ChartPoint>(count);
        var payloadPoints = new List<LinePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var rawValue = item.Value;
            var total = 0.0;

            if (serie.LineType == LineChartType.Stacked100Area)
            {
                for (var s = 0; s < allSeries.Count; s++)
                {
                    total += allSeries[s].Items[i].Value;
                }

                rawValue = total == 0 ? 0 : rawValue / total;
            }

            var bottomStack = 0.0;

            for (var s = 0; s < serieIndex; s++)
            {
                var prev = allSeries[s].Items[i].Value;

                if (serie.LineType == LineChartType.Stacked100Area)
                {
                    prev = total == 0 ? 0 : prev / total;
                }

                bottomStack += prev;
            }

            var topStack = bottomStack + rawValue;
            var x = context.XAxis!.Map(i);
            var yTop = context.YAxis!.Map(topStack);
            var yBottom = context.YAxis!.Map(bottomStack);

            topPoints.Add(new ChartPoint(x, yTop));
            bottomPoints.Add(new ChartPoint(x, yBottom));

            payloadPoints.Add(new LinePoint
            {
                Id = item.Id ?? $"stacked-area-point-{Guid.NewGuid()}",
                X = x,
                Y = yTop,
                CategoryIndex = i,
                Value = topStack
            });
        }

        return (
            new LinePath { Id = $"{serie.Id}-top", Points = topPoints, Smooth = false },
            new LinePath { Id = $"{serie.Id}-bottom", Points = bottomPoints, Smooth = false },
            payloadPoints
        );
    }
}
