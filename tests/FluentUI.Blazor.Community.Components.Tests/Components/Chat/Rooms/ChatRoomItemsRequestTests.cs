using System.Linq.Expressions;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomItemsRequestTests
{
    [Fact]
    public void Constructor_DefaultFilter_IsNull()
    {
        // Act
        var request = new ChatRoomItemsRequest();

        // Assert
        Assert.Null(request.Filter);
    }

    [Fact]
    public void Constructor_WithFilter_SetsFilter()
    {
        // Arrange
        Expression<Func<ChatRoom, bool>> filter = room => room.IsBlocked;

        // Act
        var request = new ChatRoomItemsRequest(filter);

        // Assert
        Assert.Equal(filter, request.Filter);
    }
}
