using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartPolarBarSvgBuilder
{
    public static void Build(
        SvgBuilder svg,
        PolarBarPayloadCollection payload,
        ChartThemeContext theme)
    {
        var group = svg.AddGroup()
                       .WithId($"polar-bar");

        foreach (var bar in payload.Bars)
        {
            RenderSingle(group, bar, theme);
        }

        group.Close();
    }

    private static void RenderSingle(
       SvgGroupBuilder svg,
       PolarBarPayload payload,
       ChartThemeContext theme)
    {
        var serieGroup = svg.AddGroup()
                            .WithId($"polar-bar-{payload.Id}");

        foreach (var bar in payload.Segments)
        {
            RenderBar(serieGroup, bar, theme);
        }

        serieGroup.Close();
    }

    private static void RenderBar(
        SvgGroupBuilder svg,
        PolarBarSegmentPayload bar,
        ChartThemeContext theme)
    {
        var (fill, stroke, strokeWidth, opacity) = PolarStyleResolver.Resolve(theme, bar.SerieIndex, bar);

        var cx = bar.CenterX;
        var cy = bar.CenterY;

        var r1 = bar.InnerRadius;
        var r2 = bar.OuterRadius;

        var x1 = cx + r1 * Math.Cos(bar.StartAngle);
        var y1 = cy - r1 * Math.Sin(bar.StartAngle);

        var x2 = cx + r2 * Math.Cos(bar.StartAngle);
        var y2 = cy - r2 * Math.Sin(bar.StartAngle);

        var x3 = cx + r2 * Math.Cos(bar.EndAngle);
        var y3 = cy - r2 * Math.Sin(bar.EndAngle);

        var x4 = cx + r1 * Math.Cos(bar.EndAngle);
        var y4 = cy - r1 * Math.Sin(bar.EndAngle);

        var largeArc = (bar.EndAngle - bar.StartAngle) > Math.PI;

        var group = svg.AddGroup()
            .WithAttribute("data-id", bar.Id)
            .WithAttribute("data-group", bar.GroupId)
            .WithPointerEvents("visiblePainted");

        group.AddPath()
            .WithFill(fill)
            .WithOpacity(opacity)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .MoveTo(x1, y1)
            .LineTo(x2, y2)
            .ArcTo(r2, r2, 0, largeArc, false, x3, y3)
            .LineTo(x4, y4)
            .ArcTo(r1, r1, 0, largeArc, true, x1, y1)
            .ClosePath()
            .Close();

        ChartAnimationEngine<SvgGroupBuilder>.Apply(group, bar, theme.Theme.Strategies);

        group.Close();
    }
}
