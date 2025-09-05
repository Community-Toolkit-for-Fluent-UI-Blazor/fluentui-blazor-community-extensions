using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a comparer for <see cref="IPathBarItem"/> instances.
/// </summary>
internal class PathBarItemEqualityComparer
    : IEqualityComparer<IPathBarItem>
{
    /// <summary>
    /// Prevents a default instance of the <see cref="PathBarItemEqualityComparer"/> class from being created.
    /// </summary>
    private PathBarItemEqualityComparer()
    { }

    /// <summary>
    /// Gets the default instance of the <see cref="PathBarItemEqualityComparer"/>.
    /// </summary>
    public static PathBarItemEqualityComparer Default { get; } = new PathBarItemEqualityComparer();

    /// <inheritdoc />
    public bool Equals(IPathBarItem? x, IPathBarItem? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return x is null;
        }

        return string.Equals(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] IPathBarItem obj)
    {
        return obj.Id?.GetHashCode() ?? -1;
    }
}
