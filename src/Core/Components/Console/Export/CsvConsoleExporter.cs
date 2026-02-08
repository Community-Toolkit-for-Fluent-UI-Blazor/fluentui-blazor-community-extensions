using System.Globalization;
using System.Text;
using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export console messages to a CSV file for analysis or archival purposes.
/// </summary>
/// <remarks>The CsvConsoleExporter allows customization of the exported CSV content through various options, such
/// as filtering by timestamp and including additional message properties like level, category, and exceptions. The
/// exporter is designed for asynchronous use and supports cancellation via a cancellation token. This class is sealed
/// and intended for use where a standardized, portable format for console logs is required.</remarks>
public sealed class CsvConsoleExporter : IConsoleExporter
{
    /// <summary>
    /// Represents the invariant culture, which is culture-independent and provides consistent formatting and parsing
    /// behavior regardless of the system's culture settings.
    /// </summary>
    /// <remarks>The invariant culture is useful for operations that require culture-agnostic results, such as
    /// data serialization, protocol formatting, or scenarios where consistent results are required across different
    /// locales.</remarks>
    private static readonly CultureInfo s_cultureInfo = CultureInfo.InvariantCulture;

    /// <inheritdoc />
    public string Format => "csv";

    /// <inheritdoc />
    public string MimeType => "text/csv";

    /// <inheritdoc />
    public string DefaultFileName => "logs.csv";

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

        var sb = new StringBuilder();
        var headers = new List<string>();

        if (options.IncludeTimestamp)
        {
            headers.Add("Timestamp");
        }

        if (options.IncludeLevel)
        {
            headers.Add("Level");
        }

        if (options.IncludeCategory)
        {
            headers.Add("Category");
        }

        headers.Add("Message");

        if (options.IncludeProperties)
        {
            headers.Add("Properties");
        }

        if (options.IncludeExceptions)
        {
            headers.Add("Exception");
        }

        sb.AppendLine(string.Join(",", headers));

        foreach (var m in filtered)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var row = new List<string>();

            if (options.IncludeTimestamp)
            {
                row.Add(EscapeCsv(m.Timestamp.ToString("o")));
            }

            if (options.IncludeLevel)
            {
                row.Add(EscapeCsv(m.Level.ToString()));
            }

            if (options.IncludeCategory)
            {
                row.Add(EscapeCsv(m.Category));
            }

            row.Add(EscapeCsv(m.Message));

            if (options.IncludeProperties)
            {
                var json = m.Properties is null
                    ? string.Empty
                    : JsonSerializer.Serialize(m.Properties);

                row.Add(EscapeCsv(json));
            }

            if (options.IncludeExceptions)
            {
                var ex = m.Exception is null
                    ? string.Empty
                    : $"{m.Exception.GetType().Name}: {m.Exception.Message}\n{m.Exception.StackTrace}";

                row.Add(EscapeCsv(ex));
            }

            sb.AppendLine(string.Join(",", row));
        }

        var csvContent = sb.ToString();

        return new ConsoleExportResult
        {
            Content = Encoding.UTF8.GetBytes(csvContent),
            FileName = BuildFileName(options),
            ContentType = MimeType
        };
    }

    /// <summary>
    /// Escapes a string for safe inclusion in a CSV field by enclosing it in double quotes and escaping any embedded
    /// double quotes as required by the CSV format.
    /// </summary>
    /// <remarks>This method ensures that the returned string conforms to standard CSV formatting rules,
    /// making it suitable for use in CSV file generation.</remarks>
    /// <param name="value">The string to format for CSV output. If null or empty, an empty string is returned.</param>
    /// <returns>A string formatted for CSV inclusion. If the input contains a comma, double quote, or newline character, the
    /// result is enclosed in double quotes and any embedded double quotes are escaped by doubling them. Otherwise, the
    /// original string is returned.</returns>
    private static string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (value.Contains(',') ||
            value.Contains('"') ||
            value.Contains('\n'))
        {
            value = value.Replace("\"", "\"\"");

            return $"\"{value}\"";
        }

        return value;
    }

    /// <summary>
    /// Generates a file name for a console export using the specified options.
    /// </summary>
    /// <remarks>If no prefix is provided in the options, 'console' is used as the default prefix. The
    /// timestamp reflects the current date and time at which the method is called.</remarks>
    /// <param name="options">The options used to configure the file name generation, including an optional prefix for the file name.</param>
    /// <returns>A string representing the generated file name, formatted as 'prefix_yyyyMMdd_HHmmss.csv'.</returns>
    private static string BuildFileName(ConsoleExportOptions options)
    {
        var prefix = string.IsNullOrWhiteSpace(options.FileNamePrefix)
            ? "console"
            : options.FileNamePrefix;

        var timestamp = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss", s_cultureInfo);

        return $"{prefix}_{timestamp}.csv";
    }
}

