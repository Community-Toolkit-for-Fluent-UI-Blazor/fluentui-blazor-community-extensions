using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Responsible for processing image requests, including fetching, resizing, format conversion, and caching.
/// </summary>
public class ImageProcessingMiddleware
{
    /// <summary>
    /// Represents the next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Represents the logger instance.
    /// </summary>
    private readonly ILogger<ImageProcessingMiddleware> _logger;

    /// <summary>
    /// Represents the collection of image source strategies.
    /// </summary>
    private readonly IEnumerable<IImageSourceStrategy> _strategies;

    /// <summary>
    /// Represents the memory cache for storing processed images.
    /// </summary>
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Represents the content type provider for determining MIME types.
    /// </summary>
    private static readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageProcessingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next request to process.</param>
    /// <param name="logger">Logger to use to write information.</param>
    /// <param name="strategies">List of strategies to use.</param>
    /// <param name="cache">Cache.</param>
    public ImageProcessingMiddleware(
        RequestDelegate next,
        ILogger<ImageProcessingMiddleware> logger,
        IEnumerable<IImageSourceStrategy> strategies,
        IMemoryCache cache)
    {
        _next = next;
        _logger = logger;
        _strategies = strategies;
        _cache = cache;
    }

    /// <summary>
    /// Processes HTTP requests to handle image proxying, including resizing, reformatting, and caching.
    /// </summary>
    /// <remarks>This middleware intercepts requests with the path starting with "/image-proxy" and processes
    /// them  to retrieve, resize, and reformat images based on query parameters. The processed image is cached  for
    /// subsequent requests to improve performance. If the path does not match, the request is passed  to the next
    /// middleware in the pipeline.  Query parameters: <list type="bullet"> <item> <term>url</term> <description>The URL
    /// of the image to be processed. This parameter is required.</description> </item> <item> <term>format</term>
    /// <description>The desired image format (e.g., "jpeg", "png"). Defaults to "jpeg" if not specified.</description>
    /// </item> <item> <term>quality</term> <description>The quality of the output image (1-100). Defaults to 80 if not
    /// specified or invalid.</description> </item> <item> <term>width</term> <description>The desired width of the
    /// output image. If 0 or not specified, the original width is used.</description> </item> <item>
    /// <term>height</term> <description>The desired height of the output image. If 0 or not specified, the original
    /// height is used.</description> </item> </list>  Responses: <list type="bullet"> <item> <term>200 OK</term>
    /// <description>The processed image is returned in the response body.</description> </item> <item> <term>400 Bad
    /// Request</term> <description>Returned if the "url" query parameter is missing or invalid.</description> </item>
    /// <item> <term>500 Internal Server Error</term> <description>Returned if an error occurs during image
    /// processing.</description> </item> </list></remarks>
    /// <param name="context">The <see cref="HttpContext"/> representing the current HTTP request and response.</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/image-proxy"))
        {
            await _next(context);
            return;
        }

