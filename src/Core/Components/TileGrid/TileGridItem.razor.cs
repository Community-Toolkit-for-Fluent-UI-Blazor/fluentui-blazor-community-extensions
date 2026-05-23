using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item within a tile grid layout, providing a configurable visual component for display in grid-based
/// user interfaces.
/// </summary>
/// <typeparam name="TItem">The type of the item associated with the tile grid item, which must implement the ITileGridItem interface.</typeparam>
/// <remarks>The TileGridItem class is intended for use within tile grid structures, allowing for flexible
/// arrangement and customization of visual elements. A valid LibraryConfiguration instance must be provided when
/// creating a TileGridItem to ensure proper initialization and behavior.</remarks>
public partial class TileGridItem<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the TileGridItem class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that define the behavior and properties of the TileGridItem instance. Cannot be null.</param>
    public TileGridItem(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>Use this property to specify a fragment of UI content that will be rendered as the child
    /// content of the component. This is typically set by including markup between the component's opening and closing
    /// tags in a parent component.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is a ghost.
    /// </summary>
    [Parameter]
    public bool IsGhost { get; set; }

    /// <summary>
    /// Gets or sets the number of rows that the cell spans within the table layout.
    /// </summary>
    /// <remarks>The default value is 1, which means the cell occupies a single row. Set this property to a
    /// higher value to allow the cell to extend vertically across multiple rows, which can be useful for creating
    /// complex table structures.</remarks>
    [Parameter]
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of columns that the element spans within the layout.
    /// </summary>
    /// <remarks>The default value is 1, which means the element occupies a single column. Increasing this
    /// value allows the element to span multiple columns, which can be useful for creating flexible or responsive
    /// layouts.</remarks>
    [Parameter]
    public int ColumnSpan { get; set; } = 1;

    /// <summary>
    /// Gets or sets the index of the row that this item occupies within the grid.
    /// </summary>
    /// <remarks>The default value is 1. This property determines which row of the grid the item will be
    /// rendered in. Row indices are typically one-based, with 1 representing the first row.</remarks>
    [Parameter]
    public int Row { get; set; } = 1;

    /// <summary>
    /// Gets or sets the zero-based column index that determines the horizontal position of the layout element within
    /// the grid.
    /// </summary>
    /// <remarks>The default value is 1, which typically corresponds to the first column. Ensure that the
    /// value assigned is within the valid range of columns defined by the grid layout. Setting a value outside the
    /// valid range may result in layout errors or unexpected behavior.</remarks>
    [Parameter]
    public int Column { get; set; } = 1;

    /// <summary>
    /// Gets or sets the item associated with the current context.
    /// </summary>
    /// <remarks>The value of this property may be null. Check for null before accessing members of the item
    /// to avoid runtime exceptions.</remarks>
    [Parameter]
    public TItem? Item { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current item can be reordered within its parent collection.
    /// </summary>
    /// <remarks>This property returns <see langword="false"/> if the parent does not allow reordering. It is
    /// useful for determining the reordering capability of items in a hierarchical structure.</remarks>
    private bool CanReorder => Parent?.CanActuallyReorder ?? false;

    /// <summary>
    /// Gets a value indicating whether the parent element supports resizing.
    /// </summary>
    /// <remarks>This property returns <see langword="false"/> if the parent element is <see langword="null"/>
    /// or if the parent cannot be resized. Use this property to determine whether resizing operations are permitted
    /// based on the parent's capabilities.</remarks>
    private bool CanResize => Parent?.CanActuallyResize ?? false;

    /// <summary>
    /// Gets or sets the parent tile grid that contains this tile item.
    /// </summary>
    /// <remarks>This property is automatically set when the tile item is added to a parent grid. It is used
    /// to establish the relationship between the tile item and its containing grid in a cascading parameter
    /// context.</remarks>
    [CascadingParameter]
    private FluentCxTileGrid<TItem>? Parent { get; set; }

    private string? Css => DefaultClassBuilder.AddClass("tile-grid-item-container").Build();

    /// <summary>
    /// Gets the computed CSS style string for the current element, including row and column span settings.
    /// </summary>
    /// <remarks>The returned style string is generated dynamically based on the current values of the RowSpan
    /// and ColumnSpan properties. This property is intended for internal use and may not be suitable for direct
    /// manipulation.</remarks>
    private string? InternalStyle
    {
        get
        {
            var reorderable = AsReorderable();

            var style = DefaultStyleBuilder
                .AddStyle("display", "grid")
                .AddStyle("grid-row-end", $"span {RowSpan}")
                .AddStyle("grid-column-end", $"span {ColumnSpan}")
                .AddStyle("grid-row-start", $"{Row + 1}", CanResize || CanReorder)
                .AddStyle("grid-column-start", $"{Column + 1}", CanResize || CanReorder)
                .AddStyle("order", $"{reorderable?.Index}", CanReorder);

            if (IsGhost)
            {
                style
                    .AddStyle("position", "absolute")
                    .AddStyle("visibility", "hidden")
                    .AddStyle("pointer-events", "none");
            }

            ApplyHighlightStyles(style, reorderable);

            return style.Build();
        }
    }

    /// <summary>
    /// Gets the CSS class string that represents the visual state of the tile grid item based on its reorderable status
    /// and highlight style.
    /// </summary>
    /// <remarks>The returned CSS class string is determined by whether the item can be reordered and if it is
    /// currently a preview target with an animated glow highlight. This property is intended for use in dynamically
    /// styling UI elements within a tile grid.</remarks>
    private string? InternalClass
    {
        get
        {
            var reorderable = AsReorderable();

            return new CssBuilder()
                .AddClass("tile-grid-item-animated-glow", CanReorder && reorderable is not null && reorderable.IsPreviewTarget && Parent?.HighlightStyle == TileDragOverHighlightStyle.AnimatedGlow)
                .Build();
        }
    }

    /// <summary>
    /// Applies highlight styles to a reorderable tile grid item based on the current highlight style of the parent
    /// grid.
    /// </summary>
    /// <remarks>Highlight styles are determined by the parent's highlight style setting. No styles are
    /// applied if reordering is not allowed or if the item is not a preview target.</remarks>
    /// <param name="builder">The style builder used to apply CSS styles to the tile grid item.</param>
    /// <param name="reorderable">The tile grid item to which highlight styles are applied. Styles are only applied if the item is a valid preview
    /// target.</param>
    private void ApplyHighlightStyles(StyleBuilder builder, ITileGridItem? reorderable)
    {
        if (!CanReorder || reorderable is null || !reorderable.IsPreviewTarget)
        {
            return;
        }

        switch (Parent?.HighlightStyle)
        {
            case TileDragOverHighlightStyle.Glow:
                builder
                    .AddStyle("outline", "2px solid var(--colorBrandForeground1)")
                    .AddStyle("box-shadow", "0 0 8px 2px var(--colorBrandForeground1)");
                break;

            case TileDragOverHighlightStyle.SideBar:
                builder
                    .AddStyle("border-left", "4px solid var(--colorBrandForeground1)")
                    .AddStyle("background", "color-mix(in srgb, var(--colorBrandForeground1) 10%, transparent)");
                break;

            case TileDragOverHighlightStyle.FocusRing:
                builder
                    .AddStyle("outline", "2px solid var(--colorBrandForeground1)")
                    .AddStyle("outline-offset", "3px")
                    .AddStyle("border-radius", "8px");
                break;

            case TileDragOverHighlightStyle.AnimatedGlow:
                builder.AddStyle("outline", "2px solid var(--colorBrandForeground1)");
                break;
        }
    }

    /// <summary>
    /// Returns the current item as an ITileGridItem if reordering is enabled; otherwise, returns null.
    /// </summary>
    /// <remarks>Ensure that the type specified for TItem implements ITileGridItem when reordering is enabled
    /// to avoid exceptions.</remarks>
    /// <returns>An ITileGridItem representing the current item if reordering is allowed; otherwise, null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if reordering is enabled and the current item does not implement ITileGridItem.</exception>
    private ITileGridItem? AsReorderable()
    {
        if (!CanReorder && !CanResize)
        {
            return null;
        }

        if (Item is ITileGridItem reorderable)
        {
            return reorderable;
        }

        throw new InvalidOperationException($"""
             TileGridItem requires TItem to implement ITileGridItem when CanReorder = true.

             → TItem provided : {typeof(TItem).FullName}
             → Expected : a type implementing ITileGridItem

             Fix:
             - Either disable CanReorder
             - Or implement ITileGridItem on your model 
            """);
    }

    /// <summary>
    /// Handles the mouse down event on the resize handle to initiate a resize operation if resizing is permitted.
    /// </summary>
    /// <remarks>Resizing is only initiated if the item is present and resizing is allowed. This method
    /// delegates the resize operation to the parent component.</remarks>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing data about the mouse event, including the cursor position.</param>
    private void OnResizeHandleBottomMouseDown(MouseEventArgs e)
    {
        if (!CanResize || Item is null)
        {
            return;
        }

        Parent?.BeginResize(Item, e.ClientX, e.ClientY, TileGridItemResizeHandle.Bottom);
    }

    /// <summary>
    /// Handles the mouse down event on the resize handle to initiate a resize operation if resizing is permitted.
    /// </summary>
    /// <remarks>Resizing is only initiated if the item is present and resizing is allowed. This method
    /// delegates the resize operation to the parent component.</remarks>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing data about the mouse event, including the cursor position.</param>
    private void OnResizeHandleRightMouseDown(MouseEventArgs e)
    {
        if (!CanResize || Item is null)
        {
            return;
        }

        Parent?.BeginResize(Item, e.ClientX, e.ClientY, TileGridItemResizeHandle.Right);
    }

    /// <summary>
    /// Handles the mouse down event on the corner resize handle to initiate a resize operation if resizing is
    /// permitted.
    /// </summary>
    /// <remarks>This method checks whether resizing is allowed and whether the associated item is not null
    /// before starting the resize operation. If both conditions are met, it calls the parent's resize initiation method
    /// with the current mouse position.</remarks>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the mouse event data, including the client coordinates of
    /// the mouse pointer.</param>
    private void OnResizeHandleCornerMouseDown(MouseEventArgs e)
    {
        if (!CanResize || Item is null)
        {
            return;
        }

        Parent?.BeginResize(Item, e.ClientX, e.ClientY, TileGridItemResizeHandle.BottomRight);
    }

    /// <summary>
    /// Gets the size of the handle as a percentage of the parent element's size, or returns a default value if the
    /// parent is not available.
    /// </summary>
    /// <remarks>This method is useful for determining the proportional size of a handle relative to its
    /// parent container. If the parent element is not set, a default value of "20%" is used to ensure consistent
    /// behavior.</remarks>
    /// <returns>A string that represents the handle size as a percentage of the parent element's size. Returns "20%" if the
    /// parent element is null.</returns>
    private string GetHandleSize()
    {
        return Parent?.GetHandleSize() ?? "20%";
    }
}
