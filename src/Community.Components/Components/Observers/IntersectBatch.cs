namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a batch of intersect entries grouped by a unique identifier.
/// </summary>
/// <remarks>This record is used to encapsulate a collection of intersect entries, identified by a group ID. It is
/// typically used in scenarios where multiple entries need to be processed or analyzed together as part of a single
/// logical group.</remarks>
public record IntersectBatch
{
    /// <summary>
    /// Gets the unique identifier for the group.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of intersect entries associated with the group.
    /// </summary>
    public required List<IntersectEntry> Entries { get; init; }
}
