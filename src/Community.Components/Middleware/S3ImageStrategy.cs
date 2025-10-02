using Amazon.S3;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an implementation of <see cref="IImageSourceStrategy"/> for handling image retrieval from Amazon S3.
/// </summary>
/// <remarks>This strategy is designed to handle URLs that point to Amazon S3 resources, specifically those
/// containing ".s3.amazonaws.com" in the URL. It uses an <see cref="IAmazonS3"/> client to fetch the image
/// data.</remarks>
public class S3ImageStrategy : IImageSourceStrategy
{
    /// <summary>
    /// Represents the S3 client used to fetch images.
    /// </summary>
    private readonly IAmazonS3 _s3Client;

    /// <inheritdoc />
    public int Priority => 6;

    /// <inheritdoc />
    public S3ImageStrategy(IAmazonS3 s3Client) => _s3Client = s3Client;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains(".s3.amazonaws.com");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var uri = new Uri(url);
            var bucket = uri.Host.Split('.')[0];
            var key = uri.AbsolutePath.TrimStart('/');
            var response = await _s3Client.GetObjectAsync(bucket, key);
            using var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"S3 fetch failed for {url}");
            return [];
        }
    }
}
