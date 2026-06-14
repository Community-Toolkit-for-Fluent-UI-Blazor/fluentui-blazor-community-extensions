namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an immutable console message that encapsulates its level, content, timestamp, and associated metadata for
/// logging and diagnostic purposes.
/// </summary>
/// <remarks>Each instance of this record contains all relevant information about a console message, including
/// optional exception details, correlation identifiers, and additional properties. The record is designed to facilitate
/// structured logging, debugging, and traceability across components and services. Once created, the data within a
/// ConsoleMessage instance cannot be modified, ensuring message integrity.</remarks>
public sealed record ConsoleMessage
{
    /// <summary>
    /// Gets the unique identifier for the console message.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets the timestamp indicating when the instance was created, represented in UTC.
    /// </summary>
    /// <remarks>This property is initialized to the current UTC time at the moment of instance creation. It
    /// is immutable after initialization.</remarks>
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the level of the console message.
    /// </summary>
    public ConsoleLevel Level { get; init; }

    /// <summary>
    /// Gets the message content of the console message.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Gets the formatted message of the console message, which may include additional formatting or context information.
    /// </summary>
    public string? FormattedMessage { get; init; }

    /// <summary>
    /// Gets the source of the console message, which may indicate the origin or component responsible for generating the message.
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Gets the category of the item, which can be used to classify or group related items.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Gets the exception that occurred during the operation, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets a read-only dictionary containing additional properties associated with the object.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Properties { get; init; }

    /// <summary>
    /// Gets the unique identifier used to correlate related operations or requests across different components or
    /// services.
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Gets the unique identifier for the activity associated with the current operation.
    /// </summary>
    /// <remarks>This identifier can be used for tracking and correlating logs or events related to the
    /// activity. It is initialized at the time of the object's creation and cannot be modified thereafter.</remarks>
    public string? ActivityId { get; init; }

    /// <summary>
    /// Gets the identifier of the thread that generated the console message, which can be useful for debugging and tracing.
    /// </summary>
    public int? ThreadId { get; init; }

    /// <summary>
    /// Gets the name of the machine on which the application is running.
    /// </summary>
    public string? MachineName { get; init; }

    /// <summary>
    /// Gets the collection of tags associated with the item.
    /// </summary>
    public IReadOnlyCollection<string>? Tags { get; init; }

    /// <summary>
    /// Gets the payload data associated with the object. This property is initialized at the time of object creation
    /// and cannot be modified thereafter.
    /// </summary>
    public object? Payload { get; init; }
}
