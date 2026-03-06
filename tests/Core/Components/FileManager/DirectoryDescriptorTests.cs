using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class DirectoryDescriptorTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var descriptor = new DirectoryDescriptor("id")
        {
            Name = "Folder",
            ParentId = "parent"
        };

        Assert.Equal("id", descriptor.Id);
        Assert.Equal("Folder", descriptor.Name);
        Assert.Equal("parent", descriptor.ParentId);
    }
}
