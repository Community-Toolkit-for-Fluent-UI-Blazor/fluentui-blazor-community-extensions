using System;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class EntryDescriptorTests
{
    [Fact]
    public void Directory_CreatesDirectoryDescriptor()
    {
        var created = new DateTime(2024, 1, 1);
        var modified = new DateTime(2024, 1, 2);

        var descriptor = EntryDescriptor<string>.Directory("id", "name", null, created, modified, 10, "value");

        Assert.True(descriptor.IsDirectory);
        Assert.Null(descriptor.GetBytesAsync);
        Assert.Equal(10, descriptor.Size);
        Assert.Equal("value", descriptor.Value);
    }

    [Fact]
    public void File_CreatesFileDescriptor()
    {
        var created = new DateTime(2024, 1, 1);
        var modified = new DateTime(2024, 1, 2);
        static Task<byte[]> GetBytesAsync() => Task.FromResult(new byte[] { 1, 2, 3 });

        var descriptor = EntryDescriptor<string>.File("id", "name", "parent", 5, created, modified, GetBytesAsync, "value");

        Assert.False(descriptor.IsDirectory);
        Assert.NotNull(descriptor.GetBytesAsync);
        Assert.Equal(5, descriptor.Size);
        Assert.Equal("parent", descriptor.ParentId);
    }
}
