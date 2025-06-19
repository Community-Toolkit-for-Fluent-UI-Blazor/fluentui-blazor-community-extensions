namespace FluentUI.Blazor.Community.Components;

internal sealed class FileManagerEntryComparer<TItem>
    : IComparer<FileManagerEntry<TItem>> where TItem : class, new()
{
    private FileSortBy _sortBy = FileSortBy.Name;
    private bool _isAsendingSort;

    private FileManagerEntryComparer() { }

    public static FileManagerEntryComparer<TItem> Default { get; } = new FileManagerEntryComparer<TItem>();

    public FileManagerEntryComparer<TItem> WithSortBy(FileSortBy sortBy)
    {
        _sortBy = sortBy;

        return this;
    }

    public FileManagerEntryComparer<TItem> WithSortMode(FileSortMode sortMode)
    {
        _isAsendingSort = sortMode == FileSortMode.Ascending;

        return this;
    }

    /// <inheritdoc />
    public int Compare(FileManagerEntry<TItem>? x, FileManagerEntry<TItem>? y)
    {
        if (x == null)
        {
            return y is null ? 0 : 1;
        }

        if (y == null)
        {
            return x is null ? 0 : -1;
        }

        static int Compare(string? left, string? right, bool ascending)
        {
            return ascending ? string.Compare(left, right, StringComparison.OrdinalIgnoreCase) : string.Compare(right, left, StringComparison.OrdinalIgnoreCase);
        }

        static int CompareSize(long left, long right, bool ascending)
        {
            return ascending ? left.CompareTo(right) : right.CompareTo(left);
        }

        static int CompareDate(DateTime left, DateTime right, bool ascending)
        {
            return ascending ? left.CompareTo(right) : right.CompareTo(left);
        }

        static int CompareExtensionThenName(FileManagerEntry<TItem> x, FileManagerEntry<TItem> y, bool ascending)
        {
            var compareResult = Compare(x.Extension, y.Extension, ascending);

            if (compareResult != 0)
            {
                return compareResult;
            }

            return Compare(x.NameWithoutExtension, y.NameWithoutExtension, ascending);
        }

        static int CompareNameThenExtension(FileManagerEntry<TItem> x, FileManagerEntry<TItem> y, bool ascending)
        {
            var compareResult = Compare(x.NameWithoutExtension, y.NameWithoutExtension, ascending);

            if (compareResult != 0)
            {
                return compareResult;
            }

            return Compare(x.Extension, y.Extension, ascending);
        }

        return _sortBy switch
        {
            FileSortBy.Name => CompareNameThenExtension(x, y, _isAsendingSort),
            FileSortBy.Extension => CompareExtensionThenName(x, y, _isAsendingSort),
            FileSortBy.Size => CompareSize(x.Size, y.Size, _isAsendingSort),
            FileSortBy.CreatedDate => CompareDate(x.CreatedDate, y.CreatedDate, _isAsendingSort),
            FileSortBy.ModifiedDate => CompareDate(x.ModifiedDate, y.ModifiedDate, _isAsendingSort),
            _ => CompareNameThenExtension(x, y, _isAsendingSort)
        };
    }
}

