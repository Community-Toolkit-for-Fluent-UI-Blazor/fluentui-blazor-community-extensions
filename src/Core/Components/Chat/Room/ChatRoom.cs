using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

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
    public IReadOnlyList<ChatUser> Users { get; init; } = [];

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
            return Users;
        }

        return [.. Users.Where(u => u.Id != user.Id)];
    }
}
