using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileUploaderDialog
{
    /// <summary>
    /// Represents a file in memory.
    /// </summary>
    /// <param name="Name">Name of the file.</param>
    /// <param name="Stream">Stream containing the data of the file.</param>
    private record FileMemory(string Name, MemoryStream Stream)
    { }

    /// <summary>
    /// Represents a unique identifier.
    /// </summary>
    private readonly string _id = Identifier.NewId();

    /// <summary>
    /// Accepted files by default.
    /// </summary>
    private readonly AcceptFile _acceptFiles = AcceptFile.Audio | AcceptFile.Image | AcceptFile.Video | AcceptFile.Document;

    /// <summary>
    /// Number of file the user can upload at one time.
    /// </summary>
    private readonly int _maximumFileCount = 100;

    /// <summary>
    /// Template to report a progress.
    /// </summary>
    private readonly RenderFragment<ProgressFileDetails> _progressTemplate;

    /// <summary>
    /// Template to represents an icon of the document.
    /// </summary>
    private readonly RenderFragment<ChatFileEventArgs> _iconTemplate;

    /// <summary>
    /// Dictionary containing all files uploaded.
    /// </summary>
    private readonly Dictionary<int, FileMemory> _streams = [];

    /// <summary>
    /// List of all events args containing the uploaded files.
    /// </summary>
    private readonly List<ChatFileEventArgs> _hardUploadedEventArgsList = [];

    /// <summary>
    /// Represents the provider for the content type.
    /// </summary>
    private readonly FileExtensionContentTypeProvider _extensionContentTypeProvider = new();

    /// <summary>
    /// Gets or sets the dialog reference.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content of the dialog.
    /// </summary>
    [Parameter]
    public FileUploaderContent Content { get; set; } = default!;

    /// <summary>
    /// Occurs when the progression of the uploaded file changed.
    /// </summary>
    /// <param name="e">Event args of the file.</param>
    /// <returns>Returns a task which store the progression of the upload when completed.</returns>
    private async Task OnProgressChangedAsync(FluentInputFileEventArgs e)
    {
        if (!_streams.TryGetValue(e.Index, out var value))
        {
            value = new(e.Name, new MemoryStream());
            _streams.Add(e.Index, value);
        }

        await value.Stream.WriteAsync(e.Buffer.Data.AsMemory(0, e.Buffer.BytesRead));
    }

    /// <summary>
    /// Occurs when a file is uploaded.
    /// </summary>
    /// <param name="e">Event args of the file.</param>
    /// <returns>Returns a task which creates a <see cref="ChatFileEventArgs" /> when completed.</returns>
    private async Task OnFileUploadedAsync(FluentInputFileEventArgs e)
    {
        if (_streams.TryGetValue(e.Index, out var ms) &&
            _extensionContentTypeProvider.TryGetContentType(e.Name, out var ct))
        {
            _hardUploadedEventArgsList.Add(new(ms.Name, ct, ms.Stream.ToArray()));
            await ms.Stream.DisposeAsync();
            _streams.Remove(e.Index);
        }
    }

    /// <summary>
    /// Closes the dialog with the selected files as result.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with the selected files as result.</returns>
    private async Task OnCloseAsync()
    {
        await Dialog.CloseAsync(_hardUploadedEventArgsList);
    }

    /// <summary>
    /// Closes the dialog and cancel all selected files.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with a cancel result.</returns>
    private async Task OnCancelAsync()
    {
        foreach (var item in _streams)
        {
            await item.Value.Stream.DisposeAsync();
        }

        _streams.Clear();

        await Dialog.CancelAsync();
    }
}
