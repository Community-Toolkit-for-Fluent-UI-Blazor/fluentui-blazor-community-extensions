using FluentUI.Blazor.Community.Components.Chat.Room;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents the state of the chat.
/// </summary>
internal record ChatState
{
    /// <summary>
    /// Represents the selected room.
    /// </summary>
    private ChatRoomView? _room;

    /// <summary>
    /// Represents the draft message for different rooms.
    /// </summary>
    private readonly Dictionary<long, ChatMessageDraft> _drafts = [];

    /// <summary>
    /// Events which occured when a room has changed.
    /// </summary>
    public event EventHandler? RoomViewChanged;

    /// <summary>
    /// Gets or sets the selected room.
    /// </summary>
    public ChatRoomView? RoomView
    {
        get => _room;
        internal set
        {
            if (_room != value)
            {
                _room = value;
                RoomViewChanged?.Invoke(this, System.EventArgs.Empty);
            }
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
    /// Gets the draft for the <see cref="RoomView"/>.
    /// </summary>
    /// <returns>Returns the draft for the room.</returns>
    public ChatMessageDraft? GetDraft()
    {
        return GetDraft(RoomView?.Room.Id);
    }
}
