namespace FluentUI.Blazor.Community.Components;

public class ChatMessage
    : IChatMessage
{
    public long Id { get; set; }

    public long? ReplyMessageId { get; set; }

    public DateTime CreatedDate { get; set; }

    public ChatUser? Sender { get; set; }

    public ChatMessageType MessageType { get; set; }

    public bool IsPinned { get; set; }

    public bool Edited { get; set; }

    public bool IsDeleted { get; set; }

    public List<IChatMessageSection> Sections { get; } = [];

    public List<IChatFile> Files { get; } = [];

    public List<IChatMessageReaction> Reactions { get; } = [];

    public Dictionary<ChatUser, bool> ReadStates { get; } = new Dictionary<ChatUser, bool>(ChatUserEqualityComparer.Instance);

    public IChatMessage? ReplyMessage { get; set; }

    public ChatMessageReadState GetMessageReadState(ChatUser butUser)
    {
        var allBut = ReadStates.Where(x => x.Key.Id != butUser.Id)
            .Select(x => x.Value)
            .ToList();

        if (allBut.All(x => !x))
        {
            return ChatMessageReadState.Unread;
        }

        if (allBut.All(x => x))
        {
            return ChatMessageReadState.ReadByEveryone;
        }

        return ChatMessageReadState.Read;
    }


    public void SetReadState(ChatUser user, bool read)
    {
        if (ReadStates.ContainsKey(user))
        {
            ReadStates[user] = read;
        }
    }
}
