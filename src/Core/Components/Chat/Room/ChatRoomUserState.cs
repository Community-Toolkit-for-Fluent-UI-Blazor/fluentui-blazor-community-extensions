namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Gets the state of a user in a chat room.
/// </summary>
public sealed class ChatRoomUserState
{
    /// <summary>
    /// Gets the identifier of the user.
    /// </summary>
    public long UserId { get; init; }

    /// <summary>
    /// Gets if the room is hidden for the user in the chat room.
    /// </summary>
    public bool IsHidden { get; init; }

    /// <summary>
    /// Gets if the user is blocked in the chat room.
    /// </summary>
    public bool IsBlocked { get; init; }

    /// <summary>
    /// Gets the number of unread messages for the user in the chat room.
    /// </summary>
    public uint UnreadCount { get; init; }
}
