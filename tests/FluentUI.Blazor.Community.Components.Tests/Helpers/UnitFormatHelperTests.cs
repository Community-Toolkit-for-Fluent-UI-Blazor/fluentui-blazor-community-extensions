using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Helpers;

public class UnitFormatHelperTests
{
    [Theory]
    [InlineData(null)]
    public void Format_ReturnsEmptyString_WhenValueIsNull(object value)
    {
        // Act
        var result = UnitFormatHelper.Format(value);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Format_ReturnsEmptyString_WhenValueIsEmptyOrWhitespace(string value)
    {
        // Act
        var result = UnitFormatHelper.Format(value);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("auto")]
    [InlineData("100%")]
    [InlineData("10px")]
    [InlineData("2em")]
    [InlineData("5vw")]
    [InlineData("1.5rem")]
    [InlineData("3in")]
    [InlineData("4cm")]
    [InlineData("2fr")]
    [InlineData("7mm")]
    [InlineData("8pt")]
    [InlineData("9pc")]
    [InlineData("1ch")]
    [InlineData("2ex")]
    [InlineData("100ms")]
    [InlineData("45deg")]
    [InlineData("2s")]
    [InlineData("1rad")]
    [InlineData("1grad")]
    [InlineData("10vmin")]
    [InlineData("10vmax")]
    public void Format_ReturnsValue_WhenValueMatchesPattern(string value)
    {
        // Act
        var result = UnitFormatHelper.Format(value);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(42, "42px")]
    [InlineData("3.14", "3.14px")]
    [InlineData("123", "123px")]
    [InlineData("tewt", "tewtpx")]
    [InlineData("pkx", "pkxpx")]
    [InlineData("100percent", "100percentpx")]
    public void Format_AppendsPx_WhenValueDoesNotMatchPattern(object value, string expected)
    {
        // Act
        var result = UnitFormatHelper.Format(value);

        // Assert
        Assert.Equal(expected, result);
    }
}
