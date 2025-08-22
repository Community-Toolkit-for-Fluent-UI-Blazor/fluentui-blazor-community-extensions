using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the request to get the items of the rooms.
/// </summary>
/// <param name="Filter">Predicate to search the rooms.</param>
public record ChatRoomItemsRequest(Expression<Func<ChatRoom, bool>>? Filter = null)
{
}
