namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature strokes as a JPEG image using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to generate JPEG images from digital signature
/// data. It is sealed and cannot be inherited. The exported image format is always 'image/jpeg'.</remarks>
public sealed class JpegExporter<TPayload> : SurfaceImageExporterBase<TPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JpegExporter{TPayload}" /> class.
    /// </summary>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    /// <param name="exporter">Represents the exporter for the avif format.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    public JpegExporter(
        ISurfaceRenderTarget target,
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceTargetBuilder<TPayload> builder)
        : base(target, exporter, builder)
    { }

    /// <inheritdoc />
    protected override async ValueTask<byte[]> ExportAsync(
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceRenderTarget target,
        ExportOptions options)
    {
        return await exporter.ToJpegAsync(target, options.Quality);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "jpg";
    }
}
