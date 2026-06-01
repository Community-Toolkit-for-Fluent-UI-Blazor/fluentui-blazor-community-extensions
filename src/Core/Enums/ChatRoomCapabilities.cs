namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the capabilities of a chat room, such as whether it can be blocked, deleted, or hidden.
/// </summary>
[Flags]
public enum ChatRoomCapabilities
{
    /// <summary>
    /// No capabilities are available for the chat room.
    /// </summary>
    None = 0,

    /// <summary>
    /// The chat room can be blocked, preventing the user from receiving messages from it.
    /// </summary>
    Block = 1,

    /// <summary>
    /// The chat room can be unblocked, allowing the user to receive messages from it again.
    /// </summary>
    Unblock = 2,

    /// <summary>
    /// The chat room can be hidden, removing it from the user's chat list without deleting it.
    /// </summary>
    Hide = 4,

    /// <summary>
    /// The chat room can be unhidden, restoring it to the user's chat list if it was previously hidden.
    /// </summary>
    Unhide = 8,

    /// <summary>
    /// The chat room can be archived, moving it to an archived state where it is not actively visible in the chat list but can be accessed later if needed.
    /// </summary>
    Archive = 16,

    /// <summary>
    /// The chat room can be unarchived, restoring it to an active state in the user's chat list if it was previously archived.
    /// </summary>
    Unarchive = 32,

    /// <summary>
    /// The chat room can be deleted, permanently removing it from the user's chat list and preventing any further access to its messages or information.
    /// </summary>
    Delete = 64,

    /// <summary>
    /// The chat room can be renamed, allowing the user to change its name or title for better organization or identification in the chat list.
    /// </summary>
    Rename = 128,

    /// <summary>
    /// The chat room can be pinned, keeping it at the top of the user's chat list for easy access and visibility, even if there are newer messages or other chat rooms with more recent activity.
    /// </summary>
    Pin = 256,

    /// <summary>
    /// The chat room can be muted, preventing the user from receiving notifications for new messages in the chat room while still allowing them to access and read messages at their convenience.
    /// </summary>
    Mute = 512,

    /// <summary>
    /// The chat room can be searched, allowing the user to find specific messages or participants within the chat room.
    /// </summary>
    Search = 1024,

    /// <summary>
    /// The chat room can have a new group chat created, allowing the user to start a new group conversation within the chat room.
    /// </summary>
    NewGroup = 2048,

    /// <summary>
    /// The chat room can be shown as deleted, indicating that it has been marked as deleted but may still be visible in the user's chat list with a deleted status or indicator, allowing the user to review or recover the chat room if needed.
    /// </summary>
    ShowDeleted = 4096,

    /// <summary>
    /// All capabilities are available for the chat room.
    /// </summary>
    All = ShowDeleted | NewGroup | Search | Mute | Pin | Rename | Delete | Unarchive | Archive | Unhide | Hide | Unblock | Block,
}
