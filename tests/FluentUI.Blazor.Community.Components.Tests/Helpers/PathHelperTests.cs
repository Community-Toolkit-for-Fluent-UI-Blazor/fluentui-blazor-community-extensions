using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Helpers;

public class PathHelperTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetSegments_ReturnsEmptyArray_WhenPathIsNullOrWhitespace(string? path)
    {
        // Act
        var result = PathHelper.GetSegments(path);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetSegments_ReturnsSingleSegment_WhenNoSeparator()
    {
        // Arrange
        var path = "folder";

        // Act
        var result = PathHelper.GetSegments(path);

        // Assert
        Assert.Single(result);
        Assert.Equal("folder", result[0]);
    }

    [Fact]
    public void GetSegments_TrimsAndSplitsPath_Correctly()
    {
        // Arrange
        var sep = Path.DirectorySeparatorChar;
        var path = $"  folder1{sep}folder2{sep}file.txt  ";

        // Act
        var result = PathHelper.GetSegments(path);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal("folder1", result[0]);
        Assert.Equal("folder2", result[1]);
        Assert.Equal("file.txt", result[2]);
    }

    [Fact]
    public void GetSegments_RemovesEmptySegments()
    {
        // Arrange
        var sep = Path.DirectorySeparatorChar;
        var path = $"{sep}folder1{sep}{sep}folder2{sep}";

        // Act
        var result = PathHelper.GetSegments(path);

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Equal("folder1", result[0]);
        Assert.Equal("folder2", result[1]);
    }
}
