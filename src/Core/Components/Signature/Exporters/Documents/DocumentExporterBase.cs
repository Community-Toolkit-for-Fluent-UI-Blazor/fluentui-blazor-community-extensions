namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for exporting documents from surface payloads using a specified exporter implementation.
/// Enables derived classes to define custom export formats and serialization logic.
/// </summary>
/// <remarks>This class is intended to be inherited by format-specific exporters that implement document
/// serialization and file extension logic. It abstracts common export operations and delegates format-specific behavior
/// to derived classes.</remarks>
/// <typeparam name="TPayload">The type of the payload data to be exported. Must be a non-nullable type.</typeparam>
/// <param name="builder">The payload builder used to generate the binary data for export. Cannot be null.</param>
public abstract class DocumentExporterBase<TPayload>(
        IDocumentPayloadBuilder<TPayload> builder)
    : ISurfaceExporter<TPayload>
{
    /// <inheritdoc />
    public async ValueTask<ExportResult> ExportAsync(
        string? fileName,
        SurfacePayload<TPayload> payload,
        ExportOptions options)
    {
        var bytes = await builder.BuildAsync(payload, options);

        return ExportResult.Create(fileName, GetFormatExtension(), bytes);
    }

    /// <summary>
    /// Retrieves the file extension associated with the format implemented by the derived class.
    /// </summary>
    /// <remarks>Override this method in a derived class to specify the file extension used for serialization
    /// or deserialization operations. The returned extension should match the format supported by the
    /// implementation.</remarks>
    /// <returns>A string containing the file extension for the format, including the leading period (for example, ".json" or
    /// ".xml").</returns>
    protected abstract string GetFormatExtension();
}
