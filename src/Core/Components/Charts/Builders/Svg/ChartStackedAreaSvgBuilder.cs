using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for creating SVG elements that visualize stacked area chart layers, including the filled area, top line, and data points.
/// </summary>
internal static class ChartStackedAreaSvgBuilder
{
    /// <summary>
    /// Builds a stacked area chart visualization by rendering the area, top line, and data points.
    /// </summary>
    /// <param name="svg">The SVG builder to which the stacked area elements will be added.</param>
    /// <param name="payload">The data payload containing the area path information and data points to render.</param>
    /// <param name="context">The chart theme context providing styling and theme information.</param>
    public static void Build(
        SvgBuilder svg,
        StackedAreaPayload payload,
        ChartThemeContext context)
    {
        var group = svg.AddGroup()
                       .WithId($"stacked-area-{payload.TopPath.Id}");

        RenderStackedArea(group, payload, context);
        RenderTopLine(group, payload, context);
        RenderPoints(group, payload, context);

        group.Close();
    }

    /// <summary>
    /// Renders a stacked area segment as an SVG path by connecting the top and bottom curves with appropriate fill and
    /// opacity.
    /// </summary>
    /// <param name="svg">The SVG group builder to which the path element is added.</param>
    /// <param name="payload">The stacked area data containing top and bottom path points, series index, and styling information.</param>
    /// <param name="context">The chart theme context providing palette colors and animation strategies.</param>
    private static void RenderStackedArea(
        SvgGroupBuilder svg,
        StackedAreaPayload payload,
        ChartThemeContext context)
    {
        var top = payload.TopPath.Points;
        var bottom = payload.BottomPath.Points;

        if (top.Count == 0 || bottom.Count == 0)
        {
            return;
        }

        var style = payload.GetStyleForState();
        var serieIndex = payload.SerieIndex;
        var seriesCount = context.Theme.Palette.Series.Count;

        var fill = seriesCount == 0
            ? style.Fill
            : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();

        var opacity = style.Opacity ?? 0.4;

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.Id + "-stacked-area")
            .WithFill(fill)
            .WithOpacity(opacity)
            .WithStroke("none");

        path.MoveTo(top[0].X, top[0].Y);

        for (var i = 1; i < top.Count; i++)
        {
            path.LineTo(top[i].X, top[i].Y);
        }

        for (var i = bottom.Count - 1; i >= 0; i--)
        {
            path.LineTo(bottom[i].X, bottom[i].Y);
        }

        path.ClosePath();

        ChartAnimationEngine<SvgPathBuilder>.Apply(path, payload, context.Theme.Strategies);

        path.Close();
    }

    /// <summary>
    /// Renders the top line of a stacked area chart series as an SVG path with theme-based styling and animation.
    /// </summary>
    /// <param name="svg">The SVG group builder to which the path element is added.</param>
    /// <param name="payload">The stacked area payload containing the top path points and series information.</param>
    /// <param name="context">The chart theme context providing palette, styling, and animation configuration.</param>
    private static void RenderTopLine(
        SvgGroupBuilder svg,
        StackedAreaPayload payload,
        ChartThemeContext context)
    {
        var style = payload.GetStyleForState();
        var seriesCount = context.Theme.Palette.Series.Count;
        var serieIndex = payload.SerieIndex;

        var stroke = seriesCount == 0
            ? style.Stroke
            : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();

        var strokeWidth = context.ComputedValues.FinalStrokeThickness;
        strokeWidth = strokeWidth <= 0 ? style.StrokeWidth ?? 1.5 : strokeWidth;

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.TopPath.Id)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(style.Opacity ?? 1.0)
            .WithFill("none");

        var points = payload.TopPath.Points;

        if (points.Count == 0)
        {
            return;
        }

        path.MoveTo(points[0].X, points[0].Y);

        for (var i = 1; i < points.Count; i++)
        {
            path.LineTo(points[i].X, points[i].Y);
        }

        ChartAnimationEngine<SvgPathBuilder>.Apply(path, payload, context.Theme.Strategies);

        path.Close();
    }

    /// <summary>
    /// Renders data points as SVG circles for a stacked area chart series.
    /// </summary>
    /// <param name="svg">The SVG group builder to add circle elements to.</param>
    /// <param name="payload">The stacked area payload containing the points and series information to render.</param>
    /// <param name="context">The chart theme context providing palette and styling information.</param>
    private static void RenderPoints(
        SvgGroupBuilder svg,
        StackedAreaPayload payload,
        ChartThemeContext context)
    {
        var serieIndex = payload.SerieIndex;
        var seriesCount = context.Theme.Palette.Series.Count;
        var seriesStrokeCount = context.Theme.Palette.StrokeSeries.Count;

        var fillBase = seriesCount == 0 ? null : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var strokeBase = seriesStrokeCount == 0 ? null : context.Theme.Palette.StrokeSeries[serieIndex % seriesStrokeCount].ToString();
        var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;

        foreach (var pt in payload.Points)
        {
            var style = pt.GetStyleForState();
            var fill = fillBase ?? style.Fill;
            var stroke = strokeBase ?? style.Stroke;
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            svg.AddCircle(pt.X, pt.Y, 3)
                .WithAttribute("data-id", pt.Id)
                .WithAttribute("data-category", pt.CategoryIndex)
                .WithAttribute("data-value", pt.Value.ToString("G", CultureInfo.InvariantCulture))
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);
        }
    }
}
