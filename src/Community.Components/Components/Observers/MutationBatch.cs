namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a batch of mutation entries grouped under a common identifier.
/// </summary>
/// <remarks>Use this type to organize multiple mutation operations that should be processed together as a single
/// batch. The batch is identified by the <see cref="GroupId"/> property, which can be used to correlate related
/// mutations.</remarks>
public record MutationBatch
{
    /// <summary>
    /// Gets the unique identifier for the group associated with this instance.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of mutation entries associated with this instance.
    /// </summary>
    public List<MutationEntry> Entries { get; init; } = [];
}
