using FluentUI.Blazor.Community.Components.Components.FileManager.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a file manager component for handling collections of items in a Fluent UI Blazor application.
/// </summary>
/// <typeparam name="TItem">The type of the items managed by the file manager. Must be a reference type.</typeparam>
public partial class FluentCxFileManager<TItem>
    : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Represents the menu used for sorting operations, or null if no sort menu is available.
    /// </summary>
    private FluentMenu? _sortMenu;

    /// <summary>
    /// Represents the menu instance used for sorting options on mobile devices.
    /// </summary>
    private FluentMenu? _mobileSortMenu;

    /// <summary>
    /// Represents the menu used for views options.
    /// </summary>
    private FluentMenu? _viewMenu;

    /// <summary>
    /// Represents the menu instance used for views options on mobile devices.
    /// </summary>
    private FluentMenu? _mobileViewMenu;

    /// <summary>
    /// Provides the core engine for managing file operations for items of type TItem.
    /// </summary>
    private FileManagerEngine<TItem> _engine = default!;

    /// <summary>
    /// Provides the tree engine used to manage and manipulate file manager items of type TItem.
    /// </summary>
    private FileManagerTreeEngine<TItem> _treeEngine = default!;

    /// <summary>
    /// Provides the internal engine used to manage the trail menu state and operations.
    /// </summary>
    private TrailMenuEngine<TItem> _trailEngine = default!;

    /// <summary>
    /// Value indicating whether the file manager shows detailed information about files and folders.
    /// </summary>
    private bool _showDetails;

    /// <summary>
    /// Value indicating whether the file manager is currently in a disabled state, preventing user interactions.
    /// </summary>
    private bool _isDisabled;

    /// <summary>
    /// Represents the current instance of the file manager view for the specified item type.
    /// </summary>
    private FileManager<TItem>? _fileManagerView;

    /// <summary>
    /// Represents the current file entry being processed or selected. May be null if no entry is active.
    /// </summary>
    private FileEntry<TItem>? _currentEntry;

    /// <summary>
    /// Represents the collection of currently selected file entries of type TItem.
    /// </summary>
    private readonly List<FileEntry<TItem>> _currentSelectedItems = [];

    /// <summary>
    /// Represents the root node of the tree structure, or null if the tree is empty.
    /// </summary>
    private TreeViewItem? _treeRoot;

    /// <summary>
    /// Represents the current search value entered by the user for filtering file entries.
    /// </summary>
    private string? _searchValue;

    /// <summary>
    /// Represents the current file entry being searched, or null if no search is in progress.
    /// </summary>
    private FileEntry<TItem>? _searchEntry;

    /// <summary>
    /// Represents the current progress state of file operations, such as uploading, downloading, deleting, moving...
    /// </summary>
    private FileManagerProgressState _progressState = FileManagerProgressState.None;

    /// <summary>
    /// Represents the render fragment used to generate the label content for a component.
    /// </summary>
    /// <remarks>The render fragment receives a string parameter that can be used to customize the label's
    /// display based on dynamic data or context.</remarks>
    private readonly RenderFragment<string> _renderLabel;

    /// <summary>
    /// Represents the render fragment used to display the view menu content.
    /// </summary>
    private readonly RenderFragment _renderViewMenu;

    /// <summary>
    /// Represents the render fragment used to display the sort menu content.
    /// </summary>
    private readonly RenderFragment _renderSortMenu;

    /// <summary>
    /// Represents the root trail menu item.
    /// </summary>
    private ITrailMenuItem? _rootTrail;

    /// <summary>
    /// Represents the current path within the file structure.
    /// </summary>
    private string? _path;

    /// <summary>
    /// Represents the cached flattened file entry for the current context. Used to optimize access to a single,
    /// potentially expanded, file entry.
    /// </summary>
    /// <remarks>This field is intended for internal use to improve performance when working with hierarchical
    /// file structures. It may be null if no flattened entry has been computed or cached.</remarks>
    private FileEntry<TItem>? _flattenEntry;

    /// <summary>
    /// Represents the currently selected node in the tree, or null if no node is selected.
    /// </summary>
    private ITreeViewItem? _selectedNode;

    /// <summary>
    /// Gets a value indicating whether the tree view is visible based on the current device breakpoint.
    /// </summary>
    /// <remarks>The tree view is visible on medium and larger device breakpoints, and hidden on extra small
    /// and small devices. This property can be used to conditionally render UI elements depending on the device
    /// size.</remarks>
    private bool IsTreeViewVisible
    {
        get
        {
            return Breakpoint switch
            {
                DeviceBreakpoint.Xs or DeviceBreakpoint.Sm => false,
                _ => true
            };
        }
    }

    /// <summary>
    /// Gets the current device breakpoint, indicating the responsive layout category for the device.
    /// </summary>
    /// <remarks>The breakpoint reflects the device's screen size or characteristics as determined by the
    /// associated device information. This property returns null if device information is unavailable.</remarks>
    private DeviceBreakpoint? Breakpoint => DeviceInfoState?.DeviceInfo?.Breakpoint;

    /// <summary>
    /// Gets or sets the current state of the file manager component.
    /// </summary>
    [Inject]
    internal FileManagerState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the service used to display dialogs within the component.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework and should not be set manually in
    /// most scenarios. Use this service to show modal dialogs or prompt the user for input as part of the component's
    /// UI interactions.</remarks>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the service used to download files within the component.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection and should not be set manually
    /// except for testing purposes.</remarks>
    [Inject]
    private IFileDownloader FileDownloader { get; set; } = default!;

    /// <summary>
    /// Gets or sets the width of the component, typically specified as a CSS length value.
    /// </summary>
    /// <remarks>Set this property to control the rendered width of the component. Accepts any valid CSS width
    /// value, such as "100px", "50%", or "auto". If not set, the component uses its default width.</remarks>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    /// <remarks>Specify the height using a valid CSS length value, such as "100px", "2em", or "50%". If not
    /// set, the component uses its default height.</remarks>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the options used to configure file search behavior.
    /// </summary>
    /// <remarks>Use this property to specify search parameters such as filters, sorting, or other criteria
    /// that affect how files are located and returned.</remarks>
    [Parameter]
    public FileSearchOptions SearchOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the function used to extract a metadata string from an item.
    /// </summary>
    /// <remarks>The provided function should return a string representation of the metadata for the specified
    /// item. If not set, the default implementation uses the item's ToString() method.</remarks>
    [Parameter]
    public Func<TItem, string> MetadataExtractor { get; set; } = t => t.ToString() ?? string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the button for creating a new folder is displayed.
    /// </summary>
    [Parameter]
    public bool ShowCreateFolderButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the upload button is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowUploadButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the view button is displayed.
    /// </summary>
    [Parameter]
    public bool ShowViewButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the sort button is displayed.
    /// </summary>
    [Parameter]
    public bool ShowSortButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the properties button is displayed.
    /// </summary>
    [Parameter]
    public bool ShowPropertiesButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the details button is displayed.
    /// </summary>
    [Parameter]
    public bool ShowDetailsButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the component is in a busy state.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to indicate that the component is performing a
    /// background operation or is otherwise unavailable for user interaction.</remarks>
    [Parameter]
    public bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the folder is opened automatically after it is created.
    /// </summary>
    [Parameter]
    public bool IsFolderOpenedAfterCreation { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of files that can be uploaded in a single operation.
    /// </summary>
    [Parameter]
    public int MaximumFileCount { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum allowed file size for uploads, specified in bytes. The default value is 100 MB (1024 * 1024 * 100).
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 1024 * 1024 * 100;

    /// <summary>
    /// Gets or sets the buffer size used for file upload streams, specified in bytes. The default value is 10 KB (1024 * 10).
    /// </summary>
    [Parameter]
    public uint BufferSize { get; set; } = 1024 * 10;

    /// <summary>
    /// Gets or sets the callback that is invoked when a request to create a new folder is made.
    /// </summary>
    [Parameter]
    public EventCallback OnFolderCreated { get; set; }

    /// <summary>
    /// Gets or sets the render fragment used to display additional toolbar items in the component's UI.
    /// </summary>
    [Parameter] public RenderFragment? ToolbarItems { get; set; }

    /// <summary>
    /// Gets or sets the view mode used to display the file structure, such as hierarchical or flat. The default value is
    /// </summary>
    [Parameter]
    public FileStructureView FileStructureView { get; set; } = FileStructureView.Hierarchical;

    /// <summary>
    /// Gets or sets the current progress percentage for file operations, such as uploading or downloading.
    /// </summary>
    private int? ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets the current device information state used by the component.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection and supplies information about
    /// the device's characteristics, such as screen size or capabilities, to the component. It enables responsive or
    /// adaptive behavior based on device context.</remarks>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when a request to create a new directory is made.
    /// </summary>
    /// <remarks>Use this event to handle directory creation logic in response to user actions, such as when a
    /// user initiates the creation of a new folder in the UI. The event provides details about the requested directory
    /// through the associated event arguments.</remarks>
    [Parameter]
    public EventCallback<CreateDirectoryEventArgs> OnCreateDirectoryRequested { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a delete request is made.
    /// </summary>
    /// <remarks>Use this event to handle delete operations initiated by the user, such as removing one or
    /// more entries. The event provides details about the entries to be deleted through the <see
    /// cref="DeleteEntriesEventArgs"/> parameter.</remarks>
    [Parameter]
    public EventCallback<DeleteEntriesEventArgs> OnDeleteRequested { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a delete action occurs.
    /// </summary>
    /// <remarks>Use this property to specify logic that should execute in response to a delete event, such as
    /// removing an item from a list or notifying the user. The callback is triggered when the component signals a
    /// delete operation.</remarks>
    [Parameter]
    public EventCallback OnDeleted { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a move operation is requested by the user.
    /// </summary>
    /// <remarks>Use this event to handle move requests, such as when entries are to be moved within a
    /// collection or between containers. The event provides details about the move operation through the
    /// MoveEntriesEventArgs parameter.</remarks>
    [Parameter]
    public EventCallback<MoveEntriesEventArgs> OnMoveRequested { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the component has been moved.
    /// </summary>
    [Parameter]
    public EventCallback OnMoved { get; set; }

    /// <summary>
    /// Gets the collection of currently selected file entries.
    /// </summary>
    /// <remarks>If no items are explicitly selected, the collection contains the current entry if it exists;
    /// otherwise, it is empty. This property is useful for scenarios where multiple or single selection is
    /// supported.</remarks>
    private IEnumerable<FileEntry<TItem>> SelectedItems =>
        _currentSelectedItems is null || _currentSelectedItems.Count == 0
            ? _currentEntry is null ? [] : new[] { _currentEntry }
            : _currentSelectedItems;

    /// <summary>
    /// Gets or sets the categories of files that are accepted for selection.
    /// </summary>
    /// <remarks>Use this property to restrict the types of files that can be selected by specifying one or
    /// more categories from the AcceptFileCategory enumeration. This helps ensure that users can only choose files that
    /// match the allowed categories.</remarks>
    [Parameter]
    public AcceptFileCategory AcceptCategories { get; set; } = AcceptFileCategory.None;

    /// <summary>
    /// Gets or sets the file extensions that are accepted for file selection.
    /// </summary>
    /// <remarks>Use this property to restrict the types of files that can be selected by the user. The value
    /// should be set to one or more file extensions defined in the AcceptFileExtension enumeration. By default, no
    /// restrictions are applied.</remarks>
    [Parameter]
    public AcceptFileExtension AcceptExtensions { get; set; } = AcceptFileExtension.None;

    /// <summary>
    /// Gets or sets the collection of custom file extensions that are accepted for file selection.
    /// </summary>
    /// <remarks>Specify one or more file extensions (such as ".jpg", ".png") to restrict the types of files
    /// that can be selected. If this property is null or empty, all file types are allowed.</remarks>
    [Parameter]
    public IEnumerable<string>? CustomAcceptExtensions { get; set; }

    /// <summary>
    /// Gets or sets the list of accepted file types for file input selection.
    /// </summary>
    /// <remarks>The value should be a comma-separated list of unique file type specifiers, such as MIME types
    /// or file extensions (e.g., ".jpg, .png, image/*"). This property is typically used to restrict the types of files
    /// that a user can select in a file upload control.</remarks>
    [Parameter]
    public string? Accept { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a rename operation is requested.
    /// </summary>
    /// <remarks>Use this event to handle rename requests initiated by the user. The event provides details
    /// about the entry to be renamed through the associated event arguments.</remarks>
    [Parameter]
    public EventCallback<RenameEntryEventArgs> OnRenameRequested { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a file entry is renamed.
    /// </summary>
    /// <remarks>Use this event to handle custom logic when a file entry's name changes, such as updating UI
    /// elements or persisting changes. The event provides the renamed file entry as its argument.</remarks>
    [Parameter]
    public EventCallback OnRenamed { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a file upload stream event occurs.
    /// </summary>
    /// <remarks>Use this event to handle file upload streams as they are received. The event provides access
    /// to the upload stream and related event data, allowing custom processing or validation during the upload
    /// process.</remarks>
    [Parameter]
    public EventCallback<UploadStreamEventArgs> OnUploadStream { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a file upload operation has completed.
    /// </summary>
    /// <remarks>Use this event to perform actions or update the UI in response to the completion of a file
    /// upload. The event provides details about the upload operation through the associated event arguments.</remarks>
    [Parameter]
    public EventCallback<StreamUploadedEventArgs> OnFileUploaded { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when files have been successfully uploaded.
    /// </summary>
    /// <remarks>Use this event to perform additional actions after the upload process completes, such as
    /// updating the UI or processing the uploaded files.</remarks>
    [Parameter] public EventCallback OnUploadCompleted { get; set; }

    /// <summary>
    /// Gets the file type filter string used to specify accepted file types for file selection dialogs.
    /// </summary>
    /// <remarks>The filter is determined based on the configured accept categories, extensions, and any
    /// custom extensions. If no filter is specified or constructed, the value is null.</remarks>
    private string? AcceptFilter
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Accept))
            {
                return Accept;
            }

            var rules = new AcceptFileRules
            {
                Categories = AcceptCategories,
                Extensions = AcceptExtensions
            };

            if (CustomAcceptExtensions is not null)
            {
                foreach (var ext in CustomAcceptExtensions)
                {
                    rules.AddCustom(ext);
                }
            }

            var value = rules.ToAcceptString();

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the upload button is disabled based on the current component state and event
    /// handler availability.
    /// </summary>
    /// <remarks>The upload button is disabled if the component is in a disabled state or if required event
    /// handlers are not assigned. This property can be used to control the enabled state of the upload button in the
    /// UI.</remarks>
    private bool IsUploadedButtonDisabled => _isDisabled ||
                                             !OnUploadStream.HasDelegate ||
                                             !OnFileUploaded.HasDelegate;

    /// <summary>
    /// Gets a value indicating whether the Rename button should be disabled based on the current selection and item
    /// state.
    /// </summary>
    /// <remarks>The Rename button is disabled if no items are selected, more than one item is selected, the
    /// selected item does not support renaming, or renaming is not allowed for the selected item.</remarks>
    private bool IsRenameButtonDisabled
    {
        get
        {
            if (_isDisabled ||
                _currentSelectedItems is null ||
                !OnRenameRequested.HasDelegate)
            {
                return true;
            }

            var count = _currentSelectedItems.Count;

            if (count != 1)
            {
                return true;
            }

            var item = _currentSelectedItems[0];

            if (item.Item is not IRenamable r)
            {
                return true;
            }

            return !r.IsRenameAllowed;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the download button should be disabled based on the current selection and
    /// download permissions.
    /// </summary>
    /// <remarks>The download button is disabled if the control is in a disabled state, if no items are
    /// selected, or if none of the selected items allow downloading. This property is typically used to control the
    /// enabled state of a download action in the user interface.</remarks>
    private bool IsDownloadButtonDisabled => GetIsDisabled(x => x.Item is IDownloadable d && !d.IsDownloadAllowed);

    /// <summary>
    /// Gets a value indicating whether the delete button should be disabled based on the current selection and deletion
    /// permissions.
    /// </summary>
    /// <remarks>The delete button is disabled if the control is disabled, if no items are selected, or if
    /// none of the selected items allow deletion. This property is typically used to control the enabled state of a
    /// delete action in the user interface.</remarks>
    private bool IsDeleteButtonDisabled => GetIsDisabled(x => x.Item is IDeletable d && !d.IsDeleteAllowed) ||
                                           !OnDeleteRequested.HasDelegate;

    /// <summary>
    /// Gets a value indicating whether the 'Move To' button is disabled based on the current selection state.
    /// </summary>
    /// <remarks>The button is considered disabled if the control itself is disabled or if there are no items
    /// currently selected.</remarks>
    private bool IsMoveToButtonDisabled => GetIsDisabled(x => x.Item is IMovable m && !m.IsMoveAllowed) ||
                                           !OnMoveRequested.HasDelegate;

    /// <summary>
    /// Gets a value indicating whether the upload button is disabled based on the presence of required upload event
    /// handlers.
    /// </summary>
    /// <remarks>The upload button is disabled if either the OnUploadStream or OnFileUploaded event handler is
    /// not assigned. Assign both handlers to enable the upload functionality.</remarks>
    private bool IsUploadButtonDisabled => !OnUploadStream.HasDelegate ||
                                           !OnFileUploaded.HasDelegate ||
                                           _isDisabled;

    /// <summary>
    /// Gets or sets the file provider used to retrieve and manage items of type TItem.
    /// </summary>
    /// <remarks>The provider is injected and supplies the data operations for the component. It must
    /// implement the <see cref="IFileProvider{TItem}"/> interface to ensure compatibility.</remarks>
    [Parameter]
    public IFileProvider<TItem> Provider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the service used to handle ZIP file operations for file entries of type TItem.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection framework. It provides
    /// methods for creating, extracting, or manipulating ZIP archives associated with file entries.</remarks>
    [Inject]
    private IFileEntryZipService<TItem> ZipService { get; set; } = default!;

    /// <summary>
    /// Determines whether the current selection is considered disabled based on the specified predicate and internal
    /// state.
    /// </summary>
    /// <param name="predicate">A function that evaluates each selected item to determine if it should be considered disabled. The function
    /// should return <see langword="true"/> for items that are disabled; otherwise, <see langword="false"/>.</param>
    /// <returns>Returns <see langword="true"/> if the selection is disabled due to internal state or if all selected items
    /// satisfy the predicate; otherwise, <see langword="false"/>.</returns>
    private bool GetIsDisabled(Func<FileEntry<TItem>, bool> predicate)
    {
        if (_isDisabled || _currentSelectedItems is null)
        {
            return true;
        }

        if (_currentSelectedItems.Count == 0)
        {
            return true;
        }

        return _currentSelectedItems.All(predicate);
    }

    /// <summary>
    /// Gets or sets a value indicating whether search options are displayed to the user.
    /// </summary>
    [Parameter]
    public bool ShowSearchOptions { get; set; } = true;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _engine = new FileManagerEngine<TItem>(Provider, State);
        _treeEngine = new FileManagerTreeEngine<TItem>(_engine);
        _trailEngine = new TrailMenuEngine<TItem>(Provider);
        _currentEntry = _engine.MasterRoot;

        _engine.SortUpdated += OnSortUpdated;
        await _engine.InitializeAsync();

        if (IsTreeViewVisible)
        {
            await BuildTreeViewAsync();
        }

        _flattenEntry = await _engine.BuildFlatViewAsync();
        _rootTrail = await _trailEngine.BuildAsync(_currentEntry);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Provider is null)
        {
            throw new InvalidOperationException("A file provider must be specified for the FluentCxFileManager component.");
        }

        DeviceInfoState?.DeviceInfo?.OnBreakpointChanged += OnBreakpointChanged;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        DeviceInfoState?.DeviceInfo?.OnBreakpointChanged -= OnBreakpointChanged;
        _engine.SortUpdated -= OnSortUpdated;

        GC.SuppressFinalize(this);

        return base.DisposeAsync();
    }

    /// <summary>
    /// Handles changes to the device breakpoint and updates the tree view accordingly.
    /// </summary>
    /// <remarks>When the breakpoint changes to extra small or small, the tree view is cleared. For other
    /// breakpoints, the tree view is rebuilt asynchronously.</remarks>
    /// <param name="sender">The source of the event. This parameter is not used.</param>
    /// <param name="e">The new device breakpoint value that triggered the event.</param>
    private void OnBreakpointChanged(object? sender, DeviceBreakpoint e)
    {
        if (e == DeviceBreakpoint.Xs || e == DeviceBreakpoint.Sm)
        {
            _treeRoot = null;
        }
        else
        {
            InvokeAsync(BuildTreeViewAsync);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Retrieves the current file entry based on the search state and file structure view.
    /// </summary>
    /// <remarks>If a search entry is present, it is returned. Otherwise, the returned entry depends on
    /// whether the file structure view is hierarchical or flattened.</remarks>
    /// <returns>A <see cref="FileEntry{TItem}"/> representing the current file entry, or <see langword="null"/> if no entry is
    /// available.</returns>
    private FileEntry<TItem>? GetEntry()
    {
        if (_searchEntry is not null)
        {
            return _engine.GetSortedEntry(_searchEntry);
        }

        return FileStructureView == FileStructureView.Hierarchical ? _engine.GetSortedEntry(_currentEntry) : _engine.GetSortedEntry(_flattenEntry);
    }

    /// <summary>
    /// Updates the current file sorting criterion to the specified value.
    /// </summary>
    /// <param name="sortBy">The sorting criterion to apply to the file list.</param>
    private void OnChangeSortBy(FileSortBy sortBy)
    {
        State.SortBy = sortBy;
    }

    /// <summary>
    /// Changes the current file view to the specified view.
    /// </summary>
    /// <param name="view">The file view to set as the current view.</param>
    private void OnChangeView(FileView view)
    {
        State.View = view;
    }

    /// <summary>
    /// Sets the current file sort mode to the specified value.
    /// </summary>
    /// <param name="value">The file sort mode to apply. Determines how files are ordered in the current state.</param>
    private void OnChangeSortMode(FileSortMode value)
    {
        State.SortMode = value;
    }

    /// <summary>
    /// Updates the current sort layout to the specified value.
    /// </summary>
    /// <param name="value">The new sort layout to apply.</param>
    private void OnChangeSortLayout(FileSortLayout value)
    {
        State.SortLayout = value;
    }

    /// <summary>
    /// Initializes the tree view structure by creating or retrieving the root node from the node cache.
    /// </summary>
    /// <remarks>This method ensures that the tree view has a root node based on the engine's master root. If
    /// the root node does not exist in the cache, it is created, configured, and added to the cache. The tree root is
    /// then set to this node.</remarks>
    private async Task BuildTreeViewAsync()
    {
        _treeRoot = await _treeEngine.CreateRootAsync(OnNodeExpandedAsync);
    }

    /// <summary>
    /// Handles the selection of a tree node asynchronously, expanding and navigating to the selected node if
    /// applicable.
    /// </summary>
    /// <remarks>The method performs no action if the tree view is not visible, or if the provided item is
    /// null or lacks a valid identifier.</remarks>
    /// <param name="item">The tree item that was selected. Must not be null and must have a valid identifier.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSelectedItemChangedAsync(ITreeViewItem? item)
    {
        _selectedNode = item;

        if (item is null)
        {
            return;
        }

        var entry = _engine.FindById(item.Id);

        if (entry is null)
        {
            return;
        }

        await NavigateToAsync(entry);
    }

    /// <summary>
    /// Handles the expansion of a tree node by asynchronously loading its child nodes if they have not been loaded.
    /// </summary>
    /// <remarks>This method checks whether the expanded node has already loaded its children and, if not,
    /// retrieves them from the data provider. The UI is updated to reflect loading state during the
    /// operation.</remarks>
    /// <param name="e">The event arguments containing information about the expanded tree view item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnNodeExpandedAsync(TreeViewItemExpandedEventArgs e)
    {
        if (e.Expanded)
        {
            await _treeEngine.LoadChildrenAsync(e.CurrentItem, OnNodeExpandedAsync);
        }
        else
        {
            e.CurrentItem.Items = TreeViewItem.LoadingTreeViewItems("Loading...");
        }
    }

    /// <summary>
    /// Navigates to the specified file entry asynchronously, updating the current selection and related UI state.
    /// </summary>
    /// <remarks>If the tree view is visible, the method expands and selects the corresponding node. The
    /// navigation also updates the breadcrumb trail and path display to reflect the selected entry.</remarks>
    /// <param name="entry">The file entry to navigate to. If null, the method returns without performing any action.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    private async Task NavigateToAsync(FileEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return;
        }

        await _engine.LoadChildrenWhenAsync(entry);
        _currentSelectedItems.Clear();
        _currentEntry = entry;
        _searchEntry = null;
        _rootTrail = await _trailEngine!.BuildAsync(entry);
        _path = string.Join(Path.DirectorySeparatorChar, FileManagerEngine<TItem>.GetPath(entry).Select(e => e.Name));

        if (IsTreeViewVisible)
        {
            if (_treeEngine.TryGetItem(entry.Id, out var node))
            {
                _selectedNode = node;
            }
            else
            {
                var parent = entry.Parent;

                if (parent is not null)
                {
                    _selectedNode = await _treeEngine.RefreshAsync(
                        parent.Id,
                        entry.Id,
                        OnNodeExpandedAsync
                    );
                }
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles navigation when the specified path changes by traversing the hierarchy and navigating to the
    /// corresponding node if found.
    /// </summary>
    /// <remarks>If the path does not correspond to a valid node in the hierarchy, no navigation occurs. The
    /// method skips the first segment of the path when traversing the hierarchy.</remarks>
    /// <param name="path">The new path to navigate to. Can be null or empty, in which case no navigation occurs. The path should use '/'
    /// as a separator between segments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnPathChangedAsync(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        if (path.StartsWith("Home"))
        {
            path = path["Home".Length..];
        }

        var segments = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var current = _engine.MasterRoot;

        foreach (var seg in segments)
        {
            await _engine.LoadChildrenWhenAsync(current);
            current = current.Children.FirstOrDefault(c => c.Name == seg || c.Id == seg);

            if (current is null)
            {
                return;
            }
        }

        await NavigateToAsync(current);
    }

    /// <summary>
    /// Handles the update operation for the specified file entry asynchronously.
    /// </summary>
    /// <param name="entry">The file entry to update. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnUpdateEntryAsync(FileEntry<TItem> entry)
    {
        await NavigateToAsync(entry);
    }

    /// <summary>
    /// Executes the specified asynchronous action while updating the progress state and disabling user interaction
    /// during execution.
    /// </summary>
    /// <remarks>User interaction is disabled while the action is in progress and re-enabled upon completion.
    /// The progress state is updated before and after the action executes.</remarks>
    /// <param name="state">The progress state to display while the action is running.</param>
    /// <param name="action">A delegate representing the asynchronous operation to execute.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task RunWithProgressAsync(FileManagerProgressState state, Func<Task> action)
    {
        _isDisabled = true;
        _progressState = state;
        await InvokeAsync(StateHasChanged);

        try
        {
            await action();
        }
        finally
        {
            _isDisabled = false;
            _progressState = FileManagerProgressState.None;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Handles the asynchronous creation of a new folder within the currently selected directory entry.
    /// </summary>
    /// <remarks>This method displays a dialog to prompt the user for a new folder name, validates the input,
    /// and raises the OnCreateDirectoryRequested event to allow for custom folder creation logic. If the folder is
    /// successfully created, it updates the directory structure and optionally notifies listeners via the
    /// OnFolderCreated event. The method manages UI state to reflect progress and disables interactions during the
    /// operation.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnCreateFolderAsync()
    {
        if (_currentEntry is null || !_currentEntry.IsDirectory)
        {
            return;
        }

        var name = await PromptFolderNameAsync();

        if (name is null)
        {
            return;
        }

        await RunWithProgressAsync(FileManagerProgressState.Creation, async () =>
        {
            var newEntry = await CreateFolderEntryAsync(name);

            if (newEntry is null)
            {
                return;
            }

            FileManagerEngine<TItem>.ApplyCreate(newEntry, _currentEntry);

            await InvokeCallbackAsync(OnFolderCreated);
            await NavigateToAsync(newEntry);
        });
    }

    /// <summary>
    /// Displays a dialog prompting the user to enter a new folder name and returns the entered name if confirmed.
    /// </summary>
    /// <remarks>The method returns null if the user cancels the dialog or submits an empty or whitespace-only
    /// name.</remarks>
    /// <returns>A string containing the folder name entered by the user if the dialog is confirmed; otherwise, null.</returns>
    private async Task<string?> PromptFolderNameAsync()
    {
        var result = await DialogService.ShowDialogAsync<CreateFolderDialog>(options =>
        {
            options.Header.Title = "Create a new folder";
        });

        if (result.Cancelled)
        {
            return null;
        }

        if (result.Value is not string name || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return name;
    }

    /// <summary>
    /// Asynchronously creates a new folder entry with the specified name as a child of the current directory, if
    /// directory creation is supported.
    /// </summary>
    /// <remarks>This method invokes the directory creation event if it is assigned. If the event is not
    /// handled, or if the operation is canceled, the method returns null.</remarks>
    /// <param name="name">The name of the folder to create. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created folder entry if the
    /// operation succeeds; otherwise, null.</returns>
    private async Task<FileEntry<TItem>?> CreateFolderEntryAsync(string name)
    {
        if (!OnCreateDirectoryRequested.HasDelegate)
        {
            throw new InvalidOperationException("The OnCreateDirectoryRequested must have a delegate");
        }

        var args = new CreateDirectoryEventArgs
        {
            ParentId = _currentEntry!.Id,
            Name = name
        };

        await OnCreateDirectoryRequested.InvokeAsync(args);

        if (args.Cancel || args.Descriptor is null)
        {
            return null;
        }

        var e = args.Descriptor!;
        e.Name = name;
        e.ParentId = _currentEntry.Id;

        return FileManagerEngine<TItem>.CreateDirectoryEntry(EntryDescriptor<TItem>.Directory(e.Id, e.Name, e.ParentId, DateTime.UtcNow, DateTime.UtcNow), _currentEntry!);
    }

    /// <summary>
    /// Handles the rename operation for the specified file or folder entry asynchronously, displaying a dialog to
    /// prompt for a new name and updating the entry if the rename is confirmed.
    /// </summary>
    /// <remarks>The method displays a dialog to the user to enter a new name. If the rename is successful,
    /// the entry is updated and relevant events are triggered. The operation is only performed if the entry supports
    /// renaming.</remarks>
    /// <param name="entry">The file or folder entry to be renamed. If null, or if the entry does not support renaming, the operation is not
    /// performed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnRenameAsync(FileEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return;
        }

        if (!CanRename(entry))
        {
            return;
        }

        var newName = await PromptRenameAsync(entry!);

        if (newName is null)
        {
            return;
        }

        await RunWithProgressAsync(FileManagerProgressState.Renaming, async () =>
        {
            var success = await InvokeRenameCallbackAsync(entry, newName);

            if (!success)
            {
                return;
            }

            var logical = _engine.FindById(entry.Id);
            FileManagerEngine<TItem>.ApplyRename(logical, newName);
            UpdateTreeViewItemText(logical, newName);
            await RefreshTrailMenuAsync(logical);
            await InvokeCallbackAsync(OnRenamed);
        });
    }

    /// <summary>
    /// Asynchronously invokes the callback associated with the rename event, if one is assigned.
    /// </summary>
    /// <param name="callback">The event callback to invoke. If the callback does not have a delegate assigned, the method completes without invoking any action.</param>
    /// <remarks>Use this method to trigger any logic registered for the rename event. If no callback is
    /// assigned, the method completes without invoking any action.</remarks>
    /// <returns>A task that represents the asynchronous operation of invoking the rename callback.</returns>
    private static async Task InvokeCallbackAsync(EventCallback callback)
    {
        if (callback.HasDelegate)
        {
            await callback.InvokeAsync();
        }
    }

    /// <summary>
    /// Updates the display text of the specified tree node to the provided name.
    /// </summary>
    /// <param name="entry">The file entry whose corresponding tree node text will be updated. Must not be null.</param>
    /// <param name="newName">The new text to assign to the tree node. Cannot be null.</param>
    private void UpdateTreeViewItemText(FileEntry<TItem>? entry, string newName)
    {
        if (!IsTreeViewVisible ||
            entry is null ||
            !_treeEngine.TryGetItem(entry.Id, out var node) || node is null)
        {
            return;
        }

        node.Text = newName;
    }

    /// <summary>
    /// Asynchronously refreshes the tree hierarchy by initializing the core engine and creating a new root node.
    /// </summary>
    /// <remarks>This method resets the root node and updates the selected node to the new root. It should be
    /// called when the tree structure needs to be reloaded or reinitialized.</remarks>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    private async Task RefreshHierarchyAsync()
    {
        _treeRoot = await _treeEngine.RefreshHierarchyAsync(OnNodeExpandedAsync);

        await NavigateToAsync(_engine.MasterRoot);
    }

    /// <summary>
    /// Refreshes the trail menu asynchronously if the specified entry is the current entry or an ancestor of the current entry.
    /// </summary>
    /// <param name="entry">The file entry that was renamed. The trail menu will be refreshed if this entry is the current entry or an ancestor of the current entry.</param>
    /// <returns>Returns a task that represents the asynchronous operation. The task completes when the trail menu has been refreshed if necessary.</returns>
    private async Task RefreshTrailMenuAsync(FileEntry<TItem>? entry)
    {
        if (entry is null ||
            _currentEntry is null)
        {
            return;
        }

        if (string.Equals(_currentEntry.Id, entry.Id) ||
            entry.IsAncestorOf(_currentEntry) ||
            _currentEntry.IsAncestorOf(entry))
        {
            _rootTrail = await _trailEngine.BuildAsync(_currentEntry);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Invokes the rename callback for the specified file entry and returns the result indicating whether the rename
    /// operation was successful.
    /// </summary>
    /// <remarks>This method triggers the <see cref="OnRenameRequested"/> event callback if it is assigned.
    /// The result depends on the outcome of the event handler.</remarks>
    /// <param name="entry">The file entry to be renamed. Must not be null.</param>
    /// <param name="newName">The new name to assign to the file entry. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the rename
    /// operation succeeded; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> InvokeRenameCallbackAsync(FileEntry<TItem> entry, string newName)
    {
        if (!OnRenameRequested.HasDelegate)
        {
            return false;
        }

        var args = new RenameEntryEventArgs(entry.Id)
        {
            NewName = newName
        };

        await OnRenameRequested.InvokeAsync(args);

        return args.Success;
    }

    /// <summary>
    /// Affiche une boîte de dialogue permettant à l'utilisateur de renommer le fichier ou le dossier spécifié, et
    /// retourne le nouveau nom saisi.
    /// </summary>
    /// <remarks>La méthode retourne null si l'utilisateur annule la boîte de dialogue ou si le nom saisi est
    /// vide ou composé uniquement d'espaces.</remarks>
    /// <param name="entry">L'entrée de fichier ou de dossier à renommer. Doit contenir le nom actuel et indiquer s'il s'agit d'un dossier.</param>
    /// <returns>Le nouveau nom saisi par l'utilisateur si la boîte de dialogue est validée ; sinon, null.</returns>
    private async Task<string?> PromptRenameAsync(FileEntry<TItem> entry)
    {
        var result = await DialogService.ShowDialogAsync<RenameDialog>(options =>
        {
            options.Header.Title = entry.IsDirectory ? "Rename a folder" : "Rename a file";
            options.Parameters.Add(nameof(RenameDialog.Name), Path.GetFileNameWithoutExtension(entry.Name));
        });

        if (result.Cancelled)
        {
            return null;
        }

        if (result.Value is not string name ||
            string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return name;
    }

    /// <summary>
    /// Determines whether the specified file entry can be renamed.
    /// </summary>
    /// <remarks>Use this method to check if a file entry supports renaming before attempting to perform a
    /// rename operation.</remarks>
    /// <param name="entry">The file entry to evaluate for rename capability. May be null.</param>
    /// <returns>true if the entry is not null, implements IRenamable, and renaming is allowed; otherwise, false.</returns>
    private static bool CanRename(FileEntry<TItem>? entry)
    {
        return entry is { Item: IRenamable r } && r.IsRenameAllowed;
    }

    /// <summary>
    /// Initiates the download of the selected items, downloading a single file directly or multiple files as a ZIP
    /// archive, as appropriate.
    /// </summary>
    /// <remarks>If no items are selected, the method completes without performing any action. When multiple
    /// items are selected, they are packaged into a ZIP archive before download. The method temporarily disables
    /// further download actions while the operation is in progress.</remarks>
    /// <returns>A task that represents the asynchronous download operation.</returns>
    private async Task OnDownloadAsync()
    {
        var items = SelectedItems.ToList();

        if (items.Count == 0)
        {
            return;
        }

        _isDisabled = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            if (items.Count == 1)
            {
                await DownloadSingleEntryAsync(items[0]);
            }
            else
            {
                await DownloadZippedEntriesAsync(items);
            }
        }
        finally
        {
            _isDisabled = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Downloads a single file entry by retrieving its content and invoking the file downloader service.
    /// </summary>
    /// <param name="entry">Entry to download. Must not be null and must represent a downloadable file.</param>
    /// <returns>Returns a task that represents the asynchronous download operation.</returns>
    private async Task DownloadSingleEntryAsync(FileEntry<TItem> entry)
    {
        var contentType = entry.GetContentType();
        var content = await entry.GetContentAsync();

        await FileDownloader.DownloadFileAsync(entry.Name, contentType, content);
    }

    /// <summary>
    /// Downloads the specified file entries as a single ZIP archive asynchronously.
    /// </summary>
    /// <remarks>The resulting ZIP file will contain all provided entries. The download is initiated in the
    /// user's browser and may prompt for a save location depending on browser settings.</remarks>
    /// <param name="entries">The collection of file entries to include in the ZIP archive. Cannot be null or contain null elements.</param>
    /// <returns>A task that represents the asynchronous download operation.</returns>
    private async Task DownloadZippedEntriesAsync(IEnumerable<FileEntry<TItem>> entries)
    {
        var zipEntry = await ZipService.ZipAsync(entries);
        var contentType = zipEntry.GetContentType();
        var content = await zipEntry.GetContentAsync();

        await FileDownloader.DownloadFileAsync(zipEntry.Name, contentType, content);
    }

    /// <summary>
    /// Handles the asynchronous deletion of the currently selected items after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user for confirmation before deleting the selected items. If the
    /// deletion is confirmed, it invokes the delete request event, updates the UI state, and notifies listeners upon
    /// completion. The method disables relevant UI elements during the operation to prevent concurrent
    /// actions.</remarks>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    private async Task OnDeleteAsync()
    {
        var items = SelectedItems.ToList();

        if (items.Count == 0)
        {
            return;
        }

        var confirmed = await ConfirmDeletionAsync(items.Count);

        if (!confirmed)
        {
            return;
        }

        await RunWithProgressAsync(FileManagerProgressState.Deleting, async () =>
        {
            var deleted = await InvokeDeleteCallbackAsync(items);

            if (deleted.Count == 0)
            {
                return;
            }

            var parent = deleted.FirstOrDefault()?.Parent;

            FileManagerEngine<TItem>.ApplyDelete(deleted);

            await InvokeCallbackAsync(OnDeleted);

            if (parent is not null)
            {
                await NavigateToAsync(parent);
            }
        });
    }

    /// <summary>
    /// Invokes the delete callback and returns the list of file entries that were successfully deleted.
    /// </summary>
    /// <remarks>This method triggers the delete event callback if it is assigned. Only entries for which the
    /// callback reports successful deletion are included in the returned list.</remarks>
    /// <param name="items">The collection of file entries to request deletion for. Each entry should have a valid identifier.</param>
    /// <returns>A list of file entries that were confirmed as successfully deleted. The list will be empty if no entries were
    /// deleted or if no delete callback is assigned.</returns>
    private async Task<List<FileEntry<TItem>>> InvokeDeleteCallbackAsync(List<FileEntry<TItem>> items)
    {
        if (!OnDeleteRequested.HasDelegate)
        {
            return [];
        }

        var args = new DeleteEntriesEventArgs
        {
            Ids = [.. items.Select(i => i.Id)]
        };

        await OnDeleteRequested.InvokeAsync(args);

        var deleted = new List<FileEntry<TItem>>();

        foreach (var res in args.Results.Where(r => r.Success))
        {
            var entry = items.FirstOrDefault(i => i.Id == res.Id);

            if (entry is not null)
            {
                deleted.Add(entry);
            }
        }

        return deleted;
    }

    /// <summary>
    /// Displays a confirmation dialog to the user for deleting the specified number of items and returns whether the
    /// deletion was confirmed.
    /// </summary>
    /// <remarks>The dialog message will reflect the number of items to be deleted. The method does not
    /// perform the deletion itself; it only confirms the user's intent.</remarks>
    /// <param name="count">The number of items to be deleted. Must be greater than zero.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user
    /// confirmed the deletion; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> ConfirmDeletionAsync(int count)
    {
        var result = await DialogService.ShowConfirmationAsync(
            $"Do you want to delete {count} item{(count > 1 ? "s" : string.Empty)} ?",
            "Delete");

        return !result.Cancelled;
    }

    /// <summary>
    /// Handles the move operation for the selected items by displaying a dialog to select the destination directory and
    /// invoking the move logic if confirmed.
    /// </summary>
    /// <remarks>If no items are selected, the method returns immediately. The method displays a dialog for
    /// the user to select a destination directory. If the move is confirmed and a valid destination is selected, the
    /// move request is raised and, upon success, the items are updated in the UI. The method also updates the progress
    /// state and disables interactions during the operation.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnMoveAsync()
    {
        var items = SelectedItems.ToList();

        if (items.Count == 0)
        {
            return;
        }

        var destination = await PromptMoveDestinationAsync(items.Count);

        if (destination is null)
        {
            return;
        }

        await RunWithProgressAsync(FileManagerProgressState.Moving, async () =>
        {
            var moved = await InvokeMoveCallbackAsync(items, destination);

            if (moved.Count == 0)
            {
                return;
            }

            FileManagerEngine<TItem>.ApplyMove(moved, destination);

            await RefreshTrailMenuAsync(destination);
            await RefreshHierarchyAsync();
            await InvokeCallbackAsync(OnMoved);
            await NavigateToAsync(destination);
        });
    }

    /// <summary>
    /// Invokes the move callback to request moving the specified file entries to a new destination and returns the
    /// entries that were successfully moved.
    /// </summary>
    /// <remarks>This method triggers the move operation by invoking the registered move callback, if any.
    /// Only the entries for which the move operation succeeds are included in the returned list.</remarks>
    /// <param name="items">The list of file entries to be moved. Each entry represents an item to relocate.</param>
    /// <param name="destination">The destination file entry representing the new parent location for the moved items.</param>
    /// <returns>A list of file entries that were successfully moved to the specified destination. The list is empty if no items
    /// were moved or if no move callback is registered.</returns>
    private async Task<List<FileEntry<TItem>>> InvokeMoveCallbackAsync(
        List<FileEntry<TItem>> items,
        FileEntry<TItem> destination)
    {
        if (!OnMoveRequested.HasDelegate)
        {
            return [];
        }

        var args = new MoveEntriesEventArgs
        {
            Ids = [.. items.Select(i => i.Id)],
            NewParentId = destination.Id
        };

        await OnMoveRequested.InvokeAsync(args);

        return [.. args.Results
            .Where(r => r.Success)
            .Select(r => items.First(i => i.Id == r.Id))];
    }

    /// <summary>
    /// Prompts the user to select a destination directory for moving the specified number of items.
    /// </summary>
    /// <remarks>The returned FileEntry is guaranteed to represent a directory. If the user cancels the dialog
    /// or selects an invalid destination, the method returns null.</remarks>
    /// <param name="count">The number of items to be moved. Determines the dialog title and context.</param>
    /// <returns>A FileEntry representing the selected destination directory if the user confirms the dialog; otherwise, null.</returns>
    private async Task<FileEntry<TItem>?> PromptMoveDestinationAsync(int count)
    {
        var result = await DialogService.ShowDialogAsync<MoveDialog>(options =>
        {
            options.Header.Title = $"Move {count} item{(count > 1 ? "s" : string.Empty)} to...";
            options.Parameters.Add(nameof(MoveDialog.Root), _treeRoot);
        });

        if (result.Cancelled)
        {
            return null;
        }

        if (result.Value is ITreeViewItem selectedItem)
        {
            return _engine.FindById(selectedItem.Id);
        }

        return null;
    }

    /// <summary>
    /// Handles the action to display detailed information for a file manager entry asynchronously.
    /// </summary>
    /// <remarks>This method is intended to be used in mobile scenarios to open a dialog displaying file
    /// manager entry details.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnShowDetailsAsync()
    {
        await DialogService.ShowDialogAsync<FileDetailsDialog<TItem>>(options =>
        {
            options.Header.Title = "Details";
            options.Parameters.Add(nameof(FileDetailsDialog<>.Content), (_currentSelectedItems.Count != 0 ? _currentSelectedItems : _currentEntry is null ? [] : [_currentEntry]));
        });
    }

    /// <summary>
    /// Gets a user-friendly label that describes the current progress state of a file operation.
    /// </summary>
    /// <remarks>The returned label can be used in user interfaces to indicate the current operation, such as
    /// uploading, downloading, or deleting files.</remarks>
    /// <returns>A string containing a descriptive label for the current progress state; or an empty string if the state is
    /// unrecognized.</returns>
    private string GetProgressLabelFromState()
    {
        return _progressState switch
        {
            FileManagerProgressState.Uploading => "Uploading the files...",
            FileManagerProgressState.Downloading => "Downloading the files",
            FileManagerProgressState.Deleting => "Deleting the items ...",
            FileManagerProgressState.Moving => "Moving the items ...",
            FileManagerProgressState.Creation => "Creating the folder ...",
            FileManagerProgressState.Renaming => "Renaming the item ...",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Sets the disabled state of the component.
    /// </summary>
    /// <param name="disabled">true to disable the component; otherwise, false.</param>
    private void SetDisabled(bool disabled) => _isDisabled = disabled;

    /// <summary>
    /// Occurs when all file have been uploaded.
    /// </summary>
    private async Task OnCompletedAsync()
    {
        _progressState = FileManagerProgressState.None;
        _fileManagerView?.SetBusy(false);
        SetDisabled(false);

        await InvokeCallbackAsync(OnUploadCompleted);
    }

    /// <summary>
    /// Handles the completion of a file upload event and updates the file tree structure accordingly.
    /// </summary>
    /// <remarks>If the uploaded file is a directory, a new tree node is created and added to the node cache.
    /// If the tree view is visible, the parent node's children are reloaded to reflect the new entry. If an upload
    /// callback is registered, it is invoked with the new entry.</remarks>
    /// <param name="e">The event arguments containing information about the uploaded file, including its index and name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnFileUploadedAsync(FluentInputFileEventArgs e)
    {
        if (_currentEntry is null)
        {
            return;
        }

        if (!OnFileUploaded.HasDelegate)
        {
            return;
        }

        var args = new StreamUploadedEventArgs(
            _currentEntry.Id,
            new UploadFileCandidate
            {
                Index = e.Index,
                Name = e.Name,
                Size = e.Size
            }
        );

        await OnFileUploaded.InvokeAsync(args);

        if (args.Descriptor is null)
        {
            return;
        }

        var desc = args.Descriptor;

        var newEntry = new FileEntry<TItem>(
            id: desc.Id,
            name: desc.Name,
            isDirectory: false,
            size: desc.Size,
            createdDate: desc.CreatedDate,
            modifiedDate: desc.ModifiedDate,
            item: new TItem(),
            parent: _currentEntry
        );

        FileManagerEngine<TItem>.ApplyUpload(newEntry, _currentEntry);

        _flattenEntry?.AddChild(newEntry);
    }

    /// <summary>
    /// Handles progress change events during a file upload operation and updates the upload state accordingly.
    /// </summary>
    /// <param name="e">The event arguments containing information about the file upload progress, including the file index, name, and
    /// progress percentage.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnProgressChange(FluentInputFileEventArgs e)
    {
        SetDisabled(true);
        _fileManagerView?.SetBusy(true);
        _progressState = FileManagerProgressState.Uploading;

        ProgressPercent = e.ProgressPercent;

        if (OnUploadStream.HasDelegate)
        {
            var args = new UploadStreamEventArgs(
                e.Index,
                e.Name,
                [.. e.Buffer.Data[..e.Buffer.BytesRead]]
            );

            await OnUploadStream.InvokeAsync(args);
        }
    }

    /// <summary>
    /// Performs an asynchronous search for file entries based on the current search value and updates the search
    /// results accordingly.
    /// </summary>
    /// <remarks>If the search value is null, empty, or consists only of white-space characters, the search
    /// results are cleared. The search is performed either from the current entry or the root, depending on the file
    /// structure view. The component state is updated before and after the search to reflect the searching status and
    /// results.</remarks>
    /// <returns>A task that represents the asynchronous search operation.</returns>
    private async Task OnSearchFileEntryAsync()
    {
        if (string.IsNullOrWhiteSpace(_searchValue))
        {
            _searchEntry = null;
            return;
        }

        _progressState = FileManagerProgressState.Searching;
        await InvokeAsync(StateHasChanged);

        var query = _searchValue.Trim();

        var results =
            FileStructureView == FileStructureView.Hierarchical
                ? _engine.SearchAsync(query, SearchOptions, MetadataExtractor, _currentEntry)
                : _engine.SearchAsync(query, SearchOptions, MetadataExtractor);

        var list = new List<FileEntry<TItem>>();

        await foreach (var entry in results)
        {
            list.Add(entry);
        }

        _searchEntry = list.Count == 0
            ? null
            : FileManagerEngine<TItem>.CreateSearchContainer(_currentEntry, list);

        _progressState = FileManagerProgressState.None;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the file structure view by updating the current view and triggering a UI refresh.
    /// </summary>
    /// <param name="view">The new file structure view to display.</param>
    private async Task OnChangeFileStructureViewAsync(FileStructureView view)
    {
        FileStructureView = view;

        if (IsTreeViewVisible)
        {
            await RefreshHierarchyAsync();
        }
    }

    /// <summary>
    /// Handles changes to the checked state of a view menu item and updates the corresponding view or sort option based
    /// on the selected menu item.
    /// </summary>
    /// <remarks>This method updates the file structure view, file view, or sort order depending on the menu
    /// item selected. The update only occurs if the menu item is checked.</remarks>
    /// <param name="e">The event arguments containing information about the menu item whose checked state has changed. The <see
    /// cref="MenuItemEventArgs.Id"/> property is used to determine which view or sort option to update.</param>
    private async Task OnViewMenuChechedChanged(MenuItemEventArgs e)
    {
        if (e.Checked == false)
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        var id = e.Id?.Split('-').FirstOrDefault();

        if (Enum.TryParse<FileStructureView>(id, out var view))
        {
            await OnChangeFileStructureViewAsync(view);
        }
        else if (Enum.TryParse<FileView>(id, out var fileView))
        {
            OnChangeView(fileView);
        }

        if (Breakpoint > DeviceBreakpoint.Sm &&
            _viewMenu is not null)
        {
            await _viewMenu.CloseMenuAsync();
        }

        if (Breakpoint < DeviceBreakpoint.Md &&
            _mobileViewMenu is not null)
        {
            await _mobileViewMenu.CloseMenuAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the sort menu selection by updating the sort criteria based on the selected menu item.
    /// </summary>
    /// <remarks>This method updates the sort order or sort mode depending on the selected menu item's
    /// identifier. The method only processes the event if the menu item is checked.</remarks>
    /// <param name="e">The event data associated with the menu item selection, containing the selected item's identifier and checked
    /// state.</param>
    private async Task OnSortMenuChechedChanged(MenuItemEventArgs e)
    {
        if (e.Checked == false)
        {
            return;
        }

        var id = e.Id?.Split('-').FirstOrDefault();

        if (Enum.TryParse<FileSortBy>(id, out var sortBy))
        {
            OnChangeSortBy(sortBy);
        }
        else if (Enum.TryParse<FileSortMode>(id, out var sortMode))
        {
            OnChangeSortMode(sortMode);
        }
        else if (Enum.TryParse<FileSortLayout>(id, out var sortLayout))
        {
            OnChangeSortLayout(sortLayout);
        }

        if (Breakpoint > DeviceBreakpoint.Sm &&
            _sortMenu is not null)
        {
            await _sortMenu.CloseMenuAsync();
        }

        if (Breakpoint < DeviceBreakpoint.Md &&
            _mobileSortMenu is not null)
        {
            await _mobileSortMenu.CloseMenuAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Occurs when the selected items have changed.
    /// </summary>
    /// <param name="items">List of selected items.</param>
    private void OnSelectedItemsChanged(IEnumerable<FileEntry<TItem>> items)
    {
        _currentSelectedItems.Clear();
        _currentSelectedItems.AddRange(items);

        StateHasChanged();
    }

    /// <summary>
    /// Handles the event triggered when the sort order is updated and refreshes the component state.
    /// </summary>
    /// <param name="sender">The source of the event that triggered the sort update. This can be null.</param>
    /// <param name="e">An object that contains the event data.</param>
    private void OnSortUpdated(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    /// <summary>
    /// Displays the search options asynchronously when triggered.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnShowSearchOptionsAsync()
    {
        var dialog = await DialogService.ShowDrawerAsync<SearchOptionsDialog>(options =>
        {
            options.Header.Title = "Search options";
            options.Parameters.Add(nameof(SearchOptionsDialog.Options), SearchOptions.Clone());
            options.Size = DialogSize.Small;
        });

        if (!dialog.Cancelled && dialog.Value is FileSearchOptions fso)
        {
            SearchOptions = fso;
        }
    }
}
