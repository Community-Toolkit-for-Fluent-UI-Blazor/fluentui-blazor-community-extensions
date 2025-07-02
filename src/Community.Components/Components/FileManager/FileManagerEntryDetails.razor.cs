using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the details view for a selected entry.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class FileManagerEntryDetails<TItem> where TItem : class, new()
{
    /// <summary>
    /// Represents the raw data of the entry.
    /// </summary>
    private byte[]? _entryDataContent;

    /// <summary>
    /// Represents the provider for the content type from a file extension.
    /// </summary>
    private readonly FileExtensionContentTypeProvider _fileContentTypeProvider = new();

    /// <summary>
    /// Represents the provider for the file extension.
    /// </summary>
    private FileExtensionTypeProvider? _fileExtensionTypeProvider;

    /// <summary>
    /// Represents a value indicating if the entry is an image.
    /// </summary>
    private bool _isImage;

    /// <summary>
    /// Represents the content type of the entry.
    /// </summary>
    private string? _contentType;

    /// <summary>
    /// Gets ors sets the selected entries.
    /// </summary>
    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> Entries { get; set; } = [];

    /// <summary>
    /// Gets or sets the <see cref="RenderFragment"/> to use when the content is empty.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the labels to use for the component.
    /// </summary>
    [Parameter]
    public FileManagerDetailsLabels DetailsLabel { get; set; } = FileManagerDetailsLabels.Default;

    /// <summary>
    /// Gets or sets the labels to describe the file extension.
    /// </summary>
    [Parameter]
    public FileExtensionTypeLabels FileExtensionTypeLabels { get; set; } = FileExtensionTypeLabels.Default;

    /// <summary>
    /// Gets the data of the entry in an asynchronous way.
    /// </summary>
    /// <param name="entry">Entry which contains the data to get.</param>
    /// <returns>Returns a task which get the data of the entry when completed.</returns>
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

    /// <summary>
    /// Gets the data of the entry in base64 format.
    /// </summary>
    /// <param name="data">Data to convert.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <returns>Returns the data content type as a <see cref="string" /> in base64 format.</returns>
    private static string GetBase64Content(byte[] data, string? contentType)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentOutOfRangeException.ThrowIfZero(data.Length, nameof(data));
        ArgumentException.ThrowIfNullOrEmpty(contentType, nameof(contentType));

        return $"data:{contentType};base64,{Convert.ToBase64String(data)}";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _fileExtensionTypeProvider = new(FileExtensionTypeLabels);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        _entryDataContent = [];
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
