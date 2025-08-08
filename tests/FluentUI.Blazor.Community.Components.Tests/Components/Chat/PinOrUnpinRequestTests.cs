using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Facts.Components.Chat;
public class PinOrUnpinRequestFacts
{
    private class FactChatMessage : IChatMessage
    {
        public string Content { get; set; }
        public long Id { get; set; }

        public long? ReplyMessageId => throw new NotImplementedException();

        public DateTime CreatedDate => throw new NotImplementedException();

        public ChatUser? Sender => throw new NotImplementedException();

        public ChatMessageType MessageType => throw new NotImplementedException();

        public bool IsPinned => throw new NotImplementedException();

        public bool Edited => throw new NotImplementedException();

        public bool IsDeleted => throw new NotImplementedException();

        public List<IChatMessageSection> Sections => throw new NotImplementedException();

        public List<IChatFile> Files => throw new NotImplementedException();

        public List<IChatMessageReaction> Reactions => throw new NotImplementedException();

        public Dictionary<ChatUser, bool> ReadStates => throw new NotImplementedException();

        public IChatMessage? ReplyMessage => throw new NotImplementedException();

        public ChatMessageReadState GetMessageReadState(ChatUser butUser)
        {
            throw new NotImplementedException();
        }

        public void SetReadState(ChatUser user, bool read)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        long expectedRoomId = 12345;
        var expectedMessage = new FactChatMessage { Content = "Hello", Id = 1 };
        bool expectedPin = true;

        // Act
        var request = new PinOrUnpinRequest(expectedRoomId, expectedMessage, expectedPin);

        // Assert
        Assert.Equal(expectedRoomId, request.RoomId);
        Assert.Equal(expectedMessage, request.Message);
        Assert.Equal(expectedPin, request.Pin);
    }

    [Fact]
    public void Equality_ShouldReturnTrue_ForSameValues()
    {
        // Arrange
        var message = new FactChatMessage { Content = "Hello", Id = 1 };
        var request1 = new PinOrUnpinRequest(12345, message, true);
        var request2 = new PinOrUnpinRequest(12345, message, true);

        // Act
        var areEqual = request1 == request2;

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_ForDifferentRoomId()
    {
        // Arrange
        var message = new FactChatMessage { Content = "Hello", Id = 1 };
        var request1 = new PinOrUnpinRequest(12345, message, true);
        var request2 = new PinOrUnpinRequest(54321, message, true);

        // Act
        var areEqual = request1 == request2;

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_ForDifferentMessage()
    {
        // Arrange
        var message1 = new FactChatMessage { Content = "Hello", Id = 1 };
        var message2 = new FactChatMessage { Content = "World", Id = 2 };
        var request1 = new PinOrUnpinRequest(12345, message1, true);
        var request2 = new PinOrUnpinRequest(12345, message2, true);

        // Act
        var areEqual = request1 == request2;

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void Equality_ShouldReturnFalse_ForDifferentPinStatus()
    {
        // Arrange
        var message = new FactChatMessage { Content = "Hello", Id = 1 };
        var request1 = new PinOrUnpinRequest(12345, message, true);
        var request2 = new PinOrUnpinRequest(12345, message, false);

        // Act
        var areEqual = request1 == request2;

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void ToString_ShouldReturnExpectedFormat()
    {
        // Arrange
        var message = new FactChatMessage { Content = "Hello", Id = 1 };
        var request = new PinOrUnpinRequest(12345, message, true);

        // Act
        string result = request.ToString();

        // Assert
        Assert.Contains("RoomId", result);
        Assert.Contains("Message", result);
        Assert.Contains("Pin", result);
    }
}
