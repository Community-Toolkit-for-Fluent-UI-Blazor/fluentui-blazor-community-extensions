namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents the state of a message for a specific user, including whether the message has been read and the date it was read.
/// </summary>
public sealed record ChatMessageUserState
{
    /// <summary>
    /// Gets the unique identifier for the message user state.
    /// </summary>
    public long MessageId { get; init; }

    /// <summary>
    /// Gets the unique identifier for the user associated with this message state.
    /// </summary>
    public long UserId { get; init; }

    /// <summary>
    /// Gets a value indicating whether the message has been read by the user.
    /// </summary>
    public bool IsRead { get; init; }

    /// <summary>
    /// Gets the date and time when the message was read by the user, or null if the message has not been read.
    /// </summary>
    public DateTimeOffset? ReadDate { get; init; }
}
