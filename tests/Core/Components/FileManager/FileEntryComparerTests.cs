using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileEntryComparerTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public void Equals_ReturnsFalseForNulls()
    {
        var comparer = FileEntryComparer<DummyItem>.Default;

        Assert.False(comparer.Equals(null, null));
        Assert.False(comparer.Equals(CreateEntry("a"), null));
        Assert.False(comparer.Equals(null, CreateEntry("b")));
    }

    [Fact]
    public void Equals_ComparesIdsCaseInsensitive()
    {
        var comparer = FileEntryComparer<DummyItem>.Default;

        var left = CreateEntry("ABC");
        var right = CreateEntry("abc");

        Assert.True(comparer.Equals(left, right));
    }

    [Fact]
    public void GetHashCode_UsesIdHashCode()
    {
        var comparer = FileEntryComparer<DummyItem>.Default;
        var entry = CreateEntry("id-1");

        Assert.Equal(entry.Id.GetHashCode(), comparer.GetHashCode(entry));
    }

    private static FileEntry<DummyItem> CreateEntry(string id)
    {
        return new FileEntry<DummyItem>(
            id: id,
            name: "file.txt",
            isDirectory: false,
            size: 0,
            createdDate: DateTimeOffset.UtcNow,
            modifiedDate: DateTimeOffset.UtcNow,
            item: new DummyItem());
    }
}
