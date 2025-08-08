namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a binary chat file.
/// </summary>
public sealed class BinaryChatFile
    : ChatFile, IBinaryChatFile, IChatFile
{
    /// <summary>
    /// Gets or sets the data of the file.
    /// </summary>
    public byte[] Data { get; set; } = [];
}

