namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the UI state of the FluentCxFileManager.
/// This state is shared across all views (List, Details, Mosaic…).
/// </summary>
public sealed class FileManagerState
{
    /// <summary>
    /// Specifies the current file sorting criterion.
    /// </summary>
    /// <remarks>This field determines how files are ordered, such as by name or other attributes, when
    /// displayed or processed.</remarks>
    private FileSortBy _sortBy = FileSortBy.Name;

    /// <summary>
    /// Specifies the current sort mode used for ordering files.
    /// </summary>
    private FileSortMode _sortMode = FileSortMode.Ascending;

    /// <summary>
    /// Specifies the current view mode of the file manager, such as List, Details, or Mosaic.
    /// </summary>
    private FileView _view = FileView.List;

    /// <summary>
    /// Specifies how folders are sorted in relation to files. The default is to show folders first, followed by files.
    /// </summary>
    private FileSortLayout _sortLayout = FileSortLayout.Folders;

    /// <summary>
    /// Raised when sorting options change.
    /// </summary>
    public event EventHandler? SortChanged;

    /// <summary>
    /// Raised when the view mode changes.
    /// </summary>
    public event EventHandler? ViewChanged;

    /// <summary>
    /// Raised when the folder sorting mode changes,
    ///  indicating that the order of folders and files in the display should be updated accordingly.
    /// </summary>
    public event EventHandler? SortLayoutChanged;

    /// <summary>
    /// Gets or sets the sort field.
    /// </summary>
    public FileSortBy SortBy
    {
        get => _sortBy;
        set
        {
            if (_sortBy != value)
            {
                _sortBy = value;
                SortChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the sort direction.
    /// </summary>
    public FileSortMode SortMode
    {
        get => _sortMode;
        set
        {
            if (_sortMode != value)
            {
                _sortMode = value;
                SortChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the current view mode.
    /// </summary>
    public FileView View
    {
        get => _view;
        set
        {
            if (_view != value)
            {
                _view = value;
                ViewChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets or sets the mode used for sorting folders in the file list.
    /// </summary>
    /// <remarks>Changing this property raises the FolderModeChanged event to notify listeners of the
    /// update.</remarks>
    public FileSortLayout SortLayout
    {
        get => _sortLayout;
        set
        {
            if (_sortLayout != value)
            {
                _sortLayout = value;
                SortLayoutChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
