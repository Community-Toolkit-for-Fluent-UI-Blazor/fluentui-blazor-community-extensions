using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents a layout engine for polar charts.
/// </summary>
internal static class PolarLayoutEngine
{
    /// <summary>
    /// Calculates the Cartesian coordinates for all data points in a polar chart series.
    /// </summary>
    /// <param name="serie">The polar chart series containing the data points to layout.</param>
    /// <param name="ctx">The chart context containing axis mappings and plot area dimensions.</param>
    /// <returns>A read-only list of polar points with calculated Cartesian coordinates and metadata.</returns>
    public static IReadOnlyList<PolarPoint> Layout(
        PolarSerie serie,
        ChartContext ctx)
    {
        var items = serie.Items;
        var count = items.Count;

        var (cx, cy, maxRadius) = GetPolarFrame(ctx);

        var points = new List<PolarPoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];

            var angle = ctx.XAxis!.Map(i);
            var t = ctx.YAxis!.Map(item.Value);
            var radius = t * maxRadius;

            var x = cx + radius * Math.Cos(angle);
            var y = cy - radius * Math.Sin(angle);

            points.Add(new PolarPoint
            {
                Id = item.Id ?? $"polar-{Guid.NewGuid()}",
                X = x,
                Y = y,
                Angle = angle,
                Radius = radius,
                CategoryIndex = i,
                Value = item.Value
            });
        }

        return points;
    }

    private static (double cx, double cy, double maxRadius) GetPolarFrame(ChartContext ctx)
    {
        var plot = ctx.PlotArea;
        var cx = plot.X + plot.Width / 2;
        var cy = plot.Y + plot.Height / 2;
        var maxRadius = Math.Min(plot.Width, plot.Height) / 2;

        return (cx, cy, maxRadius);
    }
}
