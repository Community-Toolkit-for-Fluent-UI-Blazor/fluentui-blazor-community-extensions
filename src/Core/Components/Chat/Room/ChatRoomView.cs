using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the view of a chat room, containing both global and user-specific data, as well as the last message and unread count for the current user.
/// </summary>
public sealed record ChatRoomView
{
    /// <summary>
    /// Gets the chat room information (id, name, description, etc.).
    /// </summary>
    public ChatRoom Room { get; init; } = default!;

    /// <summary>
    /// Gets the local data for the current user (hidden, pinned, muted, etc.).
    /// </summary>
    public ChatRoomUser? UserState { get; init; }

    /// <summary>
    /// Gets the last message in the room (or null if empty).
    /// </summary>
    public ChatMessage? LastMessage { get; init; }

    /// <summary>
    /// Gets the number of unread messages for the current user.
    /// </summary>
    public int UnreadCount { get; init; }

    /// <summary>
    /// Gets the list of users in the room.
    /// </summary>
    public IReadOnlyList<ChatUser> Users { get; init; } = [];
}

