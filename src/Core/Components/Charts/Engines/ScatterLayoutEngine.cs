using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class ScatterLayoutEngine
{
    /// <summary>
    /// Layout the scatter points based on the provided series and chart context.
    /// </summary>
    /// <param name="serie">The XY series containing the data points to be laid out.</param>
    /// <param name="context">The chart context providing the mapping functions for the axes.</param>
    /// <param name="options">The chart options that may influence the layout of the scatter points.</param>
    /// <returns>A read-only list of XY points representing the scatter points.</returns>
    public static IReadOnlyList<XYPoint> Layout(
        Series.XYSerie serie,
        ChartContext context,
        CO options)
    {
        var serieOptions = options.Scatter;
        var items = serie.Items;
        var count = items.Count;

        var points = new List<XYPoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];

            points.Add(new XYPoint
            {
                X = context.XAxis!.Map(item.X),
                Y = context.YAxis!.Map(item.Y),
                RawX = item.X,
                RawY = item.Y,
                Index = i,
                Value = item.Value,
                Radius = serieOptions.Radius
            });
        }

        return points;
    }
}
