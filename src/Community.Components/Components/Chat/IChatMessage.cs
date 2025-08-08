namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for the chat message.
/// </summary>
public interface IChatMessage
{
    /// <summary>
    /// Gets the identifier of the message.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets the id of the replied message.
    /// </summary>
    long? ReplyMessageId { get; }

    /// <summary>
    /// Gets the created date of the message.
    /// </summary>
    DateTime CreatedDate { get; }

    /// <summary>
    /// Gets the sender of the message.
    /// </summary>
    ChatUser? Sender { get; }

    /// <summary>
    /// Gets the type of the message.
    /// </summary>
    ChatMessageType MessageType { get; }

    /// <summary>
    /// Gets a value indicating if the message is pinned.
    /// </summary>
    bool IsPinned { get; }

    /// <summary>
    /// Gets a value indicating if the message is edited.
    /// </summary>
    bool Edited { get; }

    /// <summary>
    /// Gets a value indicating if the message is deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the sections of the message.
    /// </summary>
    List<IChatMessageSection> Sections { get; }

    /// <summary>
    /// Gets the files of the message.
    /// </summary>
    List<IChatFile> Files { get; }

    /// <summary>
    /// Gets the reactions of the message.
    /// </summary>
    List<IChatMessageReaction> Reactions { get; }

    /// <summary>
    /// Gets the read states of the message.
    /// </summary>
    Dictionary<ChatUser, bool> ReadStates { get; }

    /// <summary>
    /// Sets the read state of the message.
    /// </summary>
    /// <param name="user">User who read the message.</param>
    /// <param name="read">Value indicating if the message was read.</param>
    void SetReadState(ChatUser user, bool read);

    /// <summary>
    /// Gets the read message state from all users but <paramref name="butUser"/>.
    /// </summary>
    /// <param name="butUser">User to except from the read state.</param>
    /// <returns>Returns the read state of the message.</returns>
    ChatMessageReadState GetMessageReadState(ChatUser butUser);

    /// <summary>
    /// Gets the replied message if any.
    /// </summary>
    IChatMessage? ReplyMessage { get; }
}
