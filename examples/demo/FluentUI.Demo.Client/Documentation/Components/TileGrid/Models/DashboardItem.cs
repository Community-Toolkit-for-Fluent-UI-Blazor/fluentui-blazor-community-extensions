using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Demo.Client.Documentation.Components.TileGrid.Models;

internal sealed record DashboardItem
    : ITileGridItem
{
    public DashboardItem()
    {
    }

    public DashboardItem(int index, int columnSpan, int rowSpan)
    {
        Index = index;
        ColumnSpan = columnSpan;
        RowSpan = rowSpan;
    }

    public long Index { get; set; }

    public int RowSpan { get; set; }

    public int ColumnSpan { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }

    public long? PreviewIndex { get; set; }

    public bool IsDragging { get; set; }

    public bool IsPreviewTarget { get; set; }

    public string Key { get; } = Identifier.NewId();
}
