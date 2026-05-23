using System;
using System.Linq;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileEntryTests
{
    private sealed class DummyItem
    {
        public string Value { get; set; } = string.Empty;
    }

    [Fact]
    public void AddChild_AssignsParentForDirectories()
    {
        var parent = CreateDirectory("root");
        var child = CreateFile("child.txt");

        parent.AddChild(child);

        Assert.Single(parent.Children);
        Assert.Same(parent, child.Parent);
    }

    [Fact]
    public void AddChild_ThrowsForFileEntries()
    {
        var file = CreateFile("file.txt");

        Assert.Throws<InvalidOperationException>(() => file.AddChild(CreateFile("other.txt")));
    }

    [Fact]
    public void AddChild_IgnoresDuplicates()
    {
        var parent = CreateDirectory("root");
        var child = CreateFile("child.txt");

        parent.AddChild(child);
        parent.AddChild(child);

        Assert.Single(parent.Children);
    }

    [Fact]
    public void RemoveChild_RemovesAndClearsParent()
    {
        var parent = CreateDirectory("root");
        var child = CreateFile("child.txt");

        parent.AddChild(child);

        var removed = parent.RemoveChild(child);

        Assert.True(removed);
        Assert.Empty(parent.Children);
        Assert.Null(child.Parent);
    }

    [Fact]
    public void RemoveChild_ThrowsForFileEntries()
    {
        var file = CreateFile("file.txt");

        Assert.Throws<InvalidOperationException>(() => file.RemoveChild(CreateFile("other.txt")));
    }

    [Fact]
    public void Rename_PreservesExtension()
    {
        var file = CreateFile("photo.png");

        file.Rename("new-name");

        Assert.Equal("new-name.png", file.Name);
    }

    [Fact]
    public void SetSize_ThrowsForNegative()
    {
        var file = CreateFile("file.bin");

        Assert.Throws<ArgumentOutOfRangeException>(() => file.SetSize(-1));
    }

    [Fact]
    public void IsAncestorOf_ReturnsExpectedResult()
    {
        var root = CreateDirectory("root");
        var child = CreateDirectory("child");
        var file = CreateFile("file.txt");

        root.AddChild(child);
        child.AddChild(file);

        Assert.True(root.IsAncestorOf(file));
        Assert.False(child.IsAncestorOf(root));
    }

    [Fact]
    public void Clone_CopiesCoreValues()
    {
        var file = CreateFile("file.txt");

        var clone = file.Clone();

        Assert.NotSame(file, clone);
        Assert.Equal(file.Name, clone.Name);
        Assert.Equal(file.Id, clone.Id);
    }

    [Fact]
    public void GetContentType_ReturnsExpectedType()
    {
        var file = CreateFile("document.pdf");

        var contentType = file.GetContentType();

        Assert.Equal("application/pdf", contentType);
    }

    [Fact]
    public async Task GetContentAsync_ReturnsEmptyWithoutProvider()
    {
        var file = CreateFile("document.txt");

        var content = await file.GetContentAsync();

        Assert.Empty(content);
    }

    private static FileEntry<DummyItem> CreateFile(string name)
    {
        return new FileEntry<DummyItem>(
            id: Guid.NewGuid().ToString(),
            name: name,
            isDirectory: false,
            size: 0,
            createdDate: DateTimeOffset.UtcNow,
            modifiedDate: DateTimeOffset.UtcNow,
            item: new DummyItem());
    }

    private static FileEntry<DummyItem> CreateDirectory(string name)
    {
        return new FileEntry<DummyItem>(
            id: Guid.NewGuid().ToString(),
            name: name,
            isDirectory: true,
            size: 0,
            createdDate: DateTimeOffset.UtcNow,
            modifiedDate: DateTimeOffset.UtcNow,
            item: new DummyItem());
    }
}
