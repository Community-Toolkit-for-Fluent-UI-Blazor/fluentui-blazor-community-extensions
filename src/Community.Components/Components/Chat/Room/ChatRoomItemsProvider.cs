namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the delegate to use to get the items.
/// </summary>
/// <param name="request">Request of the items to get.</param>
/// <param name="cancellationToken">Token to cancel the operation.</param>
/// <returns>Returns a <see cref="ValueTask"/> which contains the room when completed.</returns>
public delegate ValueTask<IEnumerable<ChatRoom>> ChatRoomItemsProvider(ChatRoomItemsRequest request, CancellationToken cancellationToken = default);
