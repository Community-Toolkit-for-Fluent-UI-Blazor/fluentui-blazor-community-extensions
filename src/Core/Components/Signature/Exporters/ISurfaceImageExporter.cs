namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the exporter of a surface.
/// </summary>
/// <remarks>
/// Surface is a generic term for a drawing surface (like Html canvas, SKCanvas, BitmapImage...)
/// </remarks>
public interface ISurfaceImageExporter<TPayload>
{
    /// <summary>
    /// Converts the specified render target to an AVIF-encoded image asynchronously with the given quality setting.
    /// </summary>
    /// <param name="target">The render target to be encoded as an AVIF image. Cannot be null.</param>
    /// <param name="quality">The quality level for AVIF encoding, typically in the range 0–100. Higher values produce better image quality at
    /// the cost of larger file size.</param>
    /// <returns>A value task representing the asynchronous operation. The result contains a byte array with the AVIF-encoded
    /// image data.</returns>
    ValueTask<byte[]> ToAvifAsync(ISurfaceRenderTarget target, int quality);

    /// <summary>
    /// Converts the specified render target to a BMP image asynchronously.
    /// </summary>
    /// <remarks>The returned byte array contains the BMP file format data representing the rendered surface.
    /// This method does not block the calling thread.</remarks>
    /// <param name="target">The render target to convert to a BMP image. Must not be null.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the BMP image
    /// data.</returns>
    ValueTask<byte[]> ToBmpAsync(ISurfaceRenderTarget target);

    /// <summary>
    /// Converts the specified render target to a HEIF image asynchronously with the given quality setting.
    /// </summary>
    /// <remarks>The conversion is performed asynchronously and may be resource-intensive depending on the
    /// quality setting and the size of the render target. Ensure that the quality parameter is within the supported
    /// range for optimal results.</remarks>
    /// <param name="target">The render target to be converted to a HEIF image. Cannot be null.</param>
    /// <param name="quality">The quality level for the HEIF image, typically in the range 0 to 100. Higher values produce better image
    /// quality at the cost of larger file size.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the encoded HEIF
    /// image data.</returns>
    ValueTask<byte[]> ToHeifAsync(ISurfaceRenderTarget target, int quality);

    /// <summary>
    /// Asynchronously converts the specified render target to a JPEG image with the given quality setting.
    /// </summary>
    /// <remarks>Higher quality values produce larger files with better image fidelity. Lower values reduce
    /// file size at the expense of image quality. The operation does not block the calling thread.</remarks>
    /// <param name="target">The render target to be converted to a JPEG image. Cannot be null.</param>
    /// <param name="quality">The quality level for the JPEG encoding, ranging from 0 (lowest quality) to 100 (highest quality).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the JPEG-encoded
    /// image data.</returns>
    ValueTask<byte[]> ToJpegAsync(ISurfaceRenderTarget target, int quality);

    /// <summary>
    /// Asynchronously converts the specified render target surface to a PNG image and returns the resulting byte array.
    /// </summary>
    /// <remarks>The returned byte array can be used for saving, streaming, or displaying the PNG image. The
    /// operation is performed asynchronously and does not block the calling thread.</remarks>
    /// <param name="target">The render target surface to be converted to PNG format. Cannot be null.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the PNG image
    /// data.</returns>
    ValueTask<byte[]> ToPngAsync(ISurfaceRenderTarget target);

    /// <summary>
    /// Asynchronously converts the specified surface render target to a TIFF image and returns the resulting byte
    /// array.
    /// </summary>
    /// <remarks>The returned byte array can be used for saving or transmitting the TIFF image. Ensure that
    /// the target is valid and properly initialized before calling this method.</remarks>
    /// <param name="target">The surface render target to convert to TIFF format. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the TIFF image
    /// data.</returns>
    ValueTask<byte[]> ToTiffAsync(ISurfaceRenderTarget target);

    /// <summary>
    /// Asynchronously converts the specified render target to a WebP-encoded byte array with the given quality setting.
    /// </summary>
    /// <param name="target">The render target to be converted to WebP format. Cannot be null.</param>
    /// <param name="quality">The quality level for the WebP encoding, ranging from 0 (lowest quality) to 100 (highest quality).</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the WebP-encoded
    /// image data.</returns>
    ValueTask<byte[]> ToWebpAsync(ISurfaceRenderTarget target, int quality);
}
