namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving the user state of specific chat messages in a chat room. This record encapsulates the necessary information to identify the chat room and the specific messages for which the user state is being requested, along with a cancellation token to support cancellation of the operation if needed.
/// </summary>
/// <param name="MessageIds">The IDs of the chat messages for which the user state is being requested.</param>
/// <param name="Token">A cancellation token to observe while waiting for the task to complete.</param>
public sealed record ChatMessageUserStateRequest(
    IReadOnlyList<long> MessageIds,
    CancellationToken Token);
