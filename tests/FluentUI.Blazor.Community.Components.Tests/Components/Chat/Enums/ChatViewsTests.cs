namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatViewsTests
{
    [Fact]
    public void ChatViews_None_ShouldBeZero()
    {
        Assert.Equal(0, (int)ChatViews.None);
    }

    [Fact]
    public void ChatViews_Messages_ShouldBeOne()
    {
        Assert.Equal(1, (int)ChatViews.Messages);
    }

    [Fact]
    public void ChatViews_PinnedMessages_ShouldBeTwo()
    {
        Assert.Equal(2, (int)ChatViews.PinnedMessages);
    }

    [Fact]
    public void ChatViews_Images_ShouldBeFour()
    {
        Assert.Equal(4, (int)ChatViews.Images);
    }

    [Fact]
    public void ChatViews_Video_ShouldBeEight()
    {
        Assert.Equal(8, (int)ChatViews.Video);
    }

    [Fact]
    public void ChatViews_Audio_ShouldBeSixteen()
    {
        Assert.Equal(16, (int)ChatViews.Audio);
    }

    [Fact]
    public void ChatViews_Other_ShouldBeThirtyTwo()
    {
        Assert.Equal(32, (int)ChatViews.Other);
    }

    [Fact]
    public void ChatViews_Flags_CombineMultipleViews()
    {
        var combined = ChatViews.Messages | ChatViews.Images | ChatViews.Audio;
        Assert.True(combined.HasFlag(ChatViews.Messages));
        Assert.True(combined.HasFlag(ChatViews.Images));
        Assert.True(combined.HasFlag(ChatViews.Audio));
        Assert.False(combined.HasFlag(ChatViews.PinnedMessages));
    }

    [Fact]
    public void ChatViews_ToString_ShouldReturnCorrectString()
    {
        var combined = ChatViews.Messages | ChatViews.Images;
        Assert.Equal("Messages, Images", combined.ToString());
    }
}
