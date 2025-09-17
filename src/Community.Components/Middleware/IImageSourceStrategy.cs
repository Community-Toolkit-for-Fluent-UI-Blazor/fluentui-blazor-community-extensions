using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Represents a strategy for handling image sources.
/// </summary>
public interface IImageSourceStrategy
{
    /// <summary>
    /// Gets the priority of the strategy. Lower values indicate higher priority.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Checks if the strategy can handle the given URL.
    /// </summary>
    /// <param name="url">Url to check.</param>
    /// <returns>Returns true if the strategy can handle the given URL; otherwise, false.</returns>
    bool CanHandle(string url);

    /// <summary>
    /// Gets the image bytes from the given URL.
    /// </summary>
    /// <param name="url">Url to fetch.</param>
    /// <param name="logger">Logger to use.</param>
    /// <returns>Returns the image bytes.</returns>
    Task<byte[]> GetImageBytesAsync(string url, ILogger logger);
}
