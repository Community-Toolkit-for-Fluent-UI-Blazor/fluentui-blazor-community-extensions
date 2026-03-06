using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dialog component that enables moving a file or directory entry within a file system structure.
/// </summary>
public partial class MoveDialog : FluentDialogInstance
{
    /// <summary>
    /// Gets or sets the file entry to move.
    /// </summary>
    private ITreeViewItem? _selectedItem;

    /// <summary>
    /// Gets or sets the collection of items to display in the tree view.
    /// </summary>
    /// <remarks>Each item in the collection must implement the ITreeViewItem interface. The order of items in
    /// the collection determines their display order in the tree view.</remarks>
    [Parameter]
    public ITreeViewItem? Root { get; set; }

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await DialogInstance.CloseAsync(_selectedItem);
        }
        else
        {
            await DialogInstance.CancelAsync();
        }
    }

    /// <summary>
    /// Retrieves an enumerable collection containing the root tree view item, if it exists.
    /// </summary>
    /// <returns>An enumerable collection containing the root item if it is not null; otherwise, an empty collection.</returns>
    private IEnumerable<ITreeViewItem>? GetItems()
    {
        if (Root is not null)
        {
            return [Root];
        }

        return [];
    }
}
