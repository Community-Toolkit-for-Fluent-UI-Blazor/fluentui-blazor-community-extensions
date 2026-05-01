using FluentUI.Blazor.Community.Components.Charts.Series;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal sealed record XYColumnLayout
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Width { get; init; }
    public required double Height { get; init; }
    public required double Value { get; init; }
}

internal static class XYColumnLayoutEngine
{
    public static IReadOnlyList<XYColumnLayout> Layout(
        XYSerie serie,
        IReadOnlyList<XYSerie> allSeries,
        int serieIndex,
        ChartContext context,
        CO options)
    {
        if (context.XAxis == null || context.YAxis == null)
        {
            throw new InvalidOperationException("Both X and Y axes must be defined.");
        }

        var items = serie.Items;

        if (items.Count == 0)
        {
            return [];
        }

        var plot = context.PlotArea;
        var columnOptions = options.XYColumnSerie;

        var distinctX = allSeries
            .SelectMany(s => s.Items.Select(i => i.X))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var categoryCount = distinctX.Count;
        var seriesCount = allSeries.Count;
        var bandWidth = plot.Width / categoryCount;
        var rawColumnWidth = bandWidth / seriesCount;
        var columnWidth = rawColumnWidth * columnOptions.ColumnWidth;
        var columnOffset = (rawColumnWidth - columnWidth) / 2.0;

        var baseline = context.YAxis.Map(0);
        var layouts = new List<XYColumnLayout>(items.Count);

        foreach (var item in items)
        {
            var categoryIndex = distinctX.BinarySearch(item.X);

            if (categoryIndex < 0)
            {
                continue;
            }

            var xCenter =
                plot.X +
                categoryIndex * bandWidth +
                serieIndex * rawColumnWidth +
                columnOffset +
                columnWidth / 2.0;

            var y0 = baseline;
            var y1 = context.YAxis.Map(item.Y);
            var y = Math.Min(y0, y1);
            var height = Math.Abs(y1 - y0);

            layouts.Add(new XYColumnLayout
            {
                X = xCenter - columnWidth / 2.0,
                Y = y,
                Width = columnWidth,
                Height = height,
                Value = item.Value
            });
        }

        return layouts;
    }
}

