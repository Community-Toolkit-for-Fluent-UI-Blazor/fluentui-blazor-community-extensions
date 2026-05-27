namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving a collection of chat message files associated with a specific message identifier. This record encapsulates the necessary information to fetch the files related to a chat message, including the message identifier and a cancellation token for managing asynchronous operations.
/// </summary>
/// <param name="MessageIdCollection">The collection of identifiers of the chat messages for which to retrieve associated files.</param>
/// <param name="CancellationToken">A token for monitoring and canceling the asynchronous operation.</param>
public sealed record ChatMessageFileCollectionRequest(
    IReadOnlyList<long> MessageIdCollection,
    CancellationToken CancellationToken);