        var query = context.Request.Query;
        var imageUrl = query["url"].ToString();
        var format = query["format"].ToString().ToLower();
        var quality = int.TryParse(query["quality"], out var q) ? q : 80;
        var width = int.TryParse(query["width"], out var w) ? w : 0;
        var height = int.TryParse(query["height"], out var h) ? h : 0;

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing image URL.");
            return;
        }

        var cacheKey = $"img:{imageUrl}:{width}:{height}:{format}:{quality}";

        if (_cache.TryGetValue(cacheKey, out var cachedImage) &&
            cachedImage is byte[] data)
        {
            if (!_contentTypeProvider.TryGetContentType(format, out var mimeType))
            {
                mimeType = "image/jpeg";
            }

            context.Response.ContentType = mimeType;
            await context.Response.Body.WriteAsync(data);
            return;
        }

        try
        {
            var originalBytes = await DownloadImageAsync(imageUrl);
            using var inputStream = new SKMemoryStream(originalBytes);
            using var original = SKBitmap.Decode(inputStream);

            using var resized = ResizeImage(original, width, height);
            var encoded = EncodeImage(resized, format, quality, out var mimeType);

            context.Response.ContentType = mimeType;
            context.Response.Headers["Cache-Control"] = "public,max-age=31536000";
            context.Response.Headers["ETag"] = $"\"{cacheKey.GetHashCode()}\"";

            _cache.Set(cacheKey, encoded, TimeSpan.FromHours(1));
            await context.Response.Body.WriteAsync(encoded);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Image processing failed.");
            context.Response.StatusCode = 500;

            await context.Response.WriteAsync("Image processing error.");
        }
    }

    /// <summary>
    /// Downloads the image from the specified URL using the appropriate strategy.
    /// </summary>
    /// <param name="url">Url of the image to download.</param>
    /// <returns>Returns a task which downloads the url when completed.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<byte[]> DownloadImageAsync(string url)
    {
        var orderedStrategies = _strategies
            .Where(s => s.CanHandle(url))
            .OrderBy(s => s.Priority)
            .ToList();

        var performanceReport = new Dictionary<string, long>();

        foreach (var strategy in orderedStrategies)
        {
            var strategyName = strategy.GetType().Name;
            _logger.LogInformation($"Trying strategy: {strategyName} for {url}");

            var sw = Stopwatch.StartNew();
            var result = await strategy.GetImageBytesAsync(url, _logger);
            sw.Stop();

            performanceReport[strategyName] = sw.ElapsedMilliseconds;

            if (result != null)
            {
                _logger.LogInformation($"Strategy {strategyName} succeeded in {sw.ElapsedMilliseconds}ms");
                return result;
            }

            _logger.LogWarning($"Strategy {strategyName} failed in {sw.ElapsedMilliseconds}ms");
        }

        _logger.LogError($"All strategies failed for {url}");
        throw new InvalidOperationException("Image fetch failed for all strategies.");
    }

    /// <summary>
    /// Resizes the specified image to the given dimensions while maintaining the aspect ratio if one dimension is set
    /// to zero.
    /// </summary>
    /// <param name="original">The original image to resize. Cannot be <see langword="null"/>.</param>
    /// <param name="width">The desired width of the resized image. If set to 0, the width is calculated to maintain the aspect ratio.</param>
    /// <param name="height">The desired height of the resized image. If set to 0, the height is calculated to maintain the aspect ratio.</param>
    /// <returns>A new <see cref="SKBitmap"/> instance representing the resized image. If both <paramref name="width"/> and
    /// <paramref name="height"/> are 0, the original image is returned.</returns>
    private static SKBitmap ResizeImage(SKBitmap original, int width, int height)
    {
        if (width == 0 && height == 0)
        {
            return original;
        }

        if (width == 0)
        {
            width = original.Width * height / original.Height;
        }

        if (height == 0)
        {
            height = original.Height * width / original.Width;
        }

        using var resized = new SKBitmap(width, height);
        using var canvas = new SKCanvas(resized);
        canvas.DrawBitmap(original, new SKRect(0, 0, width, height));

        return resized;
    }

    /// <summary>
    /// Encodes an image into a specified format with a given quality level.
    /// </summary>
    /// <remarks>This method supports a variety of image formats, including PNG, JPEG, WebP, AVIF, HEIF, GIF,
    /// BMP, WBMP, KTX, and ICO. If the specified format is not recognized, the image is encoded as JPEG.</remarks>
    /// <param name="bitmap">The <see cref="SKBitmap"/> representing the image to encode.</param>
    /// <param name="format">The desired image format as a string (e.g., "png", "jpeg", "webp"). If the format is not recognized,  the
    /// default format is JPEG.</param>
    /// <param name="quality">The quality level of the encoded image, ranging from 0 to 100. Higher values indicate better quality.</param>
    /// <param name="mimeType">When the method returns, contains the MIME type corresponding to the encoded image format.  If the format is not
    /// recognized, the MIME type defaults to "image/jpeg".</param>
    /// <returns>A byte array containing the encoded image data.</returns>
    private static byte[] EncodeImage(SKBitmap bitmap, string format, int quality, out string mimeType)
    {
        var skFormat = format switch
        {
            "png" => SKEncodedImageFormat.Png,
            "webp" => SKEncodedImageFormat.Webp,
            "jpeg" or "jpg" => SKEncodedImageFormat.Jpeg,
            "avif" => SKEncodedImageFormat.Avif,
            "heif" => SKEncodedImageFormat.Heif,
            "gif" => SKEncodedImageFormat.Gif,
            "bmp" => SKEncodedImageFormat.Bmp,
            "wbmp" => SKEncodedImageFormat.Wbmp,
            "ktx" => SKEncodedImageFormat.Ktx,
            "ico" => SKEncodedImageFormat.Ico,
            _ => SKEncodedImageFormat.Jpeg
        };

        var realFormat = $".{format}";

        if (!_contentTypeProvider.TryGetContentType(realFormat, out mimeType))
        {
            mimeType = "image/jpeg";
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(skFormat, quality);

        return data.ToArray();
    }
}
