namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature data as an AVIF image using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This exporter generates AVIF images from signature strokes and options, suitable for scenarios where
/// high compression and modern image format support are desired. The class is sealed and intended for use with the
/// ISignatureExporter interface.</remarks>
public sealed class AvifExporter<TPayload>
    : SurfaceImageExporterBase<TPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AvifExporter{TPayload}" /> class.
    /// </summary>
    /// <param name="exporter">Represents the exporter for the avif format.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    public AvifExporter(
        ISurfaceRenderTarget target,
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceTargetBuilder<TPayload> builder
        )
        : base(target, exporter, builder)
    { }

    /// <inheritdoc /> 
    protected override async ValueTask<byte[]> ExportAsync(
        ISurfaceImageExporter<TPayload> exporter,
        ISurfaceRenderTarget target,
        ExportOptions options)
    {
        return await exporter.ToAvifAsync(target, options.Quality);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "avif";
    }
}
