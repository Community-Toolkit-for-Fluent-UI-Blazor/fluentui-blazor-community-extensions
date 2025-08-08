namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chat file that is accessible via a URL.
/// </summary>
public sealed class UrlChatFile
    : ChatFile, IUrlChatFile
{
    /// <summary>
    /// Gets or sets the URL of the file.
    /// </summary>
    public string? Url { get; set; }
}
