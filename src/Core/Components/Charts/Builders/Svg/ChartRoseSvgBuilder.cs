using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartRoseSvgBuilder
{
    public static void Build(
        SvgBuilder svg,
        RosePayloadCollection payload,
        ChartThemeContext theme)
    {
        var group = svg.AddGroup().WithId("rose-group");

        foreach (var payloadItem in payload.Roses)
        {
            BuildSingle(group, payloadItem, theme);
        }

        group.Close();
    }

    private static void BuildSingle(
        SvgGroupBuilder svg,
        RosePayload payload,
        ChartThemeContext theme)
    {
        var serieGroup = svg.AddGroup()
                            .WithId($"rose-{payload.Id}");

        foreach (var seg in payload.Segments)
        {
            RenderSegment(serieGroup, seg, theme);
        }

        serieGroup.Close();
    }

    private static void RenderSegment(
        SvgGroupBuilder svg,
        RoseSegmentPayload seg,
        ChartThemeContext theme)
    {
        var (fill, stroke, strokeWidth, opacity) =
            PolarStyleResolver.Resolve(theme, seg.Index, seg);

        var cx = seg.CenterX;
        var cy = seg.CenterY;

        var r = seg.OuterRadius;

        var x1 = cx + r * Math.Cos(seg.StartAngle);
        var y1 = cy - r * Math.Sin(seg.StartAngle);

        var x2 = cx + r * Math.Cos(seg.EndAngle);
        var y2 = cy - r * Math.Sin(seg.EndAngle);

        var g = svg.AddGroup()
            .WithAttribute("data-id", seg.Id)
            .WithAttribute("data-group", seg.GroupId)
            .WithPointerEvents("visiblePainted")
            .WithOpacity(opacity);

        g.AddPath()
            .WithFill(fill)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .MoveTo(cx, cy)
            .LineTo(x1, y1)
            .LineTo(x2, y2)
            .ClosePath()
            .Close();

        ChartAnimationEngine<SvgGroupBuilder>.Apply(g, seg, theme.Theme.Strategies);

        g.Close();
    }
}

