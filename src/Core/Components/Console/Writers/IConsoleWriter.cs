namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for writing output to the console.
/// </summary>
/// <remarks>Implementations of this interface should ensure thread safety and may support various output formats
/// as required by the application.</remarks>
public interface IConsoleWriter
{
    /// <summary>
    /// Writes a log message to the console with the specified severity level.
    /// </summary>
    /// <remarks>This method enables structured logging by allowing the caller to specify the log level and
    /// optionally include exception and context information. Use this method to record events, errors, or informational
    /// messages in a consistent format.</remarks>
    /// <param name="level">The severity level of the log message, indicating its importance or urgency.</param>
    /// <param name="message">The message to be logged, providing the details of the event or information being recorded.</param>
    /// <param name="exception">An optional exception to include with the log entry, providing additional context about an error or exception
    /// that occurred. May be <see langword="null"/> if not applicable.</param>
    /// <param name="context">An optional context object that supplies additional information relevant to the logging operation. May be <see
    /// langword="null"/> if no context is required.</param>
    Task WriteAsync(ConsoleLevel level, string message, Exception? exception = null, ConsoleWriteContext? context = null);

    /// <summary>
    /// Writes a trace message to the console, optionally using the specified context to influence formatting or output
    /// behavior.
    /// </summary>
    /// <remarks>Use this method to log diagnostic or tracing information during application execution.
    /// Supplying a context allows for customized output, which can be useful for distinguishing between different
    /// sources or types of trace messages.</remarks>
    /// <param name="message">The message to write to the console. This value should provide information relevant to tracing the application's
    /// execution.</param>
    /// <param name="context">An optional context that determines how the trace message is formatted or handled. If null, the default context
    /// is used.</param>
    Task TraceAsync(string message, ConsoleWriteContext? context = null);

    /// <summary>
    /// Writes a debug-level message to the console output, optionally using the specified context for formatting or
    /// categorization.
    /// </summary>
    /// <remarks>This method is intended for use during development to output diagnostic information. Ensure
    /// that the console or logging configuration is set to display debug-level messages as needed.</remarks>
    /// <param name="message">The message to write to the console. This should contain information relevant for debugging application
    /// behavior.</param>
    /// <param name="context">An optional context that influences how the message is formatted or processed. If null, the default context is
    /// used.</param>
    Task DebugAsync(string message, ConsoleWriteContext? context = null);

    /// <summary>
    /// Writes an informational message to the console output using the specified formatting context, if provided.
    /// </summary>
    /// <remarks>Use this method to log informational messages that do not indicate errors or warnings. The
    /// context parameter allows customization of how the message is displayed, such as color or output
    /// stream.</remarks>
    /// <param name="message">The message to write to the console. This parameter cannot be null or empty.</param>
    /// <param name="context">An optional context that specifies formatting or output details for the message. If null, the default context is
    /// used.</param>
    Task InfoAsync(string message, ConsoleWriteContext? context = null);

    /// <summary>
    /// Writes a warning message to the console output, optionally using the specified context for formatting or
    /// routing.
    /// </summary>
    /// <remarks>Use this method to alert users to potential issues or non-critical problems without
    /// interrupting program execution. Warning messages are typically used to indicate recoverable conditions or
    /// situations that may require attention.</remarks>
    /// <param name="message">The warning message to write. This value should describe the condition or issue to be highlighted.</param>
    /// <param name="context">An optional context that determines how the message is formatted or where it is written. If null, the default
    /// context is used.</param>
    Task WarningAsync(string message, ConsoleWriteContext? context = null);

    /// <summary>
    /// Logs an error message to the console, optionally including exception details and contextual information.
    /// </summary>
    /// <remarks>Use this method to record error conditions in a structured manner, facilitating
    /// troubleshooting and diagnostics. Including exception and context information can help provide more comprehensive
    /// error reports.</remarks>
    /// <param name="message">The error message to log. This message should describe the error condition encountered.</param>
    /// <param name="exception">An optional exception that provides additional details about the error, such as a stack trace. May be null if no
    /// exception is associated with the error.</param>
    /// <param name="context">An optional context that specifies how the error message is written to the console. May influence formatting or
    /// output destination. Can be null to use default behavior.</param>
    Task ErrorAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null);

    /// <summary>
    /// Logs a critical error message to the console, optionally including exception details and additional context
    /// information.
    /// </summary>
    /// <remarks>Use this method to log errors that indicate a failure requiring immediate attention. The
    /// output may be formatted differently depending on the provided context.</remarks>
    /// <param name="message">The message that describes the critical error to be logged.</param>
    /// <param name="exception">An optional exception that provides additional information about the error. Specify <see langword="null"/> if no
    /// exception is associated with the error.</param>
    /// <param name="context">An optional context object that supplies additional information about the console write operation. Specify <see
    /// langword="null"/> if no context is required.</param>
    Task CriticalAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null);

    /// <summary>
    /// Begins a logical operation scope for console output, allowing contextual information to be associated with
    /// subsequent console messages.
    /// </summary>
    /// <remarks>Use this method to group related console output and attach contextual data, such as operation
    /// names or correlation identifiers, to all messages written within the scope. Disposing the returned object will
    /// end the scope and remove the associated context.</remarks>
    /// <param name="context">The context object that contains information to be included with all console output written within the scope.
    /// Cannot be null.</param>
    /// <returns>An <see cref="IDisposable"/> that ends the current scope when disposed.</returns>
    IDisposable BeginScope(ConsoleWriteContext context);
}
