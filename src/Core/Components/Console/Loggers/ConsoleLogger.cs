using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a logger implementation that writes log messages to the console for console applications.
/// </summary>
/// <remarks>The ConsoleLogger implements the ILogger interface and supports logging at various levels. It is
/// intended for use in scenarios where log output to the console is required, such as command-line tools or interactive
/// applications. This logger enables developers to track application behavior and events during execution. Thread
/// safety and performance considerations depend on the underlying console implementation.</remarks>
internal sealed class ConsoleLogger : ILogger
{
    /// <summary>
    /// Represents the console writer used for outputting messages to the console.
    /// </summary>
    private readonly IConsoleWriter _writer;

    /// <summary>
    /// Represents the name of the category associated with this instance.
    /// </summary>
    private readonly string _categoryName;

    /// <summary>
    /// Initializes a new instance of the ConsoleLogger class using the specified console writer and category name.
    /// </summary>
    /// <remarks>Ensure that the writer parameter is properly initialized before passing it to this
    /// constructor. The category name can be used to filter or organize log messages by their origin.</remarks>
    /// <param name="writer">The IConsoleWriter instance used to output log messages to the console. Cannot be null.</param>
    /// <param name="categoryName">The category name under which log messages will be grouped. This value is used to identify the source of log
    /// entries.</param>
    public ConsoleLogger(IConsoleWriter writer, string categoryName)
    {
        _writer = writer;
        _categoryName = categoryName;
    }

    /// <summary>
    /// Extracts context properties from the specified state object for use in console logging.
    /// </summary>
    /// <remarks>This method ensures that context information is consistently formatted for logging,
    /// regardless of whether the state is a dictionary or another object type.</remarks>
    /// <typeparam name="TState">The type of the state object, which can be any object that may contain context information.</typeparam>
    /// <param name="state">The state object from which to extract context properties. If the object implements IReadOnlyDictionary{string,
    /// object?}, its entries are used as context properties; otherwise, the object itself is stored under the 'Scope'
    /// key.</param>
    /// <returns>A ConsoleWriteContext instance containing the extracted context properties.</returns>
    private static ConsoleWriteContext ExtractContext<TState>(TState state)
    {
        if (state is IReadOnlyDictionary<string, object?> properties)
        {
            return new ConsoleWriteContext
            {
                Properties = properties,
            };
        }

        return new ConsoleWriteContext
        {
            Properties = new Dictionary<string, object?>
            {
                ["Scope"] = state,
            }
        };
    }

    /// <summary>
    /// Extracts event-related properties and state information into a read-only dictionary for logging or diagnostic
    /// purposes.
    /// </summary>
    /// <remarks>If the state parameter is a dictionary, its contents are merged into the returned dictionary.
    /// If not, the state is included under the 'State' key. This method is useful for aggregating event and state data
    /// for structured logging scenarios.</remarks>
    /// <typeparam name="TState">The type of the state object, which can be a dictionary of string keys and object values, or any other object.</typeparam>
    /// <param name="state">The state object from which properties are extracted. If the state is a dictionary, its key-value pairs are
    /// included; otherwise, the state is added under the key 'State'.</param>
    /// <param name="eventId">The event identifier containing the ID and name of the event being processed.</param>
    /// <returns>A read-only dictionary containing the event ID, event name, and any additional properties extracted from the
    /// state.</returns>
    private static IReadOnlyDictionary<string, object?> ExtractProperties<TState>(TState state, EventId eventId)
    {
        var properties = new Dictionary<string, object?>
        {
            ["EventId"] = eventId.Id,
            ["EventName"] = eventId.Name
        };

        if (state is IReadOnlyDictionary<string, object?> dict)
        {
            foreach (var kvp in dict)
            {
                properties[kvp.Key] = kvp.Value;
            }
        }
        else
        {
            properties["State"] = state;
        }

        return properties;
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        var context = ExtractContext(state);

        return _writer.BeginScope(context);
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc />
    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        var context = new ConsoleWriteContext
        {
            Category = _categoryName,
            Properties = ExtractProperties(state, eventId)
        };

        await _writer.WriteAsync(LogLevelMapper.ToConsoleLevel(logLevel), message, exception, context);
    }
}
