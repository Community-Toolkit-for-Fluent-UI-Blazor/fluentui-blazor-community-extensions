namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the capabilities of a chat room, such as whether it is blocked, deleted, or hidden.
/// </summary>
public interface IChatRoomCapabilities
{
    /// <summary>
    /// Gets a value indicating whether the chat room is blocked.
    /// </summary>
    bool IsBlocked { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is hidden.
    /// </summary>
    bool IsHidden { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is muted.
    /// </summary>
    bool IsMuted { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is pinned.
    /// </summary>
    bool IsPinned { get; set; }

    /// <summary>
    /// Gets a value indicating whether the chat room is archived.
    /// </summary>
    bool IsArchived { get; set; }
}
