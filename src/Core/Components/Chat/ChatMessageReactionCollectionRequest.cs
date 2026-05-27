namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving a collection of chat message reactions associated with specific message identifiers. This record encapsulates the necessary information to fetch the reactions related to chat messages, including the message identifiers and a cancellation token for managing asynchronous operations.
/// </summary>
/// <param name="MessageIdCollection">The collection of identifiers of the chat messages for which to retrieve associated reactions.</param>
/// <param name="CancellationToken">A token for monitoring and canceling the asynchronous operation.</param>
public sealed record ChatMessageReactionCollectionRequest(
    IReadOnlyList<long> MessageIdCollection,
    CancellationToken CancellationToken
);
