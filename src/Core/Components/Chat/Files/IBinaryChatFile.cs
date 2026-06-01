namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents the interface of a binary chat file.
/// </summary>
public interface IBinaryChatFile : IChatFile
{
    /// <summary>
    /// Gets the content of the file.
    /// </summary>
    byte[] Content { get; }

    /// <summary>
    /// Gets or sets the length of the file.
    /// </summary>
    public long Length { get; }
}
