using FluentUI.Blazor.Community.Components.Chat;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;

namespace FluentUI.Demo.Client.Documentation.Components.Chat.Models;

public class ChatRoomEntity : IChatRoom
{
    public bool IsBlocked { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsHidden { get; set; }

    public bool IsPinned { get; set; }

    public bool IsMuted { get; set; }

    public bool IsArchived { get; set; }

    public long Id { get; set; }

    public string? Name { get; set; }

    public ChatUser? Owner { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public bool IsEmpty { get; set; }

    public IChatMessage? LastMessage { get; set; }

    public IReadOnlyDictionary<long, uint> UnreadMessagesForUserId { get; set; } = new Dictionary<long, uint>();

    public IReadOnlyList<ChatUser> Users { get; set; } = [];
}
