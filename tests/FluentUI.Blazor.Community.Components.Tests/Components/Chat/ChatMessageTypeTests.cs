namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageTypeTests
{
    [Fact]
    public void ChatMessageType_DefaultValueIsNone()
    {
        var defaultType = ChatMessageType.None;
        Assert.Equal(0, (int)defaultType);
    }

    [Fact]
    public void ChatMessageType_FlagAttributeApplied()
    {
        var flagAttribute = typeof(ChatMessageType).GetCustomAttributes(typeof(FlagsAttribute), false);
        Assert.True(flagAttribute.Length > 0, "Enum should have Flags attribute");
    }

    [Fact]
    public void ChatMessageType_AllowMultipleFlagCombinations()
    {
        var combinedType = ChatMessageType.Text | ChatMessageType.Document;
        Assert.Equal(3, (int)combinedType);
    }

    [Fact]
    public void ChatMessageType_IndividualFlagValues()
    {
        Assert.Equal(0, (int)ChatMessageType.None);
        Assert.Equal(1, (int)ChatMessageType.Text);
        Assert.Equal(2, (int)ChatMessageType.Document);
        Assert.Equal(4, (int)ChatMessageType.Gift);
    }

    [Fact]
    public void ChatMessageType_HasFlagsCheck()
    {
        var combinedType = ChatMessageType.Text | ChatMessageType.Document;
        Assert.True(combinedType.HasFlag(ChatMessageType.Text), "Should support HasFlag check");
        Assert.True(combinedType.HasFlag(ChatMessageType.Document), "Should support HasFlag check");
    }

    [Fact]
    public void ChatMessageType_EnumValuesUnique()
    {
        var values = Enum.GetValues<ChatMessageType>();
        var uniqueValues = new HashSet<int>();

        foreach (var value in values)
        {
            var intValue = (int)value;
            Assert.False(uniqueValues.Contains(intValue), $"Value {intValue} should be unique");
            uniqueValues.Add(intValue);
        }
    }

    [Fact]
    public void ChatMessageType_ParseValidString()
    {
        var parsed = Enum.Parse<ChatMessageType>("Text");
        Assert.Equal(ChatMessageType.Text, parsed);
    }

    [Fact]
    public void ChatMessageType_TryParseValidString()
    {
        var success = Enum.TryParse("Document", out ChatMessageType result);
        Assert.True(success, "TryParse should succeed for valid enum name");
        Assert.Equal(ChatMessageType.Document, result);
    }
}
