// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components;

public partial class FileMoverDialog<TItem> : IDialogContentComponent<FileManagerEntry<TItem>> where TItem : class, new()
{
    private ITreeViewItem? _currentSelectedItem;
    private readonly List<TreeViewItem> _items = [];
    private static readonly Icon IconCollapsed = new Size20.Folder();
    private static readonly Icon IconExpanded = new Size20.FolderOpen();

    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public FileManagerEntry<TItem> Content { get; set; } = default!;

    private async Task OnCancelAsync()
    {
        await Dialog.CancelAsync();
    }

    private async Task OnCloseAsync()
    {
        var entry = FileManagerEntry<TItem>.Find(Content, _currentSelectedItem!.Id);
        await Dialog.CloseAsync(entry);
    }

    private void BuildTreeView()
    {
        _items.Clear();
        var root = BuildTreeViewItem(Content, true);
        _items.Add(root);
    }

    private static TreeViewItem BuildTreeViewItem(
         FileManagerEntry<TItem> entry,
         bool isExpanded = false)
    {
        return new()
        {
            IconCollapsed = IconCollapsed,
            IconExpanded = IconExpanded,
            Text = entry.Name,
            Id = entry.Id,
            Expanded = isExpanded,
            Items = entry.GetDirectories().Select(x => BuildTreeViewItem(x)).ToList()
        };
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Content is not null)
        {
            BuildTreeView();
        }
    }
}
