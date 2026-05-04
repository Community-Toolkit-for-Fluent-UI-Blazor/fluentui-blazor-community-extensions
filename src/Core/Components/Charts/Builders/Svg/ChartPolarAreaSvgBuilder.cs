using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Builds SVG representations of polar area chart segments including filled areas, outline lines, and data points.
/// </summary>
internal static class ChartPolarAreaSvgBuilder
{
    /// <summary>
    /// Builds and renders a polar area chart segment to the SVG output.
    /// </summary>
    /// <param name="svg">The SVG builder used to construct the chart elements.</param>
    /// <param name="payloads">The data payload containing the points and configuration for the polar area segment.</param>
    /// <param name="theme">The theme context providing styling information for the chart.</param>
    public static void Build(
        SvgBuilder svg,
        PolarAreaPayloadCollection payloads,
        ChartThemeContext theme)
    {
        var group = svg.AddGroup()
                       .WithId($"polar-area-group");

        foreach (var payload in payloads.Areas)
        {
            BuildSingle(group, payload, theme);
        }

        group.Close();
    }

    private static void BuildSingle(
       SvgGroupBuilder svg,
       PolarAreaPayload payload,
       ChartThemeContext theme)
    {
        var serieGroup = svg.AddGroup()
                            .WithId($"polar-area-{payload.Id}");

        RenderSegments(serieGroup, payload, theme);

        serieGroup.Close();
    }

    private static void RenderSegments(
        SvgGroupBuilder svg,
        PolarAreaPayload payload,
        ChartThemeContext theme)
    {
        foreach (var seg in payload.Segments)
        {
            var (fill, stroke, strokeWidth, opacity) = PolarStyleResolver.Resolve(theme, seg.Index, seg);
            var cx = seg.CenterX;
            var cy = seg.CenterY;
            var r = seg.Radius;
            var x1 = cx + r * Math.Cos(seg.StartAngle);
            var y1 = cy - r * Math.Sin(seg.StartAngle);
            var x2 = cx + r * Math.Cos(seg.EndAngle);
            var y2 = cy - r * Math.Sin(seg.EndAngle);
            var largeArc = (seg.EndAngle - seg.StartAngle) > Math.PI;

            var group = svg.AddGroup()
                .WithAttribute("data-id", seg.Id)
                .WithAttribute("data-group", seg.GroupId)
                .WithPointerEvents("visiblePainted")
                .WithOpacity(opacity);

            var path = group.AddPath()
                .WithFill(fill)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);

            path.MoveTo(cx, cy);
            path.LineTo(x1, y1);
            path.ArcTo(r, r, 0, largeArc, false, x2, y2);
            path.ClosePath();
            path.Close();

            ChartAnimationEngine<SvgGroupBuilder>.Apply(group, seg, theme.Theme.Strategies);

            group.Close();
        }
    }
}
