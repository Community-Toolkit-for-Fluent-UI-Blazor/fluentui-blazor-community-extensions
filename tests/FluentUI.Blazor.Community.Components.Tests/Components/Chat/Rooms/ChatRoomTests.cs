using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomTests
{
    [Fact]
    public void GetUsersBut_ReturnsAllUsers_WhenUserIsNull()
    {
        // Arrange
        var user1 = new ChatUser { Id = 1, DisplayName = "Alice" };
        var user2 = new ChatUser { Id = 2, DisplayName = "Bob" };
        var chatRoom = new ChatRoom
        {
            Users = new List<ChatUser> { user1, user2 }
        };

        // Act
        var result = chatRoom.GetUsersBut(null);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(user1, result);
        Assert.Contains(user2, result);
    }

    [Fact]
    public void GetUsersBut_ExcludesSpecifiedUser_WhenUserIsPresent()
    {
        // Arrange
        var user1 = new ChatUser { Id = 1, DisplayName = "Alice" };
        var user2 = new ChatUser { Id = 2, DisplayName = "Bob" };
        var chatRoom = new ChatRoom
        {
            Users = new List<ChatUser> { user1, user2 }
        };

        // Act
        var result = chatRoom.GetUsersBut(user1);

        // Assert
        Assert.Single(result);
        Assert.DoesNotContain(user1, result);
        Assert.Contains(user2, result);
    }

    [Fact]
    public void GetUsersBut_ReturnsAllUsers_WhenUserIsNotInList()
    {
        // Arrange
        var user1 = new ChatUser { Id = 1, DisplayName = "Alice" };
        var user2 = new ChatUser { Id = 2, DisplayName = "Bob" };
        var user3 = new ChatUser { Id = 3, DisplayName = "Charlie" };
        var chatRoom = new ChatRoom
        {
            Users = new List<ChatUser> { user1, user2 }
        };

        // Act
        var result = chatRoom.GetUsersBut(user3);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(user1, result);
        Assert.Contains(user2, result);
    }

    [Fact]
    public void GetUsersBut_ReturnsEmpty_WhenNoUsers()
    {
        // Arrange
        var chatRoom = new ChatRoom
        {
            Users = new List<ChatUser>()
        };

        // Act
        var result = chatRoom.GetUsersBut(null);

        // Assert
        Assert.Empty(result);
    }
}
