using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class PathHelperTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetSegments_NullOrWhiteSpace_ReturnsEmpty(string? path)
    {
        var result = PathHelper.GetSegments(path);
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetSegments_SingleSegment()
    {
        var result = PathHelper.GetSegments("folder");
        Assert.Single(result);
        Assert.Equal("folder", result[0]);
    }

    [Fact]
    public void GetSegments_MultipleSegments()
    {
        var sep = Path.DirectorySeparatorChar;
        var path = $"folder{sep}subfolder{sep}file.txt";
        var result = PathHelper.GetSegments(path);
        Assert.Equal(3, result.Length);
        Assert.Equal("folder", result[0]);
        Assert.Equal("subfolder", result[1]);
        Assert.Equal("file.txt", result[2]);
    }

    [Fact]
    public void GetSegments_LeadingAndTrailingSeparators()
    {
        var sep = Path.DirectorySeparatorChar;
        var path = $"{sep}folder{sep}subfolder{sep}";
        var result = PathHelper.GetSegments(path);
        Assert.Equal(2, result.Length);
        Assert.Equal("folder", result[0]);
        Assert.Equal("subfolder", result[1]);
    }

    [Fact]
    public void GetSegments_MultipleConsecutiveSeparators()
    {
        var sep = Path.DirectorySeparatorChar;
        var path = $"folder{sep}{sep}subfolder{sep}{sep}file.txt";
        var result = PathHelper.GetSegments(path);
        Assert.Equal(3, result.Length);
        Assert.Equal("folder", result[0]);
        Assert.Equal("subfolder", result[1]);
        Assert.Equal("file.txt", result[2]);
    }
}
