namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents the reaction on a message.
/// </summary>
public sealed class ChatMessageReaction
{
    /// <summary>
    /// Gets or sets the unique identifier of the reaction.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the message that the reaction belongs to.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who reacted to the message.
    /// </summary>
    public long UserReactedById { get; set; }

    /// <summary>
    /// Gets or sets the user who reacted to the message.
    /// </summary>
    public ChatUser UserReactedBy { get; set; } = new ChatUser();

    /// <summary>
    /// Gets or sets the reaction as an emoji.
    /// </summary>
    public string Emoji { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the reaction was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}
