using System.Globalization;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export console messages to an XML format for logging, analysis, or archival purposes.
/// </summary>
/// <remarks>The XmlConsoleExporter implements the IConsoleExporter interface and supports customizable export
/// options, including filtering messages by timestamp, including or excluding specific message properties, and handling
/// exceptions. The exported XML structure contains detailed information for each console message, such as timestamp,
/// log level, category, message text, properties, and exception details when applicable. This exporter is suitable for
/// scenarios where a structured, portable, and machine-readable log format is required.</remarks>
public sealed class XmlConsoleExporter : IConsoleExporter
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
    public string Format => "xml";

    /// <inheritdoc />
    public string MimeType => "application/xml";

    /// <inheritdoc />
    public string DefaultFileName => "logs.xml";

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
        sb.AppendLine("<ConsoleExport>");

        foreach (var m in filtered)
        {
            cancellationToken.ThrowIfCancellationRequested();

            sb.AppendLine("\t<Message>");

            if (options.IncludeTimestamp)
            {
                sb.AppendLine(s_cultureInfo, $"\t\t<Timestamp>{m.Timestamp:O}</Timestamp>");
            }

            if (options.IncludeLevel)
            {
                sb.AppendLine(s_cultureInfo, $"\t\t<Level>{Escape(m.Level.ToString())}</Level>");
            }

            if (options.IncludeCategory)
            {
                sb.AppendLine(s_cultureInfo, $"\t\t<Category>{Escape(m.Category)}</Category>");
            }

            sb.AppendLine(s_cultureInfo, $"\t\t<Message>{Escape(m.Message)}</Message>");

            if (options.IncludeProperties && m.Properties is not null)
            {
                sb.AppendLine("\t\t<Properties>");

                foreach (var kvp in m.Properties)
                {
                    sb.AppendLine(s_cultureInfo, $"\t\t\t<Property key=\"{Escape(kvp.Key)}\">{Escape(kvp.Value?.ToString())}</Property>");
                }

                sb.AppendLine("\t\t</Properties>");
            }

            if (options.IncludeExceptions && m.Exception is not null)
            {
                sb.AppendLine("\t\t<Exception>");
                sb.AppendLine(s_cultureInfo, $"\t\t\t<Type>{Escape(m.Exception.GetType().Name)}</Type>");
                sb.AppendLine(s_cultureInfo, $"\t\t\t<Message>{Escape(m.Exception.Message)}</Message>");
                sb.AppendLine(s_cultureInfo, $"\t\t\t<StackTrace>{Escape(m.Exception.StackTrace)}</StackTrace>");
                sb.AppendLine("\t\t</Exception>");
            }

            sb.AppendLine("\t</Message>");
        }

        sb.AppendLine("</ConsoleExport>");

        var xmlContent = sb.ToString();

        return new ConsoleExportResult
        {
            Content = Encoding.UTF8.GetBytes(xmlContent),
            ContentType = MimeType,
            FileName = BuildFileName(options)
        };
    }

    /// <summary>
    /// Escapes a string to ensure it is safe for inclusion in XML content.
    /// </summary>
    /// <remarks>Use this method to sanitize input before embedding it in XML documents, preventing XML
    /// injection and ensuring well-formed output.</remarks>
    /// <param name="value">The string to be escaped. If null, an empty string is returned.</param>
    /// <returns>A string containing the XML-escaped representation of the input. Returns an empty string if the input is null.</returns>
    private static string Escape(string? value) => System.Security.SecurityElement.Escape(value) ?? "";

    /// <summary>
    /// Generates a file name for console export using the specified options.
    /// </summary>
    /// <remarks>The generated file name incorporates a timestamp to ensure uniqueness. This method does not
    /// create or write to the file; it only returns the file name string.</remarks>
    /// <param name="options">The options used to configure the file name generation. If <c>FileNamePrefix</c> is null, empty, or consists
    /// only of white-space characters, the default prefix 'console' is used.</param>
    /// <returns>A string containing the generated file name, formatted as '{prefix}_{timestamp}.xml', where 'prefix' is
    /// determined by the options and 'timestamp' is the current date and time in 'yyyyMMdd_HHmmss' format.</returns>
    private static string BuildFileName(ConsoleExportOptions options)
    {
        var prefix = string.IsNullOrWhiteSpace(options.FileNamePrefix)
            ? "console"
            : options.FileNamePrefix;

        var timestamp = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss", s_cultureInfo);

        return $"{prefix}_{timestamp}.xml";
    }
}

