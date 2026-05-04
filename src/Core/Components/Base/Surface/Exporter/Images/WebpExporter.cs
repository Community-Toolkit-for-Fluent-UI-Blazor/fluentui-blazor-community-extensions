namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature data as a WebP image using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to enable exporting drawn signature strokes to
/// the WebP image format. It is typically used in scenarios where high-quality, compressed signature images are
/// required for storage or transmission. The exporter relies on an OffscreenSignatureRenderer to render the signature
/// before encoding it as WebP.</remarks>
public sealed class WebpExporter<TPayload> : SurfaceImageExporterBase<TPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebpExporter{TPayload}" /> class.
    /// </summary>
    /// <param name="exporter">Represents the exporter for the avif format.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    public WebpExporter(
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
        return await exporter.ToWebpAsync(target, options.Quality);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "webp";
    }
}
