namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents an asynchronous operation that processes a chat group creation request.
/// </summary>
/// <param name="request">The chat group creation request to process.</param>
/// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
/// <returns>A task representing the asynchronous operation, containing the processed chat group creation result.</returns>
public delegate ValueTask<ChatGroupCreateResult> ChatRoomCreateDelegate(
    ChatGroupCreateRequest request,
    CancellationToken cancellationToken = default);
