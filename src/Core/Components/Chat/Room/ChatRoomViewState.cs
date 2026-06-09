namespace FluentUI.Blazor.Community.Components.Chat.Room;

internal sealed class ChatRoomViewState
{
    private readonly Dictionary<long, ChatRoomView> _rooms = [];

    public event EventHandler? RoomsChanged;
    public event EventHandler? RoomUpdated;

    public IReadOnlyCollection<ChatRoomView> Rooms => _rooms.Values;

    public void AddOrUpdateRoom(ChatRoomView room)
    {
        _rooms[room.Room.Id] = room;
        RoomsChanged?.Invoke(this, System.EventArgs.Empty);
    }

    public void RemoveRoom(long roomId)
    {
        if (_rooms.Remove(roomId))
        {
            RoomsChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }

    public ChatRoomView? GetRoom(long roomId) => _rooms.TryGetValue(roomId, out var room) ? room : null;

    public void UpdateRoom(ChatRoomView room)
    {
        if (_rooms.TryGetValue(room.Room.Id, out var existing))
        {
            existing = new ChatRoomView
            {
                Room = new ChatRoom
                {
                    Id = existing.Room.Id,
                    Name = room.Room.Name,
                    IsEmpty = room.Room.IsEmpty,
                    IsLocked = room.Room.IsLocked,
                    CreatedDate = room.Room.CreatedDate,
                    IsDeleted = room.Room.IsDeleted,
                    OwnerId = room.Room.OwnerId,
                    Owner = room.Room.Owner
                },
                UserState = new ChatRoomUser
                {
                    IsPinned = room.UserState?.IsPinned ?? false,
                    IsMuted = room.UserState?.IsMuted ?? false,
                    IsHidden = room.UserState?.IsHidden ?? false,
                    IsBlocked = room.UserState?.IsBlocked ?? false,
                    IsArchived = room.UserState?.IsArchived ?? false
                },
                LastMessage = room.LastMessage,
                UnreadCount = room.UnreadCount,
                Users = room.Users
            };

            _rooms[room.Room.Id] = existing;

            RoomUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
