using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Represents a strategy for handling HTTP image URLs.
/// </summary>
public class HttpImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 10;

    /// <summary>
    /// Represents the HTTP client used to fetch images.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpImageStrategy"/> class.
    /// </summary>
    /// <param name="httpClient">Http client</param>
    public HttpImageStrategy(HttpClient httpClient) => _httpClient = httpClient;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.StartsWith("http");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            return await _httpClient.GetByteArrayAsync(url);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"HTTP image fetch failed for {url}");
            return [];
        }
    }
}
