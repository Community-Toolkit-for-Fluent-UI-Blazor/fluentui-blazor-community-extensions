namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageReadStateTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)ChatMessageReadState.Unread);
        Assert.Equal(1, (int)ChatMessageReadState.Read);
        Assert.Equal(2, (int)ChatMessageReadState.ReadByEveryone);
    }

    [Theory]
    [InlineData(ChatMessageReadState.Unread, "Unread")]
    [InlineData(ChatMessageReadState.Read, "Read")]
    [InlineData(ChatMessageReadState.ReadByEveryone, "ReadByEveryone")]
    public void Enum_ToString_Returns_Expected(ChatMessageReadState option, string expected)
    {
        Assert.Equal(expected, option.ToString());
    }
}
