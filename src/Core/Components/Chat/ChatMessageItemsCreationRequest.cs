using FluentUI.Blazor.Community.Components.Chat.Messages;
namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to create chat message items in a room.
/// </summary>
/// <param name="RoomId">The ID of the chat room where the message items will be created.</param>
/// <param name="Items">A read-only list of <see cref="ChatMessageCreationItem"/> instances representing the chat message items to be created.</param>
/// <param name="Token">The cancellation token to observe while waiting for the operation to complete.</param>
public sealed record ChatMessageItemsCreationRequest(
    long RoomId,
    IReadOnlyList<ChatMessageCreationItem> Items,
    CancellationToken Token);
