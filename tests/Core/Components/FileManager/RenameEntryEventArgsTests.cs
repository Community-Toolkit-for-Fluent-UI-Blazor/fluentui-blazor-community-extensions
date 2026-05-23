using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class RenameEntryEventArgsTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var args = new RenameEntryEventArgs("id")
        {
            NewName = "new-name",
            Success = true
        };

        Assert.Equal("id", args.Id);
        Assert.Equal("new-name", args.NewName);
        Assert.True(args.Success);
    }
}
