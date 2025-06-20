using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Components.FileManager;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxFileManager<TItem>
    : FluentComponentBase where TItem : class, new()
{
    private enum ProgressState
    {
        None,
        Uploading,
        Downloading,
        Deleting,
        Moving
    }

    private bool _showDetails;
    private FileManager<TItem>? _fileManagerView;
    private FileManagerEntry<TItem>? _currentEntry;
    private IEnumerable<TreeViewItem>? _treeViewItems = [];
    private static readonly Icon IconCollapsed = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Folder();
    private static readonly Icon IconExpanded = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.FolderOpen();
    private ITreeViewItem? _currentTreeViewItem;
    private IEnumerable<FileManagerEntry<TItem>> _currentSelectedItems = [];
    private string? _searchValue;
    private FileManagerEntry<TItem>? _searchEntry;
    private ProgressState _progressState = ProgressState.None;
    private bool _isDisabled;
    private readonly Dictionary<int, MemoryStream> _fileBufferDictionary = [];
    private readonly List<FileNavigationItem> _navigationItems = [];
    private readonly FileManagerEntry<TItem> _flattenEntry;
    private IJSObjectReference? _module;
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/FileManager/FluentCxFileManager.razor.js";
    private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    public FluentCxFileManager() : base()
    {
        Id = Identifier.NewId();
        _flattenEntry = FileManagerEntry<TItem>.Home;
    }

    [Inject]
    internal FileManagerState State { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public bool ShowCreateFolderButton { get; set; } = true;

    [Parameter]
    public bool ShowUploadButton { get; set; } = true;

    [Parameter]
    public bool ShowMoveToButton { get; set; } = true;

    [Parameter]
    public bool ShowViewButton { get; set; } = true;

    [Parameter]
    public bool ShowSortButton { get; set; } = true;

    [Parameter]
    public bool ShowPropertiesButton { get; set; } = true;

    [Parameter]
    public bool ShowDetailsButton { get; set; } = true;

    [Parameter]
    public FileManagerLabels FileManagerLabels { get; set; } = FileManagerLabels.Default;

    [Parameter]
    public FileManagerDetailsLabels DetailsLabels { get; set; } = FileManagerDetailsLabels.Default;

    [Parameter]
    public FileExtensionTypeLabels FileExtensionTypeLabels { get; set; } = FileExtensionTypeLabels.Default;

    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    private int? ProgressPercent { get; set; }

    [Parameter]
    public bool IsBusy { get; set; }

    [Parameter]
    public FileManagerView View { get; set; } = FileManagerView.Desktop;

    [Parameter]
    public FileManagerEntry<TItem> Root { get; set; } = default!;

    [Parameter]
    public string? Accept { get; set; }

    [Parameter]
    public AcceptFile AcceptFiles { get; set; } = AcceptFile.None;

    [Parameter]
    public int MaximumFileCount { get; set; } = 100;

    [Parameter]
    public long MaximumFileSize { get; set; } = 1024 * 1024 * 100;

    [Parameter]
    public uint BufferSize { get; set; } = 1024 * 10;

    [Parameter]
    public EventCallback<CreateFileManagerEntryEventArgs<TItem>> OnFolderCreated { get; set; }

    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> OnRename { get; set; }

    [Parameter]
    public EventCallback<DeleteFileManagerEntryEventArgs<TItem>> OnDelete { get; set; }

    [Parameter]
    public EventCallback<FileManagerEntry<TItem>> OnFileUploaded { get; set; }

    [Parameter]
    public RenderFragment? ToolbarItems { get; set; }

    [Parameter]
    public FileStructureView FileStructureView { get; set; } = FileStructureView.Hierarchical;

    [Parameter]
    public EventCallback<FileManagerEntriesMovedEventArgs<TItem>> Moved { get; set; }

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

    private async Task OnMoveAsync()
    {
        var dialog = await DialogService.ShowDialogAsync<FileMoverDialog<TItem>>(Root, new()
        {
            Width = View == Components.FileManagerView.Mobile ? "100%" : null,
            Height = View == Components.FileManagerView.Mobile ? "100%" : null,
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

    private async Task OnFileCountExceededAsync(int maximumFileCount)
    {
        var dialog = await DialogService.ShowErrorAsync(
            FileManagerLabels.ExceededFileCountMessage,
            FileManagerLabels.ExceededFileCountTitle,
            FileManagerLabels.DialogOkLabel);

        await dialog.Result;
    }

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

    private static void BuildFlatViewItem(FileManagerEntry<TItem> entry, FileManagerEntry<TItem> item)
    {
        entry.AddRange([.. item.GetFiles()]);

        foreach (var d in item.GetDirectories())
        {
            BuildFlatViewItem(entry, d);
        }
    }

    private void OnChangeSort(FileSortBy sortBy)
    {
        State.SortBy = sortBy;
    }

    private void OnChangeView(FileView options)
    {
        State.View = options;
    }

    private void OnSortAscending()
    {
        State.SortMode = FileSortMode.Ascending;
    }

    private void OnSortDescending()
    {
        State.SortMode = FileSortMode.Descending;
    }

    private async Task OnFolderCreatedAsync(CreateFileManagerEntryEventArgs<TItem> e)
    {
        SetDisabled(true);
        _fileManagerView?.SetBusy(true);

        if (View == Components.FileManagerView.Desktop)
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

        if (OnFolderCreated.HasDelegate)
        {
            await OnFolderCreated.InvokeAsync(e);
        }

        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

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

            if (View == Components.FileManagerView.Desktop &&
                entry.IsDirectory)
            {
                var item = FindTreeViewItem(_treeViewItems, entry.Id);

                if (item is not null)
                {
                    item.Text = entry.Name;
                }
            }
        }

        if (OnRename.HasDelegate)
        {
            await OnRename.InvokeAsync(entry);
        }

        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

    private void BuildTreeView()
    {
        if (View == Components.FileManagerView.Desktop)
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

    private void OnUpdateEntry(FileManagerEntryEventArgs<TItem> e)
    {
        if (View == Components.FileManagerView.Desktop)
        {
            var node = FindTreeViewItem(_treeViewItems, e.Entry.Id);

            if (node is not null)
            {
                node.Expanded = true;
                _currentTreeViewItem = node;
            }
        }

        _currentEntry = e.Entry;
        UpdateNavigationView(e.Entry);
    }

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

    private void OnUpdateCurrentEntry()
    {
        if (View == Components.FileManagerView.Desktop &&
            _currentTreeViewItem is not null)
        {
            var node = FileManagerEntry<TItem>.Find(Root, _currentTreeViewItem.Id);

            if (node is not null)
            {
                _currentSelectedItems = [];
                _currentEntry = node;
                UpdateNavigationView(_currentEntry);
            }
        }
    }

    private void UpdateNavigationView(FileManagerEntry<TItem> entry)
    {
        _navigationItems.Clear();
        DefaultInterpolatedStringHandler handler = new();

        var segmentPath = GetSegmentsPath(entry.RelativePath);

        if (segmentPath.Length > 0)
        {
            handler.AppendLiteral(segmentPath[0]);
            handler.AppendLiteral("\\");

            _navigationItems.Add(new FileNavigationItem(handler.ToString(), null, OnClickItem));

            foreach (var item in segmentPath.Skip(1))
            {
                handler.AppendLiteral(item);

                _navigationItems.Add(new FileNavigationItem(handler.ToString(), item, OnClickItem));

                handler.AppendLiteral("\\");
            }

            handler.ToStringAndClear();
        }
    }

    private static string[] GetSegmentsPath(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return [];
        }

        return relativePath.Split('\\', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

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

    private void OnClickItem(string path)
    {
        var entry = FileManagerEntry<TItem>.Find(Root, x => x.RelativePath == CleanPath(path));

        if (entry is not null)
        {
            _currentEntry = entry;

            if (View == Components.FileManagerView.Desktop)
            {
                var node = FindTreeViewItem(_treeViewItems, _currentEntry.Id);

                if (node is not null)
                {
                    node.Expanded = true;
                    _currentTreeViewItem = node;
                }
            }

            UpdateNavigationView(_currentEntry);
            StateHasChanged();
        }
    }

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

    private async Task OnDownloadSingleAsync(FileManagerEntry<TItem> e)
    {
        await OnDownloadMultiAsync([e]);
    }

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

    private async Task OnDownloadAsync()
    {
        await OnDownloadMultiAsync(_currentSelectedItems);
    }

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

            if (View == Components.FileManagerView.Desktop)
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

            if (FileStructureView == FileStructureView.Hierarchical)
            {
                UpdateNavigationView(_searchEntry);
            }
        }
    }

    private void SetDisabled(bool isDisabled)
    {
        _isDisabled = isDisabled;

        if (View == Components.FileManagerView.Desktop)
        {
            SetDisabled(_treeViewItems, isDisabled);
        }
    }

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

    private void OnCompleted(IEnumerable<FluentInputFileEventArgs> _)
    {
        _progressState = ProgressState.None;
        _fileManagerView?.SetBusy(false);
        SetDisabled(false);
    }

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
            UpdateNavigationView(_currentEntry);
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
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Root), Root))
        {
            BuildFlatView();
            BuildTreeView();
        }
    }
}
