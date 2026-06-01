using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to add a reaction to a chat message.
/// </summary>
/// <param name="OwnerId">The identifier of the user adding the reaction.</param>
/// <param name="MessageId">The identifier of the chat message to react to.</param>
/// <param name="Emoji">The reaction to add to the message.</param>
public sealed record ChatMessageReactRequest(
    long OwnerId,
    long MessageId,
    string Emoji)
{
    /// <summary>
    /// Gets or sets the reaction to add to the message.
    /// </summary>
    public ChatMessageReaction Reaction { get; set; } = default!;
};
