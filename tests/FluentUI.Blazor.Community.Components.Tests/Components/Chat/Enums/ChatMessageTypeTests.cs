namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageTypeTests_UnitTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)ChatMessageType.None);
        Assert.Equal(1, (int)ChatMessageType.Text);
        Assert.Equal(2, (int)ChatMessageType.Document);
        Assert.Equal(4, (int)ChatMessageType.Gift);
    }

    [Fact]
    public void Flags_Combine_Correctly()
    {
        var combined = ChatMessageType.Text | ChatMessageType.Document;
        Assert.True(combined.HasFlag(ChatMessageType.Text));
        Assert.True(combined.HasFlag(ChatMessageType.Document));
        Assert.False(combined.HasFlag(ChatMessageType.Gift));
        Assert.Equal(3, (int)combined);
    }

    [Fact]
    public void Flags_Remove_Correctly()
    {
        var combined = ChatMessageType.Text | ChatMessageType.Document | ChatMessageType.Gift;
        var removed = combined & ~ChatMessageType.Document;
        Assert.True(removed.HasFlag(ChatMessageType.Text));
        Assert.False(removed.HasFlag(ChatMessageType.Document));
        Assert.True(removed.HasFlag(ChatMessageType.Gift));
        Assert.Equal(ChatMessageType.Text | ChatMessageType.Gift, removed);
    }

    [Theory]
    [InlineData("None", ChatMessageType.None)]
    [InlineData("Text", ChatMessageType.Text)]
    [InlineData("Document", ChatMessageType.Document)]
    [InlineData("Gift", ChatMessageType.Gift)]
    [InlineData("Text, Document", ChatMessageType.Text | ChatMessageType.Document)]
    public void Parse_String_To_Enum(string input, ChatMessageType expected)
    {
        var parsed = (ChatMessageType)Enum.Parse(typeof(ChatMessageType), input);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(ChatMessageType.None, "None")]
    [InlineData(ChatMessageType.Text, "Text")]
    [InlineData(ChatMessageType.Document, "Document")]
    [InlineData(ChatMessageType.Gift, "Gift")]
    [InlineData(ChatMessageType.Text | ChatMessageType.Document, "Text, Document")]
    public void ToString_Returns_Expected(ChatMessageType value, string expected)
    {
        Assert.Equal(expected, value.ToString());
    }
}
