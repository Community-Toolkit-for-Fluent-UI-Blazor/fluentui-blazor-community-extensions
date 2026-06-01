namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents a chat file that is accessible via a URL.
/// </summary>
public sealed record UrlChatFile
    : ChatFile, IUrlChatFile
{
    /// <inheritdoc />
    public string? Url { get; init; }
}
