namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public sealed class FileManagerState
{
    /// <summary>
    /// Represents the way the files are sorted by.
    /// </summary>
    private FileSortBy _sortBy;

    /// <summary>
    /// Represents the ascending or descending sort.
    /// </summary>
    private FileSortMode _sortMode;

    /// <summary>
    /// Represents the current view of the file manager.
    /// </summary>
    private FileView _view;

    /// <summary>
    /// Gets or sets the event to raise when the sort is updated.
    /// </summary>
    public event EventHandler? OnSortUpdated;

    /// <summary>
    /// Gets or sets the event to raise when the view is updated.
    /// </summary>
    public event EventHandler? OnViewUpdated;

    /// <summary>
    /// Gets or sets the way the files are sorted.
    /// </summary>
    public FileSortBy SortBy
    {
        get => _sortBy;

        internal set
        {
            if (_sortBy != value)
            {
                _sortBy = value;
                OnSortUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets the sorted mode of the files (ascending or descending).
    /// </summary>
    public FileSortMode SortMode
    {
        get => _sortMode;
        internal set
        {
            if (value != _sortMode)
            {
                _sortMode = value;
                OnSortUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets the view of the file manager.
    /// </summary>
    public FileView View
    {
        get => _view;
        internal set
        {
            if (_view != value)
            {
                _view = value;
                OnViewUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
