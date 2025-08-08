namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a request to pin or unpin a message.
/// </summary>
/// <param name="RoomId">Id of the room.</param>
/// <param name="Message">Message to pin or unpin.</param>
/// <param name="Pin">Value indicating if the message is pinned or not.</param>
public record PinOrUnpinRequest(long RoomId, IChatMessage Message, bool Pin)
{
}
