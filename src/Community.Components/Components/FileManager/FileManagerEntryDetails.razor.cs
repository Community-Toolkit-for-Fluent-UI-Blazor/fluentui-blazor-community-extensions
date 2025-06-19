using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerEntryDetails<TItem> where TItem : class, new()
{
    private byte[]? _entryDataContent;
    private readonly FileExtensionContentTypeProvider _fileContentTypeProvider = new();
    private FileExtensionTypeProvider? _fileExtensionTypeProvider;
    private bool _isImage;
    private bool _isVideo;
    private string? _contentType;

    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> Entries { get; set; } = [];

    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    [Parameter]
    public FileManagerDetailsLabels DetailsLabel { get; set; } = FileManagerDetailsLabels.Default;

    [Parameter]
    public FileExtensionTypeLabels FileExtensionTypeLabels { get; set; } = FileExtensionTypeLabels.Default;

    private async Task GetEntryDataContentAsync(FileManagerEntry<TItem>? entry)
    {
        if (entry is null ||
            string.IsNullOrEmpty(entry.Extension))
        {
            return;
        }

        if (_fileContentTypeProvider.TryGetContentType(entry.Extension, out var contentType) &&
           !string.IsNullOrEmpty(contentType))
        {
            if (contentType.StartsWith("image/"))
            {
                _isImage = true;
                _entryDataContent = await entry.GetBytesAsync();
                _contentType = contentType;
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    private static string GetBase64Content(byte[] data, string? contentType)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentOutOfRangeException.ThrowIfZero(data.Length, nameof(data));
        ArgumentException.ThrowIfNullOrEmpty(contentType, nameof(contentType));

        return $"data:{contentType};base64,{Convert.ToBase64String(data)}";
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _fileExtensionTypeProvider = new(FileExtensionTypeLabels);
    }

    protected override async Task OnParametersSetAsync()
    {
        _entryDataContent = [];
        _isVideo = false;
        _isImage = false;
        _contentType = string.Empty;

        if (Entries is not null &&
            Entries.Count() == 1 &&
            (!Entries.ElementAt(0)?.IsDirectory ?? false))
        {
            await GetEntryDataContentAsync(Entries.ElementAt(0));
        }
    }
}
