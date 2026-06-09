using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents the request to get the items of the rooms.
/// </summary>
/// <param name="RoomFilter">Predicate to search the rooms.</param>
/// <param name="UserFilter">Predicate to search the users in the rooms.</param>
/// <param name="UserId">User id to search the rooms.</param>
/// <param name="StartIndex">Starting index of the items to retrieve.</param>
/// <param name="Count">Number of items to retrieve.</param>
public record ChatRoomItemsRequest(
    Expression<Func<ChatRoom, bool>>? RoomFilter = null,
    Expression<Func<ChatRoomUser, bool>>? UserFilter = null,
    long UserId = 0,
    int StartIndex = 0,
    int Count = 0);
