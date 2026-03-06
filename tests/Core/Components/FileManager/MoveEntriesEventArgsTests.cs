using System.Collections.Generic;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class MoveEntriesEventArgsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var args = new MoveEntriesEventArgs();

        Assert.Empty(args.Ids);
        Assert.Empty(args.Results);
    }

    [Fact]
    public void Properties_AreSettable()
    {
        var args = new MoveEntriesEventArgs
        {
            Ids = new List<string> { "a" },
            NewParentId = "parent"
        };

        args.Results.Add(new MoveEntryResult("a") { Success = true });

        Assert.Equal("parent", args.NewParentId);
        Assert.Single(args.Results);
    }
}
