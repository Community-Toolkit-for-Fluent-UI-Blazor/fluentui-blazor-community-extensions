namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a view of a chat room, including the room itself and the user's state within that room.
/// </summary>
public sealed class ChatRoomView
{
    /// <summary>
    /// Gets the chat room associated with this view.
    /// </summary>
    public IChatRoom Room { get; init; } = default!;

    /// <summary>
    /// Gets the state of the user within the chat room.
    /// </summary>
    public ChatRoomUserState State { get; init; } = default!;
}
