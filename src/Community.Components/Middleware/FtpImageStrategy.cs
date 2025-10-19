using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Represents a strategy for handling FTP image URLs.
/// </summary>
public class FtpImageStrategy : IImageSourceStrategy
{
    /// <summary>
    /// Represents the HTTP client used to fetch images.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Represents a strategy for handling FTP image URLs.
    /// </summary>
    /// <param name="httpClient">Http client to handle requests.</param>
    public FtpImageStrategy(HttpClient httpClient) => _httpClient = httpClient;

    /// <inheritdoc />
    public int Priority => 7;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.StartsWith("ftp://");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            return await _httpClient.GetByteArrayAsync(url);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"FTP fetch failed for {url}");
            return [];
        }
    }
}
