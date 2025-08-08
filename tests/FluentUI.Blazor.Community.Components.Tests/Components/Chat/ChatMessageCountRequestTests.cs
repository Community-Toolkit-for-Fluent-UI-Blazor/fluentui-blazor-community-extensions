using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageCountRequestTests
{
    private class DummyMessage : IChatMessage
    {
        public long Id { get; set; }
        public long? ReplyMessageId => null;
        public DateTime CreatedDate => DateTime.Now;
        public ChatUser? Sender => null;
        public ChatMessageType MessageType => ChatMessageType.Text;
        public bool IsPinned => false;
        public bool Edited => false;
        public bool IsDeleted => false;
        public List<IChatMessageSection> Sections => [];
        public List<IChatFile> Files => [];
        public List<IChatMessageReaction> Reactions => [];
        public Dictionary<ChatUser, bool> ReadStates => [];
        public void SetReadState(ChatUser user, bool read) { }
        public ChatMessageReadState GetMessageReadState(ChatUser butUser) => ChatMessageReadState.Unread;
        public IChatMessage? ReplyMessage => null;
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        Func<IChatMessage, bool> filter = m => m.Id > 10;
        var request = new ChatMessageCountRequest(1, 2, filter);

        Assert.Equal(1, request.RoomId);
        Assert.Equal(2, request.OwnerId);
        Assert.Equal(filter, request.Filter);
    }

    [Fact]
    public void Filter_Null_ReturnsNull()
    {
        var request = new ChatMessageCountRequest(1, 2, null);
        Assert.Null(request.Filter);
    }

    [Fact]
    public void Filter_Predicate_WorksAsExpected()
    {
        Func<IChatMessage, bool> filter = m => m.Id == 42;
        var request = new ChatMessageCountRequest(99, 100, filter);
        var message = new DummyMessage { Id = 42 };
        var message2 = new DummyMessage { Id = 7 };

        Assert.True(request.Filter!(message));
        Assert.False(request.Filter!(message2));
    }
}
