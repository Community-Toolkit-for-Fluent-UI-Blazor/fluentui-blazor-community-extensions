//using FluentUI.Blazor.Community.Components.Charts.Series;

//namespace FluentUI.Blazor.Community.Components.Charts.Builders;

/*internal static class TreemapModelBuilder
{
    public static TreemapModel Build(HierarchyNode root)
    {
        ArgumentNullException.ThrowIfNull(root);

        AssignParentAndDepth(root, null, 0);
        ComputeAggregatedValues(root);

        return new TreemapModel
        {
            Root = root
        };
    }

    private static void AssignParentAndDepth(HierarchyNode node, HierarchyNode? parent, int depth)
    {
        node.Parent = parent;
        node.Depth = depth;

        foreach (var child in node.Children)
        {
            AssignParentAndDepth(child, node, depth + 1);
        }
    }

    private static double ComputeAggregatedValues(HierarchyNode node)
    {
        if (node.Children.Count == 0)
        {
            return node.Value;
        }

        double sum = 0;
        foreach (var child in node.Children)
        {
            sum += ComputeAggregatedValues(child);
        }

        node.Value = sum;
        return sum;
    }
}

internal sealed class TreemapModel
{
    public required HierarchyNode Root { get; init; }
}
*/
