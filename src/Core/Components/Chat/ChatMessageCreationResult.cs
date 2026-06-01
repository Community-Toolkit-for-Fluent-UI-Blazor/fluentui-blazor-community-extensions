using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents the result of creating chat message items, including the created chat messages and associated files.
/// </summary>
public sealed record ChatMessageCreationResult
{
    /// <summary>
    /// Gets or sets a read-only list of <see cref="ChatMessage"/> instances representing the created chat messages.
    /// </summary>
    public required IReadOnlyList<ChatMessage> Messages { get; init; }

    /// <summary>
    /// Gets or sets a read-only list of <see cref="IChatFile"/> instances representing the files associated with the created chat messages.
    /// </summary>
    public required IReadOnlyList<IChatFile> Files { get; init; }
}
