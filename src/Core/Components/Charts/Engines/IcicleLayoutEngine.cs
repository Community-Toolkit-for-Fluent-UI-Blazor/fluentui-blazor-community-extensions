using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class IcicleLayoutEngine
{
    public static void Layout(HierarchyLayoutNode root, ChartRect plot)
    {
        if (root == null)
        {
            return;
        }

        ComputeValue(root);

        var maxDepth = GetMaxDepth(root);

        if (maxDepth <= 0)
        {
            return;
        }

        var bandWidth = plot.Width / maxDepth;

        LayoutChildren(
            children: root.Children,
            depth: 1,
            maxDepth: maxDepth,
            x: plot.X,
            y: plot.Y,
            totalHeight: plot.Height,
            bandWidth: bandWidth);
    }

    private static void LayoutChildren(
        List<HierarchyLayoutNode> children,
        int depth,
        int maxDepth,
        double x,
        double y,
        double totalHeight,
        double bandWidth)
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

        var childX = x + (depth - 1) * bandWidth;
        var childWidth = bandWidth;
        var cursor = y;

        foreach (var child in children)
        {
            var ratio = child.Value / total;
            var childHeight = totalHeight * ratio;

            child.Rect = new ChartRect(
                childX,
                cursor,
                childWidth,
                childHeight);

            LayoutChildren(
                children: child.Children,
                depth: depth + 1,
                maxDepth: maxDepth,
                x: x,
                y: cursor,
                totalHeight: childHeight,
                bandWidth: bandWidth);

            cursor += childHeight;
        }
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

    private static int GetMaxDepth(HierarchyLayoutNode node)
    {
        if (node.Children.Count == 0)
        {
            return node.Depth;
        }

        return node.Children.Max(GetMaxDepth);
    }
}
