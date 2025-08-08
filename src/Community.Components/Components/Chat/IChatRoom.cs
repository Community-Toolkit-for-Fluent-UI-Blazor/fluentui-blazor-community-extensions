namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for a chat room.
/// </summary>
public interface IChatRoom
{
    /// <summary>
    /// Gets the created date of the chat room.
    /// </summary>
    DateTime CreatedDate { get; }

    /// <summary>
    /// Gets the identifier of the room.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets a value indicating if the room is blocked.
    /// </summary>
    bool IsBlocked { get; }

    /// <summary>
    /// Gets a value indicating if the room is deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets a value indicating if the room is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Gets a value indicating if the room is hidden.
    /// </summary>
    bool IsHidden { get; }

    /// <summary>
    /// Gets the last message of the room.
    /// </summary>
    IChatMessage? LastMessage { get; }

    /// <summary>
    /// Gets the name of the room.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the owner of the room.
    /// </summary>
    ChatUser? Owner { get; }

    /// <summary>
    /// Gets the number of unread messages for all users.
    /// </summary>
    Dictionary<long, uint> UnreadMessagesForUserId { get; }

    /// <summary>
    /// Gets the users in the room.
    /// </summary>
    List<ChatUser> Users { get; }

    /// <summary>
    /// Gets all users in the room but <paramref name="user"/>.
    /// </summary>
    /// <param name="user">User to skip.</param>
    /// <returns>Returns the list of users but <paramref name="user"/>.</returns>
    IEnumerable<ChatUser> GetUsersBut(ChatUser? user);
}
