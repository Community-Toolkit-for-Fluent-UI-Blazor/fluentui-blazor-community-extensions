using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for rendering polar bubble charts in SVG format.
/// </summary>
internal static class PolarBubbleSvgBuilder
{
    /// <summary>
    /// Builds the SVG representation of a polar bubble chart based on the provided payload and context.
    /// </summary>
    /// <param name="svg">The SVG builder used to construct the chart.</param>
    /// <param name="payload">The collection of polar bubble payloads containing the data points to render.</param>
    /// <param name="context">The chart theme context providing styling and configuration information.</param>
    public static void Build(
       SvgBuilder svg,
       PolarBubblePayloadCollection payload,
       ChartThemeContext context)
    {
        if (payload.Bubbles.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId($"bubble-{payload.Id}");

        foreach (var bubble in payload.Bubbles)
        {
            RenderBubble(group, bubble, context);
        }

        group.Close();
    }

    /// <summary>
    /// Renders bubble elements for a polar chart by adding circles to the SVG group with appropriate styling and data
    /// attributes.
    /// </summary>
    /// <remarks>Each bubble is rendered as a circle with fill, stroke, and opacity determined by the theme
    /// palette and point state. Data attributes are added for identification and value tracking.</remarks>
    /// <param name="group">The SVG group builder to add the bubble circles to.</param>
    /// <param name="payload">The payload containing the points and data to render.</param>
    /// <param name="context">The chart theme context providing styling information and computed values.</param>
    private static void RenderBubble(
        SvgGroupBuilder group,
        PolarBubblePayload payload,
        ChartThemeContext context)
    {
        var seriesCount = context.Theme.Palette.Series.Count;
        var strokeSeriesCount = context.Theme.Palette.StrokeSeries.Count;

        foreach (var pt in payload.Points)
        {
            var serieIndex = pt.SerieIndex;
            var fillBase = seriesCount == 0 ? null : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
            var strokeBase = strokeSeriesCount == 0 ? null : context.Theme.Palette.StrokeSeries[serieIndex % strokeSeriesCount].ToString();
            var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;
            var style = pt.GetStyleForState();
            var fill = fillBase ?? style.Fill;
            var stroke = strokeBase ?? style.Stroke;
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            group.AddCircle(pt.X, pt.Y, pt.Radius)
                .WithAttribute("data-id", pt.Id)
                .WithAttribute("data-value", pt.Value.ToSvg())
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);
        }
    }
}
