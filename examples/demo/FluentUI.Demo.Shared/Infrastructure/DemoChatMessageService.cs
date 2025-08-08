using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Services;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class DemoChatMessageService
    : IChatMessageService
{
    private readonly Dictionary<long, List<IChatMessage>> _messages = [];

    public ValueTask AddReactionAsync(ChatMessageReactRequest value)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<IChatMessage>> CreateMessagesAsync(ChatMessageCreationRequest value)
    {
        throw new NotImplementedException();
    }

    public ValueTask DeleteAsync(long id, IChatMessage message)
    {
        var messages = _messages.GetValueOrDefault(id);

        messages?.Remove(message);

        return ValueTask.CompletedTask;
    }

    public ValueTask EditMessageAsync(ChatMessageEditRequest value)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IChatMessage?> GetMessageAsync(long roomId, long messageId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<IChatMessage>> GetMessageListAsync(ChatMessageItemRequest value)
    {
        if(_messages.TryGetValue(value.RoomId, out var messages))
        {
            return ValueTask.FromResult<IEnumerable<IChatMessage>>(messages);
        }

        return ValueTask.FromResult<IEnumerable<IChatMessage>>([]);
    }

    public ValueTask<int> MessageCountAsync(ChatMessageCountRequest value)
    {
        var count = _messages.TryGetValue(value.RoomId, out var messages) ? messages.Count : 0;

        return ValueTask.FromResult(count);
    }

    public ValueTask PinOrUnpinAsync(PinOrUnpinRequest pinOrUnpinRequest)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetReadStateAsync(long roomId, long id, ChatUser owner, bool read)
    {
        throw new NotImplementedException();
    }
}
