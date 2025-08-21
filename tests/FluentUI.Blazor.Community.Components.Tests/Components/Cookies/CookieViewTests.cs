namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieViewTests
{
    [Fact]
    public void Enum_Has_Expected_Values()
    {
        Assert.Equal(0, (int)CookieView.Default);
        Assert.Equal(1, (int)CookieView.Small);
    }

    [Theory]
    [InlineData(CookieView.Default, "Default")]
    [InlineData(CookieView.Small, "Small")]
    public void Enum_ToString_Returns_Expected(CookieView option, string expected)
    {
        Assert.Equal(expected, option.ToString());
    }
}
