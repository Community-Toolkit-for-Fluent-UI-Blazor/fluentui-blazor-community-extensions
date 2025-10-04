namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a record of a single mutation event, including details about the type of mutation and affected nodes or
/// attributes.
/// </summary>
/// <remarks>Use this type to capture and convey information about changes to a document structure, such as
/// attribute modifications or node additions and removals. The properties provide context for the mutation, including
/// the mutation type, affected attribute name, previous value, and serialized representations of added or removed
/// nodes. This record is typically used in scenarios involving DOM change tracking or synchronization.</remarks>
public record MutationEntry
{
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
