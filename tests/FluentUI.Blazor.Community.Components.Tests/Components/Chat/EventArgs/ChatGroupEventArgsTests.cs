using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatGroupEventArgsTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var user1 = new ChatUser { Id = 1, DisplayName = "User One" };
        var user2 = new ChatUser { Id = 2, DisplayName = "User Two" };

        var args = new ChatGroupEventArgs(
       [
            user1,
            user2
       ]);

        Assert.Equal(user1, args.Users.ElementAt(0));
        Assert.Equal(user2, args.Users.ElementAt(1));
    }

    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var user1 = new ChatUser { Id = 1, DisplayName = "User One" };
        var user2 = new ChatUser { Id = 2, DisplayName = "User Two" };

        // Act
        var args = new ChatGroupEventArgs(
      [
           user1,
            user2
      ]);

        args.ChatGroupIdReturnValue = 12345;

        // Assert
        Assert.Equal(12345, args.ChatGroupIdReturnValue);
    }
}
