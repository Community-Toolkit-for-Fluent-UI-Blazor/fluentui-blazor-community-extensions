using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts;

internal static class PolarStyleResolver
{
    public static (string fill, string stroke, double strokeWidth, double opacity)
        Resolve(ChartThemeContext context, int serieIndex, ChartItemPayloadBase payload)
    {
        var seriesCount = context.Theme.Palette.Series.Count;
        var strokeSeriesCount = context.Theme.Palette.StrokeSeries.Count;

        var fillBase = seriesCount == 0
            ? null
            : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();

        var strokeBase = strokeSeriesCount == 0
            ? null
            : context.Theme.Palette.StrokeSeries[serieIndex % strokeSeriesCount].ToString();

        var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;

        var style = payload.GetStyleForState();

        var fill = fillBase ?? style.Fill ?? "transparent";
        var stroke = strokeBase ?? style.Stroke ?? "transparent";
        var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
        var opacity = style.Opacity ?? 1.0;

        return (fill, stroke, strokeWidth, opacity);
    }
}
