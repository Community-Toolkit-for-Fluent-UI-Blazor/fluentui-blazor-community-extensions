using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Client.Infrastructure;

public sealed class LocalStorageFileProvider : IFileProvider<FileManagerSampleFile>
{
    private readonly LocalStorageFileManagerRepository _repo;
    private List<FileManagerSampleFile>? _files;

    public LocalStorageFileProvider(LocalStorageFileManagerRepository repo)
    {
        _repo = repo;
    }

    public IFileProviderCapabilities Capabilities { get; } = new DefaultFileProviderCapabilities();

    private async Task EnsureLoadedAsync()
    {
        _files = await _repo.LoadAsync();
    }

    public async ValueTask<IReadOnlyList<EntryDescriptor<FileManagerSampleFile>>> GetChildrenAsync(string parentId)
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

    private static EntryDescriptor<FileManagerSampleFile> ToDescriptor(FileManagerSampleFile f)
    {
        if (f.IsDirectory)
        {
            return EntryDescriptor<FileManagerSampleFile>.Directory(
                id: f.Id,
                name: f.Name ?? "",
                parentId: f.ParentId,
                created: f.Created.UtcDateTime,
                modified: f.LastModified.UtcDateTime,
                size: f.Size,
                value: new FileManagerSampleFile()
            );
        }

        return EntryDescriptor<FileManagerSampleFile>.File(
            id: f.Id,
            name: f.Name ?? "",
            parentId: f.ParentId,
            size: f.Size,
            created: f.Created.UtcDateTime,
            modified: f.LastModified.UtcDateTime,
            getBytesAsync: () => Task.FromResult(f.Data),
            value: new FileManagerSampleFile()
        );
    }
}
