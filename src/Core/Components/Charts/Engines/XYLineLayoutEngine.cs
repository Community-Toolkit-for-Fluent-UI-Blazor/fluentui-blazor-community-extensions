using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class XYLineLayoutEngine
{
    /// <summary>
    /// Layouts the XY points for a given XYSerie based on the provided chart context and options.
    /// </summary>
    /// <param name="serie">The XY series containing the data points to be laid out.</param>
    /// <param name="context">The chart context providing the mapping functions for the axes.</param>
    /// <returns>A read-only list of XY points representing the XY points.</returns>
    public static IReadOnlyList<XYPoint> Layout(
        Series.XYSerie serie,
        ChartContext context)
    {
        var items = serie.Items;
        var count = items.Count;
        var bubbleValues = items.Select(i => i.Value).ToList();
        var minVal = bubbleValues.Min();
        var maxVal = bubbleValues.Max();
        var range = maxVal - minVal;

        if (range <= 0)
        {
            range = 1;
        }

        var points = new List<XYPoint>(count);

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var x = context.XAxis!.Map(item.X);
            var y = context.YAxis!.Map(item.Y);

            points.Add(new XYPoint
            {
                X = x,
                Y = y,
                Radius = 0,
                Value = item.Value,
                Index = i,
                RawX = item.X,
                RawY = item.Y,
            });
        }

        return points;
    }
}

