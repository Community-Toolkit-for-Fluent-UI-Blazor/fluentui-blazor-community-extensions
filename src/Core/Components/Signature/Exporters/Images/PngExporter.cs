namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature strokes as a PNG image using an offscreen renderer.
/// </summary>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to generate PNG images from signature data. It
/// is sealed and cannot be inherited. Use this exporter when you need to save or transmit signatures in PNG format. The
/// export process relies on the provided OffscreenSignatureRenderer to render strokes before encoding them as
/// PNG.</remarks>
public sealed class PngExporter<TPayload> : SurfaceImageExporterBase<TPayload>
{
    /// <inheritdoc /> 
    /// <summary>
    /// Initializes a new instance of the <see cref="PngExporter{TPayload}" /> class.
    /// </summary>
    /// <param name="builder">Represents the builder that will build the target from the payload.</param>
    /// <param name="exporter">Represents the exporter for the avif format.</param>
    /// <param name="target">Represents the target to use to export the signature data.</param>
    public PngExporter(
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
        return await exporter.ToPngAsync(target);
    }

    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "png";
    }
}
