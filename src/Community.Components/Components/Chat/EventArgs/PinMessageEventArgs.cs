namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a message to pin or unpin.
/// </summary>
/// <param name="Message">Message to pin or unpin.</param>
/// <param name="Pin">Value indicating if the message is pinned or not.</param>
public record PinMessageEventArgs(IChatMessage Message, bool Pin)
{
}
