using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to edit a chat message.
/// </summary>
/// <param name="RoomId">The identifier of the chat room containing the message.</param>
/// <param name="Message">The chat message to be edited.</param>
/// <param name="OwnerId">The identifier of the message owner.</param>
/// <param name="Text">The new text content for the message.</param>
/// <param name="token">The token to monitor for cancellation requests.</param>
public sealed record ChatMessageEditRequest(
    long RoomId,
    ChatMessage Message,
    long OwnerId,
    string Text,
    CancellationToken token);
