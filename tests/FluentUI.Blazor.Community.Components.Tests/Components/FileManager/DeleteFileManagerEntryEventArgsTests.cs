using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class DeleteFileManagerEntryEventArgsTests
{
    private class Dummy { public string Name { get; set; } = ""; }

    private static FileManagerEntry<Dummy> CreateEntry(string name)
    {
        return FileManagerEntry<Dummy>.CreateEntry([], name, 0);
    }

    [Fact]
    public void Constructor_SetsProperties()
    {
        var parent = CreateEntry("parent");
        var entry1 = CreateEntry("file1");
        var entry2 = CreateEntry("file2");
        var entries = new[] { entry1, entry2 };

        var args = new DeleteFileManagerEntryEventArgs<Dummy>(parent, entries);

        Assert.Equal(parent, args.Parent);
        Assert.Equal(entries, args.Entries);
    }

    [Fact]
    public void Entries_CanBeEnumerated()
    {
        var parent = CreateEntry("parent");
        var entry1 = CreateEntry("file1");
        var entry2 = CreateEntry("file2");
        var args = new DeleteFileManagerEntryEventArgs<Dummy>(parent, [entry1, entry2]);

        var list = new List<FileManagerEntry<Dummy>>(args.Entries);

        Assert.Contains(entry1, list);
        Assert.Contains(entry2, list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void Equality_Works()
    {
        var parent = CreateEntry("parent");
        var entry1 = CreateEntry("file1");
        var entry2 = CreateEntry("file2");
        var entries = new[] { entry1, entry2 };

        var args1 = new DeleteFileManagerEntryEventArgs<Dummy>(parent, entries);
        var args2 = new DeleteFileManagerEntryEventArgs<Dummy>(parent, entries);

        Assert.Equal(args1, args2);
        Assert.True(args1 == args2);
        Assert.False(args1 != args2);
    }
}
