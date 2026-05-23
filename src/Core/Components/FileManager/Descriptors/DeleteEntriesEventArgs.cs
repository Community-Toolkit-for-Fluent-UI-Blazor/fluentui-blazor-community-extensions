namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when one or more entries are deleted.
/// </summary>
public sealed class DeleteEntriesEventArgs
{
    /// <summary>
    /// Gets the collection of unique string identifiers associated with this instance.
    /// </summary>
    public IReadOnlyList<string> Ids { get; init; } = [];

    /// <summary>
    /// Gets the collection of results for each delete entry operation.
    /// </summary>
    public List<DeleteEntryResult> Results { get; } = [];
}
