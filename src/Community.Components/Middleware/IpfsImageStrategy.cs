using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Represents a strategy for handling IPFS image URLs.
/// </summary>
public class IpfsImageStrategy : IImageSourceStrategy
{
    /// <summary>
    /// Represents the HTTP client used to fetch images.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="IpfsImageStrategy"/> class.
    /// </summary>
    /// <param name="httpClient">Http client to use to fetch image.</param>
    public IpfsImageStrategy(HttpClient httpClient) => _httpClient = httpClient;

    /// <inheritdoc />
    public int Priority => 8;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.StartsWith("ipfs://");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var cid = url.Replace("ipfs://", "");
            var gatewayUrl = $"https://ipfs.io/ipfs/{cid}";
            return await _httpClient.GetByteArrayAsync(gatewayUrl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"IPFS fetch failed for {url}");
            return [];
        }
    }
}
