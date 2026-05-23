using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Tests.Components.FileManager.TestDoubles;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileManagerTreeEngineTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public async Task CreateRootAsync_LoadsChildrenAndExpands()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["home"] = [EntryDescriptor<DummyItem>.Directory("dir", "Dir", "home", DateTime.UtcNow, DateTime.UtcNow)]
        });
        var core = new FileManagerEngine<DummyItem>(provider, new FileManagerState());
        var engine = new FileManagerTreeEngine<DummyItem>(core);

        var root = await engine.CreateRootAsync(_ => Task.CompletedTask);

        Assert.True(root.Expanded);
        Assert.NotNull(root.Items);
        Assert.Single(root.Items!);
    }

    [Fact]
    public async Task RefreshAsync_ReturnsNewNodeWhenFound()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["home"] = [EntryDescriptor<DummyItem>.Directory("dir", "Dir", "home", DateTime.UtcNow, DateTime.UtcNow)]
        });
        var core = new FileManagerEngine<DummyItem>(provider, new FileManagerState());
        var engine = new FileManagerTreeEngine<DummyItem>(core);
        var root = await engine.CreateRootAsync(_ => Task.CompletedTask);

        var refreshed = await engine.RefreshAsync(root.Id, "dir", _ => Task.CompletedTask);

        Assert.NotNull(refreshed);
        Assert.Equal("dir", refreshed!.Id);
    }

    [Fact]
    public async Task TryGetItem_ReturnsItemFromIndex()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>());
        var core = new FileManagerEngine<DummyItem>(provider, new FileManagerState());
        var engine = new FileManagerTreeEngine<DummyItem>(core);
        await engine.CreateRootAsync(_ => Task.CompletedTask);

        var found = engine.TryGetItem("home", out var node);

        Assert.True(found);
        Assert.NotNull(node);
    }
}
