namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the comparer to compare two instance of <see cref="IPathBarItem"/> together.
/// </summary>
internal sealed class FileManagerEntryPathBarItemComparer
    : IComparer<IPathBarItem>
{
    /// <summary>
    /// Gets the default comparer.
    /// </summary>
    public static FileManagerEntryPathBarItemComparer Default { get; } = new();

    /// <inheritdoc />
    public int Compare(IPathBarItem? x, IPathBarItem? y)
    {
        if (x is null)
        {
            return y is null ? 0 : 1;
        }

        if (y is null)
        {
            return x is null ? 0 : -1;
        }

        return string.Compare(x.Id, y.Id, StringComparison.OrdinalIgnoreCase);
    }
}
