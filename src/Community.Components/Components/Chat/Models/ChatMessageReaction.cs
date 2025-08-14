namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the reaction on a message.
/// </summary>
public sealed class ChatMessageReaction
    : IChatMessageReaction
{
    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public long MessageId { get; set; }

    /// <inheritdoc />
    public ChatUser UserReactedBy { get; set; } = default!;

    /// <inheritdoc />
    public string? Emoji { get; set; }
}
