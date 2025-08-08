namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageReactionTests
{
    [Fact]
    public void Properties_AreSetCorrectly()
    {
        var user = new ChatUser
        {
            Id = 42,
            Avatar = "avatar.png",
            Initials = "JD",
            DisplayName = "John Doe",
            CultureId = 1,
            CultureName = "fr-FR",
            Roles = new List<string> { "user" }
        };

        var reaction = new ChatMessageReaction
        {
            Id = 100,
            MessageId = 200,
            UserReactedBy = user,
            Emoji = "😀"
        };

        Assert.Equal(100, reaction.Id);
        Assert.Equal(200, reaction.MessageId);
        Assert.Equal(user, reaction.UserReactedBy);
        Assert.Equal("😀", reaction.Emoji);
    }

    [Fact]
    public void Properties_CanBeNullOrDefault()
    {
        var reaction = new ChatMessageReaction
        {
            Id = 0,
            MessageId = 0,
            UserReactedBy = null!,
            Emoji = null
        };

        Assert.Equal(0, reaction.Id);
        Assert.Equal(0, reaction.MessageId);
        Assert.Null(reaction.UserReactedBy);
        Assert.Null(reaction.Emoji);
    }

    [Fact]
    public void Implements_IChatMessageReaction()
    {
        var reaction = new ChatMessageReaction();
        Assert.IsAssignableFrom<IChatMessageReaction>(reaction);
    }
}
