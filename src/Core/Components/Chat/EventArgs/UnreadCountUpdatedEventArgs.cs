namespace FluentUI.Blazor.Community.Components.Chat.EventArgs;

/// <summary>
/// Represents the event arguments for when the unread message count in a chat room is updated.
/// </summary>
/// <param name="RoomId">The ID of the chat room.</param>
/// <param name="Count">The updated unread message count.</param>
internal sealed record UnreadCountUpdatedEventArgs(long RoomId, int Count);

