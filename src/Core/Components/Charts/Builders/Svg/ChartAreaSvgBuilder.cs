using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Provides methods for constructing SVG elements that represent a line chart layer, including the line path and its
/// data points.
/// </summary>
/// <remarks>This static class is intended for internal use when rendering line chart visuals within the charting
/// components. It generates SVG markup based on the provided data and theme context, ensuring consistent styling and
/// structure for area chart layers.</remarks>
internal static class ChartAreaSvgBuilder
{
    /// <summary>
    /// Builds the SVG elements for an area layer based on the provided payload and theme context.
    /// </summary>
    /// <param name="svg">The SVG builder used to construct the SVG elements.</param>
    /// <param name="payload">The payload containing the data and styling information for the area layer.</param>
    /// <param name="context">The theme context providing styling information for the chart.</param>
    public static void Build(
        SvgBuilder svg,
        AreaPayload payload,
        ChartThemeContext context)
    {
        var group = svg.AddGroup()
                       .WithId($"area-{payload.Path.Id}");

        RenderArea(group, payload, context);
        RenderLinePath(group, payload, context);
        RenderPoints(group, payload, context);

        group.Close();
    }

    /// <summary>
    /// Renders an area chart region by creating a filled SVG path from the data points to the baseline.
    /// </summary>
    /// <remarks>The area is filled with the series color from the theme palette and rendered with a default
    /// opacity of 0.4 if not specified. The path traces the data points and closes back to the baseline.</remarks>
    /// <param name="svg">The SVG group builder to which the area path is added.</param>
    /// <param name="payload">The area chart data containing points, styling, baseline, and series information.</param>
    /// <param name="context">The chart theme context providing the color palette and animation strategies.</param>
    private static void RenderArea(
        SvgGroupBuilder svg,
        AreaPayload payload,
        ChartThemeContext context)
    {

        var points = payload.Path.Points;

        if (points.Count == 0)
        {
            return;
        }

        var style = payload.GetStyleForState();
        var serieIndex = payload.SerieIndex;
        var seriesCount = context.Theme.Palette.Series.Count;
        var fill = seriesCount == 0 ? style.Fill : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var opacity = style.Opacity ?? 0.4;

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.Path.Id + "-area")
            .WithFill(fill)
            .WithOpacity(opacity)
            .WithStroke("none");

        path.MoveTo(points[0].X, points[0].Y);

        for (var i = 1; i < points.Count; i++)
        {
            path.LineTo(points[i].X, points[i].Y);
        }

        path.LineTo(points[^1].X, payload.BaselineY);
        path.LineTo(points[0].X, payload.BaselineY);
        path.ClosePath();

        ChartAnimationEngine<SvgPathBuilder>.Apply(path, payload, context.Theme.Strategies);

        path.Close();
    }

    /// <summary>
    /// Renders a line path onto the specified SVG group using the style and points defined in the provided payload.
    /// </summary>
    /// <remarks>The method does not render anything if the path contains no points. The rendered path uses
    /// the stroke, stroke width, and opacity specified in the payload's style, and is not filled.</remarks>
    /// <param name="svg">The SVG group builder to which the line path will be added.</param>
    /// <param name="payload">The payload containing the path data and style information for rendering the line.</param>
    /// <param name="context">The theme context providing styling information for the chart.</param>
    private static void RenderLinePath(
        SvgGroupBuilder svg,
        AreaPayload payload,
        ChartThemeContext context)
    {
        var style = payload.GetStyleForState();
        var seriesCount = context.Theme.Palette.Series.Count;
        var serieIndex = payload.SerieIndex;
        var stroke = seriesCount == 0 ? style.Stroke : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var strokeWidth = context.ComputedValues.FinalStrokeThickness;
        strokeWidth = strokeWidth <= 0 ? style.StrokeWidth ?? 1.5 : strokeWidth;

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.Path.Id)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(style.Opacity ?? 1.0)
            .WithFill("none");

        var points = payload.Path.Points;

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
    /// Renders a collection of points as SVG circle elements within the specified SVG group builder.
    /// </summary>
    /// <remarks>Each point in the payload is rendered as a circle with attributes and styles derived from its
    /// state. The method applies data attributes for identification and categorization, as well as formatting the value
    /// using invariant culture.</remarks>
    /// <param name="svg">The SVG group builder to which the point circles will be added.</param>
    /// <param name="payload">The payload containing the collection of points and their associated data to render.</param>
    /// <param name="context">The theme context providing styling information for the chart, used to determine base colors and stroke widths.</param>
    private static void RenderPoints(
        SvgGroupBuilder svg,
        AreaPayload payload,
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
