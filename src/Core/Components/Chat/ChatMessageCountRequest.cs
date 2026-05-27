using System.Linq.Expressions;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request for retrieving the total count of chat messages for a given chat room.
/// </summary>
/// <param name="RoomId">The ID of the chat room.</param>
/// <param name="Filter">The filter expression.</param>
/// <param name="Token">The cancellation token.</param>
public sealed record ChatMessageCountRequest(long RoomId, Expression<Func<ChatMessage, bool>> Filter, CancellationToken Token);
