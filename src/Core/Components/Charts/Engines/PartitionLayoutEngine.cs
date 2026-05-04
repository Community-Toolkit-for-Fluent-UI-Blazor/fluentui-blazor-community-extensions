using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class PartitionLayoutEngine
{
    public static void Layout(HierarchyLayoutNode root, ChartRect plot)
    {
        if (root == null)
        {
            return;
        }

        var maxDepth = GetMaxDepth(root);
        var bandHeight = plot.Height / maxDepth;

        LayoutChildren(
            children: root.Children,
            depth: 1,
            maxDepth: maxDepth,
            x: plot.X,
            y: plot.Y,
            width: plot.Width,
            bandHeight: bandHeight);
    }

    private static void LayoutChildren(
        List<HierarchyLayoutNode> children,
        int depth,
        int maxDepth,
        double x,
        double y,
        double width,
        double bandHeight)
    {
        if (children.Count == 0)
        {
            return;
        }

        var total = children.Sum(c => c.Value);

        if (total <= 0)
        {
            return;
        }

        var cursor = x;
        var childY = y + (depth - 1) * bandHeight;
        var childHeight = bandHeight;

        foreach (var child in children)
        {
            var ratio = child.Value / total;
            var childWidth = width * ratio;

            child.Rect = new ChartRect(cursor, childY, childWidth, childHeight);

            LayoutChildren(
                children: child.Children,
                depth: depth + 1,
                maxDepth: maxDepth,
                x: cursor,
                y: y,
                width: childWidth,
                bandHeight: bandHeight);

            cursor += childWidth;
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
