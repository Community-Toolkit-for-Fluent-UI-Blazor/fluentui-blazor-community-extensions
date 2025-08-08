using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageEditRequestTests
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
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var message = new DummyMessage { Id = 42 };
        var text = "Nouveau texte";

        var request = new ChatMessageEditRequest(99, message, user, text);

        Assert.Equal(99, request.RoomId);
        Assert.Equal(message, request.Message);
        Assert.Equal(user, request.Owner);
        Assert.Equal(text, request.Text);
    }

    [Fact]
    public void Text_CanBeNull()
    {
        var user = new ChatUser { Id = 2, DisplayName = "Bob" };
        var message = new DummyMessage { Id = 7 };

        var request = new ChatMessageEditRequest(1, message, user, null);

        Assert.Null(request.Text);
    }
}
