using FluentUI.Blazor.Community.Helpers;

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
        if (!RunningWasmHelper.IsWasm)
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
        else
        {
            // On browser mode, we can only export SVG, for now.
            return SignatureSvgExporter.Export(width, height, strokes, signatureSettings, watermarkSettings);
        }
    }
}
