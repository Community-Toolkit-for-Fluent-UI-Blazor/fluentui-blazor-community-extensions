namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a delegate for providing the number of unread messages in a chat room.
/// </summary>
/// <param name="roomId">The ID of the chat room.</param>
/// <param name="ownerId">The ID of the owner of the chat room.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>The number of unread messages in the specified chat room.</returns>
public delegate ValueTask<int> ChatUnreadMessagesProvider(
    long roomId,
    long ownerId,
    CancellationToken cancellationToken);
