using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartPolarAxesSvgBuilder
{
    public static void Build(
        SvgBuilder svg,
        PolarAxesPayload payload,
        ChartThemeContext theme)
    {
        var group = svg.AddGroup()
            .WithStroke(theme.Theme.Palette.Axis.ToString())
            .WithStrokeWidth(theme.ComputedValues.FinalStrokeThickness);

        RenderRadialAxes(group, payload, payload.ChartType == Enums.PolarChartType.Radar);
        RenderConcentricGrid(group, payload, payload.Grid == Enums.RadarGridType.Circle || payload.ChartType == Enums.PolarChartType.Area);
        RenderPolygonGrid(group, payload, payload.Grid == Enums.RadarGridType.Polygon && payload.ChartType != Enums.PolarChartType.Area);
        RenderAngleLabels(group, payload, theme, payload.ChartType == Enums.PolarChartType.Radar);
        RenderRadiusLabels(group, payload, theme);

        group.Close();
    }

    private static void RenderRadialAxes(
        SvgGroupBuilder svg,
        PolarAxesPayload payload,
        bool when)
    {
        if (!when)
        {
            return;
        }

        foreach (var axis in payload.RadialAxes)
        {
            var path = svg.AddPath().WithFill("none");

            path.MoveTo(axis.Start.X, axis.Start.Y);
            path.LineTo(axis.End.X, axis.End.Y);
            path.Close();
        }
    }

    private static void RenderConcentricGrid(
        SvgGroupBuilder svg,
        PolarAxesPayload payload,
        bool when)
    {
        if (!when)
        {
            return;
        }

        foreach (var circle in payload.ConcentricGrid)
        {
            svg.AddCircle(circle.Center.X, circle.Center.Y, circle.Radius)
                .WithFill("none");
        }
    }

    private static void RenderPolygonGrid(
        SvgGroupBuilder svg,
        PolarAxesPayload payload,
        bool when)
    {
        if (!when)
        {
            return;
        }

        var axes = payload.RadialAxes;
        var levels = payload.ConcentricGrid;

        foreach (var level in levels)
        {
            var r = level.Radius;
            var cx = level.Center.X;
            var cy = level.Center.Y;

            var path = svg.AddPath().WithFill("none");
            var first = true;

            foreach (var axis in axes)
            {
                var angle = Math.Atan2(
                    axis.End.Y - axis.Start.Y,
                    axis.End.X - axis.Start.X
                );

                var x = cx + r * Math.Cos(angle);
                var y = cy + r * Math.Sin(angle);

                if (first)
                {
                    path.MoveTo(x, y);
                    first = false;
                }
                else
                {
                    path.LineTo(x, y);
                }
            }

            path.ClosePath();
            path.Close();
        }
    }

    private static void RenderAngleLabels(
        SvgGroupBuilder svg,
        PolarAxesPayload payload,
        ChartThemeContext context,
        bool when)
    {
        if (!when)
        {
            return;
        }

        var typo = context.Theme.Typography.Label;

        foreach (var label in payload.AngleLabels)
        {
            svg.AddText(label.Text)
                .WithX(label.Position.X)
                .WithY(label.Position.Y)
                .WithFontSize(label.FontSize)
                .WithTextAnchor(label.Anchor)
                .WithFill(context.Theme.Palette.Foreground.ToString())
                .WithRotation(label.Rotation, label.Position.X, label.Position.Y)
                .WithFontFamily(typo.FontFamily)
                .WithDominantBaseline("middle");
        }
    }

    private static void RenderRadiusLabels(
        SvgGroupBuilder svg,
        PolarAxesPayload payload,
        ChartThemeContext context)
    {
        var typo = context.Theme.Typography.Label;

        foreach (var label in payload.RadiusLabels)
        {
            svg.AddText(label.Text)
                .WithX(label.Position.X)
                .WithY(label.Position.Y)
                .WithFontSize(label.FontSize)
                .WithTextAnchor(label.Anchor)
                .WithFill(context.Theme.Palette.Foreground.ToString())
                .WithRotation(label.Rotation, label.Position.X, label.Position.Y)
                .WithFontFamily(typo.FontFamily)
                .WithDominantBaseline("middle");
        }
    }
}
