using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class HierarchyModelBuilder
{
    public static HierarchyLayoutNode Build(HierarchyNode root)
    {
        ArgumentNullException.ThrowIfNull(root);

        return BuildRecursive(root, null, 0);
    }

    private static HierarchyLayoutNode BuildRecursive(
        HierarchyNode source,
        HierarchyLayoutNode? parent,
        int depth)
    {
        var layout = new HierarchyLayoutNode
        {
            Source = source,
            Parent = parent,
            Depth = depth,
            Value = source.Value,
            Rect = ChartRect.Empty
        };

        foreach (var child in source.Children)
        {
            var layoutChild = BuildRecursive(child, layout, depth + 1);
            layout.Children.Add(layoutChild);
        }

        return layout;
    }
}
