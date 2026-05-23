using System.Collections.Generic;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class DeleteEntriesEventArgsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var args = new DeleteEntriesEventArgs();

        Assert.Empty(args.Ids);
        Assert.Empty(args.Results);
    }

    [Fact]
    public void Results_CanBeAdded()
    {
        var args = new DeleteEntriesEventArgs
        {
            Ids = new List<string> { "a" }
        };

        args.Results.Add(new DeleteEntryResult("a") { Success = true });

        Assert.Single(args.Results);
        Assert.True(args.Results[0].Success);
    }
}
