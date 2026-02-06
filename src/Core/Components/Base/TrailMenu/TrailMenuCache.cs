namespace FluentUI.Blazor.Community.Components.TrailMenu;

/// <summary>
/// Provides a cache for storing and managing size values associated with unique trail menu identifiers.
/// </summary>
/// <remarks>TrailMenuCache enables efficient retrieval, updating, and invalidation of size values for trail menu
/// items. The cache supports calculating the total size of all tracked items and allows selective clearing of entries
/// by identifier. This class is intended for internal use and is not thread-safe.</remarks>
internal sealed class TrailMenuCache
{
    /// <summary>
    /// Stores size values mapped to their corresponding string keys.
    /// </summary>
    /// <remarks>This dictionary is initialized as empty and can be populated with key-value pairs
    /// representing sizes. It is typically used to associate specific size measurements with unique
    /// identifiers.</remarks>
    private readonly Dictionary<string, double> _sizes = [];

    /// <summary>
    /// Gets the total size calculated by summing all values in the sizes collection.
    /// </summary>
    /// <remarks>This property provides a quick way to retrieve the cumulative size without needing to iterate
    /// through the collection manually. It reflects the current state of the sizes collection and updates automatically
    /// as values change.</remarks>
    public double TotalSize => _sizes.Values.Sum();

    /// <summary>
    /// Attempts to retrieve the size associated with the specified identifier.
    /// </summary>
    /// <remarks>This method is useful for safely attempting to get a value without throwing an exception if
    /// the identifier does not exist.</remarks>
    /// <param name="id">The unique identifier for which the size is to be retrieved. This parameter cannot be null or empty.</param>
    /// <param name="size">When this method returns <see langword="true"/>, this output parameter contains the size associated with the
    /// specified identifier; otherwise, it is set to zero.</param>
    /// <returns><see langword="true"/> if the size was successfully retrieved; otherwise, <see langword="false"/>.</returns>
    public bool TryGet(string id, out double size) => _sizes.TryGetValue(id, out size);

    /// <summary>
    /// Associates the specified size value with the given identifier.
    /// </summary>
    /// <remarks>If the identifier already exists, its associated size will be updated to the new
    /// value.</remarks>
    /// <param name="id">The unique identifier for which the size is being set. This value cannot be null or empty.</param>
    /// <param name="size">The size value to associate with the specified identifier. Must be a non-negative number.</param>
    public void Set(string id, double size) => _sizes[id] = size;

    /// <summary>
    /// Invalidates the size associated with the specified identifier by resetting it to zero.
    /// </summary>
    /// <remarks>If the specified identifier does not exist in the sizes dictionary, no action is
    /// taken.</remarks>
    /// <param name="id">The unique identifier for which the size should be invalidated. This parameter must not be null and must
    /// correspond to an existing entry in the sizes dictionary.</param>
    public void Invalidate(string id)
    {
        if (_sizes.ContainsKey(id))
        {
            _sizes[id] = 0;
        }
    }

    /// <summary>
    /// Removes the specified identifiers from the collection, ignoring any null or empty values.
    /// </summary>
    /// <remarks>This method modifies the internal collection by removing entries corresponding to the
    /// provided identifiers. It is important to ensure that the identifiers are valid and exist in the collection to
    /// avoid unnecessary operations.</remarks>
    /// <param name="ids">An enumerable collection of identifiers to be removed. Only non-null and non-empty identifiers are processed.</param>
    public void Clear(IEnumerable<string?> ids)
    {
        foreach (var id in ids.Where(x => !string.IsNullOrEmpty(x)))
        {
            _sizes.Remove(id!);
        }
    }
}
