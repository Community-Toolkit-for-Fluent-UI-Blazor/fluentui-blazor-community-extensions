namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class HierarchyValueAggregator
{
    public static void Aggregate(HierarchyLayoutNode root)
    {
        Compute(root);
    }

    private static double Compute(HierarchyLayoutNode node)
    {
        if (node.Children.Count == 0)
        {
            return node.Value;
        }

        var sum = 0.0;

        foreach (var child in node.Children)
        {
            sum += Compute(child);
        }

        node.Value = sum;

        return sum;
    }
}
