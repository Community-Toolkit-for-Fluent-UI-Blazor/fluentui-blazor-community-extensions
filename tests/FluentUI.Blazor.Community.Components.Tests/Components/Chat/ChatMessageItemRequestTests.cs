using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageItemRequestTests
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
        // Arrange
        long roomId = 1;
        long ownerId = 2;
        int startIndex = 10;
        int count = 5;
        Func<IChatMessage, bool>? filter = m => m.Id > 0;

        // Act
        var request = new ChatMessageItemRequest(roomId, ownerId, startIndex, count, filter);

        // Assert
        Assert.Equal(roomId, request.RoomId);
        Assert.Equal(ownerId, request.OwnerId);
        Assert.Equal(startIndex, request.StartIndex);
        Assert.Equal(count, request.Count);
        Assert.Equal(filter, request.Filter);
    }

    [Fact]
    public void Equality_WorksForSameValues()
    {
        var filter = new Func<IChatMessage, bool>(m => m.Id > 0);
        var req1 = new ChatMessageItemRequest(1, 2, 3, 4, filter);
        var req2 = new ChatMessageItemRequest(1, 2, 3, 4, filter);

        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
        Assert.False(req1 != req2);
    }

    [Fact]
    public void Filter_CanBeNull()
    {
        var req = new ChatMessageItemRequest(1, 2, 3, 4, null);
        Assert.Null(req.Filter);
    }

    [Fact]
    public void Filter_WorksAsPredicate()
    {
        var message = new TestChatMessage { Id = 42 };
        var req = new ChatMessageItemRequest(1, 2, 3, 4, m => m.Id == 42);

        Assert.NotNull(req.Filter);
        Assert.True(req.Filter!(message));
    }
}
