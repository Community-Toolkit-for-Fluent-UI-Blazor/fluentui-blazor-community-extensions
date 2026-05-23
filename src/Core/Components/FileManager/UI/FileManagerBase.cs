using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for file manager components that handle file entries, selection, and related events in a
/// Fluent UI Blazor context.
/// </summary>
/// <remarks>This class is intended to be used as a base for building file manager components that require
/// selection management, progress reporting, and event handling for user interactions such as tapping or renaming
/// items. It integrates with Fluent UI Blazor patterns and supports extensibility through event callbacks and
/// parameters.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each file entry. Must be a reference type with a parameterless
/// constructor.</typeparam>
public abstract class FileManagerBase<TItem> : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Initialize a new instance of the <see cref="FileManagerBase{TItem}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="libraryConfiguration">The library configuration to be used by the component.</param>
    protected FileManagerBase(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
    }

    /// <summary>
    /// Gets or sets the parent of this instance.
    /// </summary>
    [CascadingParameter]
    private protected FluentCxFileManager<TItem>? Parent { get; set; }

    /// <summary>
    /// Gets or sets the entry to view.
    /// </summary>
    [Parameter]
    public FileEntry<TItem>? Entry { get; set; }

    /// <summary>
    /// Gets or sets the selected items.
    /// </summary>
    [Parameter]
    public IEnumerable<FileEntry<TItem>> SelectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets an event callback which occurs when the <see cref="SelectedItems"/> changed.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FileEntry<TItem>>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets an event callback which occurs when an item is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<FileEntry<TItem>> OnItemTapped { get; set; }

    /// <summary>
    /// Gets or sets an event callback which occurs when an item is double tapped.
    /// </summary>
    [Parameter]
    public EventCallback<FileEntry<TItem>> OnItemDoubleTapped { get; set; }

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
    /// Occurs when the <see cref="SelectedItems"/> has changed.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="SelectedItemsChanged"/> event callback.</returns>
    protected async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems ?? []);
        }
    }

    /// <summary>
    /// Occurs when an item is double clicked.
    /// </summary>
    /// <param name="entry">Represents the clicked entry.</param>
    /// <returns>Returns a task which invokes the <see cref="OnItemDoubleTapped"/> event callback.</returns>
    protected async Task OnItemDoubleTappedAsync(FileEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return;
        }

        if (OnItemDoubleTapped.HasDelegate)
        {
            await OnItemDoubleTapped.InvokeAsync(entry);
        }
    }

    /// <summary>
    /// Occurs when an item is double clicked.
    /// </summary>
    /// <param name="entry">Represents the clicked entry.</param>
    /// <returns>Returns a task which invokes the <see cref="OnItemTapped"/> event callback.</returns>
    protected async Task OnItemTappedAsync(FileEntry<TItem>? entry)
    {
        if (entry is null)
        {
            return;
        }

        if (OnItemTapped.HasDelegate)
        {
            await OnItemTapped.InvokeAsync(entry);
        }
    }

    /// <summary>
    /// Determines whether renaming is disabled for the specified file entry.
    /// </summary>
    /// <remarks>Renaming is considered disabled if the entry's item does not implement IRenamable or if
    /// renaming is not permitted by the item.</remarks>
    /// <param name="entry">The file entry to evaluate for rename capability.</param>
    /// <returns>Returns <see langword="true" /> if renaming is not allowed for the entry; otherwise,
    ///  <see langword="false"/>.</returns>
    protected bool IsRenameDisabled(FileEntry<TItem> entry)
    {
        return !(entry.Item is IRenamable renamable && renamable.IsRenameAllowed);
    }

    /// <summary>
    /// Determines whether deleting is disabled for the specified file entry.
    /// </summary>
    /// <remarks>Deleting is considered disabled if the entry's item does not implement IDeletable or if
    /// deleting is not permitted by the item.</remarks>
    /// <param name="entry">The file entry to evaluate for delete capability.</param>
    /// <returns>Returns <see langword="true" /> if deleting is not allowed for the entry; otherwise,
    ///  <see langword="false"/>.</returns>
    protected bool IsDeleteDisabled(FileEntry<TItem> entry)
    {
        return !(entry.Item is IDeletable deletable && deletable.IsDeleteAllowed);
    }

    /// <summary>
    /// Determines whether moving is disabled for the specified file entry.
    /// </summary>
    /// <remarks>Moving is considered disabled if the entry's item does not implement IDeletable or if
    /// moving is not permitted by the item.</remarks>
    /// <param name="entry">The file entry to evaluate for move capability.</param>
    /// <returns>Returns <see langword="true" /> if moving is not allowed for the entry; otherwise,
    ///  <see langword="false"/>.</returns>
    protected bool IsMoveDisabled(FileEntry<TItem> entry)
    {
        return !(entry.Item is IMovable movable && movable.IsMoveAllowed);
    }

    /// <summary>
    /// Determines whether downloading is disabled for the specified file entry.
    /// </summary>
    /// <remarks>Downloading is considered disabled if the entry's item does not implement IDownloadable or if
    /// downloading is not permitted by the item.</remarks>
    /// <param name="entry">The file entry to evaluate for download capability.</param>
    /// <returns>Returns <see langword="true" /> if downloading is not allowed for the entry; otherwise,
    ///  <see langword="false"/>.</returns>
    protected bool IsDownloadDisabled(FileEntry<TItem> entry)
    {
        return !(entry.Item is IDownloadable downloadable && downloadable.IsDownloadAllowed);
    }
}
