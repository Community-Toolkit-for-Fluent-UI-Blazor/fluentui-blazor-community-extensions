using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the request to get the items of the rooms.
/// </summary>
/// <param name="Filter">Predicate to search the rooms.</param>
public record ChatRoomItemsRequest<TChatRoom>(
    Expression<Func<TChatRoom, bool>>? Filter = null) where TChatRoom : IChatRoomCapabilities
{
}
