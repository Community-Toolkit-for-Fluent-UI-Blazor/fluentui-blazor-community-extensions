using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the file mover dialog.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FileMoverDialog<TItem> : IDialogContentComponent<FileManagerEntry<TItem>> where TItem : class, new()
{
    /// <summary>
    /// Represents the selected item in the tree.
    /// </summary>
    private ITreeViewItem? _currentSelectedItem;

    /// <summary>
    /// Represents the tree.
    /// </summary>
    private readonly List<TreeViewItem> _items = [];

    /// <summary>
    /// Represents the collapsed node icon.
    /// </summary>
    private static readonly Icon IconCollapsed = new Size20.Folder();

    /// <summary>
    /// Represents the expanded node icon.
    /// </summary>
    private static readonly Icon IconExpanded = new Size20.FolderOpen();

    /// <summary>
    /// Gets or sets the dialog that this content belongs to.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content of the dialog.
    /// </summary>
    [Parameter]
    public FileManagerEntry<TItem> Content { get; set; } = default!;

    /// <summary>
    /// Occurs when the dialog is cancelled.
    /// </summary>
    /// <returns>Returns a task which cancels the dialog when completed.</returns>
    private async Task OnCancelAsync()
    {
        await Dialog.CancelAsync();
    }

    /// <summary>
    /// Occurs when the dialog is closed.
    /// </summary>
    /// <returns>Returns a task which close the dialog when completed.</returns>
    private async Task OnCloseAsync()
    {
        var entry = FileManagerEntry<TItem>.Find(Content, _currentSelectedItem!.Id);
        await Dialog.CloseAsync(entry);
    }

    /// <summary>
    /// Build the treeview from the <see cref="Content"/> root.
    /// </summary>
    private void BuildTreeView()
    {
        _items.Clear();
        var root = BuildTreeViewItem(Content, true);
        _items.Add(root);
    }

    /// <summary>
    /// Build the treeviewitem from the <paramref name="entry"/> node.
    /// </summary>
    /// <param name="entry">Entry to build as a treeview item.</param>
    /// <param name="isExpanded">Value indicating if the node is expanded or not.</param>
    /// <returns>Returns a <see cref="TreeViewItem"/> which represents the <paramref name="entry"/> node.</returns>
    private static TreeViewItem BuildTreeViewItem(
         FileManagerEntry<TItem> entry,
         bool isExpanded = false)
    {
        return new()
        {
            IconCollapsed = IconCollapsed,
            IconExpanded = IconExpanded,
            Text = entry.Name,
            Id = entry.ViewId,
            Expanded = isExpanded,
            Items = entry.GetDirectories().Select(x => BuildTreeViewItem(x)).ToList()
        };
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Content is not null)
        {
            BuildTreeView();
        }
    }
}
