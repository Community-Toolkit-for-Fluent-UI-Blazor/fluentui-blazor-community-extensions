namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a batch of resize entries grouped by a unique identifier.
/// </summary>
public record ResizeBatch
{
    /// <summary>
    /// Gets the unique identifier for the group.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of resize entries associated with the group.
    /// </summary>
    public required List<ResizeEntry> Entries { get; init; }
}
