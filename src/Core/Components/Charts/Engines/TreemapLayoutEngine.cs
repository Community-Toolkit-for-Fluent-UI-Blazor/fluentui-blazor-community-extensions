using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class TreemapLayoutEngine
{
    private const double Padding = 4.0;
    private const double HeaderHeight = 18.0;

    public static IReadOnlyList<HierarchyLayoutNode> Layout(
        HierarchyLayoutNode root,
        ChartRect plotArea)
    {
        var result = new List<HierarchyLayoutNode>();
        LayoutChildren(root, plotArea, result, horizontal: true);

        return result;
    }

    private static void LayoutChildren(
        HierarchyLayoutNode parent,
        ChartRect rect,
        List<HierarchyLayoutNode> result,
        bool horizontal)
    {
        if (parent.Children.Count == 0)
        {
            return;
        }

        var childArea = parent.Parent == null ? rect : new ChartRect(
                rect.X,
                rect.Y + HeaderHeight,
                rect.Width,
                rect.Height - HeaderHeight);

        if (childArea.Width <= 0 || childArea.Height <= 0)
        {
            return;
        }

        parent.Children.Sort((a, b) => b.Value.CompareTo(a.Value));

        var childRects = Squarify(parent.Children, childArea, horizontal);

        for (var i = 0; i < parent.Children.Count; i++)
        {
            var node = parent.Children[i];
            var r = childRects[i];

            r = Pad(r, Padding);

            node.Rect = r;
            result.Add(node);

            LayoutChildren(node, r, result, !horizontal);
        }
    }

    private static List<ChartRect> Squarify(
        List<HierarchyLayoutNode> nodes,
        ChartRect rect,
        bool horizontalStart)
    {
        var result = new List<ChartRect>();
        var total = nodes.Sum(n => n.Value);
        var scale = rect.Width * rect.Height / total;

        var remaining = nodes
            .Select(n => (node: n, area: n.Value * scale))
            .ToList();

        var row = new List<(HierarchyLayoutNode node, double area)>();
        double rowArea = 0;

        var free = rect;
        var horizontal = horizontalStart;

        while (remaining.Count > 0)
        {
            var item = remaining[0];

            if (row.Count == 0)
            {
                row.Add(item);
                rowArea += item.area;
                remaining.RemoveAt(0);
                continue;
            }

            var side = horizontal ? free.Height : free.Width;
            var currentWorst = WorstAspect(row, side);
            var newWorst = WorstAspect(row.Append(item), side);

            if (newWorst <= currentWorst)
            {
                row.Add(item);
                rowArea += item.area;
                remaining.RemoveAt(0);
            }
            else
            {
                free = LayoutRow(row, rowArea, free, horizontal, result);
                row.Clear();
                rowArea = 0;
                horizontal = !horizontal;
            }
        }

        if (row.Count > 0)
        {
            LayoutRow(row, rowArea, free, horizontal, result);
        }

        return result;
    }

    private static ChartRect LayoutRow(
        List<(HierarchyLayoutNode node, double area)> row,
        double rowArea,
        ChartRect free,
        bool horizontal,
        List<ChartRect> result)
    {
        if (horizontal)
        {
            var rowHeight = rowArea / free.Width;
            var x = free.X;
            var y = free.Y;

            foreach (var (node, area) in row)
            {
                var w = area / rowHeight;
                result.Add(new ChartRect(x, y, w, rowHeight));
                x += w;
            }

            return new ChartRect(free.X, free.Y + rowHeight, free.Width, free.Height - rowHeight);
        }
        else
        {
            var rowWidth = rowArea / free.Height;
            var x = free.X;
            var y = free.Y;

            foreach (var (node, area) in row)
            {
                var h = area / rowWidth;
                result.Add(new ChartRect(x, y, rowWidth, h));
                y += h;
            }

            return new ChartRect(free.X + rowWidth, free.Y, free.Width - rowWidth, free.Height);
        }
    }

    private static double WorstAspect(
        IEnumerable<(HierarchyLayoutNode node, double area)> row,
        double side)
    {
        var worst = 0.0;

        foreach (var (_, area) in row)
        {
            var aspect = Math.Max((side * side) / area, area / (side * side));

            if (aspect > worst)
            {
                worst = aspect;
            }
        }

        return worst;
    }

    private static ChartRect Pad(ChartRect r, double p)
    {
        return new ChartRect(
            r.X + p,
            r.Y + p,
            Math.Max(0, r.Width - 2 * p),
            Math.Max(0, r.Height - 2 * p));
    }
}
