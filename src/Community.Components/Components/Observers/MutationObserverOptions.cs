namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for observing mutations in a DOM tree using a mutation observer.
/// </summary>
/// <remarks>Use this record to specify which types of DOM changes should be monitored, such as attribute
/// modifications, changes to child elements, or updates to character data. Each property corresponds to a specific
/// aspect of mutation observation and can be set to enable or disable monitoring of that aspect. These options are
/// typically used when initializing a mutation observer to control its behavior.</remarks>
public record MutationObserverOptions
{
    /// <summary>
    /// Gets a value indicating whether attributes are enabled.
    /// </summary>
    public bool Attributes { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether child elements are included in the list.
    /// </summary>
    public bool ChildList { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether operations should include the entire subtree of elements.
    /// </summary>
    public bool Subtree { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the original value of an attribute is included during change tracking
    /// operations.
    /// </summary>
    public bool AttributeOldValue { get; init; }

    /// <summary>
    /// Gets a value indicating whether character data is included in the output.
    /// </summary>
    public bool CharacterData { get; init; }

    /// <summary>
    /// Gets a value indicating whether the old value of character data is included in mutation records.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, mutation records will contain the previous value of the
    /// character data before the change occurred. This can be useful for tracking changes or implementing undo
    /// functionality. When <see langword="false"/>, the old value is omitted from mutation records, which may improve
    /// performance if the previous value is not needed.</remarks>
    public bool CharacterDataOldValue { get; init; }
}
