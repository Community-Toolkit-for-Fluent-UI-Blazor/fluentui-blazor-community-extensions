using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerGrid<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    [Parameter]
    public RenderFragment<FileManagerEntry<TItem>>? ItemTemplate { get; set; }

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
