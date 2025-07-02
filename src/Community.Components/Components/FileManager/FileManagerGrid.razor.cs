using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the grid of the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FileManagerGrid<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    /// <summary>
    /// Gets or sets the template of the items in the grid.
    /// </summary>
    [Parameter]
    public RenderFragment<FileManagerEntry<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets the height of the row from the <see cref="FileView"/> value.
    /// </summary>
    /// <returns>Returns the height of the row.</returns>
    private string GetRowHeightFromGridViewOptions()
    {
        return Parent?.State.View switch
        {
            FileView.Mosaic => "100px",
            FileView.VeryLargeIcons => "250px",
            FileView.LargeIcons => "220px",
            FileView.MediumIcons => "200px",
            _ => "100px"
        };
    }

    /// <summary>
    /// Gets the width of the column from the <see cref="FileView"/> value.
    /// </summary>
    /// <returns>Returns the width of the column.</returns>
    private string GetColumnWidthFromGridViewOptions()
    {
        return Parent?.State.View switch
        {
            FileView.Mosaic => "300px",
            FileView.VeryLargeIcons => "250px",
            FileView.LargeIcons => "220px",
            FileView.MediumIcons => "200px",
            _ => "300px"
        };
    }

    /// <summary>
    /// Gets the size of the icon from the <see cref="FileView"/> value.
    /// </summary>
    /// <returns>Returns the size of the icons.</returns>
    private string GetIconSizeFromGridViewOptions()
    {
        return Parent?.State.View switch
        {
            FileView.VeryLargeIcons => "128px",
            FileView.LargeIcons => "96px",
            FileView.MediumIcons => "72px",
            _ => "128px"
        };
    }

    /// <summary>
    /// Gets the max width of the text from the <see cref="FileView"/> value.
    /// </summary>
    /// <returns></returns>
    private int GetMaxWidthFromGridViewOptions()
    {
        return Parent?.State.View switch
        {
            FileView.VeryLargeIcons => 184,
            FileView.LargeIcons => 154,
            FileView.MediumIcons => 134,
            _ => 180
        };
    }

    /// <summary>
    /// Occurs when an item is selected or unselected.
    /// </summary>
    /// <param name="entry">Entry on which occurs the selection.</param>
    /// <param name="isSelected">Value indicating if the entry is selected or unselected.</param>
    /// <returns>A task which contains the selecteditems when completed.</returns>
    private async Task OnCheckedItemChangedAsync(FileManagerEntry<TItem> entry, bool isSelected)
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

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }
}
