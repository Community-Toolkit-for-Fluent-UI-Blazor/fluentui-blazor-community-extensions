namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Compares two <see cref="FileEntry{TItem}"/> instances according to the specified sort options.
/// </summary>
/// <param name="state">The file manager state containing the sort options.</param>
internal sealed class FileEntrySortComparer<TItem>(FileManagerState state) : IComparer<FileEntry<TItem>>
    where TItem : class, new()
{
    /// <inheritdoc />
    public int Compare(FileEntry<TItem>? x, FileEntry<TItem>? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        if (state.SortLayout == FileSortLayout.Folders)
        {
            if (x.IsDirectory && !y.IsDirectory)
            {
                return -1;
            }

            if (!x.IsDirectory && y.IsDirectory)
            {
                return 1;
            }
        }
        else if (state.SortLayout == FileSortLayout.Files)
        {
            if (x.IsDirectory && !y.IsDirectory)
            {
                return 1;
            }

            if (!x.IsDirectory && y.IsDirectory)
            {
                return -1;
            }
        }

        var result = state.SortBy switch
        {
            FileSortBy.Name => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase),
            FileSortBy.Extension => string.Compare(x.Extension, y.Extension, StringComparison.OrdinalIgnoreCase),
            FileSortBy.Size => x.Size.CompareTo(y.Size),
            FileSortBy.CreatedDate => x.CreatedDate.CompareTo(y.CreatedDate),
            FileSortBy.ModifiedDate => x.ModifiedDate.CompareTo(y.ModifiedDate),
            _ => 0
        };

        if (state.SortMode == FileSortMode.Descending)
        {
            result = -result;
        }

        return result;
    }
}

