namespace FluentUI.Blazor.Community.Components.Charts.Engines;

internal static class HierarchyLayoutFlattener
{
    public static List<HierarchyLayoutNode> Flatten(HierarchyLayoutNode root)
    {
        var list = new List<HierarchyLayoutNode>(128);
        FlattenRecursive(root, list);

        var indexMap = list.Select((n, i) => (node: n, index: i))
                           .ToDictionary(x => x.node, x => x.index);

        foreach (var n in list)
        {
            if (n.Parent == null)
            {
                n.Source.ParentIndex = -1;
            }
            else
            {
                n.Source.ParentIndex = indexMap[n.Parent];
            }
        }

        return list;
    }

    private static void FlattenRecursive(HierarchyLayoutNode node, List<HierarchyLayoutNode> list)
    {
        list.Add(node);

        foreach (var child in node.Children)
        {
            FlattenRecursive(child, list);
        }
    }
}
