using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for mapping log levels to their corresponding console output levels.
/// </summary>
/// <remarks>This class contains static methods that facilitate the conversion of log levels defined in the
/// LogLevel enumeration to their equivalent ConsoleLevel representations. It is intended for use in logging frameworks
/// that require consistent display of log levels in a console environment.</remarks>
internal static class LogLevelMapper
{
    /// <summary>
    /// Converts the specified log level to its corresponding console log level.
    /// </summary>
    /// <remarks>Use this method to map a LogLevel value to its equivalent ConsoleLevel for consistent log
    /// output formatting. If an unrecognized log level is provided, the method defaults to
    /// ConsoleLevel.Information.</remarks>
    /// <param name="logLevel">The log level to convert to a console log level.</param>
    /// <returns>The console log level that corresponds to the specified log level. Returns ConsoleLevel.Information if the log
    /// level is not recognized.</returns>
    public static ConsoleLevel ToConsoleLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => ConsoleLevel.Trace,
            LogLevel.Debug => ConsoleLevel.Debug,
            LogLevel.Information => ConsoleLevel.Information,
            LogLevel.Warning => ConsoleLevel.Warning,
            LogLevel.Error => ConsoleLevel.Error,
            LogLevel.Critical => ConsoleLevel.Critical,
            _ => ConsoleLevel.Information
        };
    }
}
