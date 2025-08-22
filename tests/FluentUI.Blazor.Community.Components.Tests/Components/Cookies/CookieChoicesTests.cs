namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieChoicesTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)CookieChoices.AcceptOnly);
        Assert.Equal(1, (int)CookieChoices.AcceptDeny);
        Assert.Equal(2, (int)CookieChoices.AcceptDenyManage);
    }

    [Theory]
    [InlineData(CookieChoices.AcceptOnly, "AcceptOnly")]
    [InlineData(CookieChoices.AcceptDeny, "AcceptDeny")]
    [InlineData(CookieChoices.AcceptDenyManage, "AcceptDenyManage")]
    public void Enum_ToString_Returns_Expected(CookieChoices option, string expected)
    {
        Assert.Equal(expected, option.ToString());
    }
}
