namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of the chat.
/// </summary>
public record ChatState
{
    /// <summary>
    /// Represents the selected room.
    /// </summary>
    private ChatRoom? _room;

    /// <summary>
    /// Represents the draft message for different rooms.
    /// </summary>
    private readonly Dictionary<long, ChatMessageDraft> _drafts = [];

    /// <summary>
    /// Events which occured when a room has changed.
    /// </summary>
    public event EventHandler<ChatRoom?>? RoomChanged;

    /// <summary>
    /// Gets if the room is loading.
    /// </summary>
    public bool IsLoading { get; internal set; }

    /// <summary>
    /// Gets or sets the selected room.
    /// </summary>
    public ChatRoom? Room
    {
        get => _room;
        internal set
        {
            if (_room != value)
            {
                _room = value;
                RoomChanged?.Invoke(this, value);
            }
        }
    }

    /// <summary>
    /// Clear the draft of the <paramref name="roomId"/>.
    /// </summary>
    /// <param name="roomId">Identifer of the room.</param>
    public void ClearDraft(long roomId)
    {
        if (_drafts.Remove(roomId))
        {
            _drafts.TryAdd(roomId, new());
        }
    }

    /// <summary>
    /// Gets the draft of the specified room.
    /// </summary>
    /// <param name="value">Identifier of the room.</param>
    /// <returns>Returns the draft of the room if found, <see langword="null" /> otherwise.</returns>
    private ChatMessageDraft? GetDraft(long? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var id = value.GetValueOrDefault();
        
        if (!_drafts.TryGetValue(id, out var d))
        {
            d = new();
            _drafts.Add(id, d);
        }

        return d;
    }

    /// <summary>
    /// Gets the draft for the <see cref="Room"/>.
    /// </summary>
    /// <returns>Returns the draft for the room.</returns>
    public ChatMessageDraft? GetDraft()
    {
        return GetDraft(Room?.Id);
    }
}

