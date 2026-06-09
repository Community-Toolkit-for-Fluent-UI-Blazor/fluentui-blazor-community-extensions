using FluentUI.Blazor.Community.Components.Chat.Files;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents an item used for creating a chat message, containing the message and its associated files.
/// </summary>
/// <param name="Message">The chat message.</param>
/// <param name="Files">The associated files for the chat message.</param>
public sealed record ChatMessageCreationItem(ChatMessage Message, IReadOnlyList<IBinaryChatFile> Files)
{
}
