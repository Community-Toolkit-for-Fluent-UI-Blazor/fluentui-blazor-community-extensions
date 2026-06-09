namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the user chat room which contains the information of the user in the chat room.
/// </summary>
public sealed record ChatRoomUser
{
    /// <summary>
    /// Gets or sets the unique identifier of the user chat room.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets or sets the unique identifier of the chat room.
    /// </summary>
    public long RoomId { get; init; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public long UserId { get; init; }

    /// <summary>
    /// Gets or sets the user of the chat room.
    /// </summary>
    public ChatUser User { get; init; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the chat room is hidden.
    /// </summary>
    public bool IsHidden { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room is muted.
    /// </summary>
    public bool IsMuted { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room is pinned.
    /// </summary>
    public bool IsPinned { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room is archived.
    /// </summary>
    public bool IsArchived { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat room is blocked.
    /// </summary>
    public bool IsBlocked { get; init; }
}
