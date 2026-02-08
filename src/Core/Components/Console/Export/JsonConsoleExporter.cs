using System.Globalization;
using System.Text;
using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export console messages to a JSON file with customizable formatting and filtering options.
/// </summary>
/// <remarks>The JsonConsoleExporter allows users to export collections of console messages in a structured JSON
/// format. Exported output can be tailored using options to include or exclude specific message details such as
/// timestamps, log levels, categories, properties, and exceptions. The exporter supports filtering messages by time
/// range and produces indented JSON for readability. This class is typically used to persist or share console logs in a
/// standardized, machine-readable format.</remarks>
public sealed class JsonConsoleExporter : IConsoleExporter
{
    /// <summary>
    /// Represents a log message that has been exported, including its timestamp, log level, category, message text,
    /// associated properties, and any related exception.
    /// </summary>
    /// <remarks>Use this struct to encapsulate all relevant metadata for a log entry when exporting or
    /// processing logs. It is intended for scenarios where log messages need to be transferred, stored, or analyzed
    /// outside the original logging context. All properties are optional and may be null if the corresponding
    /// information is not available.</remarks>
    private readonly struct ExportedMessage
    {
        /// <summary>
        /// Gets the timestamp that indicates when the associated event occurred.
        /// </summary>
        public DateTimeOffset? Timestamp { get; init; }

        /// <summary>
        /// Gets the logging level for console output, specifying the minimum severity of log messages to be displayed.
        /// </summary>
        public ConsoleLevel? Level { get; init; }

        /// <summary>
        /// Gets the category associated with the item, which can be used to classify or group items.
        /// </summary>
        public string? Category { get; init; }

        /// <summary>
        /// Gets the message that provides additional context or information for the current instance.
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// Gets a read-only dictionary containing additional properties associated with the object, where each property
        /// is identified by a string key.
        /// </summary>
        public IReadOnlyDictionary<string, object?>? Properties { get; init; }

        /// <summary>
        /// Gets the exception that occurred during the operation, if any.
        /// </summary>
        public Exception? Exception { get; init; }
    }

    /// <summary>
    /// Represents the invariant culture, which is culture-independent and provides consistent formatting and parsing
    /// behavior regardless of the system's culture settings.
    /// </summary>
    /// <remarks>The invariant culture is useful for operations that require culture-agnostic results, such as
    /// data serialization, protocol formatting, or scenarios where consistent results are required across different
    /// locales.</remarks>
    private static readonly CultureInfo s_cultureInfo = CultureInfo.InvariantCulture;

    /// <summary>
    /// Represents the JSON serializer options configured for web serialization with indented formatting enabled.
    /// </summary>
    /// <remarks>These options are suitable for serializing and deserializing JSON data in web applications.
    /// The settings include defaults optimized for web scenarios, and enabling indented formatting improves readability
    /// of the output JSON.</remarks>
    private static readonly JsonSerializerOptions s_jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    /// <inheritdoc />
    public string Format => "json";

    /// <inheritdoc />
    public string MimeType => "application/json";

    /// <inheritdoc />
    public string DefaultFileName => "logs.json";

    /// <inheritdoc />
    public async ValueTask<ConsoleExportResult> ExportAsync(
        IReadOnlyCollection<ConsoleMessage> messages,
        ConsoleExportOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));
        ArgumentOutOfRangeException.ThrowIfZero(messages.Count, nameof(messages));

        IEnumerable<ConsoleMessage> filtered = messages;

        if (options.From is not null)
        {
            filtered = filtered.Where(m => m.Timestamp >= options.From);
        }

        if (options.To is not null)
        {
            filtered = filtered.Where(m => m.Timestamp <= options.To);
        }

        var projected = filtered.Select(m => new ExportedMessage
        {
            Timestamp = options.IncludeTimestamp ? m.Timestamp : null,
            Level = options.IncludeLevel ? m.Level : null,
            Category = options.IncludeCategory ? m.Category : null,
            Message = m.Message,
            Properties = options.IncludeProperties ? m.Properties : null,
            Exception = options.IncludeExceptions ? m.Exception : null
        });

        var json = JsonSerializer.Serialize(
            projected,
            s_jsonOptions
        );

        return new ConsoleExportResult
        {
            Content = Encoding.UTF8.GetBytes(json),
            FileName = BuildFileName(options),
            ContentType = MimeType
        };
    }

    /// <summary>
    /// Generates a file name for a console export using the specified options and the current timestamp.
    /// </summary>
    /// <remarks>This method ensures that each generated file name is unique to the time of export. The
    /// timestamp uses the local time zone.</remarks>
    /// <param name="options">The options used to configure the file name generation. If the FileNamePrefix property is null, empty, or
    /// consists only of white-space characters, a default prefix of 'console' is used.</param>
    /// <returns>A string representing the generated file name, formatted as '{prefix}_{timestamp}.json', where 'prefix' is
    /// determined by the options and 'timestamp' is the current date and time in 'yyyyMMdd_HHmmss' format.</returns>
    private static string BuildFileName(ConsoleExportOptions options)
    {
        var prefix = string.IsNullOrWhiteSpace(options.FileNamePrefix)
            ? "console"
            : options.FileNamePrefix;

        var timestamp = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss", s_cultureInfo);

        return $"{prefix}_{timestamp}.json";
    }
}

