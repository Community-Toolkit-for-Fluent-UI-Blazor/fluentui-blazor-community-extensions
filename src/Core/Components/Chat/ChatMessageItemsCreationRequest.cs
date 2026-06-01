using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to create chat message items in a room.
/// </summary>
/// <param name="Messages">A read-only list of <see cref="ChatMessage"/> instances representing the chat messages to be created.</param>
/// <param name="Files">A read-only list of <see cref="IChatFile"/> instances representing the files associated with the chat messages.</param>
/// <param name="Token">The cancellation token to observe while waiting for the operation to complete.</param>
public sealed record ChatMessageItemsCreationRequest(
    IReadOnlyList<ChatMessage> Messages,
    IReadOnlyList<IChatFile> Files,
    CancellationToken Token);
