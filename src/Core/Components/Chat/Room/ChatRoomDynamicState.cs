using FluentUI.Blazor.Community.Components.Chat.EventArgs;
using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

internal sealed class ChatRoomDynamicState
{
    private readonly Dictionary<long, IReadOnlyList<ChatUser>> _users = [];
    private readonly Dictionary<long, ChatMessage?> _lastMessages = [];
    private readonly Dictionary<long, int> _unreadCounts = [];

    public event EventHandler<UsersUpdatedEventArgs>? UsersUpdated;
    public event EventHandler<LastMessageUpdatedEventArgs>? LastMessageUpdated;
    public event EventHandler<UnreadCountUpdatedEventArgs>? UnreadCountUpdated;

    public void SetUsers(long roomId, IReadOnlyList<ChatUser> users)
    {
        _users[roomId] = users;
        UsersUpdated?.Invoke(this, new(roomId, users));
    }

    public void SetLastMessage(long roomId, ChatMessage? message)
    {
        _lastMessages[roomId] = message;
        LastMessageUpdated?.Invoke(this, new(roomId, message));
    }

    public void SetUnreadCount(long roomId, int count)
    {
        _unreadCounts[roomId] = count;
        UnreadCountUpdated?.Invoke(this, new(roomId, count));
    }

    public IReadOnlyList<ChatUser> GetUsers(long roomId) => _users.TryGetValue(roomId, out var u) ? u : [];

    public IReadOnlyList<ChatUser> GetUsersBut(long roomId, long userId)
    {
        if (!_users.TryGetValue(roomId, out var users))
        {
            return [];
        }

        var list = new List<ChatUser>();

        foreach (var user in users)
        {
            if (user.Id != userId)
            {
                list.Add(user);
            }
        }

        return list;
    }

    public ChatMessage? GetLastMessage(long roomId) => _lastMessages.TryGetValue(roomId, out var m) ? m : null;

    public int GetUnreadCount(long roomId) => _unreadCounts.TryGetValue(roomId, out var c) ? c : 0;
}
