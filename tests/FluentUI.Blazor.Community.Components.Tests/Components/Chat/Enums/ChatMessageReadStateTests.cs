namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageReadStateTestsTests
{
    [Fact]
    public void Enum_Should_Have_Expected_Values()
    {
        Assert.Equal(0, (int)ChatMessageReadState.Unread);
        Assert.Equal(1, (int)ChatMessageReadState.Read);
        Assert.Equal(2, (int)ChatMessageReadState.ReadByEveryone);
    }

    [Theory]
    [InlineData(ChatMessageReadState.Unread, "Unread")]
    [InlineData(ChatMessageReadState.Read, "Read")]
    [InlineData(ChatMessageReadState.ReadByEveryone, "ReadByEveryone")]
    public void Enum_ToString_Should_Return_Expected_Name(ChatMessageReadState state, string expected)
    {
        Assert.Equal(expected, state.ToString());
    }

    [Theory]
    [InlineData(0, ChatMessageReadState.Unread)]
    [InlineData(1, ChatMessageReadState.Read)]
    [InlineData(2, ChatMessageReadState.ReadByEveryone)]
    public void Enum_Cast_From_Int_Should_Return_Expected_Value(int value, ChatMessageReadState expected)
    {
        Assert.Equal(expected, (ChatMessageReadState)value);
    }
}
