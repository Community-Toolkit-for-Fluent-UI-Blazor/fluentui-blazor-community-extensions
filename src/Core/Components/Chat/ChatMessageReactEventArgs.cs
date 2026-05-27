using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Components.Chat;

/// <summary>
/// Represents the event args for a reaction on a message.
/// </summary>
/// <param name="Message">Message where the reaction occured.</param>
/// <param name="Reaction">The reaction (an emoji)</param>
public record ChatMessageReactEventArgs(
    ChatMessage Message, string Reaction)
{
}
