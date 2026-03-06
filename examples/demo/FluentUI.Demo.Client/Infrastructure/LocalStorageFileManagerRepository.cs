using System.Text.Json;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Infrastructure;

public sealed class LocalStorageFileManagerRepository<TItem>
    where TItem : class, new()
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
            return new List<FileManagerSampleFile>();
        }

        return JsonSerializer.Deserialize<List<FileManagerSampleFile>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new List<FileManagerSampleFile>();
    }

    public async Task SaveAsync(List<FileManagerSampleFile> files)
    {
        var json = JsonSerializer.Serialize(files, new JsonSerializerOptions
        {
            WriteIndented = false
        });

        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
