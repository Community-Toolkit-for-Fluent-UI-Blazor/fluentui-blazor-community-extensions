using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Tile Grid component.
/// </summary>
/// <typeparam name="TItem">Type of the component.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public class FluentCxTileGrid<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxTileGrid{TItem}"/> component.
    /// </summary>
    public FluentCxTileGrid() : base()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the settings for the tile grid.
    /// </summary>
    private ITileGridSettings GridSettings => new TileGridSettings()
    {
        Columns = Columns,
        ColumnWidth = ColumnWidth,
        Height = Height,
        MinimumColumnWidth = MinimumColumnWidth,
        RowHeight = RowHeight,
        Width = Width,
        MinimumRowHeight = MinimumRowHeight
    };

    /// <summary>
    /// Gets the <see cref="FluentCxDropZoneContainer{TItem}"/> component.
    /// </summary>
    internal FluentCxDropZoneContainer<TItem>? _dropContainer;

    /// <summary>
    /// Gets or sets a value indicating if the component can overflow.
    /// </summary>
    [Parameter]
    public bool CanOverflow { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the drap and drop is immediate. 
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = true;

    /// <summary>
    /// Gets or sets a function to clone an item of the grid.
    /// </summary>
    [Parameter]
    public Func<TItem, TItem>? CloneItem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the component can reorder its item.
    /// </summary>
    [Parameter]
    public bool CanReorder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the component can resize its item.
    /// </summary>
    [Parameter]
    public bool CanResize { get; set; }

    /// <summary>
    /// Gets or sets a function which allows an item can be dragged or not.
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? IsDragAllowed { get; set; }

    /// <summary>
    /// Gets or sets a function which allows the item can be dropped or not.
    /// </summary>
    [Parameter]
    public Func<TItem?, TItem?, bool>? IsDropAllowed { get; set; }

    /// <summary>
    /// Gets or sets the items to render.
    /// </summary>
    [Parameter]
    public IList<TItem>? Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the template for the item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemContent { get; set; }

    /// <summary>
    /// Gets or sets the css for an item.
    /// </summary>
    [Parameter]
    public Func<TItem, string>? ItemCss { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    /// <remarks>Default width is 1fr.</remarks>
    [Parameter]
    public string ColumnWidth { get; set; } = "1fr";

    /// <summary>
    /// Gets or sets the minimum width of the column.
    /// </summary>
    [Parameter]
    public string? MinimumColumnWidth { get; set; }

    /// <summary>
    /// Gets or sets the minimum height of the row.
    /// </summary>
    [Parameter]
    public string? MinimumRowHeight { get; set; }

    /// <summary>
    /// Gets or sets the number of columns of the grid.
    /// </summary>
    [Parameter]
    public int? Columns { get; set; }

    /// <summary>
    /// Gets or sets the height of the row.
    /// </summary>
    /// <remarks>
    /// Default height is 1fr.
    /// </remarks>
    [Parameter]
    public string RowHeight { get; set; } = "1fr";

    /// <summary>
    /// Gets or sets the width of the grid.
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the height of the grid.
    /// </summary>
    [Parameter]
    public string? Height { get; set; } = "100%";

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenComponent<CascadingValue<FluentCxTileGrid<TItem>>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<FluentCxDropZoneContainer<TItem>>.Value), this);
        builder.AddComponentParameter(2, nameof(CascadingValue<FluentCxDropZoneContainer<TItem>>.IsFixed), true);
        builder.AddChildContent(3, __builder2 =>
        {
            __builder2.OpenComponent<FluentCxDropZoneContainer<TItem>>(4);
            __builder2.AddComponentParameter(5, nameof(FluentCxDropZoneContainer<TItem>.Immediate), RuntimeHelpers.TypeCheck(Immediate));
            __builder2.AddComponentParameter(6, nameof(FluentCxDropZoneContainer<TItem>.CloneItem), CloneItem);
            __builder2.AddComponentParameter(7, nameof(FluentCxDropZoneContainer<TItem>.IsDragAllowed), IsDragAllowed);
            __builder2.AddComponentParameter(8, nameof(FluentCxDropZoneContainer<TItem>.IsDropAllowed), IsDropAllowed);
            __builder2.AddComponentParameter(9, nameof(FluentCxDropZoneContainer<TItem>.IsDragEnabled), CanReorder);
            __builder2.AddComponentParameter(10, nameof(FluentCxDropZoneContainer<TItem>.Items), Items);
            __builder2.AddComponentParameter(11, nameof(FluentCxDropZoneContainer<TItem>.ItemCss), ItemCss);
            __builder2.AddComponentParameter(12, nameof(FluentCxDropZoneContainer<TItem>.Id), Id);
            __builder2.AddComponentParameter(13, nameof(FluentCxDropZoneContainer<TItem>.ChildContent), ChildContent);
            __builder2.AddComponentParameter(14, nameof(FluentCxDropZoneContainer<TItem>.ItemContent), ItemContent);
            __builder2.AddComponentParameter(15, nameof(FluentCxDropZoneContainer<TItem>.TileGridSettings), GridSettings);
            __builder2.AddComponentParameter(16, nameof(FluentCxDropZoneContainer<TItem>.CanOverflow), CanOverflow);
            __builder2.AddComponentReferenceCapture(17, (__value) =>
            {
                _dropContainer = (FluentCxDropZoneContainer<TItem>)__value;
            }
            );
            __builder2.CloseComponent();
        });

        builder.CloseComponent();
    }
}
