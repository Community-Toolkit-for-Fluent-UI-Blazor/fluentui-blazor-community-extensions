namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for rebuilding the search index and performing asynchronous searches on console messages.
/// </summary>
/// <remarks>This interface is designed to facilitate efficient searching of console messages, allowing for
/// customizable search options and cancellation support.</remarks>
public interface IConsoleSearchService
{
    /// <summary>
    /// Rebuilds the search index to ensure that all data is accurately represented and accessible.
    /// </summary>
    /// <remarks>This operation may take a significant amount of time depending on the size of the dataset
    /// being indexed. It is recommended to invoke this method during periods of low activity to minimize performance
    /// impact.</remarks>
    void RebuildIndex();

    /// <summary>
    /// Searches for console messages that match the specified query asynchronously.
    /// </summary>
    /// <remarks>The search operation may take time depending on the number of messages and the complexity of
    /// the query. Ensure to handle potential cancellation appropriately.</remarks>
    /// <param name="query">The search term used to filter console messages. This parameter cannot be null or empty.</param>
    /// <param name="options">Optional settings that influence the search behavior, such as case sensitivity or message types. If not
    /// provided, default search options are used.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the search operation. This allows the caller to terminate the operation if
    /// needed.</param>
    /// <returns>A task that represents the asynchronous operation, containing a read-only list of console messages that match
    /// the search query.</returns>
    ValueTask<IReadOnlyList<ConsoleMessage>> SearchAsync(
        string query,
        ConsoleSearchOptions? options = null,
        CancellationToken cancellationToken = default);
}
