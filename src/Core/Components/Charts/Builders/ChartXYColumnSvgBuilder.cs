using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartXYColumnSvgBuilder
{
    public static void Build(
       SvgBuilder svg,
       PayloadCollection<XYColumnPayloadCollection> payload,
       ChartThemeContext context)
    {
        if (payload.Payloads.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId($"xy-column-{payload.Id}");

        foreach(var column in payload.Payloads)
        {
            Build(group, column, context);
        }

        group.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="payload"></param>
    /// <param name="context"></param>
    private static void Build(SvgGroupBuilder group, XYColumnPayloadCollection payload, ChartThemeContext context)
    {
        var seriesCount = context.Theme.Palette.Series.Count;
        var strokeSeriesCount = context.Theme.Palette.StrokeSeries.Count;
        var serieIndex = payload.SerieIndex;
        var fillBase = seriesCount == 0 ? null : context.Theme.Palette.Series[serieIndex % seriesCount].ToString();
        var strokeBase = strokeSeriesCount == 0 ? null : context.Theme.Palette.StrokeSeries[serieIndex % strokeSeriesCount].ToString();
        var strokeWidthBase = context.ComputedValues.FinalStrokeThickness;

        foreach (var col in payload.Columns)
        {
            var style = col.GetStyleForState();
            var fill = fillBase ?? style.Fill;
            var stroke = strokeBase ?? style.Stroke;
            var strokeWidth = style.StrokeWidth ?? strokeWidthBase;
            var opacity = style.Opacity ?? 1.0;

            var rect = group.AddRect(col.X, col.Y, col.Width, col.Height)
                .WithAttribute("data-id", col.Id)
                .WithAttribute("data-value", col.Value.ToSvg())
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);

            ChartAnimationEngine<SvgRectBuilder>.Apply(rect, col, context.Theme.Strategies);

            rect.Close();
        }
    }
}
