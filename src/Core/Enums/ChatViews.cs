namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the different views available in a chat interface.
/// </summary>
[Flags]
public enum ChatViews
{
    /// <summary>
    /// No value specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Displays the messages view, showing the conversation history.
    /// </summary>
    Messages = 1,

    /// <summary>
    /// Displays the pinned messages view, showing important or highlighted messages.
    /// </summary>
    PinnedMessages = 2,

    /// <summary>
    /// Displays the images view, showing shared images in the chat.
    /// </summary>
    Images = 4,

    /// <summary>
    /// Displays the videos view, showing shared videos in the chat.
    /// </summary>
    Video = 8,

    /// <summary>
    /// Displays the audio view, showing shared audio files in the chat.
    /// </summary>
    Audio = 16,

    /// <summary>
    /// Displays the files view, showing shared files in the chat.
    /// </summary>
    Files = 32,

    /// <summary>
    /// Displays a custom view.
    /// </summary>
    Custom = 64
}
