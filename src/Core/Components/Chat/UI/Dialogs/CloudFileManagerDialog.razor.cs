using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// A dialog component for browsing and selecting files from a cloud storage provider.
/// </summary>
/// <typeparam name="TItem">The type of file item provided by the file provider.</typeparam>
public partial class CloudFileManagerDialog<TItem>
{
    /// <summary>
    /// Represents the file manager.
    /// </summary>
    private FluentCxFileManager<TItem>? _fileManager;

    /// <summary>
    /// Gets or sets the file provider that supplies the files to be displayed in the file manager. 
    /// </summary>
    [Inject]
    private IFileProvider<TItem> Provider { get; set; } = default!;

    /// <inheritdoc />
    protected override Task OnActionClickedAsync(bool primary)
    {
        return primary ? OnCloseAsync() : OnCancelAsync();
    }

    /// <summary>
    /// Closes the dialog with the selected files as result.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with the selected files as result.</returns>
    private async Task OnCloseAsync()
    {
        List<ChatFileEventArgs> e = [];

        foreach (var item in _fileManager!.SelectedItems)
        {
            e.Add(new(item.Id!, item.Name, item.GetContentType(), item.GetContentAsync));
        }

        await DialogInstance.CloseAsync(e);
    }

    /// <summary>
    /// Closes the dialog with a cancel result.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with a cancel result.</returns>
    private async Task OnCancelAsync()
    {
        await DialogInstance.CancelAsync();
    }
}
