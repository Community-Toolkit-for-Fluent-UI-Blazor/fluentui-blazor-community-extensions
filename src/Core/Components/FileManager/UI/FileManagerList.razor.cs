using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the list view for the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FileManagerList<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    private readonly RenderFragment _content;

    /// <summary>
    /// Occurs when the row is double clicked.
    /// </summary>
    /// <param name="e">Event args associated to the clicked row.</param>
    /// <returns>Returns a task which perform the double click when completed.</returns>
    private async Task OnRowDoubleClickAsync(FluentDataGridRow<FileEntry<TItem>> e)
    {
        await OnItemDoubleTappedAsync(e.Item);
    }
}
