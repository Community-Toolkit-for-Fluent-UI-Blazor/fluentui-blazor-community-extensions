using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an implementation of <see cref="IImageSourceStrategy"/> for handling image URLs hosted on Google Cloud
/// Storage.
/// </summary>
/// <remarks>This strategy is specifically designed to handle URLs that point to objects in Google Cloud Storage,
/// identified by the presence of "storage.googleapis.com" in the URL. It uses the Google Cloud Storage API to fetch the
/// image bytes.</remarks>
public class GoogleCloudImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 5;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains("storage.googleapis.com");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var uri = new Uri(url);
            var bucket = uri.Host;
            var objectName = uri.AbsolutePath.TrimStart('/');
            var storageClient = await StorageClient.CreateAsync();
            using var ms = new MemoryStream();
            await storageClient.DownloadObjectAsync(bucket, objectName, ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Google Cloud fetch failed for {url}");
            return [];
        }
    }
}
