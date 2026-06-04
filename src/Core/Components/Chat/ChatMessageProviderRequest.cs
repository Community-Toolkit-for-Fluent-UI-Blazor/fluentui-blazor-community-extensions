using System.Linq.Expressions;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for chat messages, containing the necessary information to retrieve a specific range of messages from a chat room.
/// </summary>
/// <param name="Filter">The filter expression to apply when retrieving messages.</param>
/// <param name="StartIndex">The starting index of the messages to retrieve.</param>
/// <param name="Count">The number of messages to retrieve.</param>
/// <param name="Token">A cancellation token to cancel the operation.</param>
public sealed record ChatMessageProviderRequest(
    Expression<Func<ChatMessage, bool>> Filter,
    int StartIndex,
    int Count,
    CancellationToken Token);

