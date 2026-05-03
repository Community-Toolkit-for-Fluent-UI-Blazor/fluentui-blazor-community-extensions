using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class TreeLayoutEngine
{
    public static void Layout(HierarchyLayoutNode root, ChartRect plot)
    {
        if (root == null)
        {
            return;
        }

        var center = plot.X + plot.Width / 2.0;
        var maxDepth = GetMaxDepth(root);

        if (maxDepth == 0)
        {
            maxDepth = 1;
        }

        var bandHeight = plot.Height / maxDepth;

        ComputeSubtreeWidth(root);
        AssignX(root, center);
        AssignY(root, plot.Y, bandHeight);
    }

    private static double ComputeSubtreeWidth(HierarchyLayoutNode node)
    {
        if (node.Children.Count == 0)
        {
            node.SubtreeWidth = 1.0;
            return 1.0;
        }

        var width = 0.0;

        foreach (var c in node.Children)
        {
            width += ComputeSubtreeWidth(c);
        }

        node.SubtreeWidth = width;
        return width;
    }

    private static void AssignX(HierarchyLayoutNode node, double center)
    {
        node.Rect = new ChartRect(center, node.Rect.Y, 0, 0);

        if (node.Children.Count == 0)
        {
            return;
        }

        var total = node.SubtreeWidth;
        var start = center - total / 2.0;

        foreach (var c in node.Children)
        {
            var w = c.SubtreeWidth;
            var childCenter = start + w / 2.0;

            AssignX(c, childCenter);

            start += w;
        }
    }

    private static void AssignY(HierarchyLayoutNode node, double offsetY, double bandHeight)
    {
        var y = offsetY + node.Depth * bandHeight;
        node.Rect = new ChartRect(node.Rect.X, y, 0, 0);

        foreach (var c in node.Children)
        {
            AssignY(c, offsetY, bandHeight);
        }
    }

    private static int GetMaxDepth(HierarchyLayoutNode node)
    {
        if (node.Children.Count == 0)
        {
            return node.Depth;
        }

        return node.Children.Max(GetMaxDepth);
    }
}
