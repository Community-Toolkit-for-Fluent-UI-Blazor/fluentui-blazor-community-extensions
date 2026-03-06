using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays detailed information about one or more file entries, including content type,
/// file type label, and image preview when applicable.
/// </summary>
/// <remarks>This component is typically used to present metadata and preview information for selected files in a
/// file picker or file management interface. When a single file entry is provided and it is an image, a preview is
/// automatically loaded. The component relies on injected services to determine content types and to retrieve file data
/// as needed.</remarks>
/// <typeparam name="TItem">The type of the underlying data model associated with each file entry. Must be a reference type.</typeparam>
public partial class FileEntryDetails<TItem> where TItem : class, new()
{
    /// <summary>
    /// Represents the content data of a file entry, typically used for preview purposes.
    /// </summary>
    /// <remarks>
    /// This field is populated when there is exactly one file entry and it is an image.
    /// The content is stored as a byte array, which can be converted to a base64 string for display
    ///  in an &lt;img&gt; tag or similar component. If there are multiple entries or if the single entry
    ///  is not an image, this field remains null.</remarks>
    private byte[]? _entryDataContent;

    /// <summary>
    /// Represents the MIME content type of the file entry, such as "image/png" or "application/pdf".
    /// </summary>
    private string? _contentType;

    /// <summary>
    /// Represents a human-readable label for the file type, typically derived from the file extension (e.g., "PNG Image" for ".png").
    /// </summary>
    private string? _fileTypeLabel;

    /// <summary>
    /// Represents whether the single file entry is an image, which determines if a preview should be loaded and displayed.
    /// </summary>
    private bool _isImage;

    /// <summary>
    /// Provides a static instance of the file extension content type provider for resolving MIME types based on file
    /// extensions.
    /// </summary>
    /// <remarks>This instance can be used to map file extensions to their corresponding content types
    /// throughout the application. Using a static instance helps avoid repeated allocations and ensures consistent
    /// behavior.</remarks>
    private static readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    /// <summary>
    /// Gets or sets the collection of file entries to display or process.
    /// </summary>
    /// <remarks>Each entry in the collection represents a file and its associated data. The property can be
    /// set to update the list of files shown or handled by the component.</remarks>
    [Parameter]
    public IEnumerable<FileEntry<TItem>> Entries { get; set; } = [];

    /// <summary>
    /// Gets or sets the content to display when there are no items to show.
    /// </summary>
    /// <remarks>Use this property to provide custom UI or messaging for empty states, such as when a list or
    /// collection has no data. If not set, no content will be rendered in the empty state.</remarks>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        _entryDataContent = null;
        _contentType = null;
        _fileTypeLabel = null;
        _isImage = false;

        if (Entries.Count() == 1)
        {
            var entry = Entries.First();

            _contentType = GetContentType(entry);

            // File type label
            // TODO : implement file type label provider
            _fileTypeLabel = ""; //FileExtensionTypeProvider.GetLabel(entry.Extension);

            // Load preview only if image
            if (!entry.IsDirectory &&
                _contentType?.StartsWith("image/") == true)
            {
                _isImage = true;
                _entryDataContent = await entry.GetContentAsync();
            }
        }
    }

    /// <summary>
    /// Determines the content type (MIME type) of a given file entry based on its extension using the FileExtensionContentTypeProvider.
    /// </summary>
    /// <param name="entry">Entry for which to determine the content type. The method checks the file extension and uses the provider to resolve the MIME type. If the extension is missing or unrecognized, it defaults to "application/octet-stream".</param>
    /// <returns>Returns the content type as a string, such as "image/png" for a .png file. If the extension is not recognized or is missing, returns "application/octet-stream".</returns>
    private static string GetContentType(FileEntry<TItem> entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        if (string.IsNullOrEmpty(entry.Extension))
        {
            return "application/octet-stream";
        }

        if (_contentTypeProvider.TryGetContentType(entry.Name, out var contentType))
        {
            return contentType;
        }

        return "application/octet-stream";
    }

    /// <summary>
    /// Generates a base64-encoded data URL for the file content,
    ///  which can be used for displaying an image preview in the UI.
    /// </summary>
    /// <param name="data">Data bytes of the file content to be encoded.</param>
    /// <param name="contentType">Content type (MIME type) of the file, such as "image/png".</param>
    /// <returns>Returns a string in the format "data:{contentType};base64,{base64Data}", which can be used as a source for an image element.</returns>
    private static string GetBase64Content(byte[] data, string? contentType)
        => $"data:{contentType};base64,{Convert.ToBase64String(data)}";

    /// <summary>
    /// Retrieves the icon associated with the specified file entry based on its file extension and the details view.
    /// </summary>
    /// <param name="entry">The file entry for which to obtain the corresponding icon. The file extension of this entry determines the icon
    /// returned.</param>
    /// <returns>An icon representing the file type of the specified entry, suitable for display in a details view.</returns>
    private static Icon GetIcon(FileEntry<TItem> entry)
    {
        if (entry.IsDirectory)
        {
            return FileIconFactory.Get(FileIconKey.Folder, FileView.LargeIcons);
        }

        return FileIconFactory.Get(FileIconRegistry.Resolve(entry.Extension), FileView.LargeIcons);
    }
}
