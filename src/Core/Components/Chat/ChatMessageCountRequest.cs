using System.Linq.Expressions;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving the total count of chat messages for a given chat room.
/// </summary>
/// <param name="Filter">The filter expression.</param>
/// <param name="Token">The cancellation token.</param>
public sealed record ChatMessageCountRequest(Expression<Func<ChatMessage, bool>> Filter, CancellationToken Token);
