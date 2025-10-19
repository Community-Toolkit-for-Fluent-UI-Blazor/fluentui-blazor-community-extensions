using System.Text;
using FluentUI.Blazor.Community.Components;
using SkiaSharp;

namespace FluentUI.Blazor.Community.Helpers;

internal class SvgConverter
{
    /// <summary>
    /// Converts an SVG string to an image in the specified format and quality.
    /// </summary>
    /// <remarks>The method uses SkiaSharp to render the SVG content and encode it into the specified image
    /// format. The output image will have a transparent background if the SVG does not specify a background
    /// color.</remarks>
    /// <param name="svg">The SVG content as a string. This value cannot be <see langword="null"/> or empty.</param>
    /// <param name="format">The desired image format, such as PNG or JPEG.</param>
    /// <param name="quality">The quality of the output image, ranging from 0 to 100. Higher values indicate better quality.</param>
    /// <returns>A byte array representing the image data in the specified format.</returns>
    internal static byte[] ConvertToImage(string svg, SignatureImageFormat format, int quality)
    {
        var svgBytes = Encoding.UTF8.GetBytes(svg);
        using var svgStream = new MemoryStream(svgBytes);
        var skSvg = new Svg.Skia.SKSvg();
        skSvg.Load(svgStream);

        var bounds = skSvg.Picture!.CullRect;
        var width = (int)bounds.Width;
        var height = (int)bounds.Height;

        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);

        canvas.Clear(SKColors.Transparent);
        canvas.DrawPicture(skSvg.Picture);
        canvas.Flush();

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(GetFromFormat(format), quality);

        return data.ToArray();
    }

    private static SKEncodedImageFormat GetFromFormat(SignatureImageFormat format)
    {
        return format switch
        {
            SignatureImageFormat.Png => SKEncodedImageFormat.Png,
            SignatureImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            SignatureImageFormat.Bmp => SKEncodedImageFormat.Bmp,
            SignatureImageFormat.Gif => SKEncodedImageFormat.Gif,
            SignatureImageFormat.Webp => SKEncodedImageFormat.Webp,
            _ => throw new NotSupportedException($"The image format '{format}' is not supported for conversion."),
        };
    }

    /// <summary>
    /// Converts an SVG string to a PDF document represented as a byte array.
    /// </summary>
    /// <remarks>This method uses SkiaSharp to render the SVG content and generate a PDF document.  The
    /// dimensions of the PDF page are determined by the bounds of the SVG content.</remarks>
    /// <param name="svg">The SVG content as a string. This must be a valid SVG document.</param>
    /// <returns>A byte array containing the PDF representation of the provided SVG content.</returns>
    internal static byte[] ConvertToPdf(
        string svg)
    {
        var svgBytes = Encoding.UTF8.GetBytes(svg);
        using var svgStream = new MemoryStream(svgBytes);
        var skSvg = new Svg.Skia.SKSvg();
        skSvg.Load(svgStream);

        using var pdfStream = new MemoryStream();
        using var document = SKDocument.CreatePdf(pdfStream);
        using var canvas = document.BeginPage(skSvg.Picture!.CullRect.Width, skSvg.Picture.CullRect.Height);

        canvas.DrawPicture(skSvg.Picture);

        document.EndPage();
        document.Close();

        return pdfStream.ToArray();
    }
}
