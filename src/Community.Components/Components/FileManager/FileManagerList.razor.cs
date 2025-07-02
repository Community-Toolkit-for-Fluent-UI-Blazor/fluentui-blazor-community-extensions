using System.Runtime.CompilerServices;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the list view for the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FileManagerList<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    /// <summary>
    /// Gets the style of the component.
    /// </summary>
    private string InternalStyle => GetStyle();

    /// <summary>
    /// Occurs when the row is double clicked.
    /// </summary>
    /// <param name="e">Event args associated to the clicked row.</param>
    /// <returns>Returns a task which perform the double click when completed.</returns>
    private async Task OnRowDoubleClickAsync(FluentDataGridRow<FileManagerEntry<TItem>> e)
    {
        await OnItemDoubleTappedAsync(e.Item);
    }

    /// <summary>
    /// Gets the style of the list.
    /// </summary>
    /// <returns>Returns the style of the list.</returns>
    private string GetStyle()
    {
        DefaultInterpolatedStringHandler handler = new();
        handler.AppendLiteral("height: calc(100% - 41px); width: 100%; overflow-y: auto;");

        if (Parent?.View == FileManagerView.Mobile)
        {
            handler.AppendLiteral("overflow-x: auto;");
        }

        return handler.ToStringAndClear();
    }
}
