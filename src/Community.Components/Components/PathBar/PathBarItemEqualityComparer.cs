using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

internal class PathBarItemEqualityComparer
    : IEqualityComparer<IPathBarItem>
{
    private PathBarItemEqualityComparer()
    { }

    public static PathBarItemEqualityComparer Default { get; } = new PathBarItemEqualityComparer();

    public bool Equals(IPathBarItem? x, IPathBarItem? y)
    {
        if(x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return x is null;
        }

        return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode([DisallowNull] IPathBarItem obj)
    {
        return obj.Id?.GetHashCode() ?? -1;
    }
}
