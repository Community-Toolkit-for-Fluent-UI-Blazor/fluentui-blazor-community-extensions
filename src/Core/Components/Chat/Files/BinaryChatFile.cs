namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents a binary chat file.
/// </summary>
public sealed class BinaryChatFile
    : ChatFile, IBinaryChatFile, IChatFile
{
    /// <inheritdoc />
    public byte[] Data { get; set; } = [];
}
