using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the base for the file manager.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public class FileManagerBase<TItem> : ComponentBase where TItem : class, new()
{
    /// <summary>
    /// Gets or sets the parent of this instance.
    /// </summary>
    [CascadingParameter]
    private protected FluentCxFileManager<TItem>? Parent { get; set; }

    /// <summary>
    /// Gets or sets the entry to view.
    /// </summary>
    [Parameter]
    public FileManagerEntry<TItem>? Entry { get; set; }

    /// <summary>
    /// Gets or sets the selected items.
    /// </summary>
    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> SelectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets an event callback which occurs when the <see cref="SelectedItems"/> changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FileManagerEntry<TItem>>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets an event callback which occurs when an item is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemTapped { get; set; }

    /// <summary>
    /// Gets or sets an event callback which occurs when an item is double tapped.
    /// </summary>
    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemDoubleTapped { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the filemanager runs on mobile.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the filemanager is busy or not.
    /// </summary>
    [Parameter]
    public bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the progess is indeterminate or not.
    /// </summary>
    [Parameter]
    public bool IsIndeterminateProgress { get; set; }

    /// <summary>
    /// Gets or sets the percentage of the progression.
    /// </summary>
    [Parameter]
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets the label of the progress.
    /// </summary>
    [Parameter]
    public string? ProgressLabel { get; set; }

    /// <summary>
    /// Gets or sets the labels of the columns when the file manager renders the items as list or details.
    /// </summary>
    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    /// <summary>
    /// Occurs when the <see cref="SelectedItems"/> has changed.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="SelectedItemsChanged"/> event callback.</returns>
    protected async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    /// <summary>
    /// Occurs when an item is double clicked.
    /// </summary>
    /// <param name="entry">Represents the clicked entry.</param>
    /// <returns>Returns a task which invokes the <see cref="OnItemDoubleTapped"/> event callback.</returns>
    protected async Task OnItemDoubleTappedAsync(
        FileManagerEntry<TItem>? entry)
    {
        if (OnItemDoubleTapped.HasDelegate && entry is not null)
        {
            await OnItemDoubleTapped.InvokeAsync(new(entry));
        }
    }

    /// <summary>
    /// Occurs when an item is double clicked.
    /// </summary>
    /// <param name="entry">Represents the clicked entry.</param>
    /// <returns>Returns a task which invokes the <see cref="OnItemTapped"/> event callback.</returns>
    protected async Task OnItemTappedAsync(
        FileManagerEntry<TItem> entry)
    {
        if (OnItemTapped.HasDelegate)
        {
            await OnItemTapped.InvokeAsync(new(entry));
        }
    }

    /// <summary>
    /// Gets the icon from an extension.
    /// </summary>
    /// <param name="extension">Extension of the file.</param>
    /// <returns>Returns the icons to render.</returns>
    protected static Icon GetIconFromFile(string extension)
    {
        return FileIcons.FromExtension(extension);
    }

    /// <summary>
    /// Gets the icon from an extension and the current used view.
    /// </summary>
    /// <param name="extension">Extension of the file.</param>
    /// <param name="options">Current used view.</param>
    /// <returns>Returns the icons to render.</returns>
    protected static Icon GetIconFromExtensionAndGridViewOptions(string extension, FileView options)
    {
        return FileIcons.FromExtensionAndGridViewOptions(extension, options);
    }

    /// <summary>
    /// Gets the folder icon from the current used view.
    /// </summary>
    /// <param name="options">Current used view.</param>
    /// <returns>Returns the icons to render.</returns>
    protected static Icon GetFolderIconFromGridViewOptions(FileView options)
    {
        return FileIcons.GetFolderFromGridViewOptions(options);
    }

    /// <summary>
    /// Occurs when a rename operation is performed.
    /// </summary>
    /// <param name="entry">Entry to rename.</param>
    /// <returns>Returns a task which contains the rename process when completed.</returns>
    protected async Task OnRenameAsync(FileManagerEntry<TItem> entry)
    {
        if (Parent is not null)
        {
            await Parent.OnRenameAsync(entry);
        }
    }
}

