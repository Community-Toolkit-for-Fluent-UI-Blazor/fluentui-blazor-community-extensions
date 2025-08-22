using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageItemRequestTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        long roomId = 1;
        long ownerId = 2;
        int startIndex = 10;
        int count = 5;
        Expression<Func<IChatMessage, bool>>? filter = m => m.Id > 100;

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
    public void Equality_TwoIdenticalRequests_AreEqual()
    {
        // Arrange
        var filter = (Expression<Func<IChatMessage, bool>>)(m => m.IsPinned);
        var req1 = new ChatMessageItemRequest(1, 2, 0, 10, filter);
        var req2 = new ChatMessageItemRequest(1, 2, 0, 10, filter);

        // Act & Assert
        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
        Assert.False(req1 != req2);
        Assert.Equal(req1.GetHashCode(), req2.GetHashCode());
    }

    [Fact]
    public void Filter_CanBeNull()
    {
        // Act
        var request = new ChatMessageItemRequest(1, 2, 0, 10, null);

        // Assert
        Assert.Null(request.Filter);
    }
}
