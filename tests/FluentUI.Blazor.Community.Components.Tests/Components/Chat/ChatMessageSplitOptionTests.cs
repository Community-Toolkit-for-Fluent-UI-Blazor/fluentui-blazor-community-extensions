namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageSplitOptionTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)ChatMessageSplitOption.None);
        Assert.Equal(1, (int)ChatMessageSplitOption.Split);
    }

    [Theory]
    [InlineData(ChatMessageSplitOption.None, "None")]
    [InlineData(ChatMessageSplitOption.Split, "Split")]
    public void Enum_ToString_Returns_Expected(ChatMessageSplitOption option, string expected)
    {
        Assert.Equal(expected, option.ToString());
    }
}
