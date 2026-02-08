using System.Globalization;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export console messages in a plain text format. 
/// </summary>
public sealed class TextConsoleExporter : IConsoleExporter
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
    public string Format => "txt";

    /// <inheritdoc />
    public string MimeType => "text/plain";

    /// <inheritdoc />
    public string DefaultFileName => "logs.txt";

    /// <inheritdoc />
    public async ValueTask<ConsoleExportResult> ExportAsync(
        IReadOnlyCollection<ConsoleMessage> messages,
        ConsoleExportOptions? options,
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

        foreach (var message in filtered)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (options.IncludeTimestamp)
            {
                sb.Append(s_cultureInfo, $"[{message.Timestamp:yyyy-MM-dd HH:mm:ss}] ");
            }

            if (options.IncludeLevel)
            {
                sb.Append(s_cultureInfo, $"{message.Level} ");
            }

            if (options.IncludeCategory && !string.IsNullOrWhiteSpace(message.Category))
            {
                sb.Append(s_cultureInfo, $"{message.Category}: ");
            }

            sb.Append(message.Message);

            sb.AppendLine();

            if (options.IncludeProperties && message.Properties is not null)
            {
                foreach (var kvp in message.Properties)
                {
                    sb.AppendLine(s_cultureInfo, $"  {kvp.Key}: {kvp.Value}");
                }
            }

            if (options.IncludeExceptions && message.Exception is not null)
            {
                sb.AppendLine("  Exception:");
                sb.AppendLine(s_cultureInfo, $"    {message.Exception.GetType().Name}: {message.Exception.Message}");
                sb.AppendLine(message.Exception.StackTrace);
            }

            sb.AppendLine();
        }

        var content = sb.ToString();

        return new ConsoleExportResult
        {
            Content = Encoding.UTF8.GetBytes(content),
            FileName = BuildFileName(options),
            ContentType = MimeType
        };
    }

    /// <summary>
    /// Generates a file name for console export using the specified options and a timestamp.
    /// </summary>
    /// <remarks>If the FileNamePrefix property of the options parameter is null, empty, or consists only of
    /// white-space characters, 'console' is used as the default prefix. The timestamp is formatted as
    /// 'yyyyMMdd_HHmmss'.</remarks>
    /// <param name="options">The options that configure file name generation, including an optional prefix for the file name.</param>
    /// <returns>A string containing the generated file name, formatted as '{prefix}_{timestamp}.txt'.</returns>
    private static string BuildFileName(ConsoleExportOptions options)
    {
        var prefix = string.IsNullOrWhiteSpace(options.FileNamePrefix)
            ? "console"
            : options.FileNamePrefix;

        var timestamp = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss", s_cultureInfo);

        return $"{prefix}_{timestamp}.txt";
    }
}

