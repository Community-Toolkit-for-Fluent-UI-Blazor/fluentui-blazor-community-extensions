using FluentUI.Blazor.Community.Components.FileManager;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a fluent file manager.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FluentCxFileManager<TItem>
    : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Represents the progress state to display the good label.
    /// </summary>
    private enum ProgressState
    {
        /// <summary>
        /// No progress.
        /// </summary>
        None,

        /// <summary>
        /// Uploading a file.
        /// </summary>
        Uploading,

        /// <summary>
        /// Downloading a file.
        /// </summary>
        Downloading,

        /// <summary>
        /// Deleting a file.
        /// </summary>
        Deleting,

        /// <summary>
        /// Moving a file.
        /// </summary>
        Moving
    }

    /// <summary>
    /// Represents a value indicating if the details of an entry is shown.
    /// </summary>
    private bool _showDetails;

    /// <summary>
    /// Represents a value indicating if the root has changed.
    /// </summary>
    private bool _hasRootChanged;

    /// <summary>
    /// Represents a value indicating if the mobile view is forced.
    /// </summary>
    private bool _forceMobileView;

    /// <summary>
    /// Represents the file manager component.
    /// </summary>
    private FileManager<TItem>? _fileManagerView;

    /// <summary>
    /// Represents the current selected entry.
    /// </summary>
    private FileManagerEntry<TItem>? _currentEntry;

    /// <summary>
    /// Represents the tree view (only shown in Desktop mode)
    /// </summary>
    private IEnumerable<TreeViewItem>? _treeViewItems = [];

    /// <summary>
    /// Represents the collapsed node icon.
    /// </summary>
    private static readonly Icon IconCollapsed = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Folder();

    /// <summary>
    /// Represents the expanded node icon.
    /// </summary>
    private static readonly Icon IconExpanded = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.FolderOpen();

    /// <summary>
    /// Represents the current treeview item.
    /// </summary>
    private ITreeViewItem? _currentTreeViewItem;

    /// <summary>
    /// Represents the selected items.
    /// </summary>
    private IEnumerable<FileManagerEntry<TItem>> _currentSelectedItems = [];

    /// <summary>
    /// Represents the value to search inside the current node.
    /// </summary>
    private string? _searchValue;

    /// <summary>
    /// Represents the entry which contains all the found entries during a search operation.
    /// </summary>
    private FileManagerEntry<TItem>? _searchEntry;

    /// <summary>
    /// Represents the current state of a progress.
    /// </summary>
    private ProgressState _progressState = ProgressState.None;

    /// <summary>
    /// Represents a value indicating if the component is disabled or not.
    /// </summary>
    private bool _isDisabled;

    /// <summary>
    /// Represents the buffer dictionary of all files currenlty uploading.
    /// </summary>
    private readonly Dictionary<int, MemoryStream> _fileBufferDictionary = [];

    /// <summary>
    /// Represents the list of all navigation items.
    /// </summary>
    private IPathBarItem? _rootPath;

    /// <summary>
    /// Represents the flatten entry.
    /// </summary>
    private readonly FileManagerEntry<TItem> _flattenEntry;

    /// <summary>
    /// Represents the javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the javascript filename to use for interop.
    /// </summary>
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/FileManager/FluentCxFileManager.razor.js";

    /// <summary>
    /// Represents the provider to get the content type from a file extension.
    /// </summary>
    private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    /// <summary>
    /// Represents the path for the <see cref="FluentCxPathBar"/> component.
    /// </summary>
    private string? _path;

    /// <summary>
    /// Represents the render fragment to use for the label of a button.
    /// </summary>
    private readonly RenderFragment<string> _renderLabel;

    /// <summary>
    /// Gets or sets the state of the file manager.
    /// </summary>
    [Inject]
    internal FileManagerState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the <see cref="IDialogService"/> instance.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the create folder button is visible.
    /// </summary>
    [Parameter]
    public bool ShowCreateFolderButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the upload button is visible.
    /// </summary>
    [Parameter]
    public bool ShowUploadButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the view button is visible.
    /// </summary>
    [Parameter]
    public bool ShowViewButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the sort button is visible.
    /// </summary>
    [Parameter]
    public bool ShowSortButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the properties button is visible.
    /// </summary>
    [Parameter]
    public bool ShowPropertiesButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the details button is visible.
    /// </summary>
    [Parameter]
    public bool ShowDetailsButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the labels to use for the text of the UI.
    /// </summary>
    [Parameter]
    public FileManagerLabels FileManagerLabels { get; set; } = FileManagerLabels.Default;

    /// <summary>
    /// Gets or sets the labels to use for the details panel.
    /// </summary>
    [Parameter]
    public FileManagerDetailsLabels DetailsLabels { get; set; } = FileManagerDetailsLabels.Default;

    /// <summary>
    /// Gets or sets the labels to use for the file from its extension.
    /// </summary>
    [Parameter]
    public FileExtensionTypeLabels FileExtensionTypeLabels { get; set; } = FileExtensionTypeLabels.Default;

    /// <summary>
    /// Gets or sets the labels to use for the list view.
    /// </summary>
    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    /// <summary>
    /// Gets or sets the percentage of the progression.
    /// </summary>
    private int? ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the manager is busy or not.
    /// </summary>
    [Parameter]
    public bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets the view of the file manager.
    /// </summary>
    /// <remarks>
    /// By default, the view is desktop.
    /// </remarks>
    [Parameter]
    public FileManagerView View { get; set; } = FileManagerView.Desktop;

    /// <summary>
    /// Gets or sets the root of the file manager.
    /// </summary>
    [Parameter]
    public FileManagerEntry<TItem> Root { get; set; } = default!;

    /// <summary>
    /// Gets or sets the allowed files to be uploaded.
    /// </summary>
    [Parameter]
    public string? Accept { get; set; }

    /// <summary>
    /// Gets or sets the allowed files to be uploaded in an enum way.
    /// </summary>
    [Parameter]
    public AcceptFile AcceptFiles { get; set; } = AcceptFile.None;

    /// <summary>
    /// Gets or sets the maximum file count that can be uploaded in one time.
    /// </summary>
    /// <remarks>Default is 100.</remarks>
    [Parameter]
    public int MaximumFileCount { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum file size.
    /// </summary>
    /// <remarks>Default is 100 MiB</remarks>
    [Parameter]
    public long MaximumFileSize { get; set; } = 1024 * 1024 * 100;

    /// <summary>
    /// Gets or sets the size of the buffer.
    /// </summary>
    /// <remarks>Default is 10 KiB</remarks>
    [Parameter]
    public uint BufferSize { get; set; } = 1024 * 10;

    /// <summary>
    /// Gets or sets the callback to use when a folder is created.
    /// </summary>
    [Parameter]
    public EventCallback<CreateFileManagerEntryEventArgs<TItem>> OnFolderCreated { get; set; }

    /// <summary>
    /// Gets or sets the callback to use when a file or a folder is renamed.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> OnRename { get; set; }

    /// <summary>
    /// Gets or sets the callback to use when a file or a folder is deleted.
    /// </summary>
    [Parameter]
    public EventCallback<DeleteFileManagerEntryEventArgs<TItem>> OnDelete { get; set; }

    /// <summary>
    /// Gets or sets the callback to use when a file is uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> OnFileUploaded { get; set; }

    /// <summary>
    /// Gets or sets extra buttons on the toolbar.
    /// </summary>
    [Parameter]
    public RenderFragment? ToolbarItems { get; set; }

    /// <summary>
    /// Gets or sets the view of the file structure.
    /// </summary>
    /// <remarks>Default is <see cref="FileStructureView.Hierarchical"/>.</remarks>
    [Parameter]
    public FileStructureView FileStructureView { get; set; } = FileStructureView.Hierarchical;

    /// <summary>
    /// Gets or sets the callback to use when a file or a folder is moved.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntriesMovedEventArgs<TItem>> Moved { get; set; }

    /// <summary>
    /// Gets or sets the width of the component when the view is switched to smartphone.
    /// </summary>
    [Parameter]
    public string? SmartphoneSwitchViewWidth { get; set; }

    /// <summary>
    /// Gets the smartphone switch view width query.
    /// </summary>
    private string SmartphoneSwitchViewWidthQuery => $"(max-width: {SmartphoneSwitchViewWidth})";

    /// <summary>
    /// Gets the accept filter to use for the upload input.
    /// </summary>
    private string? AcceptFilter => AcceptFiles == AcceptFile.None ? Accept : AcceptFileProvider.Get(AcceptFiles);

    /// <summary>
    /// Gets the selected items.
    /// </summary>
    public IEnumerable<FileManagerEntry<TItem>> SelectedItems
    {
        get
        {
            if (_currentSelectedItems is null)
            {
                return _currentEntry is null ? [] : [_currentEntry];
            }

            return _currentSelectedItems;
        }
    }

    /// <summary>
    /// Gets a value indicating if the rename button is disabled.
    /// </summary>
    private bool IsRenameButtonDisabled
    {
        get
        {
            if (_isDisabled)
            {
                return true;
            }

            if (_currentSelectedItems is null)
            {
                return true;
            }

            var count = _currentSelectedItems.Count();

            if (count != 1)
            {
                return true;
            }

            var item = _currentSelectedItems.ElementAt(0);

            if (item.Data is not IRenamable r)
            {
                return true;
            }

            return !r.IsRenamable;
        }
    }

    /// <summary>
    /// Gets a value indicating if the download button is disabled.
    /// </summary>
    private bool IsDownloadButtonDisabled
    {
        get
        {
            if (_isDisabled)
            {
                return true;
            }

            if (_currentSelectedItems is null)
            {
                return true;
            }

            if (!_currentSelectedItems.Any())
            {
                return true;
            }

            return _currentSelectedItems.All(x => x.Data is IDownloadable d && !d.IsDownloadAllowed);
        }
    }

    /// <summary>
    /// Gets a value indicating if the delete button is disabled.
    /// </summary>
    private bool IsDeleteButtonDisabled
    {
        get
        {
            if (_isDisabled)
            {
                return true;
            }

            if (_currentSelectedItems is null)
            {
                return true;
            }

            if (!_currentSelectedItems.Any())
            {
                return true;
            }

            return _currentSelectedItems.All(x => x.Data is IDeletable d && !d.IsDeleteable);
        }
    }

    /// <summary>
    /// Gets a value indicating if the move button is disabled.
    /// </summary>
    private bool IsMoveToButtonDisabled
    {
        get
        {
            if (_isDisabled)
            {
                return true;
            }

            if (_currentSelectedItems is null)
            {
                return true;
            }

            if (!_currentSelectedItems.Any())
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Gets the current entry to use depending of the view.
    /// </summary>
    /// <returns>Returns the entry to use depending of the view.</returns>
    private FileManagerEntry<TItem>? GetEntry()
    {
        return _searchEntry is not null ? _searchEntry : FileStructureView == FileStructureView.Hierarchical ? _currentEntry : _flattenEntry;
    }

    /// <summary>
    /// Occurs when the user wants to move a file or a folder into an another folder.
    /// </summary>
    /// <returns>Returns a task which moves the file or the folder into the selected folder when completed.</returns>
    private async Task OnMoveAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<FileMoverDialog<TItem>>(Root, new()
        {
            Width = View == FileManagerView.Mobile ? "100%" : null,
            Height = View == FileManagerView.Mobile ? "100%" : null,
            Title = FileManagerLabels.MoveToLabel,
            PrimaryAction = FileManagerLabels.DialogOkLabel,
            SecondaryAction = FileManagerLabels.DialogCancelLabel,
        });

        var result = await dialog.Result;

        if (!result.Cancelled &&
            result.Data is FileManagerEntry<TItem> data)
        {
            SetDisabled(true);
            _fileManagerView?.SetBusy(true);

            // Check if the destination folder isn't the source folder
            if (!(_currentSelectedItems.Count() == 1 &&
               string.Equals(_currentSelectedItems.ElementAtOrDefault(0)?.Id, data.Id, StringComparison.OrdinalIgnoreCase)))
            {
                _progressState = ProgressState.Moving;
                Root.Remove(_currentSelectedItems);
                data.AddRange([.. _currentSelectedItems]);
                BuildTreeView();

                if (Moved.HasDelegate)
                {
                    await Moved.InvokeAsync(new(data, _currentSelectedItems));
                }
            }

            _currentSelectedItems = [];
            OnUpdateEntry(new(Root));
            _fileManagerView?.SetBusy(false);
            SetDisabled(false);

            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Shows an error message when the user selects a too big number of files.
    /// </summary>
    /// <param name="maximumFileCount">Maximum number of files the user can take in one upload.</param>
    /// <returns>Returns a task that show an error dialog message when completed.</returns>
    private async Task OnFileCountExceededAsync(int maximumFileCount)
    {
        var dialog = await DialogService.ShowErrorAsync(
            FileManagerLabels.ExceededFileCountMessage,
            FileManagerLabels.ExceededFileCountTitle,
            FileManagerLabels.DialogOkLabel);

        await dialog.Result;
    }

    /// <summary>
    /// Build the flat entry.
    /// </summary>
    private void BuildFlatView()
    {
        _flattenEntry.Clear();

        foreach (var item in Root.Enumerate())
        {
            if (!item.IsDirectory)
            {
                _flattenEntry.AddRange(item);
            }
            else
            {
                BuildFlatViewItem(_flattenEntry, item);
            }
        }
    }

    /// <summary>
    /// Build a flat item.
    /// </summary>
    /// <param name="entry">Entry to use to append the <paramref name="item"/>.</param>
    /// <param name="item">Item to add in the entry.</param>
    private static void BuildFlatViewItem(FileManagerEntry<TItem> entry, FileManagerEntry<TItem> item)
    {
        entry.AddRange([.. item.GetFiles()]);

        foreach (var d in item.GetDirectories())
        {
            BuildFlatViewItem(entry, d);
        }
    }

    /// <summary>
    /// Change the sort of the view.
    /// </summary>
    /// <param name="sortBy">Sort to use.</param>
    private void OnChangeSort(FileSortBy sortBy)
    {
        State.SortBy = sortBy;
    }

    /// <summary>
    /// Change the view of the file manager.
    /// </summary>
    /// <param name="view">View to use.</param>
    private void OnChangeView(FileView view)
    {
        State.View = view;
    }

    /// <summary>
    /// Sort the entries in ascending order.
    /// </summary>
    private void OnSortAscending()
    {
        State.SortMode = FileSortMode.Ascending;
    }

    /// <summary>
    /// Sort the entries in descending order.
    /// </summary>
    private void OnSortDescending()
    {
        State.SortMode = FileSortMode.Descending;
    }

    /// <summary>
    /// Occurs when a folder is created in asynchronous way.
    /// </summary>
    /// <param name="e">Event args for the created folder.</param>
    /// <returns>Returns a task which raised the <see cref="OnFolderCreated"/> event callback when completed.</returns>
    private async Task OnFolderCreatedAsync(CreateFileManagerEntryEventArgs<TItem> e)
    {
        SetDisabled(true);
        _fileManagerView?.SetBusy(true);

        UpdateTreeView(e);
        UpdatePathBar(e);

        if (OnFolderCreated.HasDelegate)
        {
            await OnFolderCreated.InvokeAsync(e);
        }

        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

    /// <summary>
    /// Updates the tree view when a folder is created.
    /// </summary>
    /// <param name="e">Event args of the created folder.</param>
    private void UpdateTreeView(CreateFileManagerEntryEventArgs<TItem> e)
    {
        if (View == FileManagerView.Desktop)
        {
            var item = FindTreeViewItem(_treeViewItems, e.Parent.Id);

            if (item is not null)
            {
                var subItems = item.Items?.ToList() ?? [];
                subItems.Add(BuildTreeViewItem(e.Entry));

                subItems.Sort(FileManagerEntryTreeViewItemComparer.Default);

                item.Items = subItems;
            }
        }
    }

    /// <summary>
    /// Updates the path bar when a folder is created.
    /// </summary>
    /// <param name="e">Event args of the created folder.</param>
    private void UpdatePathBar(CreateFileManagerEntryEventArgs<TItem> e)
    {
        var item = PathBarItemBuilder.Find([_rootPath!], e.Parent.Id, true);

        if (item is not null)
        {
            var subItems = item.Items?.ToHashSet(PathBarItemEqualityComparer.Default) ?? [];
            var itemsToAdd = PathBarItemBuilder.From([e.Entry]);

            foreach (var i in itemsToAdd)
            {
                subItems.Add(i);
            }

            var l = subItems.ToList();
            l.Sort(PathBarItemComparer.Default);

            item.Items = [.. l];

            _fileManagerView?.InvalidatePathBarItemSize(e.Parent.Id);
        }
    }

    /// <summary>
    /// Occurs when the user clicks on the show details button.
    /// </summary>
    /// <returns>Returns a task which show the details of the current selected entries.</returns>
    private async Task OnShowDetailsAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<FileManagerDetailsDialog<TItem>>(new FileManagerDetailsDialogContent<TItem>(
                FileExtensionTypeLabels,
                (_currentSelectedItems.Any() ? _currentSelectedItems : _currentEntry is null ? [] : [_currentEntry])
            ),
            new DialogParameters()
            {
                SecondaryAction = null,
                PrimaryAction = FileManagerLabels.DialogOkLabel,
                Width = "100%"
            }
        );

        await dialog.Result;
    }

    /// <summary>
    /// Occurs when the user clicks on the rename button.
    /// </summary>
    /// <param name="entry">Entry to rename.</param>
    /// <returns>Returns a task which rename the entry when completed.</returns>
    internal async Task OnRenameAsync(FileManagerEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return;
        }

        var dialog = await DialogService.ShowDialogAsync<FileManagerDialog>(
            new FileManagerContent(FileManagerLabels.FileLabel, FileManagerLabels.FilePlaceholder, entry.NameWithoutExtension, entry.IsDirectory, true),
            new DialogParameters()
            {
                PreventDismissOnOverlayClick = true,
                Title = entry.IsDirectory ? FileManagerLabels.RenameFolderDialogTitle : FileManagerLabels.RenameFileDialogTitle
            }
        );

        var result = await dialog.Result;

        SetDisabled(true);
        _fileManagerView?.SetBusy(true);

        if (!result.Cancelled &&
            result.Data is string s &&
            _fileManagerView is not null)
        {
            entry.SetName(s);

            if (View == FileManagerView.Desktop &&
                entry.IsDirectory)
            {
                var item = FindTreeViewItem(_treeViewItems, entry.Id);

                item?.Text = entry.Name;
            }

            PathBarItemBuilder.UpdateLabel(_rootPath, entry.Id, entry.Name);
        }

        if (OnRename.HasDelegate)
        {
            await OnRename.InvokeAsync(entry);
        }

        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

    /// <summary>
    /// Build the complete path root for the <see cref="FluentCxFileManager{TItem}"/>.
    /// </summary>
    private void BuildPathRoot()
    {
        _rootPath = new PathBarItem()
        {
            Label = Root.Name,
            Id = Root.Id,
            Items = PathBarItemBuilder.From(Root.GetDirectories()) 
        };
    }

    /// <summary>
    /// Occurs when the path is changed.
    /// </summary>
    /// <param name="path">Represents the new path.</param>
    private void OnPathChanged(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        _path = path;
        var entry = FileManagerEntry<TItem>.Find(Root, x => x.RelativePath == CleanPath(path));

        if (entry is not null)
        {
            _currentEntry = entry;

            if (View == FileManagerView.Desktop)
            {
                var node = FindTreeViewItem(_treeViewItems, _currentEntry.Id);

                if (node is not null)
                {
                    node.Expanded = true;
                    _currentTreeViewItem = node;
                }
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// Build a tree view.
    /// </summary>
    /// <remarks>The tree view is only build in <see cref="FileManagerView.Desktop"/></remarks>
    private void BuildTreeView()
    {
        if (View == FileManagerView.Desktop)
        {
            _currentTreeViewItem = null;
            _treeViewItems = null;

            var root = BuildTreeViewItem(Root, true);
            List<TreeViewItem> items = [];
            items.Add(root);

            _treeViewItems = items;
            _currentTreeViewItem = FindTreeViewItem(_treeViewItems, Root.Id);
        }
    }

    /// <summary>
    /// Build a <see cref="TreeViewItem"/>.
    /// </summary>
    /// <param name="entry">Entry to use to create the item.</param>
    /// <param name="isExpanded">Value indicating if the node is expanded or not.</param>
    /// <returns>Returns the created <see cref="TreeViewItem"/>.</returns>
    private static TreeViewItem BuildTreeViewItem(
        FileManagerEntry<TItem> entry,
        bool isExpanded = false)
    {
        return new()
        {
            IconCollapsed = IconCollapsed,
            IconExpanded = IconExpanded,
            Text = entry.Name,
            Id = entry.Id,
            Expanded = isExpanded,
            Items = entry.GetDirectories().Select(x => BuildTreeViewItem(x)).ToList()
        };
    }

    /// <summary>
    /// Updates the selected entry.
    /// </summary>
    /// <param name="e">The current selected entry.</param>
    private void OnUpdateEntry(FileManagerEntryEventArgs<TItem> e)
    {
        if (View == FileManagerView.Desktop)
        {
            var node = FindTreeViewItem(_treeViewItems, e.Entry.Id);

            if (node is not null)
            {
                node.Expanded = true;
                _currentTreeViewItem = node;
            }
        }

        _currentEntry = e.Entry;
        _path = _currentEntry.RelativePath;
    }

    /// <summary>
    /// Finds the <see cref="TreeViewItem"/> specified by <paramref name="id"/> inside the <paramref name="items"/> nodes.
    /// </summary>
    /// <param name="items">Items to use for the search.</param>
    /// <param name="id">Identifier of the item.</param>
    /// <returns>Returns the item if found, <see langword="null" /> otherwise.</returns>
    private static ITreeViewItem? FindTreeViewItem(IEnumerable<ITreeViewItem>? items, string? id)
    {
        if (items is null || !items.Any() || string.IsNullOrEmpty(id))
        {
            return null;
        }

        foreach (var item in items)
        {
            if (string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            var node = FindTreeViewItem(item.Items, id);

            if (node is not null)
            {
                node.Expanded = true;
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Updates the current entry.
    /// </summary>
    private void OnUpdateCurrentEntry()
    {
        if (View == FileManagerView.Desktop &&
            _currentTreeViewItem is not null)
        {
            var node = FileManagerEntry<TItem>.Find(Root, _currentTreeViewItem.Id);

            if (node is not null)
            {
                _currentSelectedItems = [];
                _currentEntry = node;
                _path = _currentEntry.IsDirectory ? _currentEntry.RelativePath : Path.GetDirectoryName(_currentEntry.RelativePath);
            }
        }
    }

    /// <summary>
    /// Clean the path (removes the last backslash if found)
    /// </summary>
    /// <param name="path">Path to clean.</param>
    /// <returns>Returns the cleaned path.</returns>
    private static string CleanPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        if (path.EndsWith('\\'))
        {
            path = path[..^1];
        }

        return path;
    }

    /// <summary>
    /// Occurs when the user wants to create a folder.
    /// </summary>
    /// <returns>Returns a task which creates a folder when completed.</returns>
    private async Task OnCreateFolderAsync()
    {
        if (_currentEntry is not null &&
            _currentEntry.IsDirectory)
        {
            var dialog = await DialogService.ShowDialogAsync<FileManagerDialog>(
                new FileManagerContent(FileManagerLabels.FolderLabel, FileManagerLabels.FolderPlaceholder, null, true, false),
                new DialogParameters()
                {
                    Title = FileManagerLabels.FolderDialogTitle,
                    PrimaryAction = FileManagerLabels.DialogOkLabel,
                    SecondaryAction = FileManagerLabels.DialogCancelLabel
                });

            var result = await dialog.Result;

            if (!result.Cancelled &&
                result.Data is string s)
            {
                var count = _currentEntry.GetDirectories().Count(x => string.Equals(s, x.Name, StringComparison.OrdinalIgnoreCase));
                var dirEntry = _currentEntry.CreateDirectory(s!);
                dirEntry.Count = count + 1;

                await OnFolderCreatedAsync(new(_currentEntry, dirEntry));
            }
        }
    }

    /// <summary>
    /// Download a single entry as a file.
    /// </summary>
    /// <param name="e">Entry to download.</param>
    /// <returns>Returns a task which download a file when completed.</returns>
    private async Task OnDownloadSingleAsync(FileManagerEntry<TItem> e)
    {
        await OnDownloadMultiAsync([e]);
    }

    /// <summary>
    /// Download a multiple entries as a zip file.
    /// </summary>
    /// <param name="items">Entries to download.</param>
    /// <returns>Returns a task which download a zip file when completed.</returns>
    private async Task OnDownloadMultiAsync(IEnumerable<FileManagerEntry<TItem>> items)
    {
        SetDisabled(true);
        _fileManagerView?.SetBusy(true);
        _progressState = ProgressState.Downloading;

        if (items.Any())
        {
            if (items.Count() == 1)
            {
                var item = items.First();

                if (item.IsDirectory)
                {
                    await ZipAsync([item]);
                }
                else
                {
                    var data = await item.GetBytesAsync();

                    if (data.Length > 0)
                    {
                        await DownloadFileAsync(item.Name, data, item.Extension);
                    }
                }
            }
            else
            {
                await ZipAsync(items);
            }
        }

        SetDisabled(false);
        _fileManagerView?.SetBusy(false);
        _progressState = ProgressState.None;

        async Task ZipAsync(IEnumerable<FileManagerEntry<TItem>> items)
        {
            var zip = await FileZipper.ZipAsync(items);

            if (zip is not null)
            {
                var data = await zip.GetBytesAsync();

                if (data.Length > 0)
                {
                    await DownloadFileAsync(zip.Name, data, zip.Extension);
                }
            }
        }
    }

    /// <summary>
    /// Download a file in an asynchronous way.
    /// </summary>
    /// <param name="filename">Name of the file.</param>
    /// <param name="data">Binary content of the file.</param>
    /// <param name="extension">Extension of the file.</param>
    /// <returns>Returns a task which download the file when completed.</returns>
    private async Task DownloadFileAsync(string filename, byte[] data, string? extension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filename);
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        await InitializeStreamAsync(filename);
        await StreamAsync(filename, data);
        await DownloadStreamAsync(filename, extension);

        async ValueTask InitializeStreamAsync(string fileName)
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("initializeStream", fileName);
            }
        }

        async ValueTask StreamAsync(string filename, byte[] content)
        {
            if (_module is not null)
            {
                var bufferSize = 1024 * 10;
                var length = 0;
                var buffer = new byte[bufferSize];

                while (length < content.Length)
                {
                    if (length + buffer.Length > content.Length)
                    {
                        buffer = new byte[content.Length - length];
                    }

                    Array.Copy(content, length, buffer, 0, buffer.Length);
                    length += buffer.Length;

                    await _module.InvokeVoidAsync("stream", filename, buffer);
                }
            }
        }

        async ValueTask DownloadStreamAsync(string filename, string? extension = null)
        {
            if (_module is not null)
            {
                if (string.IsNullOrEmpty(extension))
                {
                    await _module.InvokeVoidAsync("downloadStream", filename, "_self", "application/octet-stream");
                }

                if (!_contentTypeProvider.TryGetContentType(extension, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                await _module.InvokeVoidAsync("downloadStream", filename, "_self", contentType);
            }
        }
    }

    /// <summary>
    /// Occurs when the user clicks the download button.
    /// </summary>
    /// <returns>Returns a task which download the selected files when completed.</returns>
    private async Task OnDownloadAsync()
    {
        await OnDownloadMultiAsync(_currentSelectedItems);
    }

    /// <summary>
    /// Deletes an entry in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which deletes the entry when completed.</returns>
    internal async Task OnDeleteAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync(
            FileManagerLabels.DeleteDescriptionLabel!,
            FileManagerLabels.DialogYesLabel!,
            FileManagerLabels.DialogNoLabel!,
            FileManagerLabels.DeleteTitle);

        var result = await dialog.Result;
        var entry = _searchEntry ?? _currentEntry;

        if (!result.Cancelled &&
            entry is not null)
        {
            _progressState = ProgressState.Deleting;
            SetDisabled(true);
            _fileManagerView?.SetBusy(true);

            if (View == FileManagerView.Desktop)
            {
                var item = FindTreeViewItem(_treeViewItems, entry.Id);

                if (item is not null)
                {
                    var subItems = item.Items?.ToList() ?? [];

                    foreach (var subItem in _currentSelectedItems)
                    {
                        var itemToRemove = FindTreeViewItem(subItems, subItem.Id);

                        if (itemToRemove is not null)
                        {
                            subItems.Remove(itemToRemove);
                        }
                    }

                    subItems.Sort(FileManagerEntryTreeViewItemComparer.Default);
                    item.Items = subItems;
                }
            }

            DeleteFromPathBar(_currentSelectedItems.Select(x => x.Id));
            RemoveSelectedItemFromMainEntries(entry.Id, _currentSelectedItems);

            if (OnDelete.HasDelegate)
            {
                await OnDelete.InvokeAsync(new(entry, _currentSelectedItems));
            }

            _fileManagerView?.SetBusy(false);
            SetDisabled(false);
            _progressState = ProgressState.None;
        }
    }

    /// <summary>
    /// Deletes the items from the path bar.
    /// </summary>
    /// <param name="idCollection">Collection of segments to remove from the pathbar.</param>
    private void DeleteFromPathBar(IEnumerable<string> idCollection)
    {
        PathBarItemBuilder.Remove(_rootPath, idCollection);
        _fileManagerView?.ClearPathBar(idCollection);
    }

    /// <summary>
    /// Removes the <paramref name="items"/> from the node specified by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Identifier of the node.</param>
    /// <param name="items">Items to remove.</param>
    private void RemoveSelectedItemFromMainEntries(string id, IEnumerable<FileManagerEntry<TItem>> items)
    {
        var internalEntry = FileManagerEntry<TItem>.Find(Root, id);
        _flattenEntry.Remove(items);

        if (internalEntry is null)
        {
            return;
        }

        internalEntry.Remove(items);
    }

    /// <summary>
    /// Search the entries.
    /// </summary>
    private void OnSearchEntries()
    {
        if (string.IsNullOrEmpty(_searchValue))
        {
            _searchEntry = null;
        }
        else
        {
            var entry = FileStructureView == FileStructureView.Hierarchical ? _currentEntry : _flattenEntry;

            var items = FileManagerEntry<TItem>.FindByName(entry, _searchValue);
            _searchEntry = FileManagerEntry<TItem>.Home;
            _searchEntry.AddRange([.. items]);
        }
    }

    /// <summary>
    /// Disable the component.
    /// </summary>
    /// <param name="isDisabled">Value indicating if the component is disabled or not.</param>
    private void SetDisabled(bool isDisabled)
    {
        _isDisabled = isDisabled;

        if (View == FileManagerView.Desktop)
        {
            SetDisabled(_treeViewItems, isDisabled);
        }
    }

    /// <summary>
    /// Disable all the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items">Items to disable.</param>
    /// <param name="isDisabled">Value indicating if the items are disabled or not.</param>
    private static void SetDisabled(IEnumerable<ITreeViewItem>? items, bool isDisabled)
    {
        if (items is null)
        {
            return;
        }

        foreach (var item in items)
        {
            item.Disabled = isDisabled;
            SetDisabled(item.Items, isDisabled);
        }
    }

    /// <summary>
    /// Occurs when a file is uploading.
    /// </summary>
    /// <param name="e">Event args of the current uploading file.</param>
    private void OnProgressChange(FluentInputFileEventArgs e)
    {
        SetDisabled(true);
        _fileManagerView?.SetBusy(true);
        _progressState = ProgressState.Uploading;

        ProgressPercent = e.ProgressPercent;

        if (!_fileBufferDictionary.TryGetValue(e.Index, out _))
        {
            MemoryStream? lastData = new();
            lastData.Write(e.Buffer.Data, 0, e.Buffer.BytesRead);
            _fileBufferDictionary.Add(e.Index, lastData);
        }
        else
        {
            _fileBufferDictionary[e.Index].Write(e.Buffer.Data, 0, e.Buffer.BytesRead);
        }
    }

    /// <summary>
    /// Occurs when a file is uploaded.
    /// </summary>
    /// <param name="e">Event args of the uploaded file.</param>
    /// <returns>Returns a task which raised the <see cref="OnFileUploaded"/> event callback when completed.</returns>
    private async Task OnFileUploadedAsync(FluentInputFileEventArgs e)
    {
        var ms = _fileBufferDictionary[e.Index];
        var name = e.Name;
        _fileBufferDictionary.Remove(e.Index);

        var data = ms.ToArray();
        ms.Dispose();

        var newEntry = FileManagerEntry<TItem>.CreateEntry(
            data,
            name,
            data.Length);

        _currentEntry?.AddRange(newEntry);
        _flattenEntry.AddRange(newEntry);

        if (OnFileUploaded.HasDelegate)
        {
            await OnFileUploaded.InvokeAsync(newEntry);
        }
    }

    /// <summary>
    /// Occurs when all file have been uploaded.
    /// </summary>
    /// <param name="_">Event args of all uploaded files.</param>
    private void OnCompleted(IEnumerable<FluentInputFileEventArgs> _)
    {
        _progressState = ProgressState.None;
        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

    /// <summary>
    /// Gets the label for the progress part.
    /// </summary>
    /// <returns>Returns the label for the progress part.</returns>
    private string? GetProgressLabelFromState()
    {
        return _progressState switch
        {
            ProgressState.Uploading => FileManagerLabels.UploadingLabel,
            ProgressState.Downloading => FileManagerLabels.DownloadingLabel,
            ProgressState.Deleting => FileManagerLabels.DeletingLabel,
            ProgressState.Moving => FileManagerLabels.MovingLabel,
            _ => null
        };
    }

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender && Root is not null)
        {
            _currentSelectedItems = [];
            _currentEntry = Root;
            _path = _currentEntry.RelativePath;
            StateHasChanged();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
        }
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasRootChanged)
        {
            BuildFlatView();
            BuildTreeView();
            BuildPathRoot();
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _hasRootChanged = parameters.HasValueChanged(nameof(Root), Root);

        await base.SetParametersAsync(parameters);
    }
}
