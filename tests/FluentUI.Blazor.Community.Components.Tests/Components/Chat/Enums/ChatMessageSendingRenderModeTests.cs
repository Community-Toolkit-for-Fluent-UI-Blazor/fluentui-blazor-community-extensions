namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageSendingRenderModeTests_UnitTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)ChatMessageSendingRenderMode.Overlay);
        Assert.Equal(1, (int)ChatMessageSendingRenderMode.Inline);
    }

    [Theory]
    [InlineData("Overlay", ChatMessageSendingRenderMode.Overlay)]
    [InlineData("Inline", ChatMessageSendingRenderMode.Inline)]
    public void Parse_String_To_Enum(string input, ChatMessageSendingRenderMode expected)
    {
        var parsed = Enum.Parse<ChatMessageSendingRenderMode>(input);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(ChatMessageSendingRenderMode.Overlay, "Overlay")]
    [InlineData(ChatMessageSendingRenderMode.Inline, "Inline")]
    public void ToString_Returns_Expected(ChatMessageSendingRenderMode value, string expected)
    {
        Assert.Equal(expected, value.ToString());
    }
}
