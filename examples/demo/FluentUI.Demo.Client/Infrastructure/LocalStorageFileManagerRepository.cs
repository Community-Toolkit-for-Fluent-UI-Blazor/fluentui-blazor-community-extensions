using System.Text.Json;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Infrastructure;

public sealed class LocalStorageFileManagerRepository
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "FluentCx.FileManager.SampleFiles";

    public LocalStorageFileManagerRepository(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<List<FileManagerSampleFile>> LoadAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        var list =  JsonSerializer.Deserialize<List<SampleFileMetadata>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new List<SampleFileMetadata>();

        var result = new List<FileManagerSampleFile>(list.Count);

        foreach (var file in list)
        {
            result.Add(new FileManagerSampleFile
            {
                Id = file.Id,
                Name = file.Name,
                ParentId = file.ParentId,
                IsDirectory = file.IsDirectory,
                Size = file.Size,
                LastModified = file.LastModified,
                Created = file.Created,
                Data = File.ReadAllBytes(file.DataFile),
                IsRenameAllowed = file.IsRenameAllowed,
                IsDeleteAllowed = file.IsDeleteAllowed,
                IsDownloadAllowed = file.IsDownloadAllowed,
                IsMoveAllowed = file.IsMoveAllowed
            });
        }

        return result;
    }

    public async Task SaveAsync(List<FileManagerSampleFile> files)
    {
        var list = new List<SampleFileMetadata>(files.Count);

        foreach (var file in files)
        {
            var dataFile = Path.Combine(Path.GetTempPath(), $"{file.Id}.data");
            File.WriteAllBytes(dataFile, file.Data);
            list.Add(new SampleFileMetadata
            {
                Id = file.Id,
                Name = file.Name,
                ParentId = file.ParentId,
                IsDirectory = file.IsDirectory,
                Size = file.Size,
                LastModified = file.LastModified,
                Created = file.Created,
                DataFile = dataFile,
                IsRenameAllowed = file.IsRenameAllowed,
                IsDeleteAllowed = file.IsDeleteAllowed,
                IsDownloadAllowed = file.IsDownloadAllowed,
                IsMoveAllowed = file.IsMoveAllowed
            });
        }

        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions
        {
            WriteIndented = false
        });

        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
