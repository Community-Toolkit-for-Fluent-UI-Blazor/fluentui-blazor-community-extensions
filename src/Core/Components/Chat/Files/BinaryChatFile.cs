namespace FluentUI.Blazor.Community.Components.Chat.Files;

/// <summary>
/// Represents a binary chat file.
/// </summary>
public sealed record BinaryChatFile
    : ChatFile, IBinaryChatFile
{
    /// <inheritdoc />
    public byte[] Content { get; init; } = [];

    /// <inheritdoc />
    public long Length { get; init; }
}
