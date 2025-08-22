using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageReactionTests
{
    [Fact]
    public void Can_Set_And_Get_Id()
    {
        var reaction = new ChatMessageReaction();
        reaction.Id = 42;
        Assert.Equal(42, reaction.Id);
    }

    [Fact]
    public void Can_Set_And_Get_MessageId()
    {
        var reaction = new ChatMessageReaction();
        reaction.MessageId = 99;
        Assert.Equal(99, reaction.MessageId);
    }

    [Fact]
    public void Can_Set_And_Get_UserReactedBy()
    {
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var reaction = new ChatMessageReaction();
        reaction.UserReactedBy = user;
        Assert.Equal(user, reaction.UserReactedBy);
    }

    [Fact]
    public void Can_Set_And_Get_Emoji()
    {
        var reaction = new ChatMessageReaction();
        reaction.Emoji = "👍";
        Assert.Equal("👍", reaction.Emoji);
    }
}
