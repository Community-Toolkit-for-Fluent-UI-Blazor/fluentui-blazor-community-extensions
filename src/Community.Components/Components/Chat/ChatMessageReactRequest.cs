namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the request when a reaction is set on a message.
/// </summary>
/// <param name="RoomId">Identifier of the room.</param>
/// <param name="Owner">Owner of the chat.</param>
/// <param name="Message">The reacted message.</param>
/// <param name="Reaction">The emoji sets on the message.</param>
public record ChatMessageReactRequest(long RoomId, ChatUser Owner, IChatMessage Message, string Reaction)
{ }
