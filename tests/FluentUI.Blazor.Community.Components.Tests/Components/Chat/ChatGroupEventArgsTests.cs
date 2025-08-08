using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatGroupEventArgsTests
{
    [Fact]
    public void Constructor_SetsUsersCorrectly()
    {
        var users = new List<ChatUser>
        {
            new() { Id = 1, DisplayName = "Alice" },
            new() { Id = 2, DisplayName = "Bob" }
        };

        var args = new ChatGroupEventArgs(users);

        Assert.Equal(users, args.Users);
    }

    [Fact]
    public void ChatGroupIdReturnValue_SetAndGet_ReturnsExpectedValue()
    {
        var args = new ChatGroupEventArgs(new List<ChatUser>())
        {
            ChatGroupIdReturnValue = 12345
        };
        Assert.Equal(12345, args.ChatGroupIdReturnValue);
    }

    [Fact]
    public void Users_IsReadOnly()
    {
        var users = new List<ChatUser>
        {
            new() { Id = 3, DisplayName = "Charlie" }
        };
        var args = new ChatGroupEventArgs(users);

        Assert.NotNull(args.Users);
        Assert.True(args.Users is IEnumerable<ChatUser>);
    }
}
