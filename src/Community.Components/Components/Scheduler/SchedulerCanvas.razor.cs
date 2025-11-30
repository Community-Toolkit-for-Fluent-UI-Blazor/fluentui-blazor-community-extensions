using System.Drawing;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using TagLib.Ape;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a canvas component for displaying and interacting with scheduled items in a timeline or calendar view.
/// Supports drag-and-drop, resizing, and custom item rendering within a scheduler layout.
/// </summary>
/// <remarks>Use this component within a FluentCxScheduler to visualize and manage collections of scheduled items.
/// The canvas supports day, week, and month views, and provides event callbacks for item interactions such as dropping,
/// resizing, and clicking. Custom item templates can be provided to control the rendering of individual items. The
/// component is designed for extensibility and integration with Blazor applications.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each scheduled entry displayed on the canvas.</typeparam>
public partial class SchedulerCanvas<TItem> : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents the hash of the last rendered items to detect changes.
    /// </summary>
    private string? _lastItemsHash;

    /// <summary>
    /// Represents whether the items have changed since the last render.
    /// </summary>
    private bool _hasItemsChanged;

    /// <summary>
    /// Represents whether a forced refresh of the layout is required.
    /// </summary>
    private bool _force;

    /// <summary>
    /// Represents the JavaScript module reference used for interoperation with JavaScript code.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Holds a reference to the .NET object for interoperation with JavaScript in Blazor applications.
    /// </summary>
    private DotNetObjectReference<SchedulerCanvas<TItem>>? _dotNetRef;

    /// <summary>
    /// Represents the position mapper used to map scheduled items to their positions on the canvas.
    /// </summary>
    private IPositionMapper<TItem>? _mapper;

    /// <summary>
    /// Represents the layout measurements of the scheduler canvas.
    /// </summary>
    private MeasureLayout _layout = new();

    /// <summary>
    /// Represents the active scheduler item, if any.
    /// </summary>
    private SchedulerItem<TItem>? _activeItem;

    /// <summary>
    /// Represents whether an item is currently being dragged.
    /// </summary>
    private bool _isDragging;

    /// <summary>
    /// Represents whether an item is currently being resized.
    /// </summary>
    private bool _isResizing;

    /// <summary>
    /// Represents the starting Y coordinate during a resize operation.
    /// </summary>
    private double _startY;

    /// <summary>
    /// Represents the starting X coordinate during a resize operation.
    /// </summary>
    private double _startX;

    /// <summary>
    /// Represents the original start time of the item being resized.
    /// </summary>
    private DateTime _originalStart;

    /// <summary>
    /// Represents the original end time of the item being resized.
    /// </summary>
    private DateTime _originalEnd;

    /// <summary>
    /// Represents the direction of the resize operation.
    /// </summary>
    private ResizeDirection _resizeDirection;

    /// <summary>
    /// Initializes a new instance of the SchedulerCanvas class.
    /// </summary>
    public SchedulerCanvas()
    {
    }

    /// <summary>
    /// Gets the number of vertical pixels that represent one minute in the current time slot layout.
    /// </summary>
    /// <remarks>This value is calculated based on the parent container's day slot height and the number of
    /// subdivisions per hour. It is used to determine the visual spacing for time-based elements within the
    /// control.</remarks>
    private float PixelsPerMinute => Parent!.View == SchedulerView.Day ? Parent!.DaySlotHeight / (60.0f / Parent.DaySubdivisions) : 1;

    /// <summary>
    /// Gets the width, in units, of a single cell in the overlay layout.
    /// </summary>
    private float CellWidth => (float)Math.Round(_layout.Overlay.Width / 7.0f, 2);

    /// <summary>
    /// Gets or sets the callback that is invoked to enable or disable cells during drag-and-drop operations.
    /// </summary>
    [Parameter]
    public EventCallback<bool> DisabledCells { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    /// <remarks>Use this property to specify child markup or components that should appear within the body of
    /// this component. Typically set automatically when the component is used with child content in Razor
    /// syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the collection of scheduler slots to be displayed or managed by the component.
    /// </summary>
    /// <remarks>Each slot in the collection represents a time interval or resource allocation within the
    /// scheduler. The component will render or process the provided slots according to its scheduling logic. If the
    /// collection is empty, no slots will be shown.</remarks>
    [Parameter]
    public List<SchedulerSlot> Slots { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of items to be scheduled and displayed by the component.
    /// </summary>
    /// <remarks>Each item in the collection represents a schedulable entity of type <typeparamref
    /// name="TItem"/>. The order and contents of this list determine what is rendered and how scheduling operations are
    /// performed.</remarks>
    [Parameter]
    public List<SchedulerItem<TItem>> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the template used to render each item in the scheduler.
    /// </summary>
    /// <remarks>Specify a delegate that receives a <see cref="SchedulerItem{TItem}"/> and returns the content
    /// to display for that item. If not set, a default rendering will be used. This property enables customization of
    /// item appearance within the scheduler component.</remarks>
    [Parameter]
    public RenderFragment<SchedulerItem<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the CSS selector used to identify the content area within the scheduler component.
    /// </summary>
    /// <remarks>Specify a valid CSS selector that matches the desired content element. Changing this value
    /// affects which part of the scheduler is targeted for content rendering and manipulation.</remarks>
    [Parameter]
    public string ContentSelector { get; set; } = ".scheduler-content";

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler item is dropped onto the component.
    /// </summary>
    /// <remarks>The callback receives the dropped <see cref="SchedulerItem{TItem}"/> as its argument. Use
    /// this event to handle item drop actions, such as updating data or triggering UI changes.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> ItemDropped { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler item is resized.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the duration or size of a scheduler item, such as
    /// updating data or triggering additional logic. The callback receives the affected scheduler item as its
    /// argument.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> ItemResized { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
    /// </summary>
    /// <remarks>This property is typically injected by the Blazor framework to enable interoperability
    /// between .NET and JavaScript. Use this instance to call JavaScript functions asynchronously from your component
    /// or service.</remarks>
    [Inject]
    private IJSRuntime Js { get; set; } = default!;

    /// <summary>
    /// Gets an array containing all scheduler slots currently managed by the scheduler.
    /// </summary>
    /// <remarks>The returned array is a snapshot of the current slots. Modifying the array does not affect
    /// the underlying scheduler state.</remarks>
    private SchedulerSlot[] SlotsArray => Slots?.ToArray() ?? [];

    /// <summary>
    /// Gets or sets the maximum number of items to display in the component.
    /// </summary>
    /// <remarks>The default value is 3. Setting this property to a value less than 1 may result in no items
    /// being displayed.</remarks>
    [Parameter]
    public int MaxItemsCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxScheduler{TItem}"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent <see cref="FluentCxScheduler{TItem}"/>. It enables child components to access shared
    /// scheduling functionality or state from their parent scheduler.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem>? Parent { get; set; } = default!;

    /// <summary>
    /// Gets the computed CSS style string used internally for rendering the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("overflow-x", "auto", Parent?.View == SchedulerView.Timeline)
        .AddStyle("overflow-y", "hidden", Parent?.View == SchedulerView.Timeline)
        .Build();

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            _module = await Js.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Scheduler/SchedulerCanvas.razor.js");
            await _module.InvokeVoidAsync("initDrop", Element, ContentSelector);
            await _module.InvokeVoidAsync("observeResize", Element, ContentSelector, _dotNetRef);
            await PositionLayoutAndItemsAsync();

            StateHasChanged();
        }

        if (_hasItemsChanged)
        {
            await PositionLayoutAndItemsAsync();
            _hasItemsChanged = false;
        }

        if (_force)
        {
            await PositionLayoutAndItemsAsync();
            _force = false;
        }
    }

    /// <summary>
    /// Asynchronously calculates and updates the layout and item positions for the scheduler view based on the current
    /// items and view type.
    /// </summary>
    /// <remarks>This method repositions items only if the underlying data has changed or a forced update is
    /// requested. It determines the appropriate layout mapper according to the current scheduler view type and updates
    /// the UI accordingly. This method should be called when the scheduler's items or view configuration changes to
    /// ensure correct visual arrangement.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task PositionLayoutAndItemsAsync()
    {
        var newHash = GetHash(Items);

        if (string.Equals(_lastItemsHash, newHash, StringComparison.Ordinal) && !_force)
        {
            return;
        }

        _lastItemsHash = newHash;
        var parent = Parent!;
        var view = parent.View;
        await MeasureLayoutAsync(view);

        _mapper = view switch
        {
            SchedulerView.Day => GetDayViewColumns(),
            SchedulerView.Week => new WeekGridPositionMapper<TItem>(
                parent.Culture,
                parent.ShowNonWorkingHours,
                parent.WorkDayStart,
                parent.WorkDayEnd,
                parent.WeekSlotHeight,
                parent.ItemsByWeek,
                _layout,
                MaxItemsCount),
            SchedulerView.Month => new MonthPositionMapper<TItem>(7, parent.ItemsByDay, _layout, MaxItemsCount),
            SchedulerView.Timeline => new TimelinePositionMapper<TItem>(
                parent.TimelineSubdivisions,
                parent.ShowNonWorkingHours,
                parent.WorkDayStart,
                parent.WorkDayEnd,
                parent.ItemsByDay),
            _ => null
        };

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Asynchronously measures and updates the layout for the specified scheduler view type.
    /// </summary>
    /// <param name="view">The scheduler view type for which to measure the layout. Determines how the layout is calculated.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the layout measurement is finished.</returns>
    private async Task MeasureLayoutAsync(SchedulerView view)
    {
        if (_module is null)
        {
            _layout = new();
            return;
        }

        _layout = await _module.InvokeAsync<MeasureLayout>("measureLayout", Element, null, view);
    }

    /// <summary>
    /// Initiates a drag operation for the specified scheduler item.
    /// </summary>
    /// <remarks>Call this method to start dragging an item within the scheduler interface. This typically
    /// updates the component state to reflect the drag operation in progress.</remarks>
    /// <param name="item">The scheduler item to begin dragging. Cannot be null.</param>
    /// <param name="e">The drag event arguments containing information about the drag action.</param>
    /// <param name="position">The position rectangle of the item being dragged.</param>
    private async Task OnDragStartAsync(DragEventArgs e, SchedulerItem<TItem> item, RectangleF position)
    {
        if (_isDragging)
        {
            return;
        }

        _isDragging = true;
        _activeItem = item;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("startPointerTracking", _dotNetRef, item.Id, 0);
        }

        if (DisabledCells.HasDelegate)
        {
            await DisabledCells.InvokeAsync(true);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the completion of a drag operation for the specified scheduler item.
    /// </summary>
    /// <param name="item">The scheduler item for which the drag operation has ended. Cannot be null.</param>
    private async Task OnDragEndedAsync(SchedulerItem<TItem> item)
    {
        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        _activeItem = null;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("stopPointerTracking");
        }

        if (DisabledCells.HasDelegate)
        {
            await DisabledCells.InvokeAsync(false);
        }
    }

    /// <summary>
    /// Handles the completion of a resize operation for the specified scheduler item.
    /// </summary>
    /// <param name="item">The scheduler item for which the resize operation has ended. Cannot be null.</param>
    private async Task OnResizeEndedAsync(SchedulerItem<TItem> item)
    {
        if (!_isResizing)
        {
            return;
        }

        _isResizing = false;
        _activeItem = null;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("stopPointerTracking");
        }

        if (DisabledCells.HasDelegate)
        {
            await DisabledCells.InvokeAsync(false);
        }

        if (_mapper is TimelinePositionMapper<TItem> m)
        {
            m.InvalidateDateLayout(item.Start.Date);

            if (item.End.Date != item.Start.Date)
            {
                m.InvalidateDateLayout(item.End.Date);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the initiation of a resize operation for the specified scheduler item.
    /// </summary>
    /// <param name="e">The resize event arguments containing information about the resize action.</param>
    private async Task OnResizeStartedAsync(SchedulerResizeEventArgs<TItem> e)
    {
        if (_isResizing)
        {
            return;
        }

        _isResizing = true;
        _activeItem = e.Item;
        _resizeDirection = e.Direction;
        _startY = e.Y;
        _startX = e.X;
        _originalEnd = e.Item.End;
        _originalStart = e.Item.Start;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("startPointerTracking", _dotNetRef, e.Item.Id, 1);
        }

        if (DisabledCells.HasDelegate)
        {
            await DisabledCells.InvokeAsync(true);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();

        if (_module != null)
        {
            try
            {
                await _module.InvokeVoidAsync("disposeDrop", Element);
                await _module.InvokeVoidAsync("disposeObserve");
                await _module.DisposeAsync();
                _module = null;
            }
            catch (JSDisconnectedException) { }
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Generates a hash string that uniquely represents the collection of scheduler items based on their identifiers
    /// and time intervals.
    /// </summary>
    /// <param name="items">An enumerable collection of scheduler items to be included in the hash calculation. Each item's identifier and
    /// start/end times are used to generate the hash.</param>
    /// <returns>A string containing the computed hash for the specified items, or null if the collection is empty.</returns>
    private static string? GetHash(IEnumerable<SchedulerItem<TItem>> items)
    {
        return string.Join("|", items.Select(i => $"{i.Id}-{i.Start.Ticks}-{i.End.Ticks}").OrderBy(s => s).ToArray());
    }

    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasItemsChanged = parameters.HasValueChanged(nameof(Items), Items);

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Creates and configures a position mapper for aligning items to vertical day view columns based on the current
    /// slot layout and working hours settings.
    /// </summary>
    /// <returns>A <see cref="VerticalSlotAlignedPositionMapper{TItem}"/> instance that maps items to vertical columns in the day
    /// view according to the computed slot layout and working hours configuration.</returns>
    private VerticalSlotAlignedPositionMapper<TItem> GetDayViewColumns()
    {
        var layout = SlotLayoutEngine.ComputeLayout([.. Items]);
        var mapper = new VerticalSlotAlignedPositionMapper<TItem>(
            Parent!.ShowNonWorkingHours,
            Parent.WorkDayStart,
            Parent.WorkDayEnd,
            Parent!.DaySlotHeight,
            Parent!.DaySubdivisions);

        mapper.SetLayout(layout);

        return mapper;
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerCanvas<>)} must be used within a {nameof(FluentCxScheduler<>)}.");
        }
    }

    /// <summary>
    /// Forces a refresh of the component's state and layout asynchronously.
    /// </summary>
    /// <remarks>This method triggers a full state update and repositions layout and items. It should be used
    /// when a complete refresh is required, bypassing any incremental updates. Calling this method may have performance
    /// implications if used frequently.</remarks>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    internal void Refresh()
    {
        _force = true;

        if (_mapper is TimelinePositionMapper<TItem> m)
        {
            m.InvalidateAllLayouts();
        }

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the double-click event for a scheduler item asynchronously, typically initiating an action such as
    /// opening a dialog for the selected item.
    /// </summary>
    /// <param name="item">The scheduler item that was double-clicked. Represents the data associated with the item to be processed.</param>
    /// <param name="e">The mouse event arguments containing details about the double-click event, such as mouse position and button
    /// state.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDoubleClickItemAsync(SchedulerItem<TItem> item, MouseEventArgs e)
    {
        await Parent!.OnDoubleClickAsync(item, false);
    }

    /// <summary>
    /// Handles the drop event for a dragged item, updating its state and view according to the current scheduler view.
    /// </summary>
    /// <remarks>This method updates the dragged item's state and invokes the appropriate drop handler based
    /// on the current scheduler view. After processing, it notifies the parent component of the item update and
    /// refreshes the UI.</remarks>
    /// <param name="e">The drag event arguments containing information about the drop action.</param>
    /// <returns>A task that represents the asynchronous operation of handling the drop event.</returns>
    private async Task OnDropAsync(DragEventArgs e)
    {
        if (_activeItem is null)
        {
            return;
        }

        _isDragging = false;
        var view = Parent!.View;

        switch (view)
        {
            case SchedulerView.Day:
                await OnDayDropAsync(e);
                break;

            case SchedulerView.Week:
                await OnWeekDropAsync(e);
                break;

            case SchedulerView.Month:
                await OnMonthDropAsync(e);
                break;

            case SchedulerView.Timeline:
                await OnTimelineDropAsync(e);
                break;
        }

        await Parent!.OnItemUpdatedAsync(_activeItem);
        _activeItem = null;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the drop event for a dragged item within the month grid, updating the item's start and end dates based
    /// on the drop location.
    /// </summary>
    /// <remarks>This method updates the dates of the currently active item by calculating its new position in
    /// the month grid based on the drop coordinates. If the required module, parent, or active item is not available,
    /// the method returns without making changes.</remarks>
    /// <param name="e">The drag event arguments containing information about the drop position within the month grid.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnMonthDropAsync(DragEventArgs e)
    {
        if (_module is null ||
            Parent is null ||
            _activeItem is null)
        {
            return;
        }

        var rect = await _module.InvokeAsync<RectangleF>("getMonthGridRect", Element);
        var relativeX = e.ClientX - rect.Left;
        var relativeY = e.ClientY - rect.Top;
        var weeksInMonth = (int)Math.Ceiling(Slots.Count / 7.0);
        var columnWidth = rect.Width / 7.0;
        var rowHeight = rect.Height / weeksInMonth;
        var colIndex = Math.Clamp((int)Math.Floor(relativeX / columnWidth), 0, 6);
        var rowIndex = Math.Clamp((int)Math.Floor(relativeY / rowHeight), 0, weeksInMonth - 1);
        var cellIndex = Math.Clamp(rowIndex * 7 + colIndex, 0, Slots.Count - 1);
        var dayDate = Slots[cellIndex].Start.Date;
        var oldDay = _activeItem!.Start.Date;
        var dayDelta = (dayDate - oldDay).Days;
        var newStart = _activeItem.Start.AddDays(dayDelta);
        var newEnd = _activeItem.End.AddDays(dayDelta);

        _activeItem.Start = newStart;
        _activeItem.End = newEnd;
    }

    /// <summary>
    /// Handles the drop event for a week slot, updating the active item's start and end dates based on the drop
    /// position.
    /// </summary>
    /// <remarks>This method updates the active item's scheduling information when a drag-and-drop operation
    /// occurs over a week slot. If required dependencies are not available, the method exits without making
    /// changes.</remarks>
    /// <param name="e">The drag event arguments containing information about the drop location and client coordinates.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnWeekDropAsync(DragEventArgs e)
    {
        if (_module is null ||
            Parent is null ||
            _activeItem is null)
        {
            return;
        }

        var rect = await _module.InvokeAsync<RectangleF>("getBoundingClientRect", Element);
        var relativeX = e.ClientX - rect.Left - 160;
        var relativeY = e.ClientY - rect.Top - 60;

        var columnWidth = _layout.Overlay.Width / 7.0;
        var dayIndex = Math.Clamp((int)Math.Floor(relativeX / columnWidth), 0, 6);

        var weeks = Slots.Select(x => x.Start.Date).Distinct().OrderBy(d => d).ToArray();
        var dayDate = weeks[dayIndex];

        var (newStart, newEnd) = ComputeDrop(
            dayDate,
            0,
            relativeY,
            Parent.WeekSubdivisions,
            Parent.WeekSlotHeight,
            true);

        _activeItem.Start = newStart;
        _activeItem.End = newEnd;
    }

    /// <summary>
    /// Handles the drop event on the timeline, updating the active item's start and end times based on the drop
    /// position.
    /// </summary>
    /// <remarks>If the module, parent, or active item is not available, the method returns without making
    /// changes. This method should be called in response to a drag-and-drop operation on the timeline to reposition the
    /// active item according to the user's drop location.</remarks>
    /// <param name="e">The drag event arguments containing information about the drop location and associated data.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the drop handling is finished.</returns>
    private async Task OnTimelineDropAsync(DragEventArgs e)
    {
        if (_module is null ||
            Parent is null ||
            _activeItem is null)
        {
            return;
        }

        var rect = await _module.InvokeAsync<RectangleF>("getTimelineRect", Element);
        var scrollLeft = await _module.InvokeAsync<float>("getScrollLeft", Element);

        var relativeX = e.ClientX - rect.Left + scrollLeft;

        if (relativeX < 0)
        {
            relativeX = 0;
        }

        var (newStart, newEnd) = ComputeDrop(
            Parent.CurrentDate.Date,
            relativeX,
            0,
            Parent.TimelineSubdivisions,
            0,
            true);

        _activeItem.Start = newStart;
        _activeItem.End = newEnd;

        if (_mapper is TimelinePositionMapper<TItem> m)
        {
            m.InvalidateDateLayout(_activeItem.Start.Date);

            if (_activeItem.End.Date != _activeItem.Start.Date)
            {
                m.InvalidateDateLayout(_activeItem.End.Date);
            }
        }
    }

    /// <summary>
    /// Handles the drop event for a day cell, updating the active item's start and end times based on the drop
    /// position.
    /// </summary>
    /// <remarks>This method updates the active item's time range when a drag-and-drop operation is completed
    /// over a day cell. If the required context is not available, the method exits without making changes.</remarks>
    /// <param name="e">The drag event arguments containing information about the drop, including the cursor position.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDayDropAsync(DragEventArgs e)
    {
        if (_module is null ||
            Parent is null ||
            _activeItem is null)
        {
            return;
        }

        var rect = await _module.InvokeAsync<RectangleF>("getBoundingClientRect", Element);
        var relativeY = Math.Clamp(e.ClientY - rect.Top, 0, _layout.Overlay.Height);

        var (newStart, newEnd) = ComputeDrop(
            Parent.CurrentDate.Date,
            0,
            relativeY,
            Parent.DaySubdivisions,
            Parent.DaySlotHeight,
            true);

        _activeItem!.Start = newStart;
        _activeItem.End = newEnd;
    }

    /// <summary>
    /// Calculates the new start and end times for an item being dropped onto the scheduler grid, based on the specified
    /// position and layout parameters.
    /// </summary>
    /// <remarks>The calculation accounts for the scheduler's configuration, including working hours and view
    /// mode. If the drop would extend beyond the day's end, the time range is adjusted to fit within the allowed
    /// period.</remarks>
    /// <param name="dayDate">The date representing the day onto which the item is being dropped. Used as the base date for the computed time
    /// range.</param>
    /// <param name="relativeX">The horizontal position, in pixels, relative to the grid's origin. Used to determine the drop location when
    /// vertical slot height is not specified.</param>
    /// <param name="relativeY">The vertical position, in pixels, relative to the grid's origin. Used to determine the drop location when slot
    /// pixel height is specified.</param>
    /// <param name="subdivisionsPerHour">The number of time subdivisions per hour in the scheduler grid. Determines the granularity of time slots for
    /// drop calculations. Must be greater than zero.</param>
    /// <param name="slotPixelHeight">The height, in pixels, of each time slot in the grid. If greater than zero, vertical position is used to compute
    /// the drop time; otherwise, horizontal position is used.</param>
    /// <param name="clampToWorkingHours">Indicates whether the computed start time should be clamped to the working hours defined for the day. If <see
    /// langword="true"/>, the start time will not precede the working day start.</param>
    /// <returns>A tuple containing the new start and end <see cref="DateTime"/> values for the dropped item. The times are
    /// adjusted to fit within the day's working hours if necessary.</returns>
    private (DateTime NewStart, DateTime NewEnd) ComputeDrop(
        DateTime dayDate,
        double relativeX,
        double relativeY,
        int subdivisionsPerHour,
        double slotPixelHeight,
        bool clampToWorkingHours)
    {
        var origin = Parent!.ShowNonWorkingHours ? TimeSpan.Zero : Parent.WorkDayStart;
        var effectiveEnd = Parent.ShowNonWorkingHours ? TimeSpan.FromHours(24) : Parent.WorkDayEnd;
        var duration = _activeItem!.End - _activeItem.Start;
        var minutesPerSubdivision = 60 / subdivisionsPerHour;

        int subIndex;

        if (slotPixelHeight > 0)
        {
            subIndex = (int)Math.Floor(relativeY / slotPixelHeight);
        }
        else
        {
            var totalMinutes = (effectiveEnd - origin).TotalMinutes;
            var totalSubdivisions = (int)(totalMinutes / minutesPerSubdivision);
            var cellWidth = _layout.Overlay.Width / totalSubdivisions;
            subIndex = (int)Math.Floor(relativeX / cellWidth);
        }

        if (Parent!.View == SchedulerView.Day)
        {
            subIndex--;
        }

        var newStartTime = origin.Add(TimeSpan.FromMinutes(subIndex * minutesPerSubdivision));
        var newStart = dayDate + newStartTime;
        var newEnd = newStart + duration;

        var dayEnd = dayDate + effectiveEnd;

        if (newEnd > dayEnd)
        {
            newEnd = dayEnd;
            newStart = newEnd - duration;

            if (clampToWorkingHours && newStart < dayDate + origin)
            {
                newStart = dayDate + origin;
            }
        }

        return (newStart, newEnd);
    }

    /// <summary>
    /// Handles pointer movement events for a scheduler item, updating its start or end time or date during a resize
    /// operation.
    /// </summary>
    /// <remarks>This method is intended to be invoked from JavaScript during pointer movement, typically when
    /// resizing scheduler items. The item's start or end time or date is adjusted based on the pointer's position and
    /// the current scheduler view. No action is taken if the specified item does not exist.</remarks>
    /// <param name="itemId">The unique identifier of the scheduler item being interacted with.</param>
    /// <param name="mode">The interaction mode indicating the type of pointer operation. A value of 1 represents a resize action.</param>
    /// <param name="clientX">The horizontal position of the pointer, in pixels, relative to the client area.</param>
    /// <param name="clientY">The vertical position of the pointer, in pixels, relative to the client area.</param>
    /// <param name="pointerType">The type of pointer device used for the event, such as 'mouse', 'touch', or 'pen'.</param>
    [JSInvokable]
    public void OnPointerMove(long itemId, int mode, double clientX, double clientY, string pointerType)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);

        if (item is null)
        {
            return;
        }

        if (mode == 1 && _isResizing && _activeItem?.Id == itemId)
        {
            var view = Parent!.View;

            switch (view)
            {
                case SchedulerView.Day:
                    {
                        var deltaY = clientY - _startY;
                        var minutesDelta = (int)Math.Round(deltaY / PixelsPerMinute);

                        if (_resizeDirection == ResizeDirection.Top)
                        {
                            item.Start = _originalStart.AddMinutes(minutesDelta);
                        }
                        else if (_resizeDirection == ResizeDirection.Bottom)
                        {
                            item.End = _originalEnd.AddMinutes(minutesDelta);
                        }

                        var dayDate = Parent.CurrentDate.Date;
                        var dayStart = Parent.ShowNonWorkingHours ? TimeSpan.Zero : Parent.WorkDayStart;
                        var dayEnd = Parent.ShowNonWorkingHours ? TimeSpan.FromHours(24) : Parent.WorkDayEnd;

                        var windowStart = dayDate + dayStart;
                        var windowEnd = dayDate + dayEnd;

                        if (item.Start < windowStart)
                        {
                            item.Start = windowStart;
                        }

                        if (item.End > windowEnd)
                        {
                            item.End = windowEnd;
                        }

                        if (item.End < item.Start)
                        {
                            item.End = item.Start;
                        }
                    }

                    break;

                case SchedulerView.Week:
                    {
                        var deltaY = clientY - _startY;
                        var deltaX = clientX - _startX;
                        var minutesPerSubdivision = 60 / Parent.DaySubdivisions;
                        var subdivisionsDelta = (int)(deltaY / Parent.WeekSlotHeight);
                        var minutesDelta = subdivisionsDelta * minutesPerSubdivision;
                        var daysDelta = (int)Math.Round(deltaX / CellWidth);

                        if (_resizeDirection == ResizeDirection.Top)
                        {
                            item.Start = _originalStart.AddMinutes(minutesDelta);
                            item.End = _originalEnd;
                        }
                        else if (_resizeDirection == ResizeDirection.Bottom)
                        {
                            item.Start = _originalStart;
                            item.End = _originalEnd.AddMinutes(minutesDelta);
                        }
                        else if (_resizeDirection == ResizeDirection.Left)
                        {
                            item.Start = _originalStart.AddDays(daysDelta);
                            item.End = _originalEnd;
                        }
                        else if (_resizeDirection == ResizeDirection.Right)
                        {
                            item.Start = _originalStart;
                            item.End = _originalEnd.AddDays(daysDelta);
                        }

                        if (item.End < item.Start)
                        {
                            item.End = item.Start;
                        }
                    }

                    break;

                case SchedulerView.Month:
                    {
                        var deltaY = clientY - _startY;
                        var deltaX = clientX - _startX;
                        var daysDelta = (int)Math.Round(deltaX / CellWidth);
                        var weeksDelta = (int)Math.Round(deltaY / Parent.MonthSlotHeight);

                        if (_resizeDirection == ResizeDirection.Top)
                        {
                            item.Start = _originalStart.AddDays(weeksDelta * 7);
                            item.End = _originalEnd;
                        }
                        else if (_resizeDirection == ResizeDirection.Bottom)
                        {
                            item.Start = _originalStart;
                            item.End = _originalEnd.AddDays(weeksDelta * 7);
                        }
                        else if (_resizeDirection == ResizeDirection.Left)
                        {
                            item.Start = _originalStart.AddDays(daysDelta);
                            item.End = _originalEnd;
                        }
                        else if (_resizeDirection == ResizeDirection.Right)
                        {
                            item.Start = _originalStart;
                            item.End = _originalEnd.AddDays(daysDelta);
                        }

                        if (item.End < item.Start)
                        {
                            item.End = item.Start;
                        }
                    }

                    break;

                case SchedulerView.Timeline:
                    {
                        var deltaX = clientX - _startX;
                        var minutesPerSubdivision = 60 / Parent.TimelineSubdivisions;
                        var totalMinutes = (Parent.ShowNonWorkingHours ? 24 * 60 : (Parent.WorkDayEnd - Parent.WorkDayStart).TotalMinutes);
                        var totalSubdivisions = (int)(totalMinutes / minutesPerSubdivision);
                        var cellWidth = _layout.Overlay.Width / totalSubdivisions;
                        var subdivisionsDelta = (int)Math.Round(deltaX / cellWidth);
                        var minutesDelta = subdivisionsDelta * minutesPerSubdivision;

                        if (_resizeDirection == ResizeDirection.Left)
                        {
                            item.Start = _originalStart.AddMinutes(minutesDelta);
                            item.End = _originalEnd;
                        }
                        else if (_resizeDirection == ResizeDirection.Right)
                        {
                            item.Start = _originalStart;
                            item.End = _originalEnd.AddMinutes(minutesDelta);
                        }

                        var dayDate = Parent.CurrentDate.Date;
                        var origin = Parent.ShowNonWorkingHours ? TimeSpan.Zero : Parent.WorkDayStart;
                        var effectiveEnd = Parent.ShowNonWorkingHours ? TimeSpan.FromHours(24) : Parent.WorkDayEnd;
                        var windowStart = dayDate + origin;
                        var windowEnd = dayDate + effectiveEnd;

                        if (item.Start < windowStart)
                        {
                            item.Start = windowStart;
                        }

                        if (item.End > windowEnd)
                        {
                            item.End = windowEnd;
                        }

                        if (item.End < item.Start)
                        {
                            item.End = item.Start;
                        }

                        if (_mapper is TimelinePositionMapper<TItem> m)
                        {
                            m.InvalidateDateLayout(item.Start.Date);

                            if (item.End.Date != item.Start.Date)
                            {
                                m.InvalidateDateLayout(item.End.Date);
                            }
                        }
                    }

                    break;
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// Handles the pointer up event for an item, updating its state and triggering related UI updates asynchronously.
    /// </summary>
    /// <remarks>This method is intended to be invoked from JavaScript via interop. It updates the item's
    /// state and may trigger additional UI changes depending on the interaction mode and pointer type.</remarks>
    /// <param name="itemId">The unique identifier of the item associated with the pointer up event.</param>
    /// <param name="mode">The mode indicating the type of pointer interaction. A value of 1 typically represents a resize operation.</param>
    /// <param name="pointerType">The type of pointer device that triggered the event, such as 'mouse', 'touch', or 'pen'.</param>
    /// <returns>A task that represents the asynchronous operation of handling the pointer up event.</returns>
    [JSInvokable("OnPointerUp")]
    public async Task OnPointerUpAsync(long itemId, int mode, string pointerType)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);

        if (item is null)
        {
            return;
        }

        if (mode == 1)
        {
            _isResizing = false;
            await Parent!.OnItemUpdatedAsync(item);
        }

        _activeItem = null;

        if (DisabledCells.HasDelegate)
        {
            await DisabledCells.InvokeAsync(false);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Calculates the height, in pixels, required to display a timeline cell for the current date.
    /// </summary>
    /// <returns>The height of the timeline cell in pixels. Returns a default value of 80 if no timeline mapper is available.</returns>
    internal int GetTimelineCellHeight()
    {
        if (_mapper is TimelinePositionMapper<TItem> timelineMapper)
        {
            return Math.Max(80, timelineMapper.GetRequiredHeight(Parent!.CurrentDate));
        }

        return 80;
    }
}
