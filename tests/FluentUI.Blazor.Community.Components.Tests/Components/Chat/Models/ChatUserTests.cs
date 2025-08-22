using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatUserTests
{
    [Fact]
    public void Equals_WithSameId_ReturnsTrue()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 1 };

        Assert.True(user1.Equals(user2));
    }

    [Fact]
    public void Equals_WithDifferentId_ReturnsFalse()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };

        Assert.False(user1.Equals(user2));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var user = new ChatUser { Id = 1 };

        Assert.False(user.Equals((ChatUser?)null));
    }

    [Fact]
    public void ObjectEquals_WithSameId_ReturnsTrue()
    {
        var user1 = new ChatUser { Id = 1 };
        object user2 = new ChatUser { Id = 1 };

        Assert.True(user1.Equals(user2));
    }

    [Fact]
    public void ObjectEquals_WithDifferentType_ReturnsFalse()
    {
        var user = new ChatUser { Id = 1 };
        var notAUser = new object();

        Assert.False(user.Equals(notAUser));
    }

    [Fact]
    public void GetHashCode_ReturnsIdHashCode()
    {
        var user = new ChatUser { Id = 123 };

        Assert.Equal(123.GetHashCode(), user.GetHashCode());
    }

    [Fact]
    public void OperatorEquals_WithSameId_ReturnsTrue()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 1 };

        Assert.True(user1 == user2);
    }

    [Fact]
    public void OperatorEquals_WithDifferentId_ReturnsFalse()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };

        Assert.False(user1 == user2);
    }

    [Fact]
    public void OperatorEquals_WithNull_ReturnsFalse()
    {
        ChatUser? user1 = null;
        ChatUser? user2 = null;
        var user3 = new ChatUser { Id = 1 };

        Assert.False(user1 == user2);
        Assert.False(user1 == user3);
        Assert.False(user3 == user1);
    }

    [Fact]
    public void OperatorNotEquals_WithSameId_ReturnsFalse()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 1 };

        Assert.False(user1 != user2);
    }

    [Fact]
    public void OperatorNotEquals_WithDifferentId_ReturnsTrue()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };

        Assert.True(user1 != user2);
    }

    [Fact]
    public void OperatorNotEquals_WithNull_ReturnsTrue()
    {
        ChatUser? user1 = null;
        ChatUser? user2 = null;
        var user3 = new ChatUser { Id = 1 };

        Assert.True(user1 != user2);
        Assert.True(user1 != user3);
        Assert.True(user3 != user1);
    }

    [Fact]
    public void Properties_AreSetAndGetCorrectly()
    {
        var user = new ChatUser
        {
            Id = 42,
            Avatar = "avatar.png",
            Initials = "JD",
            DisplayName = "John Doe",
            CultureId = 1036,
            CultureName = "fr-FR",
            Roles = new List<string> { "Admin", "User" }
        };

        Assert.Equal(42, user.Id);
        Assert.Equal("avatar.png", user.Avatar);
        Assert.Equal("JD", user.Initials);
        Assert.Equal("John Doe", user.DisplayName);
        Assert.Equal(1036, user.CultureId);
        Assert.Equal("fr-FR", user.CultureName);
        Assert.Equal(new List<string> { "Admin", "User" }, user.Roles);
    }
}
