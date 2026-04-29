using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class PolarScatterSvgBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="svg"></param>
    /// <param name="payload"></param>
    /// <param name="context"></param>
    public static void Build(
       SvgBuilder svg,
       PolarScatterPayloadCollection payload,
       ChartThemeContext context)
    {
        if (payload.Scatters.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId($"scatter-{payload.Id}");

        foreach (var scatter in payload.Scatters)
        {
            RenderScatter(group, scatter, context);
        }

        group.Close();
    }

    private static void RenderScatter(
        SvgGroupBuilder group,
        PolarScatterPayload payload,
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
