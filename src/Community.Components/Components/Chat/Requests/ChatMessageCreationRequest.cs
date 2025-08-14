namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the requestion to create a message.
/// </summary>
/// <param name="RoomId">Identifier of the room.</param>
/// <param name="Owner">Owner of the message.</param>
/// <param name="ChatDraft">Draft of the message.</param>
/// <param name="SplitOption">Options to split the message into multiple messages or not.</param>
public record ChatMessageCreationRequest(
    long RoomId,
    ChatUser Owner,
    ChatMessageDraft ChatDraft,
    ChatMessageSplitOption SplitOption,
    bool IsTranslationEnabled)
{
}
