using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Exports a signature as a file in the specified format.
/// </summary>
/// <remarks>The export format is determined by the <see cref="SignatureExportSettings.Format"/> property. 
/// Supported formats include PNG, JPEG, WebP, SVG, and PDF. If the application is running in a  browser environment,
/// only SVG export is supported.</remarks>
internal static class SignatureExporter
{
    /// <summary>
    /// Exports a signature as a file in the specified format, including optional settings for quality, signature
    /// appearance, and watermarking.
    /// </summary>
    /// <remarks>The export format is determined by the <see cref="SignatureExportSettings.Format"/> property.
    /// Supported formats include PNG, JPEG, WebP, SVG, and PDF. If the application is running in a browser environment,
    /// only SVG export is supported.</remarks>
    /// <param name="width">The width of the signature canvas, in pixels.</param>
    /// <param name="height">The height of the signature canvas, in pixels.</param>
    /// <param name="strokes">A collection of strokes that define the signature.</param>
    /// <param name="exportSettings">The settings that specify the export format and quality.</param>
    /// <param name="signatureSettings">The settings that define the appearance of the signature.</param>
    /// <param name="watermarkSettings">The settings for applying a watermark to the exported signature.</param>
    /// <returns>A tuple containing the exported file's byte array, MIME type, and filename.  The MIME type and filename are
    /// determined based on the export format.</returns>
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
