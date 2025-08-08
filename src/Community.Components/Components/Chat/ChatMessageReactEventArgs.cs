namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a reaction on a message.
/// </summary>
/// <param name="Message">Message where the reaction occured.</param>
/// <param name="Reaction">The reaction (an emoji)</param>
public record ChatMessageReactEventArgs(
    IChatMessage Message, string Reaction)
{
}
