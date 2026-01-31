namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for asynchronously saving and loading items in a tile grid identified by a unique grid ID.
/// </summary>
/// <remarks>Implementations of this interface should ensure thread safety when accessing shared resources.
/// Asynchronous operations allow for efficient storage and retrieval, especially in scenarios involving I/O-bound tasks
/// or remote data sources.</remarks>
/// <typeparam name="TItem">The type of items to be stored and retrieved from the tile grid.</typeparam>
public interface ITileGridStorage<TItem>
{
    /// <summary>
    /// Asynchronously saves the specified items to the grid identified by the given grid ID.
    /// </summary>
    /// <remarks>This method may throw exceptions if the grid ID is invalid or if there are issues during the
    /// save operation.</remarks>
    /// <param name="gridId">The unique identifier of the grid where the items will be saved. This cannot be null or empty.</param>
    /// <param name="items">An enumerable collection of items to be saved. This collection must not be null and should contain valid items
    /// for the grid.</param>
    /// <returns>A task that represents the asynchronous save operation. The task completes when all items have been saved to the
    /// grid.</returns>
    Task SaveAsync(string gridId, IEnumerable<TItem> items);

    /// <summary>
    /// Asynchronously loads a list of items associated with the specified grid identifier.
    /// </summary>
    /// <remarks>Callers should ensure that the returned list is checked for null before use. This method is
    /// intended for use in asynchronous programming models.</remarks>
    /// <param name="gridId">The unique identifier of the grid for which to retrieve items. This parameter cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of items of type TItem, or
    /// null if no items are found for the specified grid identifier.</returns>
    Task<List<TItem>?> LoadAsync(string gridId);
}
