namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the capabilities of a chat view, such as support for audio, gifts, emojis, and media. This enum can be used to specify which features are enabled in a chat interface.
/// </summary>
[Flags]
public enum ChatViewCapabilities
{
    /// <summary>
    /// NNo feature is enabled.
    /// </summary>
    None = 0,

    /// <summary>
    /// The recording audio feature is enabled, allowing users to record audio messages in the chat.
    /// </summary>
    Audio = 1,

    /// <summary>
    /// The gifting feature is enabled, allowing users to send gifts in the chat.
    /// </summary>
    Gift = 2,

    /// <summary>
    /// The emoji feature is enabled, allowing users to send emojis in the chat.
    /// </summary>
    Emoji = 4,

    /// <summary>
    /// The media feature is enabled, allowing users to share media files in the chat.
    /// </summary>
    Media = 8,

    /// <summary>
    /// The react feature is enabled, allowing users to react to messages in the chat.
    /// </summary>
    React = 16,

    /// <summary>
    /// All features are enabled.
    /// </summary>
    All = Audio | Gift | Emoji | Media | React,
}
