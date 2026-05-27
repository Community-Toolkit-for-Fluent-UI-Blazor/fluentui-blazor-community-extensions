using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents the icons used in the chat message writer and other chat-related UI elements.
/// </summary>
public sealed class ChatViewIcons
{
    /// <summary>
    /// Gets or sets the icon to be used for the media button in the chat message writer.
    /// </summary>
    public Icon? MediaIcon { get; set; } = new Size20.Image();

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    public Icon? EmojiIcon { get; set; } = new Size20.Emoji();

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    public Icon? GiftIcon { get; set; } = new Size20.Gift();

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    public Icon? SendIcon { get; set; } = new Size20.Send();

    /// <summary>
    /// Gets or sets the icon to be used for the cancel edit button in the chat message writer.
    /// </summary>
    public Icon? DismissIcon { get; set; } = new Size20.Dismiss();

    /// <summary>
    /// Gets or sets the icon to be used for the commit edit button in the chat message writer.
    /// </summary>
    public Icon? CheckmarkIcon { get; set; } = new Size20.Checkmark();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when not recording in chat message writer.
    /// </summary>
    public Icon? MicroIcon { get; set; } = new Size20.Mic();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when recording in chat message writer.
    /// </summary>
    public Icon? MicroOffIcon { get; set; } = new Size20.MicOff();
}
