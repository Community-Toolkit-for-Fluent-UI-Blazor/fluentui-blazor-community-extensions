using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class ChartHierarchySvgBuilder
{
    public static void Build(
        SvgBuilder svg,
        HierarchyPayload payload,
        ChartThemeContext context)
    {
        if (payload.Nodes.Count == 0)
        {
            return;
        }

        var group = svg.AddGroup()
                       .WithId($"hierarchy-{payload.Id}");

        var root = payload.Nodes[0];
        var cx = root.X + root.Width / 2.0;
        var cy = root.Y + root.Height / 2.0;
        var type = payload.Type;
        var renderLinks = type == ChartType.Tree ||
                          type == ChartType.RadialTree ||
                          type == ChartType.Dendrogram;

        if (renderLinks)
        {
            RenderLinks(group, payload, context, cx, cy);
        }

        foreach (var node in payload.Nodes)
        {
            RenderShape(group, node, context, cx, cy);
        }

        foreach (var node in payload.Nodes)
        {
            RenderLabel(group, node, context, cx, cy);
        }

        group.Close();
    }

    private static void RenderLinks(
        SvgGroupBuilder group,
        HierarchyPayload payload,
        ChartThemeContext context,
        double cx,
        double cy)
    {
        var stroke = context.Theme.Palette.Foreground.ToString();

        for (var i = 0; i < payload.Nodes.Count; i++)
        {
            var node = payload.Nodes[i];

            if (node.ParentIndex < 0)
            {
                continue;
            }

            var parent = payload.Nodes[node.ParentIndex];
            double x1, y1, x2, y2;

            if (node.OuterRadius > 0 || parent.OuterRadius > 0)
            {
                (x1, y1) = PolarPoint(cx, cy,
                    (parent.InnerRadius + parent.OuterRadius) * 0.5,
                    (parent.StartAngle + parent.EndAngle) * 0.5);

                (x2, y2) = PolarPoint(cx, cy,
                    (node.InnerRadius + node.OuterRadius) * 0.5,
                    (node.StartAngle + node.EndAngle) * 0.5);
            }
            else
            {
                x1 = parent.X + parent.Width / 2.0;
                y1 = parent.Y + parent.Height / 2.0;
                x2 = node.X + node.Width / 2.0;
                y2 = node.Y + node.Height / 2.0;
            }

            group.AddLine()
                 .WithX1(x1)
                 .WithY1(y1)
                 .WithX2(x2)
                 .WithY2(y2)
                 .WithStroke(stroke)
                 .WithStrokeWidth(1)
                 .Close();
        }
    }

    private static (double x, double y) PolarPoint(
        double cx, double cy, double radius, double angle)
    {
        return (
            cx + Math.Cos(angle) * radius,
            cy + Math.Sin(angle) * radius
        );
    }

    private static void RenderShape(
        SvgGroupBuilder group,
        HierarchyNodePayload node,
        ChartThemeContext context,
        double cx,
        double cy)
    {
        var state = node.GetStyleForState();
        var palette = context.Theme.Palette;
        var fill = state.Fill;
        var stroke = state.Stroke;

        if (node.IsLeaf)
        {
            var idx = node.SerieIndex;

            if (idx >= 0 && palette.SeriesAsString.Count > 0)
            {
                fill = palette.SeriesAsString[idx % palette.SeriesAsString.Count];
            }

            if (idx >= 0 && palette.StrokeSeriesAsString.Count > 0)
            {
                stroke = palette.StrokeSeriesAsString[idx % palette.StrokeSeriesAsString.Count];
            }
        }

        if (node.OuterRadius > 0 && node.EndAngle > node.StartAngle)
        {
            var (x1, y1) = PolarPoint(cx, cy, node.InnerRadius, node.StartAngle);
            var (x2, y2) = PolarPoint(cx, cy, node.OuterRadius, node.StartAngle);
            var (x3, y3) = PolarPoint(cx, cy, node.OuterRadius, node.EndAngle);
            var (x4, y4) = PolarPoint(cx, cy, node.InnerRadius, node.EndAngle);
            var largeArc = (node.EndAngle - node.StartAngle) > Math.PI;

            var g = group.AddGroup()
                         .WithAttribute("data-id", node.Id)
                         .WithPointerEvents("visiblePainted");

            var path = g.AddPath()
                .WithFill(fill)
                .WithStroke(stroke)
                .WithStrokeWidth(state.StrokeWidth)
                .WithOpacity(state.Opacity)
                .MoveTo(x1, y1)
                .LineTo(x2, y2)
                .ArcTo(node.OuterRadius, node.OuterRadius, 0, largeArc, false, x3, y3)
                .LineTo(x4, y4)
                .ArcTo(node.InnerRadius, node.InnerRadius, 0, largeArc, true, x1, y1)
                .ClosePath();

            ChartAnimationEngine<SvgGroupBuilder>.Apply(g, node, context.Theme.Strategies);

            path.Close();
            g.Close();
        }
        else
        {
            var rect = group.AddRect(node.X, node.Y, node.Width, node.Height)
                            .WithFill(fill)
                            .WithStroke(stroke)
                            .WithStrokeWidth(state.StrokeWidth)
                            .WithOpacity(state.Opacity);

            if (node.SerieIndex >= 0)
            {
                rect.WithAttribute("data-serie", node.SerieIndex);
            }

            if (node.GroupId is not null)
            {
                rect.WithAttribute("data-group", node.GroupId);
            }

            if (node.IsLeaf && node.Width > 40 && node.Height > 20)
            {
                rect.WithAttribute("data-id", node.Id);
                ChartAnimationEngine<SvgRectBuilder>.Apply(rect, node, context.Theme.Strategies);
            }

            rect.Close();
        }
    }

    private static void RenderLabel(
        SvgGroupBuilder group,
        HierarchyNodePayload node,
        ChartThemeContext context,
        double cx,
        double cy)
    {
        if (string.IsNullOrWhiteSpace(node.Label))
        {
            return;
        }

        var typo = context.Theme.Typography.Label;

        double x, y;
        SvgTextAnchor anchor;
        string baseline;

        if (node.OuterRadius > 0 && node.EndAngle > node.StartAngle)
        {
            var angle = (node.StartAngle + node.EndAngle) * 0.5;
            var radius = (node.InnerRadius + node.OuterRadius) * 0.5;

            (x, y) = PolarPoint(cx, cy, radius, angle);

            anchor = SvgTextAnchor.Middle;
            baseline = "middle";
        }
        else
        {
            if (node.Width == 0 && node.Height == 0)
            {
                x = node.X;
                y = node.Y;

                anchor = SvgTextAnchor.Start;
                baseline = "middle";

                var isDendrogram = node.OuterRadius == 0 && node.Width == 0 && node.Height == 0;
                var rotateLeaf = isDendrogram && node.IsLeaf;

                group.AddText(node.Label)
                                .WithX(x + 6)
                                .WithY(y)
                                .WithFill(context.ComputedValues.FinalTextColor.ToString())
                                .WithTextAnchor(anchor)
                                .WithDominantBaseline(baseline)
                                .WithFontSize(typo.FontSize)
                                .WithFontFamily(typo.FontFamily)
                                .WithRawTransform($"rotate(-90 {x.ToSvg()} {y.ToSvg()}) translate({(-typo.FontSize * 10).ToSvg(), 0})", rotateLeaf)
                                .Close();
                return;
            }
            else if ( node.Width < 40 || node.Height < 20)
            {
                return;
            }

            const int header = 32;

            if (!node.IsLeaf)
            {
                if (node.Height < header)
                {
                    return;
                }

                x = node.X + 4;
                y = node.Y + (header - typo.FontSize) / 2;
                anchor = SvgTextAnchor.Start;
                baseline = "hanging";
            }
            else
            {
                x = node.X + node.Width / 2.0;
                y = node.Y + node.Height / 2.0;
                anchor = SvgTextAnchor.Middle;
                baseline = "middle";
            }
        }

        var text = group.AddText(node.Label)
                        .WithX(x)
                        .WithY(y)
                        .WithFill(context.ComputedValues.FinalTextColor.ToString())
                        .WithTextAnchor(anchor)
                        .WithDominantBaseline(baseline)
                        .WithFontSize(typo.FontSize)
                        .WithFontFamily(typo.FontFamily);

        if (node.IsLeaf)
        {
            text.WithAttribute("data-id", node.Id + "-label");
        }

        text.Close();
    }
}
