using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/// <summary>
/// Represents a builder for generating SVG elements for histogram charts.
/// </summary>
internal static class ChartHistogramSvgBuilder
{
    /// <summary>
    /// Builds histogram bars in the SVG from the provided payload collection.
    /// </summary>
    /// <param name="svg">The SVG builder to add histogram elements to.</param>
    /// <param name="payloads">The collection of histogram payloads to render.</param>
    /// <param name="context">The chart theme context for styling.</param>
    public static void Build(
        SvgBuilder svg,
        HistogramPayloadCollection payloads,
        ChartThemeContext context)
    {
        if (payloads.Payloads.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId("xy-histogram");

        foreach (var payload in payloads.Payloads)
        {
            RenderBars(group, payload, context);
        }

        group.Close();
    }

    /// <summary>
    /// Renders histogram bars as SVG rectangles with theme-based styling and animations.
    /// </summary>
    /// <param name="group">The SVG group builder to contain the bar rectangles.</param>
    /// <param name="payload">The histogram data containing bars and series information.</param>
    /// <param name="context">The chart theme context providing palette colors and computed styling values.</param>
    private static void RenderBars(
        SvgGroupBuilder group,
        HistogramPayload payload,
        ChartThemeContext context)
    {
        var seriesCount = context.Theme.Palette.Series.Count;
        var strokeSeriesCount = context.Theme.Palette.StrokeSeries.Count;

        foreach (var bar in payload.Bars)
        {
            var fillBase = seriesCount == 0 ? null : context.Theme.Palette.Series[bar.Index % seriesCount].ToString();
            var strokeBase = strokeSeriesCount == 0 ? null : context.Theme.Palette.StrokeSeries[bar.Index % strokeSeriesCount].ToString();
            var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;
            var style = bar.GetStyleForState();
            var fill = fillBase ?? style.Fill;
            var stroke = strokeBase ?? style.Stroke;
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            var rect = group.AddRect(bar.X, bar.Y, bar.Width, bar.Height)
                .WithAttribute("data-id", bar.Id)
                .WithAttribute("data-count", bar.Count.ToSvg())
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);

            ChartAnimationEngine<SvgRectBuilder>.Apply(rect, bar, context.Theme.Strategies);

            rect.Close();
        }
    }
}
