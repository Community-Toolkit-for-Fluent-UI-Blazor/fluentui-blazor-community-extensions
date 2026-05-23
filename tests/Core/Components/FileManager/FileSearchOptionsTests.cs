using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileSearchOptionsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var options = new FileSearchOptions();

        Assert.True(options.SearchInNames);
        Assert.True(options.Recursive);
        Assert.Equal(2, options.FuzzyThreshold);
        Assert.False(options.SearchInContent);
        Assert.False(options.SearchInMetadata);
        Assert.False(options.Fuzzy);
    }

    [Fact]
    public void Clone_CopiesValues()
    {
        var options = new FileSearchOptions
        {
            SearchInContent = true,
            SearchInMetadata = true,
            Fuzzy = true,
            FuzzyThreshold = 5,
            Extensions = ["txt", "json"],
            CreatedAfter = new DateTime(2024, 1, 1),
            CreatedBefore = new DateTime(2024, 2, 1),
            ModifiedAfter = new DateTime(2024, 3, 1),
            ModifiedBefore = new DateTime(2024, 4, 1),
            MinSize = 10,
            MaxSize = 100
        };

        var clone = options.Clone();

        Assert.NotSame(options, clone);
        Assert.Equal(options.SearchInContent, clone.SearchInContent);
        Assert.Equal(options.SearchInMetadata, clone.SearchInMetadata);
        Assert.Equal(options.Fuzzy, clone.Fuzzy);
        Assert.Equal(options.FuzzyThreshold, clone.FuzzyThreshold);
        Assert.Equal(options.CreatedAfter, clone.CreatedAfter);
        Assert.Equal(options.CreatedBefore, clone.CreatedBefore);
        Assert.Equal(options.ModifiedAfter, clone.ModifiedAfter);
        Assert.Equal(options.ModifiedBefore, clone.ModifiedBefore);
        Assert.Equal(options.MinSize, clone.MinSize);
        Assert.Equal(options.MaxSize, clone.MaxSize);
        Assert.Same(options.Extensions, clone.Extensions);
    }
}
