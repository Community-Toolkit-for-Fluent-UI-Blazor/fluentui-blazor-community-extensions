using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageEditRequestTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var roomId = 42L;
        var messageMock = new Mock<IChatMessage>();
        var owner = new ChatUser { Id = 1, DisplayName = "Test" };
        var text = "Nouveau texte";

        // Act
        var request = new ChatMessageEditRequest(roomId, messageMock.Object, owner, text);

        // Assert
        Assert.Equal(roomId, request.RoomId);
        Assert.Equal(messageMock.Object, request.Message);
        Assert.Equal(owner, request.Owner);
        Assert.Equal(text, request.Text);
    }

    [Fact]
    public void Equality_TwoIdenticalRequests_AreEqual()
    {
        // Arrange
        var roomId = 1L;
        var messageMock = new Mock<IChatMessage>();
        var owner = new ChatUser { Id = 2, DisplayName = "User" };
        var text = "Message";

        var req1 = new ChatMessageEditRequest(roomId, messageMock.Object, owner, text);
        var req2 = new ChatMessageEditRequest(roomId, messageMock.Object, owner, text);

        // Assert
        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
        Assert.Equal(req1.GetHashCode(), req2.GetHashCode());
    }

    [Fact]
    public void Deconstruct_ReturnsAllProperties()
    {
        // Arrange
        var roomId = 7L;
        var messageMock = new Mock<IChatMessage>();
        var owner = new ChatUser { Id = 3, DisplayName = "Démo" };
        var text = "Déconstruction";

        var request = new ChatMessageEditRequest(roomId, messageMock.Object, owner, text);

        // Act
        var (actualRoomId, actualMessage, actualOwner, actualText) = request;

        // Assert
        Assert.Equal(roomId, actualRoomId);
        Assert.Equal(messageMock.Object, actualMessage);
        Assert.Equal(owner, actualOwner);
        Assert.Equal(text, actualText);
    }
}
