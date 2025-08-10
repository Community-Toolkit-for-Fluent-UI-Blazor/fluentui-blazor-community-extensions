namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content of the <see cref="ChatMessageViewer"/> dialog.
/// </summary>
/// <param name="owner">Owner of the view.</param>
/// <param name="message">Message to view. </param>
public record ChatMessageViewerContent(ChatUser owner, IChatMessage message)
{
}
