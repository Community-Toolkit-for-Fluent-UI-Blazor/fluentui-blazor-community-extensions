namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a service for managing the storage and retrieval of tile grid items using an asynchronous storage provider.
/// </summary>
/// <remarks>This service requires both a storage provider and a storage key to perform load and save operations.
/// If either is not supplied, storage actions are skipped. The service is intended to facilitate persistence of tile
/// grid layouts or data across application sessions.</remarks>
/// <typeparam name="TItem">The type of items managed and persisted by the tile grid storage service.</typeparam>
internal class TileGridStorageService<TItem>
{
    /// <summary>
    /// Represents the storage provider responsible for managing the underlying data of the tile grid.
    /// </summary>
    /// <remarks>This field is initialized through the constructor and is read-only. It may be null if no
    /// storage provider is specified.</remarks>
    private readonly ITileGridStorage<TItem>? _provider;

    /// <summary>
    /// Represents the key used to identify the tile grid storage.
    /// </summary>
    private readonly string? _key;

    /// <summary>
    /// Represents the collection of items managed by the tile grid storage service.
    /// </summary>
    private readonly List<TItem> _items;

    /// <summary>
    /// Represents the current state of the tile grid, including metadata and configuration information.
    /// </summary>
    private readonly TileGridState<TItem> _state;

    /// <summary>
    /// Initializes a new instance of the TileGridStorageService class, providing access to tile grid storage
    /// functionality.
    /// </summary>
    /// <remarks>The constructor requires a non-empty list of items to function correctly. Ensure that the
    /// provided state is valid and corresponds to the items.</remarks>
    /// <param name="provider">The storage provider used to manage tile grid data. This parameter can be null if no provider is specified.</param>
    /// <param name="key">An optional key that identifies the tile grid storage. This parameter can be null if a key is not required.</param>
    /// <param name="items">A list of items to be managed within the tile grid. This list must not be empty.</param>
    /// <param name="state">The current state of the tile grid, containing metadata and configuration information for the grid.</param>
    public TileGridStorageService(
        ITileGridStorage<TItem>? provider,
        string? key,
        List<TItem> items,
        TileGridState<TItem> state)
    {
        _provider = provider;
        _key = key;
        _items = items;
        _state = state;
    }

    /// <summary>
    /// Asynchronously loads data from the configured provider using the specified key and updates the collection with
    /// the loaded items.
    /// </summary>
    /// <remarks>If the provider or key is not set, the method completes without performing any operation.
    /// When data is successfully loaded, the existing items are replaced with the new items, and the initial layout
    /// state is updated to reflect the current collection.</remarks>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    public async Task LoadAsync()
    {
        if (_provider == null || _key == null)
        {
            return;
        }

        var loaded = await _provider.LoadAsync(_key);

        if (loaded != null && loaded.Count > 0)
        {
            _items.Clear();
            _items.AddRange(loaded);
        }

        _state.InitialLayout = [.. _items];
    }

    /// <summary>
    /// Performs an asynchronous save operation for the current items using the configured storage provider and storage
    /// key, if both are available.
    /// </summary>
    /// <remarks>The save operation is only executed if both the StorageProvider and StorageKey properties are
    /// not null. If either is null, the method completes without performing any action.</remarks>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        if (_provider != null && _key != null)
        {
            await _provider.SaveAsync(_key, _items);
        }
    }
}
