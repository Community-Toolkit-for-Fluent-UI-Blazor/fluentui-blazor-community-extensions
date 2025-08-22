using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageCountRequestTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        long roomId = 1;
        long ownerId = 2;
        Expression<Func<IChatMessage, bool>> filter = m => m.Id > 0;

        // Act
        var request = new ChatMessageCountRequest(roomId, ownerId, filter);

        // Assert
        Assert.Equal(roomId, request.RoomId);
        Assert.Equal(ownerId, request.OwnerId);
        Assert.Equal(filter, request.Filter);
    }

    [Fact]
    public void Equality_WorksForIdenticalValues()
    {
        // Arrange
        var filter = (Expression<Func<IChatMessage, bool>>)(m => m.Id > 0);
        var req1 = new ChatMessageCountRequest(1, 2, filter);
        var req2 = new ChatMessageCountRequest(1, 2, filter);

        // Act & Assert
        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
    }

    [Fact]
    public void Inequality_WorksForDifferentValues()
    {
        // Arrange
        var filter = (Expression<Func<IChatMessage, bool>>)(m => m.Id > 0);
        var req1 = new ChatMessageCountRequest(1, 2, filter);
        var req2 = new ChatMessageCountRequest(1, 3, filter);

        // Act & Assert
        Assert.NotEqual(req1, req2);
        Assert.True(req1 != req2);
    }
}
