namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export signature data as an SVG image using a specified SVG signature renderer.
/// </summary>
/// <param name="payload">The payload builder responsible for constructing the SVG document content based on the provided signature data and export options. Must not be null.</param>
/// <typeparam name="TPayload">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class implements the ISignatureExporter interface to enable exporting digital signatures in SVG
/// format. It is designed to work with a provided ISvgSignatureRenderer to generate SVG output from signature stroke
/// data. Instances of this class are immutable and thread-safe for concurrent use.</remarks>
public sealed class SvgExporter<TPayload>(SvgDocumentPayloadBuilderBase<TPayload> payload)
    : DocumentExporterBase<TPayload>(payload)
{
    /// <inheritdoc />
    protected override string GetFormatExtension()
    {
        return "svg";
    }
}
