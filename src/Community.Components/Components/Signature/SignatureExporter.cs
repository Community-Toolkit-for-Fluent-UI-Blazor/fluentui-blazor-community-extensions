using System.Text;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Exports a signature in the specified format, returning the file name, content type, and binary data.    
/// </summary>
/// <remarks>The export format is determined by the <see cref="SignatureExportOptions.Format"/> property: <list
/// type="bullet"> <item><description>If the format is <see cref="SignatureImageFormat.Svg"/>, the method returns the
/// SVG data as-is.</description></item> <item><description>If the format is <see cref="SignatureImageFormat.Pdf"/>, the
/// SVG is converted to a PDF document.</description></item> <item><description>For other image formats (e.g., PNG,
/// JPEG), the SVG is converted to the specified format using the provided quality setting.</description></item>
/// </list></remarks>
internal static class SignatureExporter
{
    /// <summary>
    /// Exports a signature image in the specified format.
    /// </summary>
    /// <remarks>The method supports exporting the signature in various formats, including SVG, PDF, and
    /// common image formats such as PNG, JPEG, BMP, GIF, TIFF, and WebP. The content type and file extension are
    /// determined based on the selected format. For SVG format, the method directly returns the input SVG data. For PDF
    /// and other image formats, the method converts the SVG data using the appropriate conversion logic.</remarks>
    /// <param name="options">The export options, including the desired image format and quality settings.</param>
    /// <param name="svg">The SVG representation of the signature to be exported. Cannot be null or empty.</param>
    /// <returns>A tuple containing the filename, content type, and binary data of the exported signature image. The filename
    /// includes the appropriate file extension based on the selected format.</returns>
    public static (string filename, string contentType, byte[] data) Export(
        SignatureExportOptions options,
        string svg)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(svg);

        if (options.Format == SignatureImageFormat.Svg || RunningWasmHelper.IsWasm)
        {
            return ("signature.svg", "image/svg+xml", Encoding.UTF8.GetBytes(svg));
        }
        else if (options.Format == SignatureImageFormat.Pdf)
        {
            var pdfData = SvgConverter.ConvertToPdf(svg);

            return ("signature.pdf", "application/pdf", pdfData);
        }
        else
        {
            var imageData = SvgConverter.ConvertToImage(svg, options.Format, options.Quality);
            var extension = options.Format.ToString().ToLower();
            var contentType = options.Format switch
            {
                SignatureImageFormat.Png => "image/png",
                SignatureImageFormat.Jpeg => "image/jpeg",
                SignatureImageFormat.Bmp => "image/bmp",
                SignatureImageFormat.Gif => "image/gif",
                SignatureImageFormat.Webp => "image/webp",
                _ => "application/octet-stream"
            };
            return ($"signature.{extension}", contentType, imageData);
        }
    }
}
