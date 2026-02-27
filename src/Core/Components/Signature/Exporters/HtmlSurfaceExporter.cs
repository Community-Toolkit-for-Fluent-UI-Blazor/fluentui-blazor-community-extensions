using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an exporter of an html signature.
/// </summary>
/// <typeparam name="TPayload">The type of the payload to export.</typeparam>
/// <param name="module">Module to use to export the surface.</param>
public sealed class HtmlSurfaceExporter<TPayload>(IJSObjectReference module)
    : ISurfaceImageExporter<TPayload>
{
    /// <summary>
    /// Asynchronously encodes the specified render target to an image in the given MIME format.
    /// </summary>
    /// <param name="target">The render target to encode as an image. Must provide a valid native HTML canvas handle.</param>
    /// <param name="mime">The MIME type of the image format to use for encoding, such as "image/png" or "image/jpeg".</param>
    /// <param name="quality">The quality level for image encoding, as a value between 0 and 1. Applies only to formats that support quality
    /// settings, such as JPEG. If null, the default quality is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the encoded image
    /// data.</returns>
    /// <exception cref="NotSupportedException">Thrown if the render target does not provide a native HTML canvas handle, or if the specified MIME type is not
    /// supported by the browser.</exception>
    private async ValueTask<byte[]> EncodeToImageAsync(ISurfaceRenderTarget target, string mime, double? quality = null)
    {
        var canvasId = target.GetNativeHandle() ?? throw new NotSupportedException("This target is not an Html canvas.");

        try
        {
            return await module.InvokeAsync<byte[]>(
                "",
                canvasId,
                mime,
                quality);
        }
        catch
        {
            throw new NotSupportedException($"{mime} is not supported by this browser");
        }
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToAvifAsync(ISurfaceRenderTarget target, int quality)
    {
        return EncodeToImageAsync(target, "image/avif", quality / 100.0);
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToBinAsync(SurfacePayload<TPayload> payload, ExportOptions options)
    {
        return BinaryUtils.WriteAsync(payload, options);
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToBmpAsync(ISurfaceRenderTarget target)
    {
        return EncodeToImageAsync(target, "image/bmp");
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToHeifAsync(ISurfaceRenderTarget target, int quality)
    {
        return EncodeToImageAsync(target, "image/heif", quality / 100.0);
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToJpegAsync(ISurfaceRenderTarget target, int quality)
    {
        return EncodeToImageAsync(target, "image/jpg", quality / 100.0);
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToPngAsync(ISurfaceRenderTarget target)
    {
        return EncodeToImageAsync(target, "image/png");
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToTiffAsync(ISurfaceRenderTarget target)
    {
        return EncodeToImageAsync(target, "image/tiff");
    }

    /// <inheritdoc />
    public ValueTask<byte[]> ToWebpAsync(ISurfaceRenderTarget target, int quality)
    {
        return EncodeToImageAsync(target, "image/webp", quality / 100.0);
    }
}
