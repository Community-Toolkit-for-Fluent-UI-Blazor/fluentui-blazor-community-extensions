namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a chat room.
/// </summary>
public sealed record ChatRoom
{
    /// <summary>
    /// Gets the creation date of the chat room.
    /// </summary>
    public DateTimeOffset CreatedDate { get; init; }

    /// <summary>
    /// Gets the unique identifier of the chat room.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets or sets the unique identifier of the owner of the chat room.
    /// </summary>
    public long OwnerId { get; init; }

    /// <summary>
    /// Gets a value indicating whether the chat room is empty.
    /// </summary>
    public bool IsEmpty { get; init; }

    /// <summary>
    /// Gets the name of the chat room.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the owner of the chat room.
    /// </summary>
    public ChatUser? Owner { get; init; }

    /// <summary>
    /// Gets a value indicating whether the chat room is locked.
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>
    /// Gets a value indicating whether the chat room is deleted.
    /// </summary>
    public bool IsDeleted { get; init; }

    /// <summary>
    /// Gets the deletion date of the chat room if it is deleted, <see langword="null" /> otherwise.
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }
}
