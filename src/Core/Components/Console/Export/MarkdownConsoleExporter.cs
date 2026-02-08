using System.Globalization;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Exports console messages to a Markdown formatted string, allowing for customizable output based on specified
/// options.
/// </summary>
/// <remarks>The export process can filter messages based on a specified time range and includes options to
/// include timestamps, log levels, categories, properties, and exceptions. The resulting Markdown content is suitable
/// for saving to a file with a default name of 'console.md'.</remarks>
public sealed class MarkdownConsoleExporter : IConsoleExporter
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
    public string Format => "markdown";

    /// <inheritdoc />
    public string MimeType => "text/markdown";

    /// <inheritdoc />
    public string DefaultFileName => "console.md";

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

        foreach (var m in filtered)
        {
            cancellationToken.ThrowIfCancellationRequested();

            sb.Append("# CONSOLE MESSAGES");
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("## ");

            if (options.IncludeTimestamp)
            {
                sb.Append(s_cultureInfo, $"[{m.Timestamp:yyyy-MM-dd HH:mm:ss}] ");
            }

            if (options.IncludeLevel)
            {
                sb.Append(s_cultureInfo, $"{m.Level} ");
            }

            if (options.IncludeCategory && !string.IsNullOrWhiteSpace(m.Category))
            {
                sb.Append(s_cultureInfo, $"{m.Category} ");
            }

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine(m.Message);
            sb.AppendLine();

            if (options.IncludeProperties && m.Properties is not null)
            {
                sb.AppendLine("**Properties**");

                foreach (var kvp in m.Properties)
                {
                    sb.AppendLine(s_cultureInfo, $"- **{kvp.Key}**: {kvp.Value}");
                }

                sb.AppendLine();
            }

            if (options.IncludeExceptions && m.Exception is not null)
            {
                sb.AppendLine("**Exception**");
                sb.AppendLine(s_cultureInfo, $"- **Type**: {m.Exception.GetType().Name}");
                sb.AppendLine(s_cultureInfo, $"- **Message**: {m.Exception.Message}");
                sb.AppendLine();
                sb.AppendLine("```\n" + m.Exception.StackTrace + "\n```");
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();
        }

        var markdownContent = sb.ToString();

        return new ConsoleExportResult
        {
            Content = Encoding.UTF8.GetBytes(markdownContent),
            ContentType = MimeType,
            FileName = BuildFileName(options)
        };
    }

    /// <summary>
    /// Generates a file name for a console export using the specified options and the current timestamp.
    /// </summary>
    /// <remarks>The generated file name includes a timestamp to ensure uniqueness and to indicate when the
    /// export was created.</remarks>
    /// <param name="options">The options used to configure the file name generation. If the FileNamePrefix property is null, empty, or
    /// consists only of white-space characters, a default prefix of "console" is used.</param>
    /// <returns>A string containing the generated file name, formatted as '{prefix}_{timestamp}.md', where 'prefix' is
    /// determined by the options and 'timestamp' is the current date and time in 'yyyyMMdd_HHmmss' format.</returns>
    private static string BuildFileName(ConsoleExportOptions options)
    {
        var prefix = string.IsNullOrWhiteSpace(options.FileNamePrefix)
            ? "console"
            : options.FileNamePrefix;

        var timestamp = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss", s_cultureInfo);

        return $"{prefix}_{timestamp}.md";
    }
}
