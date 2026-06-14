using System.IO.Compression;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality for exporting data to the console.
/// </summary>
/// <param name="consoleExporters">A collection of console exporters to handle different export formats.</param>
/// <param name="consoleState">The console state to manage the current state of the console.</param>
internal sealed class ConsoleExportService(IConsoleState consoleState,
                                          IEnumerable<IConsoleExporter> consoleExporters) : IConsoleExportService

{
    /// <summary>
    /// Gets the collection of console exporters, indexed by their format identifier.
    /// </summary>
    /// <remarks>This dictionary allows efficient retrieval of an IConsoleExporter instance based on the
    /// desired output format. Each exporter in the collection supports a specific console output format, as indicated
    /// by its Format property.</remarks>
    private readonly Dictionary<string, IConsoleExporter> _exporters = consoleExporters.ToDictionary(e => e.Format);

    /// <summary>
    /// Selects console messages that match the specified export options, applying timestamp filters if provided.
    /// </summary>
    /// <remarks>If no timestamp filters are specified in the options, all messages from the selected source
    /// are returned. The method uses either the filtered or unfiltered message set based on the export
    /// options.</remarks>
    /// <param name="options">The options that determine which console messages to include in the selection. This includes filters for message
    /// timestamps and whether to export only filtered messages.</param>
    /// <returns>A read-only collection of console messages that satisfy the specified criteria. The collection is empty if no
    /// messages match the filters.</returns>
    private IReadOnlyCollection<ConsoleMessage> SelectMessages(ConsoleExportOptions options)
    {
        var source = options.ExportFilteredMessagesOnly ? consoleState.FilteredMessages : consoleState.Messages;

        return [.. source.Where(m =>
            (!options.From.HasValue || m.Timestamp >= options.From.Value) &&
            (!options.To.HasValue || m.Timestamp <= options.To.Value))];
    }

    /// <inheritdoc />
    public async Task<ConsoleExportResult> ExportAsync(
        string format,
        ConsoleExportOptions options,
        CancellationToken cancellationToken = default)
    {
        var exporter = _exporters[format];
        var messages = SelectMessages(options);

        return await exporter.ExportAsync(messages, options, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ConsoleExportResult> ExportAsync(
        IReadOnlyCollection<string> formats,
        ConsoleExportOptions options,
        CancellationToken cancellationToken = default)
    {
        var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var messages = SelectMessages(options);

            foreach (var format in formats)
            {
                var exporter = _exporters[format];
                var result = await exporter.ExportAsync(messages, options, cancellationToken);
                var entry = archive.CreateEntry(result.FileName);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(result.Content, cancellationToken);
            }
        }

        await zipStream.FlushAsync(cancellationToken);
        zipStream.Position = 0;

        return new ConsoleExportResult
        {
            FileName = $"console_export_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.zip",
            ContentType = "application/zip",
            Content = zipStream.ToArray()
        };
    }
}
