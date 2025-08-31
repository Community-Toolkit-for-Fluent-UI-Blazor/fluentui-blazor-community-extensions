
namespace FluentUI.Blazor.Community.Components;

internal static class SignatureExporter
{
    internal static (byte[] bytes, string mime, string filename) Export(
        int width,
        int height,
        List<SignatureStroke> strokes,
        SignatureExportSettings exportSettings,
        SignatureSettings signatureSettings,
        SignatureWatermarkSettings watermarkSettings)
    {
        try
        {
            return exportSettings.Format switch
            {
                SignatureExportFormat.Png => SignatureSkiaExporter.Export(SignatureExportFormat.Png, width, height, strokes, exportSettings.Quality, signatureSettings, watermarkSettings),
                SignatureExportFormat.Jpeg => SignatureSkiaExporter.Export(SignatureExportFormat.Jpeg, width, height, strokes, exportSettings.Quality, signatureSettings, watermarkSettings),
                SignatureExportFormat.Webp => SignatureSkiaExporter.Export(SignatureExportFormat.Webp, width, height, strokes, exportSettings.Quality, signatureSettings, watermarkSettings),
                SignatureExportFormat.Svg => SignatureSvgExporter.Export(width, height, strokes, signatureSettings, watermarkSettings),
                SignatureExportFormat.Pdf => SignatureSkiaExporter.Export(SignatureExportFormat.Pdf, width, height, strokes, 0, signatureSettings, watermarkSettings),
                _ => ([], "application/octet-stream", "signature.bin"),
            };
        }
        catch (Exception)
        {
            // Fallback to SVG export in case of any error during other export methods.
            // Here, if the signature component is used in a Blazor WebAssembly app, it might be due to
            // the lack of support for SkiaSharp's native libraries.
            return SignatureSvgExporter.Export(width, height, strokes, signatureSettings, watermarkSettings);
        }
    }
}
