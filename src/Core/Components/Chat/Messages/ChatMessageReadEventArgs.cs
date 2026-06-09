namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents the event arguments for a chat message read event, containing information about the message, the user, and the read status.
/// </summary>
/// <param name="MessageId">The ID of the message that was read.</param>
/// <param name="UserId">The ID of the user who read the message.</param>
/// <param name="IsRead">A value indicating whether the message has been read.</param>
/// <param name="ReadDate">The date and time when the message was read.</param>
public sealed record ChatMessageReadEventArgs(
    long MessageId,
    long UserId,
    bool IsRead,
    DateTimeOffset? ReadDate)
{
}
