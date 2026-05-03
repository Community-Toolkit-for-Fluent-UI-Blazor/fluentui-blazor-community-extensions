using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;

internal static class SunburstLayoutEngine
{
    public static void Layout(HierarchyLayoutNode root, ChartRect plot)
    {
        if (root == null)
        {
            return;
        }

        ComputeValue(root);

        var total = root.Value;
        if (total <= 0)
        {
            total = 1;
        }

        var cx = plot.X + plot.Width / 2.0;
        var cy = plot.Y + plot.Height / 2.0;
        var maxRadius = Math.Min(plot.Width, plot.Height) / 2.0;
        var maxDepth = GetMaxDepth(root);
        var band = maxRadius / (maxDepth + 1);

        AssignAngles(root, 0, Math.PI * 2.0, total);
        AssignRadii(root, band, cx, cy);
    }

    private static double ComputeValue(HierarchyLayoutNode node)
    {
        if (node.Children.Count == 0)
        {
            if (node.Value <= 0)
            {
                node.Value = 1;
            }

            return node.Value;
        }

        var sum = 0.0;

        foreach (var c in node.Children)
        {
            sum += ComputeValue(c);
        }

        node.Value = sum;
        return sum;
    }

    private static void AssignAngles(
        HierarchyLayoutNode node,
        double start,
        double end,
        double total)
    {
        node.StartAngle = start;
        node.EndAngle = end;

        if (node.Children.Count == 0)
        {
            return;
        }

        var span = end - start;
        var cursor = start;

        foreach (var c in node.Children)
        {
            var ratio = c.Value / total;
            var childEnd = cursor + ratio * span;

            AssignAngles(c, cursor, childEnd, c.Value);

            cursor = childEnd;
        }
    }

    private static int GetMaxDepth(HierarchyLayoutNode node)
    {
        var max = node.Depth;

        foreach (var c in node.Children)
        {
            max = Math.Max(max, GetMaxDepth(c));
        }

        return max;
    }

    private static void AssignRadii(
        HierarchyLayoutNode node,
        double band,
        double cx,
        double cy)
    {
        node.InnerRadius = node.Depth * band;
        node.OuterRadius = (node.Depth + 1) * band;

        var midAngle = (node.StartAngle + node.EndAngle) * 0.5;
        var midRadius = (node.InnerRadius + node.OuterRadius) * 0.5;

        node.Rect = new ChartRect(
            cx + Math.Cos(midAngle) * midRadius,
            cy + Math.Sin(midAngle) * midRadius,
            0, 0
        );

        foreach (var c in node.Children)
        {
            AssignRadii(c, band, cx, cy);
        }
    }
}
