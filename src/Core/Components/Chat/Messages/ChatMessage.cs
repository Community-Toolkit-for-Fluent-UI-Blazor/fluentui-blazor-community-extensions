using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal class ChatMessage : IChatMessage
{
    /// <inheritdoc />
    public long Id {get; init; }

    /// <inheritdoc />
    public long RoomId {get; init; }

    /// <inheritdoc />
    public long? ReplyToMessageId {get; init; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDate {get; init; }

    /// <inheritdoc />
    public DateTimeOffset? EditedDate {get; init; }

    /// <inheritdoc />
    public DateTimeOffset? DeletedDate {get; init; }

    /// <inheritdoc />
    public ChatUser Sender { get; init; } = default!;

    /// <inheritdoc />
    public bool IsPinned {get; set; }

    /// <inheritdoc />
    public bool IsDeleted {get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IChatMessageSection> Sections { get; init; } = [];

    /// <inheritdoc />
    public IReadOnlyList<IChatFile> Files {get; init; } = [];

    /// <inheritdoc />
    public IReadOnlyList<IChatMessageReaction> Reactions {get; init; } = [];

    /// <inheritdoc />
    public IChatMessage? ReplyToMessage {get; set; }

    /// <inheritdoc />
    public ChatMessageType Type {get; init; }
}
