using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Helpers;

public class CssColorNameUtilsTests
{
    [Theory]
    [InlineData("red", "#FF0000")]
    [InlineData("RED", "#FF0000")]
    [InlineData("ReBeccapurple", "#663399")]
    [InlineData("blue", "#0000FF")]
    [InlineData("BlAcK", "#000000")]
    public void TryGetHex_ValidColorNames_ReturnsTrueAndCorrectHex(string colorName, string expectedHex)
    {
        // Act
        var result = CssColorNamesUtils.TryGetHex(colorName, out var hex);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedHex, hex);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryGetHex_NullOrEmptyName_ReturnsFalseAndDefaultHex(string? colorName)
    {
        // Act
        var result = CssColorNamesUtils.TryGetHex(colorName, out var hex);

        // Assert
        Assert.False(result);
        Assert.Equal("#000000", hex);
    }

    [Theory]
    [InlineData("notacolor")]
    [InlineData("123456")]
    [InlineData("rouge")]
    public void TryGetHex_InvalidColorNames_ReturnsFalseAndNull(string colorName)
    {
        // Act
        var result = CssColorNamesUtils.TryGetHex(colorName, out var hex);

        // Assert
        Assert.False(result);
        Assert.Null(hex);
    }
}
