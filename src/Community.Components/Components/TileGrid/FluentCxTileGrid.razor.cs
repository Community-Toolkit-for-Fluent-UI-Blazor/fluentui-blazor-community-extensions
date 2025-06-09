// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxTileGrid<TItem>
    : FluentComponentBase
{
    public FluentCxTileGrid()
    {
        Id = StringHelper.GenerateId();
    }

    private IGridSettings GridSettings => new TileGridSettings()
    {
        Columns = Columns,
        ColumnWidth = ColumnWidth,
        Height = Height,
        MinimumColumnWidth = MinimumColumnWidth,
        RowHeight = RowHeight,
        Width = Width,
        MinimumRowHeight = MinimumRowHeight
    };

    internal FluentCxDropZoneContainer<TItem>? _dropContainer;

    [Parameter]
    public bool CanOverflow { get; set; }

    [Parameter]
    public bool Immediate { get; set; } = true;

    [Parameter]
    public Func<TItem, TItem>? CloneItem { get; set; }

    [Parameter]
    public bool CanReorder { get; set; }

    [Parameter]
    public bool CanResize { get; set; }

    [Parameter]
    public Func<TItem, bool>? IsDragAllowed { get; set; }

    [Parameter]
    public Func<TItem?, TItem?, bool>? IsDropAllowed { get; set; }

    [Parameter]
    public IList<TItem>? Items { get; set; } = [];

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ItemContent { get; set; }

    [Parameter]
    public Func<TItem, string>? ItemCss {  get; set; }

    [Parameter]
    public string ColumnWidth { get; set; } = "1fr";

    [Parameter]
    public string? MinimumColumnWidth { get; set; }

    [Parameter]
    public string? MinimumRowHeight { get; set; }

    [Parameter]
    public int? Columns { get; set; }

    [Parameter]
    public string RowHeight { get; set; } = "1fr";

    [Parameter]
    public string? Width { get; set; } = "100%";

    [Parameter]
    public string? Height { get; set; } = "100%";
}
