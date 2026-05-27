using FluentUI.Blazor.Community.Components.Chat.Room;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

internal sealed class ChatMessageState
{
    private readonly Dictionary<long, Dictionary<long, ChatMessage>> _messages = [];

    public event EventHandler? MessageUpdated;
    public event EventHandler? MessageRemoved;

    public void AddOrUpdateMessage(ChatRoom room, ChatMessage message)
    {
        if (!_messages.TryGetValue(room.Id, out var dict))
        {
            dict = [];
            _messages[room.Id] = dict;
        }

        dict[message.Id] = message;
        MessageUpdated?.Invoke(this, System.EventArgs.Empty);
    }

    public void RemoveMessage(ChatRoom room, long messageId)
    {
        if (_messages.TryGetValue(room.Id, out var dict))
        {
            dict.Remove(messageId);
        }

        MessageRemoved?.Invoke(this, System.EventArgs.Empty);
    }

    public ChatMessage? GetLastMessage(long roomId)
    {
        if (!_messages.TryGetValue(roomId, out var dict) ||
            dict.Count == 0)
        {
            return null;
        }

        ChatMessage? last = null;

        foreach (var msg in dict.Values)
        {
            if (last is null || msg.CreatedDate > last.CreatedDate)
            {
                last = msg;
            }
        }

        return last;
    }

    public IEnumerable<ChatMessage> GetMessages(long roomId)
    {
        if (!_messages.TryGetValue(roomId, out var dict))
        {
            return [];
        }

        return dict.Values;
    }
}
