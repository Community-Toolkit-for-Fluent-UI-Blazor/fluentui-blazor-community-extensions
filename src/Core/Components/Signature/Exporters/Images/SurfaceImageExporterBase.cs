namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the base exporter.
/// </summary>
/// <param name="target">Represents the target that will be used to export the signature.</param>
/// <param name="exporter">Represents the exporter that will export the data to the good format.</param>
/// <param name="builder">Represents the builder that will build the target from the payload.</param>
public abstract class SurfaceImageExporterBase<TPayload>(
    ISurfaceRenderTarget target,
    ISurfaceImageExporter<TPayload> exporter,
    ISurfaceTargetBuilder<TPayload> builder)
    : ISurfaceExporter<TPayload>
{
    /// <inheritdoc />
    public async ValueTask<ExportResult> ExportAsync(
        string? fileName,
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options)
    {
        try
        {
            await builder.BuildAsync(target, payload, options);
            var bytes = await ExportAsync(exporter, target, options);

            return ExportResult.Create(fileName, GetFormatExtension(), bytes);
        }
        catch(NotSupportedException)
        {
            throw;
        }
        catch(Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Asynchronously exports the provided byte data using the specified surface exporter.
    /// </summary>
    /// <param name="exporter">The surface exporter used to process and export the data. Cannot be null.</param>
    /// <param name="options">The options that influence the export process, such as quality settings, background configuration, and other parameters. Cannot be null.</param>
    /// <param name="target">The surface render target associated with the data being exported. This parameter provides context for the export operation and may be used to access additional information about the rendering environment. Cannot be null.</param>
    /// <returns>A ValueTask representing the asynchronous export operation.</returns>
    protected abstract ValueTask<byte[]> ExportAsync(
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceRenderTarget target,
        SurfaceExportOptions options);

    /// <summary>
    /// Retrieves the file extension associated with the format supported by the implementation.
    /// </summary>
    /// <remarks>Override this method in a derived class to specify the file extension used for serialization
    /// or deserialization operations. The returned extension should match the format handled by the
    /// implementation.</remarks>
    /// <returns>A string containing the file extension for the supported format, including the leading period (for example,
    /// ".json").</returns>
    protected abstract string GetFormatExtension();
}
