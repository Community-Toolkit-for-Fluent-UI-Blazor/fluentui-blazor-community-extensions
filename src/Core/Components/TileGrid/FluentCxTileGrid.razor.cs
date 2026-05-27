using System.Globalization;
using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a grid layout component for displaying tiles using a fluent interface style.
/// </summary>
/// <typeparam name="TItem">The type of items contained within the tile grid, which must implement the ITileGridItem interface.</typeparam>
/// <remarks>Use this class to arrange and display a collection of tiles in a customizable grid format. The
/// component is suitable for scenarios such as dashboards, galleries, or any UI requiring a flexible tile-based layout.
/// Tile properties and behaviors can be configured to fit various application needs.</remarks>
public partial class FluentCxTileGrid<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Represents the file name and relative path of the JavaScript file used for the FluentCx Tile Grid component.
    /// </summary>
    /// <remarks>This constant is constructed by combining the root JavaScript path with the specific location
    /// of the FluentCxTileGrid.js file. Ensure that the referenced file exists at the specified path to enable proper
    /// client-side functionality of the Tile Grid component.</remarks>
    private const string JavaScriptFileName = FluentCxConstants.JAVASCRIPT_ROOT + "TileGrid/FluentCxTileGrid.razor.js";

    /// <summary>
    /// Represents a reference to the .NET object for interoperation with JavaScript.
    /// </summary>
    private DotNetObjectReference<FluentCxTileGrid<TItem>>? _dotNetRef;

    /// <summary>
    /// Represents whether the CanReorder property has changed.
    /// </summary>
    private bool _hasCanReorderChanged;

    private readonly TileGridState<TItem> _tileGridState = new();

    private TileGridLayoutEngine<TItem>? _tileGridLayoutEngine;

    private TileGridResizeHandler<TItem>? _tileGridResizeHandler;

    private TileGridStorageService<TItem>? _tileGridStorageService;

    private TileGridDragHandler<TItem>? _tileGridDragHandler;

    /// <summary>
    /// Initializes a new instance of the FluentCxTileGrid class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that define the behavior and properties of the tile grid. Cannot be null.</param>
    public FluentCxTileGrid(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

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
    /// Gets or sets the collection of items to be displayed or processed by the component.
    /// </summary>
    /// <remarks>This property supports data binding scenarios and should be initialized before use to avoid
    /// null reference exceptions. Modifying the collection after it is set may not automatically update the UI unless
    /// the component is designed to observe collection changes.</remarks>
    [Parameter]
    public List<TItem> Items { get; set; } = [];

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

    /// <summary>
    /// Gets or sets the vertical spacing between rows in the layout, specified as a CSS length value.
    /// </summary>
    /// <remarks>Set this property to control the space between rows when rendering content in a grid or flex
    /// container. Acceptable values include standard CSS units such as "10px", "1em", or "2rem". If not set, the
    /// default spacing defined by the component or stylesheet will be used.</remarks>
    [Parameter]
    public string? RowGap { get; set; }

    /// <summary>
    /// Gets or sets the function used to retrieve a unique key for each item in the collection.
    /// </summary>
    /// <remarks>The provided function should return a unique string key for each item of type TItem.
    /// Supplying a unique key is important for scenarios such as data binding, efficient rendering, or tracking items
    /// in a collection. If the function returns duplicate or null keys, item identification may not work as
    /// expected.</remarks>
    [Parameter]
    public Func<TItem, string> GetKeyFunc { get; set; } = _ => Identifier.NewId();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>This property allows the parent component to provide custom markup or child components that
    /// will be displayed within this component. Typically, this is used to specify the body content of the component in
    /// Razor syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the template used to render the content for each item in the collection.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of each item by providing a render fragment
    /// that receives the item as a parameter. The specified template will be invoked for every item in the data source,
    /// allowing flexible and dynamic UI composition.</remarks>
    [Parameter]
    public RenderFragment<TItem>? ItemContent { get; set; }

    /// <summary>
    /// Gets or sets the CSS column gap value that defines the spacing between columns in the layout.
    /// </summary>
    /// <remarks>Set this property to specify the distance between columns using any valid CSS length or
    /// keyword, such as "16px", "1em", or "normal". If not set, the default browser styling will be applied.</remarks>
    [Parameter]
    public string? ColumnGap { get; set; }

    /// <summary>
    /// Gets or sets a function that determines the number of columns an item spans in the grid.
    /// </summary>
    /// <remarks>The function receives an item of type TItem and returns an integer specifying the column span
    /// for that item. By default, each item spans a single column. Use this property to customize the layout of items
    /// within the grid based on their content or type.</remarks>
    [Parameter]
    public Func<TItem, int> GetColumnSpanFunc { get; set; } = _ => 1;

    /// <summary>
    /// Gets or sets a function that determines the row of an item spans in the grid.
    /// </summary>
    /// <remarks>The function receives an item of type TItem and returns an integer specifying the row span
    /// for that item. By default, each item spans a single row. Override this function to customize row spanning
    /// behavior based on item data.</remarks>
    [Parameter]
    public Func<TItem, int> GetRowFunc { get; set; } = _ => 1;

    /// <summary>
    /// Gets or sets a function that determines the columns of an item in the grid.
    /// </summary>
    /// <remarks>The function receives an item of type TItem and returns an integer specifying the column span
    /// for that item. By default, each item spans a single column. Use this property to customize the layout of items
    /// within the grid based on their content or type.</remarks>
    [Parameter]
    public Func<TItem, int> GetColumnFunc { get; set; } = _ => 1;

    /// <summary>
    /// Gets or sets a function that determines the number of rows an item spans in the grid.
    /// </summary>
    /// <remarks>The function receives an item of type TItem and returns an integer specifying the row span
    /// for that item. By default, each item spans a single row. Override this function to customize row spanning
    /// behavior based on item data.</remarks>
    [Parameter]
    public Func<TItem, int> GetRowSpanFunc { get; set; } = _ => 1;

    /// <summary>
    /// Gets or sets the storage provider responsible for managing the persistence of tile data in the grid.
    /// </summary>
    /// <remarks>Assign a custom implementation of ITileGridStorage{TItem} to control how tile data is stored
    /// and retrieved. The storage provider should be properly initialized before use. Changing this property at runtime
    /// may affect the state and availability of tile data.</remarks>
    [Parameter]
    public ITileGridStorage<TItem>? StorageProvider { get; set; }

    /// <summary>
    /// Gets or sets the storage key used to identify the associated data.
    /// </summary>
    /// <remarks>This property can be null. Ensure that a valid key is provided when storing or retrieving
    /// data to avoid unexpected behavior.</remarks>
    [Parameter]
    public string? StorageKey { get; set; }

    /// <summary>
    /// Gets or sets the size of the resize handle displayed for the tile grid item.
    /// </summary>
    /// <remarks>Adjusting this property can improve accessibility and visual clarity by changing the area
    /// available for user interaction when resizing a tile grid item.</remarks>
    [Parameter]
    public TileGridItemResizeHandleSize HandleSize { get; set; } = TileGridItemResizeHandleSize.Small;

    /// <summary>
    /// Gets or sets the style used to highlight a tile when it is dragged over another tile.
    /// </summary>
    [Parameter]
    public TileDragOverHighlightStyle HighlightStyle { get; set; } = TileDragOverHighlightStyle.Glow;

    /// <summary>
    /// Gets or sets a value indicating whether the grid is displayed.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the grid is rendered on the interface, providing a visual
    /// structure for the layout. This property can be useful for enhancing the readability of data
    /// presentations.</remarks>
    [Parameter]
    public bool ShowGrid { get; set; }

    /// <summary>
    /// Gets the computed CSS style string for the element based on the current layout and sizing properties.
    /// </summary>
    /// <remarks>The returned style string includes values for width, height, grid gaps, and custom CSS
    /// variables related to column configuration. This property is intended for internal use to facilitate dynamic and
    /// responsive styling of the component.</remarks>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("grid-row-gap", RowGap, !string.IsNullOrEmpty(RowGap))
        .AddStyle("grid-column-gap", ColumnGap, !string.IsNullOrEmpty(ColumnGap))
        .AddStyle("grid-auto-rows", GetRows(), !string.IsNullOrEmpty(RowHeight))
        .AddStyle("--columns", Columns.HasValue ? Columns.Value.ToString(CultureInfo.InvariantCulture) : "auto-fit")
        .AddStyle("--minimum-column-width", string.IsNullOrEmpty(MinimumColumnWidth) ? "0px" : MinimumColumnWidth)
        .AddStyle("--column-width", ColumnWidth)
        .Build();

    /// <summary>
    /// Gets a value indicating whether items in the collection can currently be reordered.
    /// </summary>
    /// <remarks>Reordering is allowed only when the collection supports reordering, contains at least one
    /// item, and the item content is not null. Use this property to determine whether to enable reordering
    /// functionality in the user interface.</remarks>
    internal bool CanActuallyReorder => CanReorder && Items is not null && Items.Count > 0 && ItemContent is not null;

    /// <summary>
    /// Gets a value indicating whether the current instance can be resized based on the presence of items and content.
    /// </summary>
    /// <remarks>Resizing is allowed only when resizing is enabled, there is at least one item present, and
    /// item content is available. This property is intended for internal use.</remarks>
    internal bool CanActuallyResize => CanResize && Items is not null && Items.Count > 0 && ItemContent is not null;

    /// <summary>
    /// Gets the number of columns to display in the grid, based on the configured value or a default internal value.
    /// </summary>
    /// <remarks>If the Columns property is not set or is less than or equal to zero, this property returns an
    /// internally determined column count. Otherwise, it returns the value specified by Columns.</remarks>
    internal int ColumnCount => !Columns.HasValue || Columns.Value <= 0 ? _tileGridState?.ColumnCount ?? 0 : Columns.Value;

    /// <summary>
    /// Gets the callback for handling the drag start event, which initiates the drag-and-drop operation for items in the grid.
    /// </summary>
    /// <returns>An <see cref="EventCallback{TItem}"/> representing the drag start event handler.</returns>
    private EventCallback<FluentDragEventArgs<TItem>> GetDragStartCallback()
    {
        return CreateDefaultCallback<FluentDragEventArgs<TItem>>(CanActuallyReorder, OnDragStart);
    }

    /// <summary>
    /// Gets the callback for handling the drop end event, which finalizes the drag-and-drop operation and updates the grid layout accordingly.
    /// </summary>
    /// <returns>An <see cref="EventCallback"/> representing the drop end event handler.</returns>
    private EventCallback GetDropEndCallback()
    {
        return CreateDefaultCallback(CanActuallyReorder, OnDropEnd);
    }
    
    /// <summary>
    /// Gets the callback for handling the drag end event, which occurs when a drag-and-drop operation is completed.
    /// </summary>
    /// <returns>An <see cref="EventCallback"/> representing the drag end event handler.</returns>
    private EventCallback GetDragEndCallback()
    {
        return CreateDefaultCallback(CanActuallyReorder, OnDragEnd);
    }

    /// <summary>
    /// Gets the callback for handling the drag enter event, which occurs when a dragged item enters a valid drop target area within the grid.
    /// </summary>
    /// <returns>An <see cref="EventCallback{TItem}"/> representing the drag enter event handler.</returns>
    private EventCallback<FluentDragEventArgs<TItem>> GetDragEnterCallback()
    {
        return CreateDefaultCallback<FluentDragEventArgs<TItem>>(CanActuallyReorder, OnDragEnter);
    }

    /// <summary>
    /// Gets the callback for handling the drag leave event, which occurs when a dragged item leaves a valid drop target area within the grid.
    /// </summary>
    /// <returns>An <see cref="EventCallback"/> representing the drag leave event handler.</returns>
    private EventCallback GetDragLeaveCallback()
    {
        return CreateDefaultCallback(CanActuallyReorder, OnDragLeave);
    }

    /// <summary>
    /// Creates an event callback for the specified action if the given condition is true; otherwise, returns an empty callback.
    /// </summary>
    /// <param name="condition">A boolean value indicating whether the callback should be created.</param>
    /// <param name="action">The action to be invoked by the callback.</param>
    /// <returns>An <see cref="EventCallback"/> representing the event handler.</returns>
    private EventCallback CreateDefaultCallback(bool condition, Action action)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(this, action);
        }

        return EventCallback.Empty;
    }

    /// <summary>
    /// Creates an event callback for the specified action if the given condition is true; otherwise, returns an empty callback.
    /// </summary>
    /// <typeparam name="T">The type of the parameter passed to the action.</typeparam>
    /// <param name="condition">A boolean value indicating whether the callback should be created.</param>
    /// <param name="action">The action to be invoked by the callback.</param>
    /// <returns>An <see cref="EventCallback{T}"/> representing the event handler.</returns>
    private EventCallback<T> CreateDefaultCallback<T>(bool condition, Action<T> action)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(this, action);
        }

        return EventCallback<T>.Empty;
    }

    /// <summary>
    /// Generates a CSS row height specification using the minimum and current row height values.
    /// </summary>
    /// <remarks>If the minimum row height is not specified, the value defaults to '0px'. This method is
    /// useful for dynamically setting row heights in a grid layout.</remarks>
    /// <returns>A string representing the CSS 'minmax' function for row heights, formatted as 'minmax(minimumRowHeight,
    /// rowHeight);'.</returns>
    private string? GetRows()
    {
        DefaultInterpolatedStringHandler handler = new();

        // Rows
        handler.AppendLiteral("minmax(");

        if (string.IsNullOrEmpty(MinimumRowHeight))
        {
            handler.AppendLiteral("0px, ");
        }
        else
        {
            handler.AppendFormatted(MinimumRowHeight);
            handler.AppendLiteral(", ");
        }

        handler.AppendFormatted(RowHeight);
        handler.AppendLiteral(");");

        return handler.ToString();
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _tileGridLayoutEngine = new(this);
        _tileGridDragHandler = new(_tileGridState, _tileGridLayoutEngine, Items);
        _tileGridResizeHandler = new(_tileGridState, _tileGridLayoutEngine);
        _tileGridStorageService = new(StorageProvider, StorageKey, Items, _tileGridState);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && ChildContent is null)
        {
            if (_tileGridStorageService is not null)
            {
                await _tileGridStorageService.LoadAsync();
            }

            _tileGridLayoutEngine?.ApplyLayout();
            await InvokeAsync(StateHasChanged);

            _dotNetRef = DotNetObjectReference.Create(this);
            var module = await JSModule.ImportJavaScriptModuleAsync(JavaScriptFileName);
            await module.InvokeVoidAsync(
                "FluentUI.Blazor.Community.TileGrid.Initialize",
                Id,
                _dotNetRef);
        }
    }

    /// <summary>
    /// Handles updates to the grid layout when a new cell value is computed from JavaScript interop.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop to synchronize the grid's
    /// state with client-side computations. It updates the grid's internal layout and triggers a re-render to reflect
    /// the changes.</remarks>
    /// <param name="value">The cell value containing layout information used to update the grid's configuration.</param>
    [JSInvokable]
    public void OnGridComputed(TileGridCell value)
    {
        _tileGridState.ColumnCount = value.ColumnCount;
        _tileGridState.CellWidth = value.CellWidth;
        _tileGridState.CellHeight = value.CellHeight;
        _tileGridState.RowHeights = value.RowHeights;

        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ChildContent is not null && (CanReorder || CanResize))
        {
            throw new InvalidOperationException("FluentCxTileGrid does not support CanReorder or CanResize when using ChildContent. Use ItemContent and Items instead.");
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasCanReorderChanged = parameters.TryGetValue<bool>(nameof(CanReorder), out var newCanReorder) && newCanReorder != CanReorder;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (_hasCanReorderChanged)
        {
            _hasCanReorderChanged = false;
            EnsureItemIsReorderable();
        }

        base.OnParametersSet();
    }

    /// <summary>
    /// Ensures that the specified item can be reordered by verifying that it implements the ITileGridItem interface
    /// when reordering is enabled.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if CanReorder is <see langword="true"/> and the provided item does not implement ITileGridItem.</exception>
    private void EnsureItemIsReorderable()
    {
        if (!CanReorder)
        {
            return;
        }

        if (!typeof(ITileGridItem).IsAssignableFrom(typeof(TItem)))
        {
            throw new InvalidOperationException($"""
        FluentCxTileGrid requires TItem to implement ITileGridItem when CanReorder = true.

        → TItem provided : {typeof(TItem).FullName}
        → Expected      : a type implementing ITileGridItem

        Fix:
        - Either disable CanReorder
        - Or implement ITileGridItem on your model
    """);

        }
    }

    /// <summary>
    /// Handles the initiation of a drag operation for an item in the grid.
    /// </summary>
    /// <remarks>Override this method to implement custom logic when a drag operation begins, such as updating
    /// UI state or providing visual feedback to the user.</remarks>
    /// <param name="e">The event data associated with the drag start action, containing information about the item being dragged.</param>
    private void OnDragStart(FluentDragEventArgs<TItem> e)
    {
        if (!CanActuallyReorder)
        {
            return;
        }

        _tileGridDragHandler?.Start(e);

        StateHasChanged();
    }

    /// <summary>
    /// Handles the drag enter event by updating the preview target and applying visual feedback to indicate the
    /// potential drop location.
    /// </summary>
    /// <remarks>This method updates the visual state of items to reflect which item is currently the preview
    /// target during a drag-and-drop operation. It ensures that the drag source and target are valid before applying
    /// changes, and resets the preview state for all items except the current target.</remarks>
    /// <param name="e">The event data containing information about the current drag operation, including the item being dragged over.</param>
    private void OnDragEnter(FluentDragEventArgs<TItem> e)
    {
        if (!CanActuallyReorder)
        {
            return;
        }

        _tileGridDragHandler?.Enter(e);

        StateHasChanged();
    }

    /// <summary>
    /// Handles the event when a drag operation leaves the drop target area.
    /// </summary>
    /// <remarks>This method resets the preview state of the drag target and triggers a UI update to reflect
    /// that the item is no longer a valid drop target.</remarks>
    private void OnDragLeave()
    {
        if (!CanActuallyReorder)
        {
            return;
        }

        _tileGridDragHandler?.Leave();

        StateHasChanged();
    }

    /// <summary>
    /// Handles the completion of a drag-and-drop operation by processing the final event data.
    /// </summary>
    /// <remarks>This method is typically called after a drop action is finalized, allowing for any necessary
    /// updates or cleanup related to the dropped item.</remarks>
    private void OnDropEnd()
    {
        if (!CanActuallyReorder)
        {
            return;
        }

        _tileGridDragHandler?.Drop();
    }

    /// <summary>
    /// Handles the completion of a drag-and-drop operation by resetting the drag state and clearing any visual
    /// indicators.
    /// </summary>
    /// <remarks>This method resets the state of the drag source and all items involved in the drag-and-drop
    /// operation. It also triggers a UI refresh to reflect the updated state. Call this method when a drag operation
    /// concludes to ensure the component returns to its default state.</remarks>
    private void OnDragEnd()
    {
        if (!CanActuallyReorder)
        {
            return;
        }

        _tileGridDragHandler?.End();
        _tileGridLayoutEngine?.ApplyLayout();

        if (_tileGridStorageService is not null)
        {
            InvokeAsync(_tileGridStorageService.SaveAsync);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Determines the number of columns that the specified item occupies in the grid layout.
    /// </summary>
    /// <remarks>If the item implements ITileGridItem, its ColumnSpan property is used. Otherwise, the column
    /// span is determined by the GetColumnSpanFunc delegate. This allows support for both strongly-typed and
    /// delegate-based span calculation.</remarks>
    /// <param name="item">The item for which to retrieve the column span. Must implement ITileGridItem or be compatible with the
    /// GetColumnSpanFunc delegate.</param>
    /// <returns>The number of columns spanned by the item in the grid.</returns>
    private int GetColumnSpan(TItem item)
    {
        if (item is ITileGridItem tileItem)
        {
            return tileItem.ColumnSpan;
        }

        return GetColumnSpanFunc(item);
    }

    /// <summary>
    /// Determines the number of rows that the specified grid item spans within the layout.
    /// </summary>
    /// <remarks>Use this method to dynamically obtain the row span for items in a grid, supporting both
    /// ITileGridItem implementations and other types via a custom function. Ensure that the item provided is valid to
    /// avoid unexpected results.</remarks>
    /// <param name="item">The grid item for which to calculate the row span. If the item implements the ITileGridItem interface, its
    /// RowSpan property is used; otherwise, a custom function is applied.</param>
    /// <returns>The number of rows spanned by the specified item.</returns>
    private int GetRowSpan(TItem item)
    {
        if (item is ITileGridItem tileItem)
        {
            return tileItem.RowSpan;
        }

        return GetRowSpanFunc(item);
    }

    /// <summary>
    /// Retrieves the unique key associated with the specified item.
    /// </summary>
    /// <remarks>This method enables flexible key retrieval for items that may or may not implement the
    /// ITileGridItem interface. If the item does not implement ITileGridItem, the key is obtained using the GetKeyFunc
    /// delegate.</remarks>
    /// <param name="item">The item for which to obtain the key. The item must implement the ITileGridItem interface to access its Key
    /// property directly; otherwise, a custom key retrieval function is used.</param>
    /// <returns>A string that represents the unique key of the specified item.</returns>
    private string GetKey(TItem item)
    {
        if (item is ITileGridItem tileItem)
        {
            return tileItem.Key;
        }

        return GetKeyFunc(item);
    }

    /// <summary>
    /// Retrieves the index of the specified item within the collection.
    /// </summary>
    /// <remarks>If the item implements ITileGridItem, its Index property is used directly. Otherwise, the
    /// method searches for the item in the collection using the default equality comparer.</remarks>
    /// <param name="item">The item whose index is to be determined. This can be an object that implements ITileGridItem or any item
    /// contained in the collection.</param>
    /// <returns>The zero-based index of the specified item if found; otherwise, -1.</returns>
    private long GetItemIndex(TItem? item)
    {
        if (item is null)
        {
            return -1;
        }

        if (item is ITileGridItem tileItem)
        {
            return tileItem.Index;
        }

        return Items.IndexOf(item);
    }

    /// <summary>
    /// Initiates the resizing operation for the specified tile grid item using the current mouse position as the
    /// starting point.
    /// </summary>
    /// <remarks>The resizing process will only begin if resizing is currently allowed and the specified item
    /// is a valid tile grid item. The initial row and column spans of the item are recorded to support the resizing
    /// operation.</remarks>
    /// <param name="item">The item to be resized. Must implement the ITileGridItem interface.</param>
    /// <param name="mouseX">The X-coordinate, in pixels, of the mouse pointer at the start of the resize operation.</param>
    /// <param name="mouseY">The Y-coordinate, in pixels, of the mouse pointer at the start of the resize operation.</param>
    /// <param name="handle">The selected handle in the resize operation.</param>
    internal void BeginResize(
        TItem item,
        double mouseX,
        double mouseY,
        TileGridItemResizeHandle handle)
    {
        if (!CanActuallyResize)
        {
            return;
        }

        _tileGridResizeHandler?.BeginResize(item, handle, mouseX, mouseY);
        StateHasChanged();
    }

    /// <summary>
    /// Handles mouse movement events during a tile resize operation, updating the tile's column and row spans based on
    /// the current mouse position.
    /// </summary>
    /// <remarks>This method only performs updates if a resize operation is active and the tile grid item is
    /// valid. It ensures that the new column and row spans are within valid bounds before applying them to the
    /// tile.</remarks>
    /// <param name="e">The mouse event data containing the current position of the mouse pointer.</param>
    private void OnResizeMove(MouseEventArgs e)
    {
        _tileGridResizeHandler?.Move(e);
        _tileGridLayoutEngine?.ApplyLayout(true);

        StateHasChanged();
    }

    /// <summary>
    /// Handles the completion of a resize operation triggered by a mouse event.
    /// </summary>
    /// <remarks>This method is typically called when a user finishes resizing an element. It resets the
    /// resizing state and initiates an asynchronous save operation, followed by a UI update.</remarks>
    /// <param name="e">The event data associated with the mouse event that signals the end of resizing.</param>
    private void OnResizeEnd(MouseEventArgs e)
    {
        _tileGridResizeHandler?.End();
        _tileGridLayoutEngine?.ApplyLayout();

        if (_tileGridStorageService is not null)
        {
            InvokeAsync(_tileGridStorageService.SaveAsync);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Resets the tile grid layout to its initial state, restoring all items and clearing any drag-and-drop indicators.
    /// </summary>
    /// <remarks>This method only performs the reset if an initial layout is available. After restoring the
    /// items and clearing drag-and-drop states, it updates the user interface to reflect the changes.</remarks>
    /// <returns>A task that represents the asynchronous reset operation.</returns>
    public async Task ResetLayoutAsync()
    {
        _tileGridLayoutEngine?.Reset();

        if (_tileGridStorageService is not null)
        {
            await _tileGridStorageService.SaveAsync();
        }

        _tileGridState?.Reset();

        foreach (var item in Items.OfType<ITileGridItem>())
        {
            item.IsPreviewTarget = false;
            item.IsDragging = false;
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Calculates the starting column index for the specified tile grid item.
    /// </summary>
    /// <remarks>The method uses the current column count or a predefined column count to build the logical
    /// layout before determining the column index.</remarks>
    /// <param name="item">The tile grid item for which to determine the starting column index.</param>
    /// <returns>The starting column index of the specified tile grid item, adjusted to be one-based.</returns>
    private int GetColumn(TItem item)
    {
        if (item is ITileGridItem tileItem)
        {
            return tileItem.Column;
        }

        return GetColumnFunc(item);
    }

    /// <summary>
    /// Calculates the starting row index for the specified tile grid item.
    /// </summary>
    /// <remarks>This method builds a logical layout based on the current column count and retrieves the row
    /// index from the layout.</remarks>
    /// <param name="item">The tile grid item for which to determine the starting row index.</param>
    /// <returns>The starting row index of the specified tile grid item, incremented by one to match a one-based index.</returns>
    private int GetRow(TItem item)
    {
        if (item is ITileGridItem tileItem)
        {
            return tileItem.Row;
        }

        return GetRowFunc(item);
    }

    /// <summary>
    /// Determines the appropriate CSS cursor style for the current resize handle.
    /// </summary>
    /// <remarks>The returned cursor style corresponds to the direction of resizing indicated by the active
    /// handle. This method is typically used to provide visual feedback during interactive resizing
    /// operations.</remarks>
    /// <returns>A string representing the CSS cursor style to display, such as "ew-resize" for horizontal resizing or
    /// "ns-resize" for vertical resizing. Returns "default" if the handle type is not recognized.</returns>
    private string GetHandleCursor()
    {
        return _tileGridState?.ResizeHandle switch
        {
            TileGridItemResizeHandle.Right => "ew-resize",
            TileGridItemResizeHandle.Bottom => "ns-resize",
            TileGridItemResizeHandle.BottomRight => "nwse-resize",
            _ => "default"
        };
    }

    /// <summary>
    /// Gets the size of the resize handle as a percentage string based on the current handle size setting.
    /// </summary>
    /// <remarks>The method returns a default value of "20%" if the handle size does not match any predefined
    /// sizes.</remarks>
    /// <returns>A string representing the size of the handle in percentage, such as "10%", "20%", "33%", "50%", "75%", or
    /// "100%".</returns>
    internal string GetHandleSize()
    {
        return HandleSize switch
        {
            TileGridItemResizeHandleSize.ExtraSmall => "10%",
            TileGridItemResizeHandleSize.Small => "20%",
            TileGridItemResizeHandleSize.Medium => "33%",
            TileGridItemResizeHandleSize.Large => "50%",
            TileGridItemResizeHandleSize.ExtraLarge => "75%",
            TileGridItemResizeHandleSize.Full => "100%",
            _ => "20%"
        };
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        await jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.TileGrid.Dispose", Id);
        await base.DisposeAsync(jsModule);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        _dotNetRef = null;

        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}
