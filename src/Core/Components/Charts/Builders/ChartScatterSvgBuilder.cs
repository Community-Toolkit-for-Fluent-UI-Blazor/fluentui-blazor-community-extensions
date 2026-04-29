using System.Globalization;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartScatterSvgBuilder
{
    public static void Build(
       SvgBuilder svg,
       XYScatterPayloadCollection payload,
       ChartThemeContext context)
    {
        if (payload.Payloads.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId($"scatter");

        foreach (var item in payload.Payloads)
        {
            RenderScatter(group, item, context);
        }

        group.Close();
    }

    private static void RenderScatter(
        SvgGroupBuilder group,
        XYScatterPayload payload,
        ChartThemeContext context)
    {
        var serieIndex = payload.SerieIndex;
        var seriesCount = context.Theme.Palette.Series.Count;
        var strokeSeriesCount = context.Theme.Palette.StrokeSeries.Count;

        var fillBase = seriesCount == 0 ? null : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var strokeBase = strokeSeriesCount == 0 ? null : context.Theme.Palette.StrokeSeries[serieIndex % strokeSeriesCount].ToString();
        var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;

        foreach (var pt in payload.Points)
        {
            var style = pt.GetStyleForState();
            var fill = fillBase ?? style.Fill;
            var stroke = strokeBase ?? style.Stroke;
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            group.AddCircle(pt.X, pt.Y, pt.Radius)
                .WithAttribute("data-id", pt.Id)
                .WithAttribute("data-value", pt.Value.ToString("G", CultureInfo.InvariantCulture))
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);
        }
    }
}
