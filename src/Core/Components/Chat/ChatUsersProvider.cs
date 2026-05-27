namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Delegate for providing a list of chat users in a chat room.
/// </summary>
/// <param name="roomId">The ID of the chat room.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of chat users.</returns>
public delegate ValueTask<IReadOnlyList<ChatUser>> ChatRoomUsersProvider(long roomId, CancellationToken cancellationToken);
