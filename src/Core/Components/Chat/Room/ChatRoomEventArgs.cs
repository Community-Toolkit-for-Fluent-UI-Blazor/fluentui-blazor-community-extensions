namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Event arguments for chat room events, encapsulating the room ID, user ID, and a boolean value indicating the event's state or outcome.
/// </summary>
/// <param name="User">The user associated with the chat room event.</param>
public sealed record ChatRoomEventArgs(ChatRoomUser User) { }
