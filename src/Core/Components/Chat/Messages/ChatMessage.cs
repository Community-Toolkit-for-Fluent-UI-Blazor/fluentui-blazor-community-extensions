using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a chat message in the chat system.
/// </summary>
public sealed record ChatMessage
{
    /// <summary>
    /// Gets the unique identifier of the chat message.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the identifier of the room to which the chat message belongs.
    /// </summary>
    public long RoomId { get; init; }

    /// <summary>
    /// Gets the identifier of the reply-to message, if this message is a reply to another message; otherwise, null.
    /// </summary>
    public long? ReplyToMessageId { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the user who sent the chat message.
    /// </summary>
    public long SenderId { get; init; }

    /// <summary>
    /// Gets the date and time when the chat message was created.
    /// </summary>
    public DateTimeOffset CreatedDate { get; init; }

    /// <summary>
    /// Gets the date and time when the chat message was last edited, if applicable; otherwise, null.
    /// </summary>
    public DateTimeOffset? EditedDate { get; init; }

    /// <summary>
    /// Gets the date and time when the chat message was deleted, if applicable; otherwise, null.
    /// </summary>
    public DateTimeOffset? DeletedDate { get; init; }

    /// <summary>
    /// Gets the user who sent the chat message.
    /// </summary>
    public ChatUser Sender { get; init; } = new();

    /// <summary>
    /// Gets a value indicating whether the chat message is pinned in the chat room.
    /// </summary>
    public bool IsPinned { get; init; }

    /// <summary>
    /// Gets a value indicating whether the chat message has been deleted.
    /// </summary>
    public bool IsDeleted { get; init; }

    /// <summary>
    /// Gets the sections of the chat message, which may include text, images, or other content.
    /// </summary>
    public IReadOnlyList<ChatMessageSection> Sections { get; init; } = [];

    /// <summary>
    /// Gets the message to which this chat message is replying, if applicable; otherwise, null.
    /// </summary>
    public ChatMessage? ReplyToMessage { get; init; }

    /// <summary>
    /// Gets the type of the chat message, indicating whether it is a text message, image message, or another type of message.
    /// </summary>
    public ChatMessageType Type { get; init; }
}
