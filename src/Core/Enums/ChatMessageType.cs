namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the type of the message.
/// </summary>
[Flags]
public enum ChatMessageType
{
    /// <summary>
    /// The message is undefined.
    /// </summary>
    /// <remarks>This state is not used.</remarks>
    None = 0,

    /// <summary>
    /// The message contains some text.
    /// </summary>
    Text = 1,

    /// <summary>
    /// The message contains audio files.
    /// </summary>
    Audio = 2,

    /// <summary>
    /// The message contains image files.
    /// </summary>
    Images = 4,

    /// <summary>
    /// The message contains video files.
    /// </summary>
    Videos = 8,

    /// <summary>
    /// The message contains other files (pdf, doc, docx, pptx, etc).
    /// </summary>
    Files = 16,

    /// <summary>
    /// The message contains a gift.
    /// </summary>
    Gift = 32,

    /// <summary>
    /// The message contains a custom content.
    /// </summary>
    /// <remarks>Custom is defined by the user.</remarks>
    Custom = 64,
}
