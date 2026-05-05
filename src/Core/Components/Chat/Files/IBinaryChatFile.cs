namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents the interface of a binary chat file.
/// </summary>
public interface IBinaryChatFile
    : IChatFile
{
    /// <summary>
    /// Gets the data of the file.
    /// </summary>
    byte[] Data { get; }
}
