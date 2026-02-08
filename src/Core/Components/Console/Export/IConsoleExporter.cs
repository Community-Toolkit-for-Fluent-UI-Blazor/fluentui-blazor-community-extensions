namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for exporting console messages to a specific format.
/// </summary>
/// <remarks>Implementations of this interface provide mechanisms to export collections of console messages using
/// a defined format and options. The exported data can be used for reporting, archiving, or integration with other
/// systems. The interface exposes properties describing the export format, MIME type, and a recommended default file
/// name for the exported content.</remarks>
public interface IConsoleExporter
{
    /// <summary>
    /// Gets a string that represents the format of the export, such as "JSON", "CSV", or "XML".
    /// </summary>
    string Format { get; }

    /// <summary>
    /// Gets the MIME type of the content, which indicates the nature and format of the data.
    /// </summary>
    string MimeType { get; }

    /// <summary>
    /// Gets the default file name that is suggested when saving data.
    /// </summary>
    string DefaultFileName { get; }

    /// <summary>
    /// Asynchronously exports a collection of console messages using the specified export options.
    /// </summary>
    /// <remarks>This method may throw exceptions if the export fails due to invalid options or other issues.
    /// Callers should handle exceptions as appropriate for their scenario.</remarks>
    /// <param name="messages">The collection of console messages to export. This collection must not be null or empty.</param>
    /// <param name="options">The options that configure the export operation, such as format and destination settings. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the export operation.</param>
    /// <returns>A value task that represents the asynchronous export operation. The result contains information about the
    /// outcome of the export.</returns>
    ValueTask<ConsoleExportResult> ExportAsync(
        IReadOnlyCollection<ConsoleMessage> messages,
        ConsoleExportOptions options,
        CancellationToken cancellationToken = default);
}
