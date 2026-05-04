namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export surface data as a TIFF image file using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to generate TIFF images from signature
/// strokes. It is typically used in scenarios where signatures need to be saved or transmitted in a high-fidelity
/// raster format. Instances of this class are not thread-safe.</remarks>
public sealed class TiffExporter<TPayload> : SurfaceImageExporterBase<TPayload>
{
    /// <inheritdoc /> 
    /// <summary>
    /// Initializes a new instance of the <see cref="TiffExporter{TPayload}" /> class.
    /// </summary>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    /// <param name="exporter">Represents the export that will export the target into a tiff file.</param>
    public TiffExporter(
        ISurfaceRenderTarget target,
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceTargetBuilder<TPayload> builder)
        : base(target, exporter, builder)
    { }

    /// <inheritdoc />
    protected override async ValueTask<byte[]> ExportAsync(
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceRenderTarget target,
        SurfaceExportOptions options)
    {
        return await exporter.ToTiffAsync(target);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "tiff";
    }
}
