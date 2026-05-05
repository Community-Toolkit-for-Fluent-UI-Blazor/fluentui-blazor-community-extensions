using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a chat room.
/// </summary>
public interface IChatRoom
{
    /// <summary>
    /// Gets the creation date of the chat room.
    /// </summary>
    DateTime CreatedDate { get; }

    /// <summary>
    /// Gets the unique identifier of the chat room.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets a value indicating whether the chat room is blocked.
    /// </summary>
    bool IsBlocked { get; }

    /// <summary>
    /// Gets a value indicating whether the chat room is deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets a value indicating whether the chat room is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Gets a value indicating whether the chat room is hidden.
    /// </summary>
    bool IsHidden { get; }

    /// <summary>
    /// Gets the last message in the chat room.
    /// </summary>
    IChatMessage? LastMessage { get; }

    /// <summary>
    /// Gets the name of the chat room.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the owner of the chat room.
    /// </summary>
    ChatUser? Owner { get; }

    /// <summary>
    /// Gets the number of unread messages for each user in the chat room, keyed by user ID.
    /// </summary>
    IReadOnlyDictionary<long, uint> UnreadMessagesForUserId { get; }

    /// <summary>
    /// Gets the list of users in the chat room.
    /// </summary>
    IReadOnlyList<ChatUser> Users { get; }
}
