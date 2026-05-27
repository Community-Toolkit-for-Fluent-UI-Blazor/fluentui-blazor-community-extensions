namespace FluentUI.Blazor.Community.Components.Chat.EventArgs;

/// <summary>
/// Represents the event arguments for when the users in a chat room are updated.
/// </summary>
/// <param name="RoomId">The ID of the chat room.</param>
/// <param name="Users">The updated list of users in the chat room.</param>
internal sealed record UsersUpdatedEventArgs(long RoomId, IReadOnlyList<ChatUser> Users);
