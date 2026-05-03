using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class DendrogramLayoutEngine
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
        var maxDepth = GetMaxDepth(root);
        var bandHeight = plot.Height / (maxDepth + 1);

        ApplyLayout(root, plot, minX, span, bandHeight);
    }

    private static double ComputeX(HierarchyLayoutNode node, ref double leafCounter)
    {
        if (node.Children.Count == 0)
        {
            node.StartAngle = leafCounter;
            leafCounter += 1;
            return node.StartAngle;
        }

        var sum = 0.0;

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
        if (node.Children.Count == 0)
        {
            return node.Depth;
        }

        var max = node.Depth;

        foreach (var c in node.Children)
        {
            max = Math.Max(max, GetMaxDepth(c));
        }

        return max;
    }

    private static void ApplyLayout(
        HierarchyLayoutNode node,
        ChartRect plot,
        double minX,
        double span,
        double bandHeight)
    {
        var normX = (node.StartAngle - minX) / span;
        var x = plot.X + normX * plot.Width;
        var y = plot.Y + node.Depth * bandHeight;

        node.Rect = new ChartRect(x, y, 0, 0);

        node.EndAngle = 0;
        node.InnerRadius = 0;
        node.OuterRadius = 0;

        foreach (var child in node.Children)
        {
            ApplyLayout(child, plot, minX, span, bandHeight);
        }
    }
}
