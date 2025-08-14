namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content of the <see cref="ChatMessageViewer"/> dialog.
/// </summary>
/// <param name="Owner">Owner of the view.</param>
/// <param name="Message">Message to view. </param>
/// <param name="LoadingLabel">Loading label</param>
public record ChatMessageViewerContent(ChatUser Owner, IChatMessage Message, string? LoadingLabel)
{
}
