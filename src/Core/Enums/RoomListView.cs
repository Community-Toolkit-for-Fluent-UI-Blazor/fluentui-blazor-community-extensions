namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the different views available for the chat room list.
/// </summary>
public enum RoomListView
{
    /// <summary>
    /// Represents the normal view of the chat room list, showing all rooms that are not blocked, hidden, or archived.
    /// </summary>
    Normal,

    /// <summary>
    /// Represents the blocked view of the chat room list, showing only the rooms that are blocked by the user.
    /// </summary>
    Blocked,

    /// <summary>
    /// Represents the hidden view of the chat room list, showing only the rooms that are hidden by the user.
    /// </summary>
    Hidden,

    /// <summary>
    /// Represents the archived view of the chat room list, showing only the rooms that are archived by the user.
    /// </summary>
    Archived
}
