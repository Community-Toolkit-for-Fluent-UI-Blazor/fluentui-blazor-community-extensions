using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides tree view management functionality for file system items, enabling creation, expansion, and refresh
/// operations for hierarchical file navigation in a file manager context.
/// </summary>
/// <remarks>This engine coordinates between a core file manager and a tree view representation, maintaining an
/// index of tree nodes for efficient lookup and update. It supports asynchronous loading and expansion of directory
/// nodes, and is intended for use in scenarios where a dynamic, interactive file tree is required.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each file or directory entry. Must be a reference type with a
/// parameterless constructor.</typeparam>
internal sealed class FileManagerTreeEngine<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Provides a case-insensitive index of tree view items by their string keys.
    /// </summary>
    /// <remarks>This dictionary enables efficient lookup of tree view items using string keys, ignoring
    /// character casing. It is intended for internal use to support fast access and management of tree view
    /// nodes.</remarks>
    private readonly Dictionary<string, TreeViewItem> _treeIndex = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Provides the core engine for file management operations.
    /// </summary>
    private readonly FileManagerEngine<TItem> _core;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileManagerTreeEngine{TItem}"/> class with the specified core file manager engine.
    /// </summary>
    /// <param name="core">Core engine responsible for file management operations, providing access to file entries and their hierarchical structure.</param>
    public FileManagerTreeEngine(FileManagerEngine<TItem> core)
    {
        _core = core;
    }

    /// <summary>
    /// Asynchronously creates and initializes the root node of the tree view, including loading its immediate children.
    /// </summary>
    /// <remarks>The returned root node will have its Expanded property set to <see langword="true"/> and its
    /// children loaded. The provided callback is used for handling expansion events during initialization and can be
    /// reused for further expansion events.</remarks>
    /// <param name="onExpandedAsync">A callback that is invoked asynchronously when a tree view item is expanded. The callback receives event
    /// arguments describing the expansion event.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the initialized root tree view item
    /// with its children loaded and expanded.</returns>
    public async ValueTask<TreeViewItem> CreateRootAsync(Func<TreeViewItemExpandedEventArgs, Task> onExpandedAsync)
    {
        var root = ToTreeViewItem(_core.MasterRoot, onExpandedAsync);

        await LoadChildrenAsync(root, onExpandedAsync);
        root.Expanded = true;
        return root;
    }

    /// <summary>
    /// Creates a new TreeViewItem that represents the specified file or directory entry, configuring its display and
    /// expansion behavior.
    /// </summary>
    /// <remarks>If the entry represents a directory, the returned TreeViewItem is initialized with loading
    /// placeholder items and folder icons for collapsed and expanded states. The onExpandedAsync callback is assigned
    /// to handle expansion events.</remarks>
    /// <param name="entry">The file or directory entry to represent as a TreeViewItem. Must not be null.</param>
    /// <param name="onExpandedAsync">A callback function that is invoked asynchronously when the TreeViewItem is expanded. This function receives the
    /// expansion event arguments.</param>
    /// <returns>A TreeViewItem configured to represent the specified entry, with appropriate icons and expansion behavior.</returns>
    public TreeViewItem ToTreeViewItem(
        FileEntry<TItem> entry,
        Func<TreeViewItemExpandedEventArgs, Task> onExpandedAsync)
    {
        var item = new TreeViewItem
        {
            Id = entry.Id,
            Text = entry.Name,
            OnExpandedAsync = onExpandedAsync,

            IconCollapsed = entry.IsDirectory
                ? new Size16.Folder().WithColor(Color.Primary)
                : null,

            IconExpanded = entry.IsDirectory
                ? new Size16.FolderOpen().WithColor(Color.Primary)
                : null,

            Items = entry.IsDirectory
                ? TreeViewItem.LoadingTreeViewItems("Loading...")
                : null
        };

        _treeIndex[entry.Id] = item;

        return item;
    }

    /// <summary>
    /// Asynchronously loads the child items for the specified tree view node and updates its item collection.
    /// </summary>
    /// <remarks>If the specified node does not correspond to a directory, its items are cleared. The method
    /// updates the node's items with the loaded children, each configured to use the provided expansion
    /// callback.</remarks>
    /// <param name="uiNode">The tree view node whose children are to be loaded. Must represent a directory entry; otherwise, its items will
    /// be set to null.</param>
    /// <param name="onExpandedAsync">A callback function that is invoked asynchronously when a tree view item is expanded. Used to handle expansion
    /// events for dynamically loaded child nodes.</param>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    public async Task LoadChildrenAsync(
        ITreeViewItem uiNode,
        Func<TreeViewItemExpandedEventArgs, Task> onExpandedAsync)
    {
        var entry = _core.FindById(uiNode.Id);

        if (entry is null || !entry.IsDirectory)
        {
            uiNode.Items = null;
            return;
        }

        var children = await _core.LoadChildrenAsync(entry);

        uiNode.Items = children
            .Select(c => ToTreeViewItem(c, onExpandedAsync))
            .ToList();
    }

    /// <summary>
    /// Asynchronously refreshes the tree view starting from the specified parent node,
    ///  optionally focusing on a new node after refresh.
    /// </summary>
    /// <param name="parentId">The identifier of the parent node to refresh. The method will attempt to find this node in the tree index and refresh its children. If the node is not found, the method returns null.</param>
    /// <param name="newId">The identifier of a new node to focus on after the refresh operation. If this parameter is provided and the corresponding node is found in the tree index after refreshing, that node will be returned. If the newId is null or empty, or if the corresponding node is not found, the method will return null.</param>
    /// <param name="onNodeExpandedAsync">The callback function to be used for handling expansion events during the refresh operation. This function will be passed to the LoadChildrenAsync method to ensure that any newly loaded nodes have the appropriate expansion behavior configured.</param>
    /// <returns>Returns a task that represents the asynchronous refresh operation. The task result contains the tree view item corresponding to the newId if it is found after the refresh; otherwise, it returns null.</returns>
    public async Task<ITreeViewItem?> RefreshAsync(
        string parentId,
        string? newId,
        Func<TreeViewItemExpandedEventArgs, Task> onNodeExpandedAsync)
    {
        if (!_treeIndex.TryGetValue(parentId, out var parentUiNode))
        {
            return null;
        }

        if (!parentUiNode.Expanded)
        {
            parentUiNode.Expanded = true;
        }

        await LoadChildrenAsync(parentUiNode, onNodeExpandedAsync);

        if (!string.IsNullOrEmpty(newId) &&
            _treeIndex.TryGetValue(newId, out var newUiNode))
        {
            return newUiNode;
        }

        return null;
    }

    /// <summary>
    /// Attempts to retrieve a tree view item with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tree view item to locate. Cannot be null.</param>
    /// <param name="node">When this method returns, contains the tree view item associated with the specified identifier, if found;
    /// otherwise, null. This parameter is passed uninitialized.</param>
    /// <returns>true if an item with the specified identifier is found; otherwise, false.</returns>
    public bool TryGetItem(string id, out TreeViewItem? node)
    {
        return _treeIndex.TryGetValue(id, out node);
    }

    /// <summary>
    /// Asynchronously refreshes the tree hierarchy and returns the root item after reinitialization.
    /// </summary>
    /// <remarks>Call this method to reload the entire tree structure, typically after changes to the
    /// underlying data source. The provided callback is used to handle expansion events during the refresh
    /// process.</remarks>
    /// <param name="onExpandedAsync">A callback function that is invoked asynchronously when a tree view item is expanded. The function receives
    /// event arguments describing the expansion.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the root tree view item after the
    /// hierarchy has been refreshed.</returns>
    public async Task<TreeViewItem> RefreshHierarchyAsync(Func<TreeViewItemExpandedEventArgs, Task> onExpandedAsync)
    {
        await _core.InitializeAsync();
        _treeIndex.Clear();

        return await CreateRootAsync(onExpandedAsync);
    }
}

