namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the reaction of a message.
/// </summary>
public interface IChatMessageReaction
{
    /// <summary>
    /// Gets the identifier of the reaction.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets the identifier of the message.
    /// </summary>
    long MessageId { get; }

    /// <summary>
    /// Gets the user which reacts to the message.
    /// </summary>
    ChatUser UserReactedBy { get; }

    /// <summary>
    /// Gets the reaction as an emoji.
    /// </summary>
    public string? Emoji { get; }
}
