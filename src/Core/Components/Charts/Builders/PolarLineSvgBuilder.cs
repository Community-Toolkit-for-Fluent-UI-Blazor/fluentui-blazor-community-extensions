using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartPolarLineSvgBuilder
{
    public static void Build(
        SvgBuilder svg,
        PolarLinePayloadCollection payload,
        ChartThemeContext theme)
    {
        var group = svg.AddGroup()
                      .WithId("polar-line-group");

        foreach (var line in payload.Lines)
        {
            BuildSingle(group, line, theme);
        }

        group.Close();
    }

    private static void BuildSingle(
        SvgGroupBuilder svg,
        PolarLinePayload payload,
        ChartThemeContext theme)
    {
        var serieGroup = svg.AddGroup()
                            .WithId($"polar-line-{payload.Id}");

        RenderLine(serieGroup, payload, theme);
        RenderPoints(serieGroup, payload, theme);

        serieGroup.Close();
    }

    private static void RenderLine(
        SvgGroupBuilder svg,
        PolarLinePayload payload,
        ChartThemeContext theme)
    {
        var (_, stroke, strokeWidth, opacity) = PolarStyleResolver.Resolve(theme, payload.SerieIndex, payload);
        var pts = payload.Path.Points;

        if (pts.Count == 0)
        {
            return;
        }

        var path = svg.AddPath()
            .WithAttribute("data-id", payload.Path.Id)
            .WithStroke(stroke)
            .WithStrokeWidth(strokeWidth)
            .WithOpacity(opacity)
            .WithFill("none");

        path.MoveTo(pts[0].X, pts[0].Y);

        for (var i = 1; i < pts.Count; i++)
        {
            path.LineTo(pts[i].X, pts[i].Y);
        }

        path.Close();
    }

    private static void RenderPoints(
        SvgGroupBuilder svg,
        PolarLinePayload payload,
        ChartThemeContext theme)
    {
        foreach (var pt in payload.Points)
        {
            var (fill, stroke, strokeWidth, opacity) = PolarStyleResolver.Resolve(theme, pt.SerieIndex, pt);

            svg.AddCircle(pt.X, pt.Y, 3)
                .WithAttribute("data-id", pt.Id)
                .WithAttribute("data-category", pt.CategoryIndex)
                .WithAttribute("data-value", pt.Value.ToSvg())
                .WithFill(fill)
                .WithOpacity(opacity)
                .WithStroke(stroke)
                .WithStrokeWidth(strokeWidth);
        }
    }
}
