using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents a layout engine for polar bar charts.
/// </summary>
internal static class PolarBarLayoutEngine
{
    /// <summary>
    /// Calculates the layout for polar bar chart segments based on the series data and chart context.
    /// </summary>
    /// <param name="serie">The polar series containing the data items to layout.</param>
    /// <param name="ctx">The chart context containing plot area dimensions and scale information.</param>
    /// <param name="serieIndex">The index of the current series being laid out (used for multi-series charts).</param>
    /// <param name="seriesCount">The total number of series in the chart (used for multi-series charts).</param>
    /// <returns>A read-only list of polar bar segments with calculated positions and angles, or an empty list if the series
    /// contains no items.</returns>
    public static IReadOnlyList<PolarBarSegment> Layout(
        PolarSerie serie,
        ChartContext ctx,
        int serieIndex,
        int seriesCount)
    {
        if (seriesCount == 0)
        {
            return [];
        }

        var items = serie.Items;
        var count = items.Count;

        if (items.Count == 0)
        {
            return []; 
        }

        var cx = ctx.PlotArea.X + ctx.PlotArea.Width / 2.0;
        var cy = ctx.PlotArea.Y + ctx.PlotArea.Height / 2.0;

        var maxRadius = Math.Min(ctx.PlotArea.Width, ctx.PlotArea.Height) / 2.0;
        var maxValue = ctx.PolarNiceMax <= 0 ? 1.0 : ctx.PolarNiceMax;

        var angleStep = (Math.PI * 2.0) / count;
        var totalBarWidth = angleStep * 0.6;
        var barWidth = totalBarWidth / seriesCount;
        var segments = new List<PolarBarSegment>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var value = item.Value;
            var centerAngle = i * angleStep;
            var serieOffset = -totalBarWidth / 2.0 + serieIndex * barWidth;
            var startAngle = centerAngle + serieOffset;
            var endAngle = startAngle + barWidth;
            var outerRadius = (value / maxValue) * maxRadius;

            segments.Add(new PolarBarSegment
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
