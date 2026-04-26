using FluentUI.Blazor.Community.Components.Charts.Drawing;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class ScatterLayoutEngine
{
    /// <summary>
    /// Layout the scatter points based on the provided series and chart context.
    /// </summary>
    /// <param name="serie">The line series containing the data points to be laid out.</param>
    /// <param name="context">The chart context providing the mapping functions for the axes.</param>
    /// <param name="options">The chart options that may influence the layout of the scatter points.</param>
    /// <returns>A read-only list of line points representing the scatter points.</returns>
    public static IReadOnlyList<LinePoint> Layout(
        Series.LineSerie serie,
        ChartContext context,
        CO options)
    {
        var serieOptions = options.DefaultScatterOptions;
        var items = CategoryAxisEngine.Sort(serie.Items, serieOptions);
        var count = items.Count;

        var points = new List<LinePoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var x = context.XAxis!.Map(i);
            var y = context.YAxis!.Map(item.Value);

            points.Add(new LinePoint
            {
                Id = item.Id ?? $"scatter-point-{Guid.NewGuid()}",
                X = x,
                Y = y,
                CategoryIndex = i,
                Value = item.Value
            });
        }

        return points;
    }
}
