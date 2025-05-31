// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public class FileManagerBase<TItem> : ComponentBase where TItem : class, new()
{
    [CascadingParameter]
    private protected FluentCxFileManager<TItem>? Parent { get; set; }

    [Parameter]
    public FileManagerEntry<TItem>? Entry { get; set; }

    [Parameter]
    public IEnumerable<FileManagerEntry<TItem>> SelectedItems { get; set; } = [];

    [Parameter]
    public EventCallback<IEnumerable<FileManagerEntry<TItem>>> SelectedItemsChanged { get; set; }

    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemTapped { get; set; }

    [Parameter]
    public EventCallback<FileManagerEntryEventArgs<TItem>> OnItemDoubleTapped { get; set; }

    [Parameter]
    public bool IsMobile { get; set; }

    [Parameter]
    public bool IsBusy { get; set; }

    [Parameter]
    public bool IsIndeterminateProgress { get; set; }

    [Parameter]
    public int? ProgressPercent { get; set; }

    [Parameter]
    public string? ProgressLabel { get; set; }

    [Parameter]
    public FileListDataGridColumnLabels ColumnLabels { get; set; } = FileListDataGridColumnLabels.Default;

    protected async Task OnSelectedItemsChangedAsync()
    {
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }

    protected async Task OnItemDoubleTappedAsync(
        FileManagerEntry<TItem>? entry)
    {
        if (OnItemDoubleTapped.HasDelegate && entry is not null)
        {
            await OnItemDoubleTapped.InvokeAsync(new(entry));
        }
    }

    protected async Task OnItemTappedAsync(
        FileManagerEntry<TItem> entry)
    {
        if (OnItemTapped.HasDelegate)
        {
            await OnItemTapped.InvokeAsync(new(entry));
        }
    }

    protected static async Task OnMenuItemClickedAsync(
        Func<FileManagerEntry<TItem>, Task>? clickFunc,
        FileManagerEntry<TItem> entry)
    {
        if (clickFunc is not null)
        {
            await clickFunc(entry);
        }
    }

    protected static Icon GetIconFromFile(string extension)
    {
        return FileManagerIcons.FromExtension(extension);
    }

    protected async Task OnRenameAsync(FileManagerEntry<TItem> entry)
    {
        if (Parent is not null)
        {
            await Parent.OnRenameAsync(entry);
        }
    }

    public void Refresh()
    {
        StateHasChanged();
    }
}

