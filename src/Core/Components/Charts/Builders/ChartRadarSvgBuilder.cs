using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for rendering radar chart SVG elements based on the provided payload and theme context.
/// </summary>
internal static class ChartRadarSvgBuilder
{
    /// <summary>
    /// Builds a radar chart visualization by rendering its area, line, and points within an SVG group.
    /// </summary>
    /// <param name="svg">SVG builder to add the radar chart elements to.</param>
    /// <param name="payload">Radar chart data and configuration.</param>
    /// <param name="context">Theme context for styling the chart elements.</param>
    public static void Build(
        SvgBuilder svg,
        RadarPayload payload,
        ChartThemeContext context)
    {
        var group = svg.AddGroup()
                       .WithId($"radar-{payload.Id}");

        RenderArea(group, payload, context);
        RenderLine(group, payload, context);
        RenderPoints(group, payload, context);

        group.Close();
    }

    /// <summary>
    /// Renders a line path on an SVG element for a radar chart using the provided payload and theme styling.
    /// </summary>
    /// <param name="svg">The SVG group builder to which the line path will be added.</param>
    /// <param name="payload">The radar payload containing the path points and series information.</param>
    /// <param name="theme">The theme context used to resolve stroke, width, and opacity styling.</param>
    private static void RenderLine(
         SvgGroupBuilder svg,
         RadarPayload payload,
         ChartThemeContext theme)
    {
        var (_, stroke, strokeWidth, opacity) = PolarStyleResolver.Resolve(theme, payload.SerieIndex, payload);
        var pts = payload.Path.Points;

        if (pts.Count == 0)
        {
            return;
        }

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.Path.Id)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(opacity)
            .WithFill("none");

        path.MoveTo(pts[0].X, pts[0].Y);

        for (var i = 1; i < pts.Count; i++)
        {
            path.LineTo(pts[i].X, pts[i].Y);
        }

        if (payload.Path.Closed)
        {
            path.LineTo(pts[0].X, pts[0].Y);
            path.ClosePath();
        }

        path.Close();
    }

    /// <summary>
    /// Renders the filled area of a radar chart series as a semi-transparent SVG path.
    /// </summary>
    /// <param name="svg">The SVG group builder to which the area path is added.</param>
    /// <param name="payload">The radar chart data containing path points and series information.</param>
    /// <param name="theme">The theme context used to resolve fill color and opacity.</param>
    private static void RenderArea(
        SvgGroupBuilder svg,
        RadarPayload payload,
        ChartThemeContext theme)
    {
        if (!payload.FillArea)
        {
            return;
        }

        var (fill, _, _, opacity) = PolarStyleResolver.Resolve(theme, payload.SerieIndex, payload);

        if (string.IsNullOrEmpty(fill))
        {
            return;
        }

        var pts = payload.Path.Points;

        if (pts.Count == 0)
        {
            return;
        }

        var path = svg.AddPath()
            .WithFill(fill)
            .WithOpacity(opacity * 0.4)
            .WithStroke("none");

        path.MoveTo(pts[0].X, pts[0].Y);

        for (var i = 1; i < pts.Count; i++)
        {
            path.LineTo(pts[i].X, pts[i].Y);
        }

        if (payload.Path.Closed)
        {
            path.LineTo(pts[0].X, pts[0].Y);
            path.ClosePath();
        }

        path.Close();
    }

    /// <summary>
    /// Renders circular data points for a radar chart series onto the SVG canvas.
    /// </summary>
    /// <param name="svg">The SVG group builder to which circle elements are added.</param>
    /// <param name="payload">The radar chart data containing points and series information to render.</param>
    /// <param name="theme">The theme context providing color palette and stroke styling information.</param>
    private static void RenderPoints(
        SvgGroupBuilder svg,
        RadarPayload payload,
        ChartThemeContext theme)
    {
        var serieIndex = payload.SerieIndex;
        var seriesCount = theme.Theme.Palette.Series.Count;
        var strokeSeriesCount = theme.Theme.Palette.StrokeSeries.Count;
        var fillBase = seriesCount == 0 ? null : theme.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var strokeBase = strokeSeriesCount == 0 ? null : theme.Theme.Palette.StrokeSeries[serieIndex % strokeSeriesCount].ToString();
        var strokeWidthBase = theme.ComputedValues.FinalStrokeThickness;

        foreach (var pt in payload.Points)
        {
            var style = pt.GetStyleForState();
            var fill = fillBase ?? style.Fill ?? "transparent";
            var stroke = strokeBase ?? style.Stroke ?? "transparent";
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            svg.AddCircle(pt.X, pt.Y, 3)
                .WithAttribute("data-id", pt.Id)
                .WithAttribute("data-category", pt.CategoryIndex)
                .WithAttribute("data-value", pt.Value.ToSvg())
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);
        }
    }
}
