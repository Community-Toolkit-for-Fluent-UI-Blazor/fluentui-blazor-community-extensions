using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class CreateDirectoryEventArgsTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var args = new CreateDirectoryEventArgs
        {
            ParentId = "parent",
            Name = "new-folder"
        };

        Assert.Equal("parent", args.ParentId);
        Assert.Equal("new-folder", args.Name);
        Assert.False(args.Cancel);
    }
}
