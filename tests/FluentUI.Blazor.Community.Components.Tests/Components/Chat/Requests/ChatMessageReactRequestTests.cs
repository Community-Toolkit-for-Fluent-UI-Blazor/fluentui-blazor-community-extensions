using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageReactRequestTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var roomId = 123L;
        var owner = new ChatUser { Id = 1, DisplayName = "Alice" };
        var messageMock = new Mock<IChatMessage>();
        var reaction = "👍";

        // Act
        var request = new ChatMessageReactRequest(roomId, owner, messageMock.Object, reaction);

        // Assert
        Assert.Equal(roomId, request.RoomId);
        Assert.Equal(owner, request.Owner);
        Assert.Equal(messageMock.Object, request.Message);
        Assert.Equal(reaction, request.Reaction);
    }

    [Fact]
    public void Records_WithSameValues_AreEqual()
    {
        // Arrange
        var roomId = 123L;
        var owner = new ChatUser { Id = 1, DisplayName = "Alice" };
        var messageMock = new Mock<IChatMessage>();
        var reaction = "👍";

        var request1 = new ChatMessageReactRequest(roomId, owner, messageMock.Object, reaction);
        var request2 = new ChatMessageReactRequest(roomId, owner, messageMock.Object, reaction);

        // Assert
        Assert.Equal(request1, request2);
        Assert.True(request1 == request2);
        Assert.False(request1 != request2);
    }

    [Fact]
    public void Records_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var owner = new ChatUser { Id = 1, DisplayName = "Alice" };
        var messageMock = new Mock<IChatMessage>();

        var request1 = new ChatMessageReactRequest(1, owner, messageMock.Object, "👍");
        var request2 = new ChatMessageReactRequest(2, owner, messageMock.Object, "👎");

        // Assert
        Assert.NotEqual(request1, request2);
        Assert.False(request1 == request2);
        Assert.True(request1 != request2);
    }
}
