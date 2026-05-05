namespace FluentUI.Blazor.Community.Components.Chat.Messages;

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
    string Emoji { get; }

    /// <summary>
    /// Gets the date when the reaction was created.
    /// </summary>
    DateTime CreatedDate { get; }
}
