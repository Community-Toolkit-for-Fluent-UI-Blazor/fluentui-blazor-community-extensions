// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Comparers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Tile Grid where the items can be resized or reordered.
/// </summary>
public partial class FluentCxTileGrid<TItem>
    : FluentComponentBase where TItem : class, new()
{
    private readonly RenderFragment _contentFragment;

    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<TileGridItemResizeHandle, string> _ltrResize = new(EqualityComparer<TileGridItemResizeHandle>.Default)
    {
        [TileGridItemResizeHandle.Horizontally] = "top: 0px; right: 0px; bottom: 0px; width: 9px;",
        [TileGridItemResizeHandle.Vertically] = "left: 0px; right: 0px; bottom: 0px; height: 9px;",
        [TileGridItemResizeHandle.Both] = "right: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<TileGridItemResizeHandle, string> _rtlResize = new(EqualityComparer<TileGridItemResizeHandle>.Default)
    {
        [TileGridItemResizeHandle.Horizontally] = "top: 0px; left: 0px; bottom: 0px; width: 9px;",
        [TileGridItemResizeHandle.Vertically] = "left: 0px; left: 0px; bottom: 0px; height: 9px;",
        [TileGridItemResizeHandle.Both] = "left: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    [Inject]
    public GlobalState GlobalState { get; set; } = default!;

    internal Dictionary<TileGridItemResizeHandle, string> ResizeHandles => GlobalState.Dir == LocalizationDirection.LeftToRight ? _ltrResize : _rtlResize;

    [Parameter]
    public bool CanReorder { get; set; }

    [Parameter]
    public bool CanResize { get; set; }

    [Parameter]
    public string? DataId { get; set; }

    /// <summary>
    /// Gets or sets the number of columns in the <see cref="FluentCxTileGrid"/>.
    /// </summary>
    /// <remarks>
    /// When Columns is set to 0 (or below 0), the grid will use auto-fit to fill the container.
    /// </remarks>
    [Parameter]
    public int Columns { get; set; } = 0;

    /// <summary>
    /// Gets or sets the height of the rows.
    /// </summary>
    [Parameter]
    public string RowHeight { get; set; } = "1fr";

    /// <summary>
    /// Gets or sets the height of the rows.
    /// </summary>
    /// <remarks>
    /// Use this parameter only if you set the IsVirtualized parameter to <see langword="true" />.
    /// </remarks>
    [Parameter]
    public float RowHeightInt { get; set; }

    /// <summary>
    /// Gets or sets the width of the columns.
    /// </summary>
    [Parameter]
    public string ColumnWidth { get; set; } = "1fr";

    [Parameter]
    public string? MinimumColumnWidth { get; set; }

    [Parameter]
    public int Spacing { get; set; } = 3;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("grid-template-columns", GetColumns())
        .AddStyle("grid-auto-rows", GetRows())
        .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
        .Build();

    private string? InternalClass => new CssBuilder(Class)
            .AddClass("fluent-tile-grid")
            .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    private void OnDragEnd(FluentDragEventArgs<FluentCxTileGridItem<TItem>> e)
    {
        System.Diagnostics.Debug.WriteLine("Drag end");
    }

    private void OnDropEnd(FluentDragEventArgs<FluentCxTileGridItem<TItem>> e)
    {
        if (!string.IsNullOrEmpty(e.Source.Id) &&
            !string.IsNullOrEmpty(e.Target.Id))
        {
            //var sourceIndex = _children.FindIndex(x => x.Id == e.Source.Id);
            //var destIndex = _children.FindIndex(x => x.Id == e.Target.Id);

            //if (sourceIndex >= 0 &&
            //    destIndex >= 0)
            //{
            //    var firstElement = _children[sourceIndex];
            //    var lastElement = _children[destIndex];

            //    (lastElement.Order, firstElement.Order) = (firstElement.Order, lastElement.Order);
            //    _children.Sort(FluentCxTileGridItemComparer<TItem>.Default);
            //}
        }
    }

    private string GetRows()
    {
        DefaultInterpolatedStringHandler handler = new();

        // Rows
        handler.AppendLiteral("minmax(0px, ");
        handler.AppendFormatted(RowHeight);
        handler.AppendLiteral(");");
        handler.AppendLiteral("px; padding: ");
        handler.AppendFormatted(Spacing * 4);
        handler.AppendLiteral("px;");

        return handler.ToString();
    }

    private string GetColumns()
    {
        DefaultInterpolatedStringHandler handler = new();

        // Columns
        handler.AppendLiteral("repeat(");

        if (Columns > 0)
        {
            handler.AppendFormatted(Columns);
        }
        else
        {
            handler.AppendLiteral("auto-fit");
        }

        handler.AppendLiteral(", minmax(");

        if (string.IsNullOrEmpty(MinimumColumnWidth))
        {
            handler.AppendLiteral("0px, ");
        }
        else
        {
            handler.AppendFormatted(MinimumColumnWidth);
            handler.AppendLiteral(", ");
        }

        handler.AppendFormatted(ColumnWidth);
        handler.AppendLiteral("));");

        return handler.ToString();
    }

    protected internal virtual void OnItemParemetersChanged(FluentCxTileGridItem<TItem> _)
    {
        StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ChildContent is not null && Items.Any())
        {
            throw new NotSupportedException("The FluentCxTileGrid cannot use both ChildContent and Items properties at the same time. Use ChildContent or Items");
        }
    }
}

