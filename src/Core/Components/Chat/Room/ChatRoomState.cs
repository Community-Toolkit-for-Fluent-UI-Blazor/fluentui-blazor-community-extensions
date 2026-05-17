namespace FluentUI.Blazor.Community.Components.Chat.Room;

internal sealed class ChatRoomState
{
    private readonly Dictionary<long, ChatRoom> _rooms = [];

    public event EventHandler? RoomsChanged;
    public event EventHandler<ChatRoom>? RoomUpdated;

    public IReadOnlyCollection<ChatRoom> Rooms => _rooms.Values;

    public void AddOrUpdateRoom(ChatRoom room)
    {
        _rooms[room.Id] = room;
        RoomsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveRoom(long roomId)
    {
        if (_rooms.Remove(roomId))
        {
            RoomsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public ChatRoom? GetRoom(long roomId) => _rooms.TryGetValue(roomId, out var room) ? room : null;

    public void UpdateRoom(ChatRoom room)
    {
        if (_rooms.TryGetValue(room.Id, out var existing))
        {
            existing.Name = room.Name;
            existing.IsPinned = room.IsPinned;
            existing.IsMuted = room.IsMuted;
            existing.IsHidden = room.IsHidden;
            existing.IsBlocked = room.IsBlocked;
            existing.IsArchived = room.IsArchived;
            existing.LastMessage = room.LastMessage;

            RoomUpdated?.Invoke(this, existing);
        }
    }
}
