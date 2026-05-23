namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides core file management operations and hierarchical structure handling for file entries of type TItem, using a
/// specified file provider.
/// </summary>
/// <remarks>This engine is designed to support advanced file management scenarios, including recursive loading,
/// searching, and manipulation of file and directory entries. It maintains a logical root entry and exposes methods for
/// common operations such as renaming, moving, deleting, and creating entries. All operations adhere to the structure
/// and conventions of the underlying file provider. Thread safety is not guaranteed; concurrent access should be
/// managed externally if required.</remarks>
/// <typeparam name="TItem">The type of the file entry item managed by the engine. Must be a reference type with a parameterless constructor.</typeparam>
internal sealed class FileManagerEngine<TItem>
    : IDisposable
    where TItem : class, new()
{
    /// <summary>
    /// Represents the file provider used to retrieve file and directory information.
    /// </summary>
    private readonly IFileProvider<TItem> _provider;

    /// <summary>
    /// Represents the state of the file manager, which retains sort states.
    /// </summary>
    private readonly FileManagerState _state;

    /// <summary>
    /// Provides access to the file search service used for searching items of type TItem.
    /// </summary>
    /// <remarks>This field is intended for internal use within the containing class to perform file search
    /// operations. It is not exposed publicly.</remarks>
    private readonly FileSearchService<TItem> _searchService;

    /// <summary>
    /// Provides the service used to flatten file entry items of type TItem.
    /// </summary>
    private readonly FileEntryFlattenService<TItem> _flattenService = default!;

    /// <summary>
    /// Provides the comparer used to sort file entries of type TItem.
    /// </summary>
    private readonly FileEntrySortComparer<TItem> _comparer;

    /// <summary>
    /// Représente la racine logique utilisée par le FileManager (typiquement le MASTERROOT du composant).
    /// Elle peut être vide (aucun enfant).
    /// </summary>
    public FileEntry<TItem> MasterRoot { get; }

    /// <summary>
    /// Occurs when the sort order is updated.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the sorting criteria or order changes. Handlers
    /// can use this event to update UI elements or perform additional actions in response to sort changes.</remarks>
    public event EventHandler? SortUpdated;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileManagerEngine{TItem}"/> class with the specified file provider.
    /// </summary>
    /// <param name="provider">The file provider used to retrieve file and directory information. Cannot be null.</param>
    /// <param name="state">The state of the file manager, which retains sort states. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Occurs when the provided file provider is null.</exception>
    public FileManagerEngine(IFileProvider<TItem> provider, FileManagerState state)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _searchService = new FileSearchService<TItem>(provider);
        _flattenService = new FileEntryFlattenService<TItem>(provider);
        _comparer = new FileEntrySortComparer<TItem>(state);
        MasterRoot = new(
            id: "home",
            name: "Home",
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new(),
            parent: null);

        _state.SortChanged += OnSortChanged;
        _state.SortLayoutChanged += OnSortChanged;
    }

    /// <summary>
    /// Initializes the component asynchronously by loading its child elements.
    /// </summary>
    /// <remarks>Call this method to ensure that all child elements are loaded before interacting with the
    /// component. This method should be awaited to guarantee that initialization completes before further actions are
    /// taken.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    public async Task InitializeAsync()
    {
        await LoadChildrenAsync(MasterRoot);
    }

    /// <summary>
    /// Asynchronously loads all child entries of the specified parent directory and its subdirectories recursively.
    /// </summary>
    /// <remarks>This method traverses the directory tree starting from the specified parent, loading all
    /// child entries recursively. Only directory entries are traversed further; non-directory entries are not processed
    /// recursively.</remarks>
    /// <param name="parent">The parent directory entry for which to load child entries. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task LoadChildrenRecursiveAsync(FileEntry<TItem> parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var children = await LoadChildrenAsync(parent);

        foreach (var child in children)
        {
            if (child.IsDirectory)
            {
                await LoadChildrenRecursiveAsync(child);
            }
        }
    }

    /// <summary>
    /// Asynchronously loads the child entries of the specified parent directory entry.
    /// </summary>
    /// <remarks>The returned list reflects the current children of the parent after loading. If the parent is
    /// not a directory, no children are loaded and an empty list is returned.</remarks>
    /// <param name="parent">The parent file entry for which to load child entries. Must represent a directory; cannot be null.</param>
    /// <returns>A list of child file entries contained within the specified parent directory. Returns an empty list if the
    /// parent is not a directory or has no children.</returns>
    public async Task<IList<FileEntry<TItem>>> LoadChildrenAsync(FileEntry<TItem> parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if (!parent.IsDirectory)
        {
            return [];
        }

        parent.Children.Clear();

        var descriptors = await _provider.GetChildrenAsync(parent.Id) ?? [];

        foreach (var desc in descriptors)
        {
            var child = Map(desc, parent);
            parent.AddChild(child);
        }

        return parent.Children;
    }

    /// <summary>
    /// Searches for a file entry with the specified identifier and returns it if found.
    /// </summary>
    /// <param name="id">The unique identifier of the file entry to locate. Cannot be null, empty, or consist only of white-space
    /// characters.</param>
    /// <returns>The file entry associated with the specified identifier if found; otherwise, null.</returns>
    public FileEntry<TItem>? FindById(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return FindByIdRecursive(MasterRoot, id);
    }

    /// <summary>
    /// Searches recursively for a file entry with the specified identifier within the file entry hierarchy.
    /// </summary>
    /// <remarks>This method traverses the hierarchy of file entries starting from the specified root. The
    /// search is performed in a depth-first manner.</remarks>
    /// <param name="current">The root file entry from which the recursive search begins. Must not be null.</param>
    /// <param name="id">The identifier of the file entry to locate. Comparison is case-sensitive.</param>
    /// <returns>The file entry whose identifier matches the specified value, or null if no matching entry is found.</returns>
    private static FileEntry<TItem>? FindByIdRecursive(FileEntry<TItem> current, string id)
    {
        if (current.Id == id)
        {
            return current;
        }

        foreach (var child in current.Children)
        {
            var found = FindByIdRecursive(child, id);

            if (found is not null)
            {
                return found;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns an enumerable collection representing the path from the specified file entry to the root of the
    /// hierarchy.
    /// </summary>
    /// <remarks>The returned collection includes the specified entry and all its ancestors, ordered from the
    /// root to the entry. This can be used to reconstruct the full path within a hierarchical file structure.</remarks>
    /// <param name="entry">The file entry for which to retrieve the path. Cannot be null.</param>
    /// <returns>An enumerable collection of file entries, starting from the root and ending with the specified entry.</returns>
    public static IEnumerable<FileEntry<TItem>> GetPath(FileEntry<TItem> entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        var list = new List<FileEntry<TItem>>();
        var current = entry;

        while (current is not null)
        {
            list.Insert(0, current);
            current = current.Parent;
        }

        return list;
    }

    /// <summary>
    /// Renames the specified file entry using the provided name without extension and updates its modification date.
    /// </summary>
    /// <param name="entry">The file entry to rename. Cannot be null.</param>
    /// <param name="newNameWithoutExtension">The new name to assign to the file entry, excluding the file extension.</param>
    public static void ApplyRename(FileEntry<TItem>? entry, string newNameWithoutExtension)
    {
        ArgumentNullException.ThrowIfNull(entry);
        entry.Rename(newNameWithoutExtension);
        entry.SetModifiedDate(DateTime.UtcNow);
    }

    /// <summary>
    /// Removes each specified file entry from its parent and updates parent sizes accordingly.
    /// </summary>
    /// <remarks>This method updates the parent entries after deletion to ensure their sizes reflect the
    /// removal of child entries. If an entry does not have a parent, it is ignored.</remarks>
    /// <param name="entries">A collection of file entries to be deleted. Each entry must have a valid parent to be removed from.</param>
    public static void ApplyDelete(IEnumerable<FileEntry<TItem>> entries)
    {
        var parents = FileEntry<TItem>.CaptureParents(entries);

        foreach (var entry in entries)
        {
            entry.Parent?.RemoveChild(entry);
        }

        foreach (var parent in parents)
        {
            parent.RecalculateSizes();
        }
    }

    /// <summary>
    /// Moves the specified file entries to the given destination entry, updating parent-child relationships and
    /// recalculating sizes as needed.
    /// </summary>
    /// <remarks>After the move, the sizes of the destination entry and all affected parent entries are
    /// recalculated to reflect the updated structure.</remarks>
    /// <param name="entries">The collection of file entries to move. Each entry will be removed from its current parent and added as a child
    /// to the destination entry.</param>
    /// <param name="destination">The destination file entry that will become the new parent of the moved entries. Cannot be null.</param>
    public static void ApplyMove(
        IEnumerable<FileEntry<TItem>> entries,
        FileEntry<TItem> destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        var parents = FileEntry<TItem>.CaptureParents(entries);

        foreach (var entry in entries)
        {
            entry.Parent?.RemoveChild(entry);
            destination.AddChild(entry);
        }

        destination.RecalculateSizes();

        foreach (var parent in parents)
        {
            parent.RecalculateSizes();
        }
    }

    /// <summary>
    /// Adds a new file entry as a child to the specified parent entry and updates the parent's size information.
    /// </summary>
    /// <remarks>After adding the child entry, the parent's size and related metadata are recalculated to
    /// reflect the change.</remarks>
    /// <param name="newEntry">The file entry to add as a child. Cannot be null.</param>
    /// <param name="parent">The parent file entry to which the new child will be added. Cannot be null.</param>
    public static void ApplyCreate(FileEntry<TItem> newEntry, FileEntry<TItem> parent)
    {
        ArgumentNullException.ThrowIfNull(newEntry);
        ArgumentNullException.ThrowIfNull(parent);

        parent.AddChild(newEntry);
        parent.RecalculateSizes();
    }

    /// <summary>
    /// Adds a new file entry as a child to the specified parent entry and updates the parent's size information.
    /// </summary>
    /// <param name="newEntry">The file entry to be added as a child. Cannot be null.</param>
    /// <param name="parent">The parent file entry to which the new entry will be added. Cannot be null.</param>
    public static void ApplyUpload(FileEntry<TItem> newEntry, FileEntry<TItem> parent)
    {
        ArgumentNullException.ThrowIfNull(newEntry);
        ArgumentNullException.ThrowIfNull(parent);

        parent.AddChild(newEntry);
        parent.RecalculateSizes();
    }

    /// <summary>
    /// Creates a new directory container representing the search results, containing the specified file entries as its
    /// children.
    /// </summary>
    /// <remarks>The created container has a unique identifier and its name reflects the number of search
    /// results included. The container's timestamps are set to the current UTC time.</remarks>
    /// <param name="parent">The parent file entry under which the search results container should be placed. Can be null if the container is to be created at the root level.</param>
    /// <param name="results">The list of file entries to include as children in the search results container. Cannot be null.</param>
    /// <returns>A new directory-type file entry that contains the provided search result entries as its children.</returns>
    public static FileEntry<TItem> CreateSearchContainer(
        FileEntry<TItem>? parent,
        List<FileEntry<TItem>> results)
    {
        var container = new FileEntry<TItem>(
            id: $"search-{Guid.NewGuid()}",
            name: $"Search results ({results.Count})",
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new(),
            parent: parent
        );

        foreach (var entry in results)
        {
            container.AddChild(entry);
        }

        return container;
    }

    /// <summary>
    /// Asynchronously enumerates all file entries in the hierarchy starting from the specified root entry.
    /// </summary>
    /// <param name="root">The root file entry from which to begin enumeration. Cannot be null.</param>
    /// <returns>An asynchronous sequence of file entries representing all items found in the hierarchy rooted at the specified
    /// entry.</returns>
    private IAsyncEnumerable<FileEntry<TItem>> FlattenAsync(FileEntry<TItem> root)
    {
        ArgumentNullException.ThrowIfNull(root);
        return _flattenService.EnumerateAsync(root);
    }

    /// <summary>
    /// Asynchronously searches for file entries that match the specified query and options, using a provided metadata
    /// extraction function.
    /// </summary>
    /// <param name="query">The search query string used to filter file entries. If null, an empty string is used.</param>
    /// <param name="options">The options that control the search behavior, such as filtering and matching criteria. Cannot be null.</param>
    /// <param name="metadataExtractor">A function that extracts metadata from each item for use in the search. Cannot be null.</param>
    /// <param name="entry">The root file entry from which to begin the search. If null, the search starts from the master root.</param>
    /// <returns>An asynchronous sequence of file entries that match the search criteria.</returns>
    public IAsyncEnumerable<FileEntry<TItem>> SearchAsync(
        string query,
        FileSearchOptions options,
        Func<TItem, string> metadataExtractor,
        FileEntry<TItem>? entry = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(metadataExtractor);

        var currentEntry = entry ?? MasterRoot;

        return _searchService.SearchAsync(currentEntry, query ?? string.Empty, metadataExtractor, options);
    }

    /// <summary>
    /// Creates a new <see cref="FileEntry{TItem}"/> instance based on the provided <see cref="EntryDescriptor{TItem}"/> and parent entry.
    /// </summary>
    /// <param name="desc">Entry descriptor containing the information needed to create the file entry. Cannot be null.</param>
    /// <param name="parent">Parent file entry to which the new entry will be associated. Can be null if the new entry is to be created at the root level.</param>
    /// <returns>Returns a new <see cref="FileEntry{TItem}"/> instance initialized with the data from the provided descriptor and associated with the specified parent.</returns>
    internal static FileEntry<TItem> Map(
        EntryDescriptor<TItem> desc,
        FileEntry<TItem>? parent)
    {
        ArgumentNullException.ThrowIfNull(desc);

        var entry = new FileEntry<TItem>(
            desc.Id,
            desc.Name,
            desc.IsDirectory,
            desc.Size ?? 0,
            desc.CreatedDate,
            desc.ModifiedDate,
            desc.Value ?? new TItem(),
            parent);

        if (!desc.IsDirectory && desc.GetBytesAsync is not null)
        {
            entry.DataProviderAsync = desc.GetBytesAsync;
        }

        return entry;
    }

    /// <summary>
    /// Asynchronously builds a flat view of all non-directory file entries under the master root directory.
    /// </summary>
    /// <remarks>The returned flat view groups all files directly under a single root entry named "Home".
    /// Directory entries are excluded from the flat view.</remarks>
    /// <returns>A <see cref="FileEntry{TItem}"/> representing the root of the flat view, containing all non-directory entries as
    /// children.</returns>
    public async Task<FileEntry<TItem>> BuildFlatViewAsync()
    {
        var flat = new FileEntry<TItem>(
            id: Guid.NewGuid().ToString(),
            name: "Home",
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new TItem());

        await foreach (var entry in FlattenAsync(MasterRoot))
        {
            if (!entry.IsDirectory)
            {
                flat.AddChild(entry);
            }
        }

        return flat;
    }

    /// <summary>
    /// Creates a new directory-type file entry based on the provided entry descriptor and parent entry.
    /// </summary>
    /// <param name="descriptor">The entry descriptor containing the information needed to create the directory entry. Cannot be null.</param>
    /// <param name="parent">The parent file entry to which the new directory entry will be associated. Cannot be null.</param>
    /// <returns>Returns a new <see cref="FileEntry{TItem}"/> instance representing a directory, initialized with the data from the provided descriptor and associated with the specified parent.</returns>
    public static FileEntry<TItem> CreateDirectoryEntry(
        EntryDescriptor<TItem> descriptor,
        FileEntry<TItem> parent)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(parent);

        var entry = new FileEntry<TItem>(
            descriptor.Id,
            descriptor.Name,
            isDirectory: true,
            size: 0L,
            createdDate: descriptor.CreatedDate,
            modifiedDate: descriptor.ModifiedDate,
            item: descriptor.Value ?? new TItem(),
            parent: parent);

        return entry;
    }

    /// <summary>
    /// Asynchronously loads the child entries for the specified file entry if the given condition is met.
    /// </summary>
    /// <param name="entry">The file entry for which to load child entries. If null, no action is taken.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async ValueTask LoadChildrenWhenAsync(FileEntry<TItem>? entry)
    {
        if (entry is not null &&
            entry.IsDirectory &&
            entry.Children.Count == 0)
        {
            await LoadChildrenAsync(entry);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _state.SortChanged -= OnSortChanged;
        _state.SortLayoutChanged -= OnSortChanged;
    }

    /// <summary>
    /// Handles the event that occurs when the sort order changes.
    /// </summary>
    /// <param name="sender">The source of the event. This is typically the control that raised the event.</param>
    /// <param name="e">An object that contains the event data.</param>
    /// <exception cref="NotImplementedException">Thrown in all cases as the method is not yet implemented.</exception>
    private void OnSortChanged(object? sender, EventArgs e)
    {
        SortUpdated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sorts the specified list of file entries according to the current sort criteria and order defined in the state.
    /// </summary>
    /// <remarks>The sorting is performed based on the sort field and direction specified in the current
    /// state. Supported sort fields include name, extension, size, created date, modified date, and type. The method
    /// modifies the input list directly and does not return a new list.</remarks>
    /// <param name="entries">The list of file entries to sort. The list is modified in place to reflect the sorted order.</param>
    private void SortEntries(List<FileEntry<TItem>> entries)
    {
        if (entries.Count <= 1)
        {
            return;
        }

        entries.Sort(_comparer);
    }

    /// <summary>
    /// Creates a sorted copy of the specified file entry, including its child entries.
    /// </summary>
    /// <remarks>The returned entry is a deep copy of the input, with child entries sorted according to the
    /// default sorting logic. Use this method to obtain a sorted view without altering the original
    /// structure.</remarks>
    /// <param name="entry">The file entry to clone and sort. Cannot be null.</param>
    /// <returns>A new instance of the file entry with its children sorted. The original entry is not modified.</returns>
    public FileEntry<TItem>? GetSortedEntry(FileEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return null;
        }

        var clone = entry.Clone();
        var children = entry.Children.ToList();

        SortEntries(children);

        foreach (var child in children)
        {
            clone.AddChild(child);
        }

        return clone;
    }
}
