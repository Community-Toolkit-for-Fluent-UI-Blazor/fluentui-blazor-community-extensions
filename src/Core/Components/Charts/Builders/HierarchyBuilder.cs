using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts.Builders;

internal static class HierarchyBuilder
{
    public static HierarchyNode Build(
        IEnumerable<HierarchyItem> items,
        Func<HierarchyItem, string[]>? group)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));
        ArgumentNullException.ThrowIfNull(group, nameof(group));

        var root = new HierarchyNode
        {
            Name = "root",
            Value = 0,
        };

        var map = new Dictionary<string, HierarchyNode>();

        foreach (var item in items)
        {
            var levels = group(item);
            var current = root;
            var path = "";

            foreach (var level in levels)
            {
                path += "/" + level;

                if (!map.TryGetValue(path, out var node))
                {
                    node = new HierarchyNode
                    {
                        Name = level,
                        Value = 0,
                    };

                    current.Children.Add(node);
                    map[path] = node;
                }

                current = node;
            }

            current.Value += item.Value;
            current.SourceItem = item;
        }

        return root;
    }
}
