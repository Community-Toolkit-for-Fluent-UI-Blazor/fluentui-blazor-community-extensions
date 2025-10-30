using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Represents a strategy for handling Base64-encoded image URLs.
/// </summary>
public sealed class Base64ImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 1;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.StartsWith("data:image");

    /// <inheritdoc />
    public Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var base64Data = url[(url.IndexOf(',') + 1)..];
            var bytes = Convert.FromBase64String(base64Data);
            return Task.FromResult(bytes);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Base64 decoding failed for {url[..30]}...");

            return Task.FromResult<byte[]>([]);
        }
    }
}
