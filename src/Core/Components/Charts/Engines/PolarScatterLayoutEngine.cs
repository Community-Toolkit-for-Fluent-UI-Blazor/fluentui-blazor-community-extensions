using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Charts.Series;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the layout engine for the polar scatter chart.
/// </summary>
internal static class PolarScatterLayoutEngine
{
    /// <summary>
    /// Computes the layout positions for polar scatter data points by transforming polar coordinates to Cartesian
    /// coordinates.
    /// </summary>
    /// <param name="serie">The polar series containing the data points to layout.</param>
    /// <param name="ctx">The chart context providing axis mappings and polar frame dimensions.</param>
    /// <param name="options">The chart options containing marker size and other configuration settings.</param>
    /// <returns>A read-only list of tuples where each tuple contains the item index, x coordinate, y coordinate, and marker
    /// size.</returns>
    public static IReadOnlyList<(int Index, double X, double Y, double Radius)> Layout(
        PolarSerie serie,
        ChartContext ctx,
        CO options)
    {
        var items = serie.Items;
        var count = items.Count;

        if (count == 0)
        {
            return [];
        }

        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);
        var points = new List<(int, double, double, double)>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var angle = ctx.XAxis!.Map(i);
            var t = ctx.YAxis!.Map(item.Value);
            var radius = t * maxRadius;
            var x = cx + radius * Math.Cos(angle);
            var y = cy - radius * Math.Sin(angle);

            var size = options.PolarScatter.MarkerSize ?? 4.0;

            points.Add((i, x, y, size));
        }

        return points;
    }
}
