using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to add a reaction to a chat message.
/// </summary>
/// <param name="RoomId">The identifier of the chat room containing the message.</param>
/// <param name="OwnerId">The identifier of the user adding the reaction.</param>
/// <param name="message">The chat message to react to.</param>
/// <param name="Reaction">The reaction to add to the message.</param>
public sealed record ChatMessageReactRequest(
    long RoomId,
    long OwnerId,
    ChatMessage message,
    string Reaction);
