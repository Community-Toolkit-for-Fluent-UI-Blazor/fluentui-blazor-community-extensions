using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an image source strategy for retrieving image data from Azure Blob Storage.
/// </summary>
/// <remarks>This strategy is designed to handle URLs pointing to Azure Blob Storage resources.  It determines
/// whether a given URL can be processed and retrieves the image data as a byte array.</remarks>
public class AzureBlobImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 4;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains(".blob.core.windows.net");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var blobClient = new BlobClient(new Uri(url));
            using var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Azure Blob fetch failed for {url}");
            return [];
        }
    }
}
