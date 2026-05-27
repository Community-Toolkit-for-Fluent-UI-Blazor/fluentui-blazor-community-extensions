namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a static class that defines constant string values for various chat message types used in the chat application. These message types are used to identify the type of action or event that occurred in the chat, such as room creation, message editing, or message deletion. Each constant string value corresponds to a specific chat event,
///  allowing the application to handle and process these events accordingly.
/// </summary>
internal static class ChatMessageTypes
{
    public const string RoomCreated = "room.created";
    public const string RoomUpdated = "room.updated";

    public const string MessageNew = "message.new";
    public const string MessageEdit = "message.edit";
    public const string MessageDelete = "message.delete";
    public const string MessageRead = "message.read";
    public const string MessageReact = "message.react";
    public const string MessagesNew = "messages.new";
}
