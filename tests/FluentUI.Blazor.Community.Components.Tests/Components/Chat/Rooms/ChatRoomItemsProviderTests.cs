using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomItemsProviderTests
{
    [Fact]
    public async Task ChatRoomItemsProvider_ReturnsExpectedRooms()
    {
        // Arrange
        var expectedRooms = new List<ChatRoom> { new ChatRoom { Id = 1, Name = "Test" } };
        ChatRoomItemsProvider provider = (req, ct) => new ValueTask<IEnumerable<ChatRoom>>(expectedRooms);

        // Act
        var result = await provider(new ChatRoomItemsRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test", result.First().Name);
    }
}
