namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageSplitOptionTestsTests
{
    [Fact]
    public void Enum_Should_Have_None_And_Split_Values()
    {
        Assert.Equal(0, (int)ChatMessageSplitOption.None);
        Assert.Equal(1, (int)ChatMessageSplitOption.Split);
    }

    [Theory]
    [InlineData(ChatMessageSplitOption.None, "None")]
    [InlineData(ChatMessageSplitOption.Split, "Split")]
    public void Enum_ToString_Should_Return_Correct_Name(ChatMessageSplitOption option, string expected)
    {
        Assert.Equal(expected, option.ToString());
    }

    [Theory]
    [InlineData(0, ChatMessageSplitOption.None)]
    [InlineData(1, ChatMessageSplitOption.Split)]
    public void Enum_Cast_From_Int_Should_Return_Correct_Enum(int value, ChatMessageSplitOption expected)
    {
        Assert.Equal(expected, (ChatMessageSplitOption)value);
    }
}
