namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a chat room.
/// </summary>
public sealed class ChatRoom : IChatRoomCapabilities
{
    /// <summary>
    /// Gets the creation date of the chat room.
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Gets the unique identifier of the chat room.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the owner of the chat room.
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is empty.
    /// </summary>
    public bool IsEmpty { get; set; }

    /// <summary>
    /// Gets the name of the chat room.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets the owner of the chat room.
    /// </summary>
    public ChatUser? Owner { get; set; }

    /// <inheritdoc />
    public bool IsBlocked { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public bool IsHidden { get; set; }

    /// <inheritdoc />
    public bool IsMuted { get; set; }

    /// <inheritdoc />
    public bool IsPinned { get; set; }

    /// <inheritdoc />
    public bool IsArchived { get; set; }
}
