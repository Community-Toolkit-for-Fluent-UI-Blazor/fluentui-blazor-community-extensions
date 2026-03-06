namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when entries are moved to a new parent location.
/// </summary>
/// <remarks>This event argument contains the identifiers of the entries being moved, the identifier of the new
/// parent, and the results of each move operation. Use this class to access information about the move operation in
/// event handlers.</remarks>
public sealed class MoveEntriesEventArgs
{
    /// <summary>
    /// Gets the collection of unique string identifiers associated with this instance.
    /// </summary>
    public IReadOnlyList<string> Ids { get; init; } = [];

    /// <summary>
    /// Gets the identifier of the new parent entity to which an item will be reassigned.
    /// </summary>
    public string NewParentId { get; init; } = default!;

    /// <summary>
    /// Gets the collection of results for each move entry operation.
    /// </summary>
    public List<MoveEntryResult> Results { get; } = [];
}
