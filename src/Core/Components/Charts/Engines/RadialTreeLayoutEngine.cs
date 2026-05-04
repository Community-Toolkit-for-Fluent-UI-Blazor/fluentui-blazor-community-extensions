using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;

internal static class RadialTreeLayoutEngine
{
    public static void Layout(HierarchyLayoutNode root, ChartRect plot)
    {
        if (root == null)
        {
            return;
        }

        var leafCounter = 0.0;
        ComputeX(root, ref leafCounter);

        var minX = GetMinX(root);
        var maxX = GetMaxX(root);
        var span = Math.Max(1e-9, maxX - minX);
        var cx = plot.X + plot.Width / 2.0;
        var cy = plot.Y + plot.Height / 2.0;
        var maxRadius = Math.Min(plot.Width, plot.Height) / 2.0;
        var maxDepth = GetMaxDepth(root);
        var bandRadius = maxRadius / (maxDepth + 1);

        ApplyLayout(root, cx, cy, minX, span, bandRadius);
    }

    private static double ComputeX(HierarchyLayoutNode node, ref double leafCounter)
    {
        if (node.Children.Count == 0)
        {
            node.StartAngle = leafCounter;
            leafCounter++;
            return node.StartAngle;
        }

        double sum = 0;

        foreach (var child in node.Children)
        {
            sum += ComputeX(child, ref leafCounter);
        }

        var mid = sum / node.Children.Count;
        node.StartAngle = mid;

        return mid;
    }

    private static double GetMinX(HierarchyLayoutNode node)
    {
        var x = node.StartAngle;

        foreach (var c in node.Children)
        {
            x = Math.Min(x, GetMinX(c));
        }

        return x;
    }

    private static double GetMaxX(HierarchyLayoutNode node)
    {
        var x = node.StartAngle;

        foreach (var c in node.Children)
        {
            x = Math.Max(x, GetMaxX(c));
        }

        return x;
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

    private static void ApplyLayout(
        HierarchyLayoutNode node,
        double cx,
        double cy,
        double minX,
        double span,
        double bandRadius)
    {
        var normX = (node.StartAngle - minX) / span;
        var angle = normX * Math.PI * 2.0;
        var radius = node.Depth * bandRadius;

        node.InnerRadius = radius;
        node.OuterRadius = radius;
        node.EndAngle = angle;

        node.Rect = new ChartRect(
            cx + Math.Cos(angle) * radius,
            cy + Math.Sin(angle) * radius,
            0,
            0
        );

        foreach (var child in node.Children)
        {
            ApplyLayout(child, cx, cy, minX, span, bandRadius);
        }
    }
}
