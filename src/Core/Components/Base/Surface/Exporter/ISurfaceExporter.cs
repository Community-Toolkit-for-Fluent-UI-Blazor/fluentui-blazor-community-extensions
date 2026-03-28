namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for exporting a payload to a file asynchronously using configurable export options.
/// </summary>
/// <remarks>Implementations of this interface should handle serialization and file output according to the
/// provided options. The export operation is performed asynchronously and may involve I/O or transformation of the
/// payload. The result provides details about the outcome, including any errors encountered during export.</remarks>
/// <typeparam name="TPayload">The type of the data contained in the payload to be exported.</typeparam>
public interface ISurfaceExporter<TPayload>
{
    /// <summary>
    /// Exports the specified payload to a file asynchronously using the provided export options.
    /// </summary>
    /// <param name="fileName">The name of the file to which the payload will be exported. Can be null to use a default name.</param>
    /// <param name="payload">The payload containing the data to export. Must not be null.</param>
    /// <param name="options">The options that configure how the export operation is performed. Must not be null.</param>
    /// <returns>A ValueTask that represents the asynchronous export operation. The result contains information about the export,
    /// including success or failure details.</returns>
    ValueTask<ExportResult> ExportAsync(
        string? fileName,
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options);
}

