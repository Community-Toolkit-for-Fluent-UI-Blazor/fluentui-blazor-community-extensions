using System.Runtime.CompilerServices;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerList<TItem>
    : FileManagerBase<TItem> where TItem : class, new()
{
    private string InternalStyle => GetStyle();

    private async Task OnRowDoubleClickAsync(FluentDataGridRow<FileManagerEntry<TItem>> e)
    {
        await OnItemDoubleTappedAsync(e.Item);
    }

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
