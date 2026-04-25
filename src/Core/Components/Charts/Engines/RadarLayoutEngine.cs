using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Converts polar series data into line paths for radar chart rendering.
/// </summary>
internal static class RadarLayoutEngine
{
    /// <summary>
    /// Computes the layout for a polar series and generates the corresponding line path.
    /// </summary>
    /// <param name="serie">The polar series to layout.</param>
    /// <param name="ctx">The chart context.</param>
    /// <returns>A tuple containing the generated line path and the computed polar points.</returns>
    public static (PolarPathPayload path, IReadOnlyList<PolarPoint> points) Layout(
        PolarSerie serie,
        ChartContext ctx)
    {
        var points = PolarLayoutEngine.Layout(serie, ctx);

        var path = new PolarPathPayload
        {
            Id = serie.Id + "-radar-path",
            Points = points,
            Closed = true
        };

        return (path, points);
    }
}
