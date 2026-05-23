using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Client.Infrastructure;

public sealed class LocalStorageFileProvider<TItem> : IFileProvider<TItem>
    where TItem : class, new()
{
    private readonly LocalStorageFileManagerRepository<TItem> _repo;
    private List<FileManagerSampleFile>? _files;

    public LocalStorageFileProvider(LocalStorageFileManagerRepository<TItem> repo)
    {
        _repo = repo;
    }

    public IFileProviderCapabilities Capabilities { get; } = new DefaultFileProviderCapabilities();

    private async Task EnsureLoadedAsync()
    {
        _files = await _repo.LoadAsync();
    }

    public async ValueTask<IReadOnlyList<EntryDescriptor<TItem>>> GetChildrenAsync(string parentId)
    {
        await EnsureLoadedAsync();

        return [.. _files!
            .Where(f => f.ParentId == parentId)
            .Select(ToDescriptor)];
    }

    public async ValueTask<bool> HasChildrenAsync(string parentId)
    {
        await EnsureLoadedAsync();

        return _files!.Any(f => f.ParentId == parentId);
    }

    private static EntryDescriptor<TItem> ToDescriptor(FileManagerSampleFile f)
    {
        if (f.IsDirectory)
        {
            return EntryDescriptor<TItem>.Directory(
                id: f.Id,
                name: f.Name ?? "",
                parentId: f.ParentId,
                created: f.Created.UtcDateTime,
                modified: f.LastModified.UtcDateTime,
                size: f.Size,
                value: new TItem()
            );
        }

        return EntryDescriptor<TItem>.File(
            id: f.Id,
            name: f.Name ?? "",
            parentId: f.ParentId,
            size: f.Size,
            created: f.Created.UtcDateTime,
            modified: f.LastModified.UtcDateTime,
            getBytesAsync: () => Task.FromResult(f.Data),
            value: new TItem()
        );
    }
}
