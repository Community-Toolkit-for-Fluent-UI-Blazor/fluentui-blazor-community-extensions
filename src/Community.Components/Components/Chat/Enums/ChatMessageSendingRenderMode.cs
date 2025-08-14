namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the render mode for sending chat messages.
/// </summary>
public enum ChatMessageSendingRenderMode
{
    /// <summary>
    /// The sending message is rendered in an overlay.
    /// </summary>
    Overlay,

    /// <summary>
    /// The sending message is rendered inline in the chat message writer.
    /// </summary>
    Inline
}
