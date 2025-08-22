namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatFileRenderingModeTests
{
    [Fact]
    public void Enum_Has_Discrete_And_Viewer_Members()
    {
        Assert.Equal(0, (int)ChatFileRenderingMode.Discrete);
        Assert.Equal(1, (int)ChatFileRenderingMode.Viewer);
    }

    [Theory]
    [InlineData("Discrete", ChatFileRenderingMode.Discrete)]
    [InlineData("Viewer", ChatFileRenderingMode.Viewer)]
    public void Parse_String_To_Enum(string name, ChatFileRenderingMode expected)
    {
        var parsed = Enum.Parse<ChatFileRenderingMode>(name);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(ChatFileRenderingMode.Discrete, "Discrete")]
    [InlineData(ChatFileRenderingMode.Viewer, "Viewer")]
    public void ToString_Returns_Expected_Name(ChatFileRenderingMode mode, string expected)
    {
        Assert.Equal(expected, mode.ToString());
    }
}
