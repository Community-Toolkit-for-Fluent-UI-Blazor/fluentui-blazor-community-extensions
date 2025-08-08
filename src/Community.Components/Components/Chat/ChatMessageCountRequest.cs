namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the request to count the total number of messages to retrieve.
/// </summary>
/// <param name="RoomId">Identifier of the room.</param>
/// <param name="OwnerId">Identifier of the owner of the room.</param>
/// <param name="Filter">Predicate to retrieve specific messages.</param>
public record ChatMessageCountRequest(long RoomId, long OwnerId, Func<IChatMessage, bool>? Filter)
{
}
