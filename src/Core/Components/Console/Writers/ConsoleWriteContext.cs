namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides contextual metadata for console output operations, including source, category, correlation identifiers, and
/// custom properties or tags.
/// </summary>
/// <remarks>Use this class to supply additional information when writing to the console, such as identifying the
/// origin of a message, categorizing output, or associating related operations through correlation and activity IDs.
/// Custom properties and tags can be used to extend the context with application-specific data, enabling richer
/// logging, filtering, or diagnostics scenarios.</remarks>
public sealed class ConsoleWriteContext
{
    /// <summary>
    /// Gets the source of the data, which may be null if not specified.
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Gets the category of the item, which can be used to classify or group related items.
    /// </summary>
    /// <remarks>The category is optional and may be null if not specified. It is intended to provide
    /// additional context about the item.</remarks>
    public string? Category { get; init; }

    /// <summary>
    /// Gets the unique identifier used to correlate related operations or requests.
    /// </summary>
    /// <remarks>This property is typically used for tracking and logging purposes, allowing developers to
    /// trace the flow of requests across different services or components. It is especially useful in distributed
    /// systems where operations may span multiple services.</remarks>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Gets the unique identifier for the activity associated with the current operation.
    /// </summary>
    /// <remarks>This identifier can be used for tracking and correlating logs or events related to the
    /// activity. It is initialized at the time of the object's creation and cannot be modified thereafter.</remarks>
    public string? ActivityId { get; init; }

    /// <summary>
    /// Gets a read-only dictionary containing additional properties associated with the object.
    /// </summary>
    /// <remarks>The dictionary may include various key-value pairs that provide context, configuration, or
    /// metadata relevant to the object. Keys represent property names, and values can be of any type. The dictionary
    /// may be null if no properties are defined.</remarks>
    public IReadOnlyDictionary<string, object?>? Properties { get; init; }

    /// <summary>
    /// Gets the collection of tags associated with the item.
    /// </summary>
    /// <remarks>The tags can be used to categorize or label the item for easier identification and filtering.
    /// This property is read-only and can be initialized at the time of object creation.</remarks>
    public IReadOnlyCollection<string>? Tags { get; init; }
}
