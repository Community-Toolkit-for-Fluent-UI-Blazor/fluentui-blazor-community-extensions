using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a comparer for the <see cref="ITreeViewItem"/> for the <see cref="FluentCxFileManager{TItem}"/> component.
/// </summary>
internal sealed class FileManagerEntryTreeViewItemComparer
    : IComparer<ITreeViewItem>
{
    /// <summary>
    /// Gets the default comparer.
    /// </summary>
    public static FileManagerEntryTreeViewItemComparer Default { get; } = new();

    /// <inheritdoc />
    public int Compare(ITreeViewItem? x, ITreeViewItem? y)
    {
        if (x is null)
        {
            return y is null ? 0 : 1;
        }

        if (y is null)
        {
            return x is null ? 0 : -1;
        }

        return x.Id.CompareTo(y.Id);
    }
}
