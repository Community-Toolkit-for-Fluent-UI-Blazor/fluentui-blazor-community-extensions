using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a chat message in the chat system.
/// </summary>
public interface IChatMessage
{
    /// <summary>
    /// Gets the unique identifier of the chat message.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets the identifier of the room to which the chat message belongs.
    /// </summary>
    long RoomId { get; }

    /// <summary>
    /// Gets the identifier of the reply-to message, if this message is a reply to another message; otherwise, null.
    /// </summary>
    long? ReplyToMessageId { get; }

    /// <summary>
    /// Gets the date and time when the chat message was created.
    /// </summary>
    DateTimeOffset CreatedDate { get; }

    /// <summary>
    /// Gets the date and time when the chat message was last edited, if applicable; otherwise, null.
    /// </summary>
    DateTimeOffset? EditedDate { get; }

    /// <summary>
    /// Gets the date and time when the chat message was deleted, if applicable; otherwise, null.
    /// </summary>
    DateTimeOffset? DeletedDate { get; }

    /// <summary>
    /// Gets the user who sent the chat message.
    /// </summary>
    ChatUser Sender { get; }

    /// <summary>
    /// Gets a value indicating whether the chat message is pinned in the chat room.
    /// </summary>
    bool IsPinned { get; }

    /// <summary>
    /// Gets a value indicating whether the chat message has been deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the sections of the chat message, which may include text, images, or other content.
    /// </summary>
    IReadOnlyList<IChatMessageSection> Sections { get; }

    /// <summary>
    /// Gets the files attached to the chat message, if any.
    /// </summary>
    IReadOnlyList<IChatFile> Files { get; }

    /// <summary>
    /// Gets the reactions to the chat message, if any.
    /// </summary>
    IReadOnlyList<IChatMessageReaction> Reactions { get; }

    /// <summary>
    /// Gets the message to which this chat message is replying, if applicable; otherwise, null.
    /// </summary>
    IChatMessage? ReplyToMessage { get; }

    /// <summary>
    /// Gets the type of the chat message, indicating whether it is a text message, image message, or another type of message.
    /// </summary>
    ChatMessageType Type { get; }
}
