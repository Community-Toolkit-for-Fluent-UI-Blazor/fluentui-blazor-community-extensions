namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for mutation-related events, encapsulating information about changes to attributes, child nodes, or
/// subtrees within a group or instance.
/// </summary>
/// <remarks>Use this record to access details about a mutation event, such as the type of mutation, affected
/// attributes, and lists of added or removed nodes. All properties are immutable after initialization, ensuring event
/// data remains consistent throughout its lifecycle.</remarks>
public record MutationEventArgs
{
    /// <summary>
    /// Gets the unique identifier for the group associated with this instance.
    /// </summary>
    public string GroupId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the type of mutation to observe or process.
    /// </summary>
    /// <remarks>Valid values include "attributes", "childList", and "subtree". The meaning of each value
    /// depends on the context in which the property is used. This property is immutable after initialization.</remarks>
    public MutationType Type { get; init; }

    /// <summary>
    /// Gets the name of the attribute associated with this instance.
    /// </summary>
    public string? AttributeName { get; init; }

    /// <summary>
    /// Gets the previous value before the most recent change, or null if no prior value exists.
    /// </summary>
    public string? OldValue { get; init; }

    /// <summary>
    /// Gets the serialized representation of the nodes that were added.
    /// </summary>
    public IEnumerable<string>? AddedNodes { get; init; }

    /// <summary>
    /// Gets the serialized representation of nodes that have been removed.
    /// </summary>
    public IEnumerable<string>? RemovedNodes { get; init; }
}
