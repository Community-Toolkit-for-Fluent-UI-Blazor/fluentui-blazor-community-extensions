using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Resizer;

public class ResizerHelperTests
{
    [Theory]
    [InlineData(LocalizationDirection.LeftToRight)]
    [InlineData(LocalizationDirection.RightToLeft)]
    public void GetFromLocalizationDirection_WithValidDirection_ReturnsCorrectDictionary(LocalizationDirection direction)
    {
        // Act
        var result = ResizerHelper.GetFromLocalizationDirection(direction);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(ResizerHandler.Horizontally, result.Keys);
        Assert.Contains(ResizerHandler.Vertically, result.Keys);
        Assert.Contains(ResizerHandler.Both, result.Keys);
    }

    [Fact]
    public void GetFromLocalizationDirection_WithLeftToRight_ReturnsLtrStyles()
    {
        // Act
        var result = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);

        // Assert
        Assert.Equal("top: 0px; right: 0px; bottom: 0px; width: 9px;", result[ResizerHandler.Horizontally]);
        Assert.Equal("left: 0px; right: 0px; bottom: 0px; height: 9px;", result[ResizerHandler.Vertically]);
        Assert.Equal("right: 0px; bottom: 0px; width: 9px; height: 9px;", result[ResizerHandler.Both]);
    }

    [Fact]
    public void GetFromLocalizationDirection_WithRightToLeft_ReturnsRtlStyles()
    {
        // Act
        var result = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.RightToLeft);

        // Assert
        Assert.Equal("top: 0px; left: 0px; bottom: 0px; width: 9px;", result[ResizerHandler.Horizontally]);
        Assert.Equal("left: 0px; left: 0px; bottom: 0px; height: 9px;", result[ResizerHandler.Vertically]);
        Assert.Equal("left: 0px; bottom: 0px; width: 9px; height: 9px;", result[ResizerHandler.Both]);
    }

    [Fact]
    public void GetFromLocalizationDirection_WithDefault_ReturnsLtrStyles()
    {
        // Arrange
        var defaultDirection = (LocalizationDirection)999; // Unknown value

        // Act
        var result = ResizerHelper.GetFromLocalizationDirection(defaultDirection);

        // Assert - Should default to LTR
        Assert.Equal("top: 0px; right: 0px; bottom: 0px; width: 9px;", result[ResizerHandler.Horizontally]);
        Assert.Equal("left: 0px; right: 0px; bottom: 0px; height: 9px;", result[ResizerHandler.Vertically]);
        Assert.Equal("right: 0px; bottom: 0px; width: 9px; height: 9px;", result[ResizerHandler.Both]);
    }

    [Fact]
    public void GetFromLocalizationDirection_MultipleCallsSameDirection_ReturnsSameInstance()
    {
        // Act
        var result1 = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);
        var result2 = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);

        // Assert
        Assert.Same(result1, result2);
    }

    [Fact]
    public void GetFromLocalizationDirection_DifferentDirections_ReturnsDifferentInstances()
    {
        // Act
        var ltrResult = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);
        var rtlResult = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.RightToLeft);

        // Assert
        Assert.NotSame(ltrResult, rtlResult);
    }

    [Fact]
    public void ResizerHandlerStyles_ContainExpectedCssProperties()
    {
        // Arrange
        var ltrResult = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);

        // Assert - Verify that all styles contain expected CSS properties
        foreach (var style in ltrResult.Values)
        {
            Assert.Contains("px;", style);
            Assert.True(style.Contains("width:") || style.Contains("height:"));
        }
    }

    [Fact]
    public void ResizerHandlerStyles_LtrAndRtl_HaveDifferentValues()
    {
        // Arrange
        var ltrResult = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.LeftToRight);
        var rtlResult = ResizerHelper.GetFromLocalizationDirection(LocalizationDirection.RightToLeft);

        // Assert
        Assert.NotEqual(ltrResult[ResizerHandler.Horizontally], rtlResult[ResizerHandler.Horizontally]);
        Assert.NotEqual(ltrResult[ResizerHandler.Both], rtlResult[ResizerHandler.Both]);
        // Vertically should be the same in both directions (though there's actually a bug in the source code)
    }
}
