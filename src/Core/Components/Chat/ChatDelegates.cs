using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a delegate that provides chat messages for the chat component.
/// </summary>
/// <param name="request">The request containing the details for retrieving chat messages.</param>
/// <returns>Returns a task that represents the asynchronous operation. The task result contains a read-only list of chat messages.</returns>
public delegate ValueTask<IReadOnlyList<ChatMessage>> ChatMessageItemsProvider(ChatMessageProviderRequest request);

/// <summary>
/// Represents a delegate that provides the total count of chat messages for a given chat room.
/// </summary>
/// <param name="request">The request containing the details for retrieving the total count of chat messages.</param>
/// <returns>Returns a task that represents the asynchronous operation. The task result contains the total count of chat messages.</returns>
public delegate ValueTask<int> ChatMessageCountProvider(ChatMessageCountRequest request);

/// <summary>
/// Represents a method that asynchronously provides a chat message item based on a request.
/// </summary>
/// <param name="request">The request containing the parameters for retrieving the chat message.</param>
/// <returns>Returns a task that represents the asynchronous operation. The task result contains the chat message item.</returns>
public delegate ValueTask<IReadOnlyList<ChatMessage>> ChatMessageItemCollectionProvider(ChatMessageItemsRequest request);

/// <summary>
/// Represents an asynchronous operation that retrieves user state information for chat messages.
/// </summary>
/// <param name="request">The request containing the criteria for retrieving chat message user states.</param>
/// <returns>Returns a task that represents the asynchronous operation. The task result contains a dictionary mapping message identifiers
/// to their associated user states.</returns>
public delegate ValueTask<IReadOnlyDictionary<long, IReadOnlyList<ChatMessageUserState>>> ChatMessageUserStateProvider(ChatMessageUserStateRequest request);

/// <summary>
/// Represents an asynchronous operation that retrieves a collection of chat files associated with chat messages.
/// </summary>
/// <param name="request">The request containing the parameters for retrieving the chat message files.</param>
/// <returns>Returns a task that represents the asynchronous operation. The task result contains a dictionary mapping message identifiers to their associated chat files.</returns>
public delegate ValueTask<IReadOnlyDictionary<long, IReadOnlyList<IChatFile>>> ChatMessageFileCollectionProvider(ChatMessageFileCollectionRequest request);

/// <summary>
/// Represents a delegate that retrieves collections of chat message reactions grouped by identifier.
/// </summary>
/// <param name="request">The request containing parameters for retrieving the reaction collections.</param>
/// <returns>A dictionary mapping identifiers to read-only collections of chat message reactions.</returns>
public delegate ValueTask<IReadOnlyDictionary<long, IReadOnlyList<ChatMessageReaction>>> ChatMessageReactionCollectionProvider(ChatMessageReactionCollectionRequest request);
