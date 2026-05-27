namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving specific chat message items based on the provided room ID and message ID collection. This record encapsulates the necessary information to identify and retrieve specific chat messages from a chat room, along with a cancellation token to support cancellation of the operation if needed.
/// </summary>
/// <param name="RoomId">The ID of the chat room.</param>
/// <param name="MessageIdCollection">The collection of chat message IDs.</param>
/// <param name="Token">The cancellation token.</param>
public sealed record ChatMessageItemsRequest(
    long RoomId,
    IReadOnlyList<long> MessageIdCollection,
    CancellationToken Token);
