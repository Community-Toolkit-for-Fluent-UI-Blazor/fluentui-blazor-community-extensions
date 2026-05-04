namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature data as a BMP image using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to generate BMP images from signature strokes.
/// It is typically used in scenarios where signatures need to be saved or transmitted in the BMP image format.
/// Instances of this class are not thread-safe.</remarks>
public sealed class BmpExporter<TPayload>
    : SurfaceImageExporterBase<TPayload>
{
    /// <inheritdoc /> 
    /// <summary>
    /// Initializes a new instance of the <see cref="BmpExporter{TPayload>" /> class.
    /// </summary>
    /// <param name="exporter">Represents the exporter for the avif format.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    public BmpExporter(
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
        return await exporter.ToBmpAsync(target);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "bmp";
    }
}
