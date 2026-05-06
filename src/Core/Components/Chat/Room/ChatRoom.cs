using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a chat room.
/// </summary>
public class ChatRoom : IChatRoom
{
    private readonly List<ChatUser> _users = [];

    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public string? Name { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<ChatUser> Users => _users;

    /// <inheritdoc />
    public ChatUser? Owner { get; set; }

    /// <inheritdoc />
    public bool IsEmpty { get; set; }

    /// <inheritdoc />
    public bool IsBlocked { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedDate { get; set; }

    /// <inheritdoc />
    public IChatMessage? LastMessage { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<long, uint> UnreadMessagesForUserId { get; internal set; } = new Dictionary<long, uint>();

    /// <inheritdoc />
    public bool IsHidden { get; set; }

    /// <inheritdoc />
    internal IReadOnlyList<ChatUser> GetUsersBut(ChatUser? user)
    {
        if (user is null)
        {
            return _users;
        }

        return _users.FindAll(u => u.Id != user.Id);
    }

    /// <summary>
    /// Sets the users of the chat room, replacing any existing users.
    /// </summary>
    /// <param name="users">The users to set for the chat room.</param>
    internal void SetUsers(IEnumerable<ChatUser> users)
    {
        _users.Clear();
        _users.AddRange(users);
    }
}
