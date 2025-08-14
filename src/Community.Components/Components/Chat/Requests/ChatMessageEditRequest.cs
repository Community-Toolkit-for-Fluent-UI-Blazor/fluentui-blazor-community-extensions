namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the request to edit a message.
/// </summary>
/// <param name="RoomId">Id of the room.</param>
/// <param name="Message">Message to edit.</param>
/// <param name="Owner">Owner of the message.</param>
/// <param name="Text">Text of the message.</param>
public record ChatMessageEditRequest(
    long RoomId,
    IChatMessage Message,
    ChatUser Owner,
    string? Text)
{
}
