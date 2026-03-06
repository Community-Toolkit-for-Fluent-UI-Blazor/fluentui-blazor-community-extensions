using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a file manager grid component that displays file and directory entries in a grid layout, supporting
/// customizable item templates and various view modes.
/// </summary>
/// <remarks>This component allows users to browse, select, and interact with files and directories in a grid
/// format. The layout and appearance of items adapt to the current view mode, such as mosaic or icon sizes. Developers
/// can provide a custom template for rendering each item using the ItemTemplate parameter.</remarks>
/// <typeparam name="TItem">The type of the data item represented by each file or directory entry. Must be a reference type.</typeparam>
public partial class FileManagerGrid<TItem> : FileManagerBase<TItem>
    where TItem : class, new()
{
    /// <summary>
    /// Initializes a new instance of the FileManagerGrid class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to use for initializing the file manager grid. Cannot be null.</param>
    public FileManagerGrid(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the template used to render each file entry in the collection.
    /// </summary>
    /// <remarks>Use this property to customize the appearance and layout of individual file entries by
    /// providing a render fragment that receives a <see cref="FileEntry{TItem}"/> as context. If not set, a default
    /// rendering will be used.</remarks>
    [Parameter]
    public RenderFragment<FileEntry<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets the column width in pixels based on the current file view mode.
    /// </summary>
    /// <remarks>The returned width varies depending on the selected view mode, allowing the UI to adjust
    /// column sizes for different icon layouts.</remarks>
    private string ColumnWidth => Parent?.State.View switch
    {
        FileView.Mosaic => "300px",
        FileView.VeryLargeIcons => "250px",
        FileView.LargeIcons => "220px",
        FileView.MediumIcons => "200px",
        FileView.SmallIcons => "300px",
        _ => "300px"
    };

    /// <summary>
    /// Gets the CSS height value, in pixels, for a row based on the current file view mode.
    /// </summary>
    /// <remarks>The returned height varies depending on the selected file view, such as Mosaic,
    /// VeryLargeIcons, LargeIcons, MediumIcons, or SmallIcons. This value is intended for use in styling UI elements to
    /// ensure consistent row sizing across different view modes.</remarks>
    private string RowHeight => Parent?.State.View switch
    {
        FileView.Mosaic => "60px",
        FileView.VeryLargeIcons => "250px",
        FileView.LargeIcons => "220px",
        FileView.MediumIcons => "200px",
        FileView.SmallIcons => "100px",
        _ => "100px"
    };

    /// <summary>
    /// Updates the selection state of the specified item and notifies listeners of the change asynchronously.
    /// </summary>
    /// <remarks>This method modifies the collection of selected items and triggers the associated change
    /// event. Use this method to respond to user interactions that alter item selection.</remarks>
    /// <param name="entry">The file entry whose selection state is being changed.</param>
    /// <param name="isSelected">A value indicating whether the item should be selected. Set to <see langword="true"/> to select the item;
    /// otherwise, <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnCheckedItemChangedAsync(FileEntry<TItem> entry, bool isSelected)
    {
        var items = SelectedItems.ToList();

        if (isSelected)
        {
            items.Add(entry);
        }
        else
        {
            items.Remove(entry);
        }

        SelectedItems = items;
        await SelectedItemsChanged.InvokeAsync(SelectedItems);
    }
}
