using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a logger provider that creates instances of <see cref="ConsoleLogger"/> to log messages to the console.
/// </summary>
/// <param name="writer"></param>
internal sealed class ConsoleLoggerProvider(IConsoleWriter writer) : ILoggerProvider
{
    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(writer, categoryName);
    }

    /// <inheritdoc />
    public void Dispose() { }
}
