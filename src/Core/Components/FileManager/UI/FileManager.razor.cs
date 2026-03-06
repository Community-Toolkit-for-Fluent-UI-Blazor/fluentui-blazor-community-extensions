using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a file manager component for displaying and interacting with a hierarchical collection of file or folder
/// entries. Supports selection, navigation, and progress indication for file operations.
/// </summary>
/// <remarks>The FileManager component enables users to browse, select, and perform actions on files or folders
/// represented by the generic type parameter. It supports customizable item templates, selection tracking, and
/// navigation through a path bar. The component is designed to be flexible and can be integrated with various data
/// sources or file systems by providing appropriate entry data.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each file or folder entry. Must be a reference type.</typeparam>
public partial class FileManager<TItem> : FluentComponentBase where TItem : class, new()
{
    /// <summary>
    /// Represents the current instance of the path bar component used for navigation.
    /// </summary>
    private FluentCxTrailMenu? _pathBar;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileManager{TItem}"/> component with the specified configuration.
    /// </summary>
    /// <param name="configuration">Library configuration for the component.</param>
    public FileManager(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the file entry associated with the component.
    /// </summary>
    /// <remarks>Use this property to provide or retrieve the file entry data of type <see cref="FileEntry{TItem}"/>. The
    /// file entry typically contains metadata and content information for a file being processed or displayed by the
    /// component.</remarks>
    [Parameter]
    public FileEntry<TItem>? Entry { get; set; }

    /// <summary>
    /// Gets or sets the collection of file entries that are currently selected.
    /// </summary>
    /// <remarks>The selected items represent the files chosen by the user. Modifying this collection updates
    /// the selection state of the component.</remarks>
    [Parameter]
    public IEnumerable<FileEntry<TItem>> SelectedItems { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when the collection of selected file entries changes.
    /// </summary>
    /// <remarks>Use this parameter to respond to changes in the selected items, such as updating application
    /// state or triggering additional logic when the user selects or deselects files.</remarks>
    [Parameter]
    public EventCallback<IEnumerable<FileEntry<TItem>>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a file entry is double-tapped by the user.
    /// </summary>
    /// <remarks>Use this event to handle double-tap interactions on file entries, such as opening or
    /// previewing the selected item. The event argument provides information about the file entry that was
    /// double-tapped.</remarks>
    [Parameter]
    public EventCallback<FileEntry<TItem>> OnItemDoubleTapped { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the path bar is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowTrailMenu { get; set; } = true;

    /// <summary>
    /// Gets or sets the root menu item for the breadcrumb trail.
    /// </summary>
    /// <remarks>Set this property to define the starting point of the breadcrumb navigation. If not set, the
    /// breadcrumb may not display a root element.</remarks>
    [Parameter]
    public ITrailMenuItem? RootPath { get; set; }

    /// <summary>
    /// Gets or sets the path associated with the component or operation.
    /// </summary>
    [Parameter]
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the path value changes.
    /// </summary>
    /// <remarks>Use this parameter to handle path change events and perform custom logic when the path is
    /// updated by the user or component.</remarks>
    [Parameter]
    public EventCallback<string> OnPathChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is in a busy state.
    /// </summary>
    [Parameter]
    public bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress indicator displays an indeterminate state.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to show a continuous, non-specific progress
    /// animation when the exact progress value is unknown.</remarks>
    [Parameter]
    public bool IsIndeterminateProgress { get; set; }

    /// <summary>
    /// Gets or sets the progress percentage for the current operation.
    /// </summary>
    [Parameter]
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// Gets or sets the label text displayed alongside the progress indicator.
    /// </summary>
    [Parameter]
    public string? ProgressLabel { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each file entry in the collection.
    /// </summary>
    /// <remarks>Use this property to customize the appearance and layout of individual file entries. The
    /// template receives a context of type <see cref="FileEntry{TItem}"/> , which provides information about the file to be
    /// rendered.</remarks>
    [Parameter]
    public RenderFragment<FileEntry<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the current state of the file manager component.
    /// </summary>
    [Inject]
    private FileManagerState State { get; set; } = default!;

    /// <summary>
    /// Invokes the SelectedItemsChanged event callback asynchronously if a delegate has been assigned.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    /// <summary>
    /// Invokes the double-tap event handler for the specified file entry, if one is assigned.
    /// </summary>
    /// <remarks>This method should be called when a file entry is double-tapped to notify any registered
    /// event handlers. If no handler is assigned, the method completes without performing any action.</remarks>
    /// <param name="entry">The file entry that was double-tapped. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnItemDoubleTappedAsync(FileEntry<TItem> entry)
    {
        if (OnItemDoubleTapped.HasDelegate)
        {
            await OnItemDoubleTapped.InvokeAsync(entry);
        }
    }

    /// <summary>
    /// Handles changes to the path by invoking the associated path changed event asynchronously, if a delegate is
    /// assigned.
    /// </summary>
    /// <param name="newPath">The new path value to be passed to the path changed event handler.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnPathChangedAsync(string newPath)
    {
        if (OnPathChanged.HasDelegate)
        {
            await OnPathChanged.InvokeAsync(newPath);
        }
    }

    /// <summary>
    /// Sets the busy state of the component.
    /// </summary>
    /// <remarks>Call this method to update the component's busy status and trigger a UI refresh. This is
    /// typically used to reflect ongoing operations such as loading or processing.</remarks>
    /// <param name="isBusy">true to indicate the component is busy; otherwise, false.</param>
    public void SetBusy(bool isBusy)
    {
        IsBusy = isBusy;
        StateHasChanged();
    }
}
