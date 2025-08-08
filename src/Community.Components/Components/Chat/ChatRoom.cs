namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chat room.
/// </summary>
public class ChatRoom : IChatRoom
{
    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public string? Name { get; set; }

    /// <inheritdoc />
    public List<ChatUser> Users { get; set; } = [];

    /// <inheritdoc />
    public ChatUser? Owner { get; set; }

    /// <inheritdoc />
    public bool IsEmpty { get; set; }

    /// <inheritdoc />
    public bool IsBlocked { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTime CreatedDate { get; set; }

    /// <inheritdoc />
    public IChatMessage? LastMessage { get; set; }

    /// <inheritdoc />
    public Dictionary<long, uint> UnreadMessagesForUserId { get; set; } = [];

    /// <inheritdoc />
    public bool IsHidden { get; set; }

    /// <inheritdoc />
    public IEnumerable<ChatUser> GetUsersBut(ChatUser? user)
    {
        if (user is null)
        {
            return Users;
        }

        return Users.Except([user], ChatUserEqualityComparer.Instance);
    }
}
