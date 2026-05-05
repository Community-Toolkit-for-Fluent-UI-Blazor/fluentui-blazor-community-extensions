namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents an url chat file.
/// </summary>
public interface IUrlChatFile
    : IChatFile
{
    /// <summary>
    /// Gets the url of the file.
    /// </summary>
    string? Url { get; }
}
