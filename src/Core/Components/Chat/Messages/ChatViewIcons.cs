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
    public Icon? Media { get; set; } = new Size20.Image();

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    public Icon? Emoji { get; set; } = new Size20.Emoji();

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    public Icon? Gift { get; set; } = new Size20.Gift();

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    public Icon? Send { get; set; } = new Size20.Send();

    /// <summary>
    /// Gets or sets the icon to be used for the cancel edit button in the chat message writer.
    /// </summary>
    public Icon? Dismiss { get; set; } = new Size20.Dismiss();

    /// <summary>
    /// Gets or sets the icon to be used for the commit edit button in the chat message writer.
    /// </summary>
    public Icon? Checkmark { get; set; } = new Size20.Checkmark();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when not recording in chat message writer.
    /// </summary>
    public Icon? Micro { get; set; } = new Size20.Mic();

    /// <summary>
    /// Gets or sets the icon to be used for the audio recorder button when recording in chat message writer.
    /// </summary>
    public Icon? MicroOff { get; set; } = new Size20.MicOff();
}
