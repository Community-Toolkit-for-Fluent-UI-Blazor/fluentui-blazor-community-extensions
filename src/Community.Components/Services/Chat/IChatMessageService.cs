using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Services;

public interface IChatMessageService
{
    ValueTask AddReactionAsync(ChatMessageReactRequest value);
    ValueTask<IEnumerable<IChatMessage>> CreateMessagesAsync(ChatMessageCreationRequest value);
    ValueTask DeleteAsync(long id, IChatMessage message);
    ValueTask EditMessageAsync(ChatMessageEditRequest value);
    ValueTask<IChatMessage?> GetMessageAsync(long roomId, long messageId);
    ValueTask<IEnumerable<IChatMessage>> GetMessageListAsync(ChatMessageItemRequest value);
    ValueTask<int> MessageCountAsync(ChatMessageCountRequest value);
    ValueTask PinOrUnpinAsync(PinOrUnpinRequest pinOrUnpinRequest);
    ValueTask SetReadStateAsync(long roomId, long id, ChatUser owner, bool v);
}
