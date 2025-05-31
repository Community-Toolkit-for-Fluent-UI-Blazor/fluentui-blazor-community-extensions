// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerGrid<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    [Parameter]
    public RenderFragment<FileManagerEntry<TItem>>? ItemTemplate { get; set; }

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
