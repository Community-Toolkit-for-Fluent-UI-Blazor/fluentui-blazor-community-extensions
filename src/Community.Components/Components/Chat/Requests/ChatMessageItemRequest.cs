using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the request to get a list of messages.
/// </summary>
/// <param name="RoomId">Identifier of the room.</param>
/// <param name="OwnerId">Identifier of the owner.</param>
/// <param name="StartIndex">Index to skip.</param>
/// <param name="Count">Number of items to take.</param>
/// <param name="Filter">Predicate to filter the messages.</param>
public record ChatMessageItemRequest(
    long RoomId,
    long OwnerId,
    int StartIndex,
    int Count,
    Expression<Func<IChatMessage, bool>>? Filter)
{
}
