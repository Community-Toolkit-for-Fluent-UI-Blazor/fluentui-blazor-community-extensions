using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a delegate for providing the last message in a chat room.
/// </summary>
/// <param name="roomId">The ID of the chat room.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the last message in the chat room.</returns>
public delegate ValueTask<ChatMessage?> ChatLastMessageProvider(
    long roomId,
    CancellationToken cancellationToken);

