using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageReactEventArgsTests
{
    private class TestChatMessage : IChatMessage
    {
        public long Id { get; set; }
        public long? ReplyMessageId => null;
        public DateTime CreatedDate => DateTime.Now;
        public ChatUser? Sender => null;
        public ChatMessageType MessageType => ChatMessageType.Text;
        public bool IsPinned => false;
        public bool Edited => false;
        public bool IsDeleted => false;
        public List<IChatMessageSection> Sections => new();
        public List<IChatFile> Files => new();
        public List<IChatMessageReaction> Reactions => new();
        public Dictionary<ChatUser, bool> ReadStates => new();
        public void SetReadState(ChatUser user, bool read) { }
        public ChatMessageReadState GetMessageReadState(ChatUser butUser) => ChatMessageReadState.Unread;
        public IChatMessage? ReplyMessage => null;
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var message = new TestChatMessage { Id = 123 };
        var reaction = "👍";
        var args = new ChatMessageReactEventArgs(message, reaction);

        Assert.Equal(message, args.Message);
        Assert.Equal(reaction, args.Reaction);
    }

    [Fact]
    public void Equality_WorksForSameValues()
    {
        var message = new TestChatMessage { Id = 1 };
        var args1 = new ChatMessageReactEventArgs(message, "😀");
        var args2 = new ChatMessageReactEventArgs(message, "😀");

        Assert.Equal(args1, args2);
        Assert.True(args1 == args2);
        Assert.False(args1 != args2);
    }

    [Fact]
    public void CanHandleNullValues()
    {
        var args = new ChatMessageReactEventArgs(null!, null!);
        Assert.Null(args.Message);
        Assert.Null(args.Reaction);
    }
}
