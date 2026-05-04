using FluentUI.Blazor.Community.Components.Charts.Helpers;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Converts polar series data into line paths for polar area chart rendering.
/// </summary>
internal static class PolarAreaLayoutEngine
{
    /// <summary>
    /// Computes the layout for a polar series and generates the corresponding line path.
    /// </summary>
    /// <param name="serie">The polar series to layout.</param>
    /// <param name="ctx">The chart context.</param>
    /// <returns>A list of polar area segments representing the layout.</returns>
    public static IReadOnlyList<PolarAreaSegment> Layout(PolarSerie serie, ChartContext ctx)
    {
        var values = serie.Values.ToList();
        var count = values.Count;

        var (cx, cy, maxRadius) = PolarHelper.GetPolarFrame(ctx);
        var angleStep = 2 * Math.PI / count;
        var maxValue = ctx.PolarNiceMax;
        var segments = new List<PolarAreaSegment>(count);

        for (var i = 0; i < count; i++)
        {
            var value = values[i];
            var t = value / maxValue;
            var radius = t * maxRadius;
            var start = i * angleStep;
            var end = (i + 1) * angleStep;

            segments.Add(new PolarAreaSegment
            {
                StartAngle = start,
                EndAngle = end,
                Radius = radius,
                CenterX = cx,
                CenterY = cy,
                Index = i
            });
        }

        return segments;
    }
}
