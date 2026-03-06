using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;

namespace Components.Tests.Components.FileManager.TestDoubles;

internal sealed class TestFileProvider<TItem> : IFileProvider<TItem>
    where TItem : class, new()
{
    private readonly Dictionary<string, IReadOnlyList<EntryDescriptor<TItem>>> _map;

    public TestFileProvider(Dictionary<string, IReadOnlyList<EntryDescriptor<TItem>>> map, IFileProviderCapabilities? capabilities = null)
    {
        _map = map;
        Capabilities = capabilities ?? new TestFileProviderCapabilities();
    }

    public IFileProviderCapabilities Capabilities { get; }

    public ValueTask<IReadOnlyList<EntryDescriptor<TItem>>> GetChildrenAsync(string parentId)
    {
        return new ValueTask<IReadOnlyList<EntryDescriptor<TItem>>>(
            _map.TryGetValue(parentId, out var list) ? list : Array.Empty<EntryDescriptor<TItem>>());
    }

    public ValueTask<bool> HasChildrenAsync(string parentId)
    {
        return new ValueTask<bool>(_map.TryGetValue(parentId, out var list) && list.Count > 0);
    }
}

internal sealed class TestFileProviderCapabilities : IFileProviderCapabilities
{
    public bool CanDelete { get; set; }
    public bool CanRename { get; set; }
    public bool CanMove { get; set; }
    public bool CanCreateDirectory { get; set; }
    public bool CanUpload { get; set; }
}
