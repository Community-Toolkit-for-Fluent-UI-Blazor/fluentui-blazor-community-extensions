namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static methods for creating instances of the ConsoleMessage class with customizable parameters for logging
/// purposes.
/// </summary>
/// <remarks>This factory class streamlines the creation of ConsoleMessage objects by allowing callers to specify
/// severity, message content, source, category, exception details, and additional properties. It is intended to be used
/// statically and does not require instantiation. Use this class to ensure consistent and structured logging throughout
/// the application.</remarks>
public static class ConsoleMessageFactory
{
    /// <summary>
    /// Creates a new ConsoleMessage instance with the specified log level, message details, and optional metadata for
    /// advanced logging scenarios.
    /// </summary>
    /// <remarks>Use this method to create structured log messages with rich metadata for advanced logging,
    /// diagnostics, or correlation scenarios. All parameters are optional except for the log level, allowing
    /// flexibility in message composition.</remarks>
    /// <param name="level">The severity level of the console message, indicating its importance or type.</param>
    /// <param name="message">An optional message describing the event or log entry. Can be null if not applicable.</param>
    /// <param name="source">An optional string identifying the source component or module that generated the message.</param>
    /// <param name="category">An optional category used to organize or filter log entries.</param>
    /// <param name="exception">An optional exception associated with the message, providing additional error context.</param>
    /// <param name="properties">An optional read-only dictionary containing additional properties or metadata for the message.</param>
    /// <param name="activityId">An optional identifier for tracking the activity related to the message, useful for correlation in distributed
    /// systems.</param>
    /// <param name="correlationId">An optional identifier used to correlate related log messages across operations or services.</param>
    /// <param name="formattedMessage">An optional pre-formatted message string for display or output purposes.</param>
    /// <param name="machineName">An optional name of the machine where the message was generated.</param>
    /// <param name="threadId">An optional thread identifier indicating the thread that produced the message.</param>
    /// <param name="tags">An optional collection of tags for categorizing or annotating the message.</param>
    /// <param name="payload">An optional payload object containing additional structured data relevant to the message.</param>
    /// <returns>A ConsoleMessage object populated with the provided parameters and metadata.</returns>
    public static ConsoleMessage Create(
        ConsoleLevel level,
        string? message = null,
        string? source = null,
        string? category = null,
        Exception? exception = null,
        IReadOnlyDictionary<string, object?>? properties = null,
        string? activityId = null,
        string? correlationId = null,
        string? formattedMessage = null,
        string? machineName = null,
        int? threadId = null,
        IReadOnlyCollection<string>? tags = null,
        object? payload = null
        )
    {
        return new ConsoleMessage
        {
            Level = level,
            Message = message,
            Source = source,
            Category = category,
            Exception = exception,
            Properties = properties,
            ActivityId = activityId,
            CorrelationId = correlationId,
            FormattedMessage = formattedMessage,
            MachineName = machineName,
            ThreadId = threadId,
            Tags = tags,
            Payload = payload
        };
    }

    /// <summary>
    /// Creates a new instance of the ConsoleMessage class with the specified severity level, message, source, category,
    /// exception, and additional properties.
    /// </summary>
    /// <param name="level">The severity level of the console message. Determines the importance and visibility of the log entry.</param>
    /// <param name="message">An optional message describing the event or log entry. Can be null if no message is required.</param>
    /// <param name="source">An optional string identifying the origin of the message, such as the component or module name. Can be null if
    /// not specified.</param>
    /// <param name="category">An optional category used to organize or filter log entries. Can be null if not specified.</param>
    /// <param name="exception">An optional exception associated with the message, providing additional context for error handling. Can be null
    /// if no exception is relevant.</param>
    /// <param name="properties">An optional read-only dictionary containing additional properties to include with the message for extended
    /// context. Can be null if no extra properties are needed.</param>
    /// <returns>A new ConsoleMessage instance populated with the provided parameters.</returns>
    public static ConsoleMessage Create(
        ConsoleLevel level,
        string? message = null,
        string? source = null,
        string? category = null,
        Exception? exception = null,
        IReadOnlyDictionary<string, object?>? properties = null)
    {
        return Create(level, message, source, category, exception, properties, null, null, null, null, null, null, null);
    }

    /// <summary>
    /// Creates a new instance of the ConsoleMessage class with the specified severity level, message and exception.
    /// </summary>
    /// <param name="level">The severity level of the console message. Determines the importance and visibility of the log entry.</param>
    /// <param name="message">An optional message describing the event or log entry. Can be null if no message is required.</param>
    /// <param name="exception">An optional exception associated with the message, providing additional context for error handling. Can be null
    /// if no exception is relevant.</param>
    /// <returns>A new ConsoleMessage instance populated with the provided parameters.</returns>
    public static ConsoleMessage Create(
        ConsoleLevel level,
        string? message,
        Exception? exception)
    {
        return Create(level, message, null, null, exception, null);
    }
}
