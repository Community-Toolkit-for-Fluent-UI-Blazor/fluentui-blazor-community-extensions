using System.Collections.Concurrent;
using System.Globalization;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a flexible, customizable scheduler component for displaying and managing items across various calendar
/// views such as day, week, month, and year. Supports templating, localization, and event callbacks for item creation,
/// update, and deletion.
/// </summary>
/// <remarks>Use <c>FluentCxScheduler&lt;TItem&gt;</c> to build interactive scheduling interfaces with support for
/// custom item templates, dynamic data loading, and configurable calendar views. The component is designed for
/// extensibility and can be tailored to a wide range of scheduling scenarios, including resource planning, event
/// management, and agenda tracking. Thread safety is not guaranteed; use appropriate synchronization if accessing from
/// multiple threads.</remarks>
/// <typeparam name="TItem">The type of the data item managed and displayed by the scheduler. Each scheduler entry is represented as a <see
/// cref="SchedulerItem{TItem}"/>.</typeparam>
public partial class FluentCxScheduler<TItem>
{
    /// <summary>
    /// Provides the collection of slot builders used to construct scheduling slots for different time intervals.
    /// </summary>
    /// <remarks>Each slot builder in the array is responsible for generating slots for a specific time
    /// period, such as day, week, month, year, or timeline. The order of builders may affect how slots are generated or
    /// selected.</remarks>
    private static readonly ISchedulerSlotBuilder[] _slotBuilders =
    [
        new DaySlotBuilder(),
        new WeekSlotBuilder(),
        new MonthSlotBuilder(),
        new YearSlotBuilder(),
        new TimelineSlotBuilder(),
        new AgendaSlotBuilder(7)
    ];

    /// <summary>
    /// Indicates whether the component is currently in a loading state.
    /// </summary>
    private bool _isLoading;

    /// <summary>
    /// Indicates whether the DisabledCells parameter has changed.
    /// </summary>
    private bool _disabledCells;

    /// <summary>
    /// Indicates whether the DaySubdivisions parameter has changed.
    /// </summary>
    private bool _hasDaySubdivisionsChanged;

    /// <summary>
    /// Indicates whether the WeekSubdivisions parameter has changed.
    /// </summary>
    private bool _hasWeekSubdivisionsChanged;

    /// <summary>
    /// Indicates whether the TimelineSubdivisions parameter has changed.
    /// </summary>
    private bool _hasTimelineSubdivisionsChanged;

    /// <summary>
    /// Indicates whether the NumberOfDays parameter has changed.
    /// </summary>
    private bool _hasNumberOfDaysChanged;

    /// <summary>
    /// Represents the internal collection of scheduler items managed by the component.
    /// </summary>
    private readonly List<SchedulerItem<TItem>> _items = [];

    /// <summary>
    /// Represents the start date for the current view range of the scheduler.
    /// </summary>
    private DateTime _startDate;

    /// <summary>
    /// Represents the end date for the current view range of the scheduler.
    /// </summary>
    private DateTime _endDate;

    /// <summary>
    /// Represents the collection of scheduler slots for the current view.
    /// </summary>
    private List<SchedulerSlot> _schedulerSlots = [];

    /// <summary>
    /// Represents the current view type of the scheduler component.
    /// </summary>
    private SchedulerView _currentView;

    /// <summary>
    /// References the scheduler canvas component used for rendering the scheduler UI.
    /// </summary>
    private SchedulerCanvas<TItem>? _schedulerCanvas;

    /// <summary>
    /// Represents the dictionary of scheduler items grouped by day.
    /// </summary>
    private Dictionary<DateTime, List<SchedulerItem<TItem>>> _itemsByDay = [];

    /// <summary>
    /// Represents the dictionary of scheduler items grouped by week and hour.
    /// </summary>
    private Dictionary<(DateTime, int), List<SchedulerItem<TItem>>> _itemsByWeek = [];

    /// <summary>
    /// Represents the list of visible scheduler items based on the current view and filtering criteria.
    /// </summary>
    private List<SchedulerItem<TItem>> _visibleItems = [];

    /// <summary>
    /// Represents the dictionary tracking the number of overflow items per day.
    /// </summary>
    private Dictionary<DateTime, int> _overflowByDay = [];

    /// <summary>
    /// References the scheduler view menu component used for managing view selection.
    /// </summary>
    private SchedulerViewMenu<TItem>? _viewMenu;

    /// <summary>
    /// References the scheduler business hours component used for displaying business hour settings.
    /// </summary>
    private SchedulerBusinessHours<TItem>? _businessHours;

    /// <summary>
    /// References the scheduler agenda menu component used for managing agenda view settings.
    /// </summary>
    private SchedulerAgendaMenu<TItem>? _agendaMenu;

    /// <summary>
    /// References the scheduler day view settings menu component used for managing day view settings.
    /// </summary>
    private SchedulerDayViewSettings<TItem>? _daysViewSettingsMenu;

    /// <summary>
    /// References the scheduler week view settings menu component used for managing week view settings.
    /// </summary>
    private SchedulerWeekViewSettings<TItem>? _weekViewSettingsMenu;

    /// <summary>
    /// Represents a value indicating whether the available views have changed.
    /// </summary>
    private bool _hasAvailableViewsChanged;

    /// <summary>
    /// Represents the cache for recurrence calculations to optimize performance.
    /// </summary>
    private readonly ConcurrentDictionary<RecurrenceCacheKey, List<DateTime>> _recurrenceCache = new();

    /// <summary>
    /// Gets a dictionary that maps each day to a list of scheduled items of type <typeparamref name="TItem"/>.
    /// </summary>
    internal Dictionary<DateTime, List<SchedulerItem<TItem>>> ItemsByDay => _itemsByDay;

    /// <summary>
    /// Gets the collection of scheduled items grouped by week and year.
    /// </summary>
    internal Dictionary<(DateTime, int), List<SchedulerItem<TItem>>> ItemsByWeek => _itemsByWeek;

    /// <summary>
    /// Gets or sets the content to render in the toolbar area of the component.
    /// </summary>
    /// <remarks>Assign a <see cref="RenderFragment"/> to customize the toolbar with additional controls,
    /// buttons, or other UI elements. If not set, the toolbar area will remain empty.</remarks>
    [Parameter]
    public RenderFragment? Toolbar { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each item in the scheduler.
    /// </summary>
    /// <remarks>The template receives a <see cref="SchedulerItem{TItem}"/> representing the item to be
    /// rendered. Use this property to customize the appearance and layout of scheduler items. If not set, a default
    /// rendering will be used.</remarks>
    [Parameter]
    public RenderFragment<SchedulerItem<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the number of subdivisions for each day slot in the scheduler.
    /// </summary>
    [Parameter]
    public int DaySubdivisions { get; set; } = 4;

    /// <summary>
    /// Gets or sets the number of subdivisions within a week used for scheduling or display purposes.
    /// </summary>
    /// <remarks>A higher value increases the granularity of week-based divisions, which may affect how events
    /// or data are grouped and presented. The default value is 4, representing quarters of a week.</remarks>
    [Parameter]
    public int WeekSubdivisions { get; set; } = 4;

    /// <summary>
    /// Gets or sets the number of subdivisions to display within each timeline segment.
    /// </summary>
    /// <remarks>A higher value increases the granularity of the timeline, allowing for more detailed visual
    /// breakdowns. The value must be a positive integer.</remarks>
    [Parameter]
    public int TimelineSubdivisions { get; set; } = 4;

    /// <summary>
    /// Gets or sets the collection of scheduler view types that are available for selection.
    /// </summary>
    /// <remarks>Use this property to specify which views (such as Day, Week, Month, or Year) the scheduler
    /// component should present to users. The order of items in the list determines the order in which views are
    /// displayed. Modifying this collection allows customization of the scheduler's available views.</remarks>
    [Parameter]
    public List<SchedulerView> AvailableViews { get; set; } =
    [
        SchedulerView.Day,
        SchedulerView.Week,
        SchedulerView.Month,
        SchedulerView.Year,
        SchedulerView.Agenda,
        SchedulerView.Timeline
    ];

    /// <summary>
    /// Gets or sets the current date value used by the component.
    /// </summary>
    [Parameter]
    public DateTime CurrentDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the delegate used to asynchronously provide items for the scheduler based on the specified fetch
    /// request.
    /// </summary>
    /// <remarks>
    /// The delegate should accept a <see cref="SchedulerFetchRequest"/> and return a <see cref="ValueTask{T}"/> containing the items to display. 
    /// This property enables dynamic data loading, such as paging or filtering, in response to scheduler queries. 
    /// If <see langword="null"/>, no items will be provided.
    /// </remarks>
    [Parameter]
    public Func<SchedulerFetchRequest, ValueTask<IEnumerable<SchedulerItem<TItem>>>>? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a new scheduler item is created.
    /// </summary>
    /// <remarks>Use this callback to handle custom logic or initialization when a new item is added to the
    /// scheduler. The callback receives the newly created <see cref="SchedulerItem{TItem}"/> as its argument.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> OnItemCreate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler item is updated.
    /// </summary>
    /// <remarks>Use this property to handle updates to individual scheduler items, such as when their
    /// properties change or they are modified by user interaction. The callback receives the updated item as its
    /// argument. This event is typically used to synchronize changes with external data sources or to trigger
    /// additional logic in response to item updates.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> OnItemUpdate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler item is deleted.
    /// </summary>
    /// <remarks>The callback receives the deleted item as its argument. Assign this property to handle custom
    /// logic when an item is removed from the scheduler, such as updating data sources or triggering
    /// notifications.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> OnItemDelete { get; set; }

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing operations within the component.
    /// </summary>
    /// <remarks>If not explicitly set, the property defaults to the current culture of the executing thread.
    /// Changing this property affects how dates, numbers, and other culture-sensitive data are displayed and
    /// interpreted.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the template used to render each slot in the scheduler.
    /// </summary>
    [Parameter]
    public RenderFragment<SchedulerSlot>? SlotTemplate { get; set; }

    /// <summary>
    /// Gets or sets the height, in pixels, of each day slot in the calendar display.
    /// </summary>
    /// <remarks>Adjust this value to control the vertical size of individual day cells. Larger values
    /// increase the space available for content within each day slot.</remarks>
    [Parameter]
    public int DaySlotHeight { get; set; } = 60;

    /// <summary>
    /// Gets or sets the height, in pixels, of each month slot in the calendar layout.
    /// </summary>
    [Parameter]
    public int MonthSlotHeight { get; set; } = 180;

    /// <summary>
    /// Gets or sets the height, in pixels, of each time slot displayed in the agenda day view.
    /// </summary>
    [Parameter]
    public int AgendaDaySlotHeight { get; set; } = 40;

    /// <summary>
    /// Gets or sets a value indicating whether hour-specific content is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowHourContent { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether hour labels are displayed on the time picker.
    /// </summary>
    [Parameter]
    public bool ShowHourLabels { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether weekends are displayed in gray in the calendar view.
    /// </summary>
    [Parameter]
    public bool ShowWeekEndInGray { get; set; } = true;

    /// <summary>
    /// Gets or sets the time of day when the workday ends.
    /// </summary>
    /// <remarks>Set this property to specify the end time for scheduling or time-based calculations within
    /// the workday. The default value is 17:00 (5:00 PM).</remarks>
    [Parameter]
    public TimeSpan WorkDayEnd { get; set; } = TimeSpan.FromHours(17);

    /// <summary>
    /// Gets or sets the start time of the workday.
    /// </summary>
    /// <remarks>The default value is 8:00 AM. Adjust this property to define when the workday begins for
    /// scheduling or time-based calculations.</remarks>
    [Parameter]
    public TimeSpan WorkDayStart { get; set; } = TimeSpan.FromHours(8);

    /// <summary>
    /// Gets or sets the height, in pixels, of each week slot in the calendar display.
    /// </summary>
    /// <remarks>Set this property to adjust the vertical size of week slots to fit different layout
    /// requirements or visual preferences.</remarks>
    [Parameter]
    public int WeekSlotHeight { get; set; } = 60;

    /// <summary>
    /// Gets or sets the maximum number of items allowed in each slot.
    /// </summary>
    /// <remarks>Set this property to control how many items can be assigned to a single slot. The default
    /// value is 3. Work only with MonthView.</remarks>
    [Parameter]
    public int MaxItemsPerSlot { get; set; } = 3;

    /// <summary>
    /// Gets or sets the set of labels used for customizing the text displayed by the scheduler component.
    /// </summary>
    /// <remarks>Use this property to provide localized or custom labels for various elements within the
    /// scheduler, such as headers, buttons, and tooltips. If not set, default labels are used. Changing this property
    /// will update the displayed text accordingly.</remarks>
    [Parameter]
    public SchedulerLabels Labels { get; set; } = SchedulerLabels.Default;

    /// <summary>
    /// Gets or sets the callback that is invoked when a delete occurrence action is requested.
    /// </summary>
    /// <remarks>The callback receives a <see cref="DeleteOccurrenceRequest"/> containing details about the
    /// occurrence to be deleted. Assign this property to handle custom logic when a delete operation is initiated, such
    /// as updating data or displaying confirmation dialogs.</remarks>
    [Parameter]
    public EventCallback<DeleteOccurrenceRequest> OnDeleteOccurence { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the agenda settings panel is displayed.
    /// </summary>
    [Parameter]
    public bool ShowAgendaSettings { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether non-working hours are displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowNonWorkingHours { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether week settings are displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowWeekSettings { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether day-specific settings are displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowDaySettings { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the business hours settings are displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowBusinessHoursSettings { get; set; } = true;

    /// <summary>
    /// Gets a list of scheduler items grouped by month and limited to the maximum number of items per slot.
    /// </summary>
    /// <remarks>The returned list contains items ordered by their start date within each group. Only the
    /// first items up to the maximum per slot are included for each date group.</remarks>
    private List<SchedulerItem<TItem>> GroupedMonthItems => [.. _items.GroupBy(i => i.Start.Date).SelectMany(g => g.OrderBy(i => i.Start).Take(MaxItemsPerSlot))];

    /// <summary>
    /// Gets a list of scheduler items grouped by week day and ordered by start time within each day.
    /// </summary>
    private List<SchedulerItem<TItem>> GroupedWeekItems => [.. _items.GroupBy(i => i.Start.Date).SelectMany(g => g.OrderBy(i => i.Start))];

    /// <summary>
    /// Gets the current view type used by the scheduler.
    /// </summary>
    internal SchedulerView View => _currentView;

    /// <summary>
    /// Gets or sets the number of days used for the operation.
    /// </summary>
    [Parameter]
    public int NumberOfDays { get; set; } = 7;

    /// <summary>
    /// Gets or sets a value indicating whether days with no scheduled events are hidden from display
    ///  in the agenda view.
    /// </summary>
    [Parameter]
    public bool HideEmptyDays { get; set; }

    /// <summary>
    /// Gets or sets the service used to display dialogs within the component.
    /// </summary>
    /// <remarks>This property is typically injected by the framework to provide dialog functionality, such as
    /// showing modal windows or alerts. It should not be set manually outside of dependency injection
    /// scenarios.</remarks>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to display while the component is in a loading state.
    /// </summary>
    /// <remarks>If not set, a default loading indicator may be shown depending on the component's
    /// implementation. The content can include markup or other components to customize the loading
    /// experience.</remarks>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        SetDaySubdivisions(DaySubdivisions);
        SetWeekSubdivisions(WeekSubdivisions);
        SetTimelineSubdivisions(TimelineSubdivisions);
        await LoadItemsAsync();
    }

    /// <inheritdoc />
    protected async Task LoadItemsAsync()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);

        var slotBuilder = _slotBuilders[(int)_currentView];
        _startDate = slotBuilder.GetStartDate(CurrentDate, Culture);
        _endDate = slotBuilder.GetEndDate(CurrentDate, Culture);
        _schedulerSlots = [.. slotBuilder.GetSlots(Culture, _startDate, _endDate)];

        if (ItemsProvider == null)
        {
            return;
        }

        var request = new SchedulerFetchRequest(_currentView, _startDate, _endDate);

        _items.Clear();
        var rawItems = await ItemsProvider.Invoke(request);

        foreach (var item in rawItems)
        {
            if (item.Recurrence == null)
            {
                _items.Add(item);
            }
            else
            {
                var key = new RecurrenceCacheKey(
                    item.Id,
                    _startDate,
                    _endDate,
                    item.Start,
                    item.Recurrence,
                    item.Exceptions
                );

                if (!_recurrenceCache.TryGetValue(key, out var cachedOccurrences))
                {
                    // Chercher une occurrence plus large déjà calculée
                    var broaderOccurrences = _recurrenceCache
                        .Where(kvp =>
                            kvp.Key.ItemId == item.Id &&
                            kvp.Key.From <= _startDate.Ticks && kvp.Key.To >= _endDate.Ticks &&
                            kvp.Key.AnchorStart == item.Start.Ticks &&
                            kvp.Key.RuleHash == key.RuleHash &&
                            kvp.Key.ExceptionsHash == key.ExceptionsHash)
                        .OrderBy(kvp => kvp.Key.To - kvp.Key.From)
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault();

                    if (broaderOccurrences?.Count > 0)
                    {
                        cachedOccurrences = [.. broaderOccurrences.Where(o => o >= _startDate && o < _endDate)];
                    }
                    else
                    {
                        cachedOccurrences = [.. RecurrenceEngine.GetOccurrences(
                        item.Recurrence,
                        item.Start,
                        _startDate,
                        _endDate,
                        item.Exceptions)];

                        _recurrenceCache[key] = cachedOccurrences;
                    }
                }

                foreach (var occ in cachedOccurrences)
                {
                    _items.Add(new SchedulerItem<TItem>
                    {
                        Id = item.Id,
                        Data = item.Data,
                        Start = occ,
                        End = occ + (item.End - item.Start),
                        Recurrence = item.Recurrence,
                        Exceptions = item.Exceptions,
                        Description = item.Description,
                        Title = item.Title
                    });
                }
            }
        }

        if (AvailableViews.Contains(SchedulerView.Month) ||
            AvailableViews.Contains(SchedulerView.Timeline))
        {
            _itemsByDay = _items
                .GroupBy(i => i.Start.Date)
                .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Start).ToList());

            _visibleItems = [];
            _overflowByDay = [];

            foreach (var kvp in _itemsByDay)
            {
                var visible = kvp.Value.Take(MaxItemsPerSlot).ToList();
                var hidden = kvp.Value.Count - visible.Count;

                _visibleItems.AddRange(visible);

                if (hidden > 0)
                {
                    _overflowByDay.Add(kvp.Key, hidden);
                }
            }
        }

        if (AvailableViews.Contains(SchedulerView.Week))
        {
            _itemsByWeek = _items
                .GroupBy(i => (i.Start.Date, i.Start.Hour))
                .ToDictionary(g => g.Key, g => g.OrderBy(i => i.Start).Take(MaxItemsPerSlot).ToList());
        }

        _isLoading = false;
        await InvokeAsync(StateHasChanged);
        _schedulerCanvas?.Refresh();
    }

    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasAvailableViewsChanged = parameters.HasEnumerableValueChanged(nameof(AvailableViews), AvailableViews);
        _hasDaySubdivisionsChanged = parameters.HasValueChanged(nameof(DaySubdivisions), DaySubdivisions);
        _hasWeekSubdivisionsChanged = parameters.HasValueChanged(nameof(WeekSubdivisions), WeekSubdivisions);
        _hasTimelineSubdivisionsChanged = parameters.HasValueChanged(nameof(TimelineSubdivisions), TimelineSubdivisions);
        _hasNumberOfDaysChanged = parameters.HasValueChanged(nameof(NumberOfDays), NumberOfDays);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasAvailableViewsChanged)
        {
            _hasAvailableViewsChanged = false;

            if (!AvailableViews.Contains(_currentView))
            {
                _currentView = AvailableViews.First();
            }

            await LoadItemsAsync();
        }

        if (_hasDaySubdivisionsChanged)
        {
            _hasDaySubdivisionsChanged = false;
            SetDaySubdivisions(DaySubdivisions);
        }

        if (_hasWeekSubdivisionsChanged)
        {
            _hasWeekSubdivisionsChanged = false;
            SetWeekSubdivisions(WeekSubdivisions);
        }

        if (_hasTimelineSubdivisionsChanged)
        {
            _hasTimelineSubdivisionsChanged = false;
            SetTimelineSubdivisions(TimelineSubdivisions);
        }

        if (_hasNumberOfDaysChanged)
        {
            _hasNumberOfDaysChanged = false;
            SetAgendaDays(NumberOfDays);
        }
    }

    /// <summary>
    /// Handles the selection of a specific day in the scheduler, updating the current view and loading relevant items
    /// asynchronously.
    /// </summary>
    /// <remarks>This method updates the scheduler's current date and view to reflect the selected day,
    /// refreshes related UI components, and loads items for the selected date. It should be awaited to ensure that all
    /// updates and data loading are completed before proceeding.</remarks>
    /// <param name="day">The date representing the day to select and display in the scheduler.</param>
    /// <returns>A task that represents the asynchronous operation of updating the view and loading items.</returns>
    private async Task HandleDaySelectedAsync(DateTime day)
    {
        CurrentDate = day;
        _currentView = SchedulerView.Day;

        _viewMenu?.Refresh();
        _businessHours?.Refresh();
        _daysViewSettingsMenu?.Refresh();
        _weekViewSettingsMenu?.Refresh();
        _schedulerCanvas?.Refresh();

        await LoadItemsAsync();
    }

    /// <summary>
    /// Retrieves the collection of scheduler items corresponding to the current view type.
    /// </summary>
    /// <remarks>The returned items reflect the current view mode, such as day, week, month, year, or agenda.
    /// For grouped views, the items may be aggregated or organized differently than in single-day or agenda
    /// views.</remarks>
    /// <returns>A list of <see cref="SchedulerItem{TItem}"/> objects representing the items for the active scheduler view. The
    /// returned list may vary depending on the selected view type.</returns>
    private List<SchedulerItem<TItem>> GetItems()
    {
        return _currentView switch
        {
            SchedulerView.Month => [.. GroupedMonthItems],
            SchedulerView.Week => [.. GroupedWeekItems],
            _ => _items
        };
    }

    /// <summary>
    /// Asynchronously moves the scheduler view to the previous date range based on the current view type.
    /// </summary>
    /// <remarks>The date range adjustment depends on the current view type: for example, the view moves back
    /// by one day, week, month, or year, or by a custom number of days for agenda views. This method should be awaited
    /// to ensure that items are fully loaded before further actions are taken.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task completes when the scheduler has updated to the
    /// previous date range and loaded the corresponding items.</returns>
    internal Task MoveToPreviousAsync()
    {
        return MoveToAsync(-1);
    }

    /// <summary>
    /// Advances the current view to the next date range asynchronously and loads the corresponding items.
    /// </summary>
    /// <remarks>The date range advanced depends on the current scheduler view type. This method updates the
    /// current date and reloads items for the new range.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal Task MoveToNextAsync()
    {
        return MoveToAsync(1);
    }

    /// <summary>
    /// Asynchronously moves the current date forward or backward by an interval determined by the active scheduler view
    /// and the specified multiplier, then loads the corresponding items.
    /// </summary>
    /// <remarks>The interval applied to the current date depends on the active view: days for Day view, weeks
    /// for Week view, months for Month view, years for Year view, and a custom number of days for Agenda view. This
    /// method should be awaited to ensure items are loaded before further actions are taken.</remarks>
    /// <param name="multiplier">A value that determines the direction and magnitude of the move. Positive values move forward; negative values
    /// move backward. The interval is based on the current scheduler view.</param>
    /// <returns>A task that represents the asynchronous operation of loading items for the new date range.</returns>
    private Task MoveToAsync(int multiplier)
    {
        CurrentDate = _currentView switch
        {
            SchedulerView.Day => CurrentDate.AddDays(1 * multiplier),
            SchedulerView.Week => CurrentDate.AddDays(7 * multiplier),
            SchedulerView.Month => CurrentDate.AddMonths(1 * multiplier),
            SchedulerView.Year => CurrentDate.AddYears(1 * multiplier),
            SchedulerView.Agenda => CurrentDate.AddDays(NumberOfDays * multiplier),
            SchedulerView.Timeline => CurrentDate.AddDays(1 * multiplier),
            _ => CurrentDate
        };

        return LoadItemsAsync();
    }

    /// <summary>
    /// Asynchronously updates the current date to today and reloads the associated items.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task MoveToTodayAsync()
    {
        CurrentDate = DateTime.Today;

        await LoadItemsAsync();
    }

    /// <summary>
    /// Returns the display name associated with the specified scheduler view type.
    /// </summary>
    /// <param name="view">The scheduler view type for which to retrieve the display name.</param>
    /// <returns>A string containing the display name for the specified view type. If the view type is not recognized, returns
    /// the string representation of the view type.</returns>
    internal string GetViewName(SchedulerView view)
    {
        return view switch
        {
            SchedulerView.Day => Labels.DayView,
            SchedulerView.Week => Labels.WeekView,
            SchedulerView.Month => Labels.MonthView,
            SchedulerView.Year => Labels.YearView,
            SchedulerView.Agenda => Labels.AgendaView,
            SchedulerView.Timeline => Labels.TimelineView,
            _ => view.ToString()
        };
    }

    /// <summary>
    /// Asynchronously changes the current scheduler view to the specified type and refreshes all related UI components.
    /// </summary>
    /// <remarks>This method updates the scheduler's view and ensures that all dependent menus and canvases
    /// are refreshed to reflect the change. The operation is asynchronous and may involve UI updates that require
    /// awaiting completion.</remarks>
    /// <param name="value">The scheduler view type to switch to. Determines which view is displayed and which UI elements are refreshed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task ChangeViewAsync(SchedulerView value)
    {
        _currentView = value;
        _viewMenu?.Refresh();
        _businessHours?.Refresh();
        _agendaMenu?.Refresh();
        _daysViewSettingsMenu?.Refresh();
        _weekViewSettingsMenu?.Refresh();

        await LoadItemsAsync();
        await InvokeAsync(StateHasChanged);

        _schedulerCanvas?.Refresh();
    }

    /// <summary>
    /// Toggles the visibility of non-working hours in the scheduler and refreshes the display.
    /// </summary>
    internal void ToggleShowNonWorkingHours()
    {
        ShowNonWorkingHours = !ShowNonWorkingHours;

        InvokeAsync(StateHasChanged);
        _schedulerCanvas?.Refresh();
    }

    /// <summary>
    /// Sets the view menu to be used by the scheduler.
    /// </summary>
    /// <param name="viewMenu">The view menu to assign to the scheduler. Specify <c>null</c> to remove the current view menu.</param>
    internal void SetViewMenu(SchedulerViewMenu<TItem>? viewMenu)
    {
        _viewMenu = viewMenu;
    }

    /// <summary>
    /// Sets the business hours for the scheduler using the specified value.
    /// </summary>
    /// <param name="value">The business hours to apply to the scheduler. Specify <see langword="null"/> to clear any previously set
    /// business hours.</param>
    internal void SetBusinessHour(SchedulerBusinessHours<TItem>? value)
    {
        _businessHours = value;
    }

    /// <summary>
    /// Asynchronously refreshes the component's data and updates its state.
    /// </summary>
    /// <remarks>This method reloads the component's items and triggers a state update. It should be called
    /// when the underlying data changes and the UI needs to reflect the latest state.</remarks>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    internal async Task RefreshAsync()
    {
        await LoadItemsAsync();
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the agenda menu used by the scheduler component.
    /// </summary>
    /// <param name="value">The agenda menu to assign. Can be null to remove the current menu.</param>
    internal void SetAgendaMenu(SchedulerAgendaMenu<TItem>? value)
    {
        _agendaMenu = value;
    }

    /// <summary>
    /// Forces the component to re-render by notifying the framework that its state has changed.
    /// </summary>
    /// <remarks>Call this method when the component's state has been updated outside of the normal
    /// data-binding or event flow, and a UI refresh is required. This method should be used sparingly, as excessive
    /// calls may impact performance.</remarks>
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Handles the double-click event for a scheduler slot asynchronously, triggering the associated action for the
    /// corresponding item or creating a new item if none exists.
    /// </summary>
    /// <param name="slot">The scheduler slot that was double-clicked. Provides the label, start time, and end time used to identify or
    /// create the associated item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSlotDoubleClickAsync(SchedulerSlot slot)
    {
        var item = _items.Find(i => string.Equals(i.Title, slot.Label, StringComparison.OrdinalIgnoreCase) &&
                                    i.Start.Date == slot.Start.Date &&
                                    i.Start.TimeOfDay == slot.Start.TimeOfDay);

        item ??= new SchedulerItem<TItem>
        {
            Start = slot.Start,
            End = slot.End,
        };

        await OnDoubleClickAsync(item, true);
    }

    /// <summary>
    /// Displays a dialog for creating or editing a scheduler item in response to a double-click event, and triggers the
    /// appropriate event callback based on user action.
    /// </summary>
    /// <remarks>If the user confirms the dialog, the method invokes either the item creation or update
    /// callback, depending on the value of <paramref name="create"/>. No callback is invoked if the dialog is
    /// cancelled.</remarks>
    /// <param name="item">The scheduler item to be created or edited. Represents the event data shown in the dialog.</param>
    /// <param name="create">Indicates whether the dialog is for creating a new item (<see langword="true"/>) or editing an existing item
    /// (<see langword="false"/>).</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the dialog interaction and any
    /// associated event callbacks have finished.</returns>
    internal async Task OnDoubleClickAsync(SchedulerItem<TItem> item, bool create)
    {
        var dialog = await DialogService.ShowDialogAsync<SchedulerEventDialog<TItem>>(new SchedulerEventContent<TItem>()
        {
            Labels = Labels,
            Item = item,
            Culture = Culture
        }, new DialogParameters()
        {
            Title = item.Id == 0 ? Labels.CreateEventTitle : Labels.EditEventTitle,
            PrimaryAction = null,
            SecondaryAction = null,
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,
            DismissTitle = Labels.CloseButtonTitle,
            Width = "650px"
        });

        var result = await dialog.Result;

        if (!result.Cancelled && result.Data is SchedulerItem<TItem> schedulerItem)
        {
            if (create && OnItemCreate.HasDelegate)
            {
                await OnItemCreate.InvokeAsync(schedulerItem);
                await LoadItemsAsync();
            }
            else if (!create && OnItemUpdate.HasDelegate)
            {
                await OnItemUpdate.InvokeAsync(schedulerItem);
                await LoadItemsAsync();
            }
        }
    }

    /// <summary>
    /// Asynchronously updates the scheduler's day subdivisions and refreshes the UI to reflect the changes.
    /// </summary>
    /// <param name="subdivision">The number of subdivisions to set for each day slot in the scheduler.</param>
    /// <remarks>This method recalculates the scheduler slots based on the current day subdivisions and
    /// ensures that the UI is updated. If a scheduler canvas is present, it is force-refreshed before the component
    /// state is updated.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal void SetDaySubdivisions(int subdivision)
    {
        DaySubdivisions = subdivision;
        _slotBuilders[0] = new DaySlotBuilder(subdivision);
        var slotBuilder = _slotBuilders[0];
        _startDate = slotBuilder.GetStartDate(CurrentDate, Culture);
        _endDate = slotBuilder.GetEndDate(CurrentDate, Culture);
        _schedulerSlots = [.. slotBuilder.GetSlots(Culture, _startDate, _endDate)];

        _schedulerCanvas?.Refresh();
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Asynchronously updates the timeline subdivisions and refreshes the scheduler display to reflect the new
    /// configuration.
    /// </summary>
    /// <remarks>This method updates internal state related to timeline subdivisions and triggers a UI refresh
    /// if the scheduler canvas is available. The operation is performed asynchronously and should be awaited to ensure
    /// completion before further actions are taken.</remarks>
    /// <param name="subdivisions">The number of subdivisions to apply to the timeline. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal void SetTimelineSubdivisions(int subdivisions)
    {
        TimelineSubdivisions = subdivisions;
        _slotBuilders[4] = new TimelineSlotBuilder(subdivisions);
        var slotBuilder = _slotBuilders[4];
        _startDate = slotBuilder.GetStartDate(CurrentDate, Culture);
        _endDate = slotBuilder.GetEndDate(CurrentDate, Culture);
        _schedulerSlots = [.. slotBuilder.GetSlots(Culture, _startDate, _endDate)];

        _schedulerCanvas?.Refresh();
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Configures the agenda view to display the specified number of days and refreshes the scheduler UI
    /// asynchronously.
    /// </summary>
    /// <remarks>This method updates the agenda configuration and triggers a UI refresh. If the scheduler
    /// canvas is available, it will be refreshed to reflect the new agenda days. The method should be awaited to ensure
    /// the UI is updated before further actions are taken.</remarks>
    /// <param name="numberOfDays">The number of consecutive days to display in the agenda view. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous operation of updating the agenda days and refreshing the scheduler UI.</returns>
    internal void SetAgendaDays(int numberOfDays)
    {
        NumberOfDays = numberOfDays;
        _slotBuilders[5] = new AgendaSlotBuilder(numberOfDays);
        var slotBuilder = _slotBuilders[5];
        _startDate = slotBuilder.GetStartDate(CurrentDate, Culture);
        _endDate = slotBuilder.GetEndDate(CurrentDate, Culture);
        _schedulerSlots = [.. slotBuilder.GetSlots(Culture, _startDate, _endDate)];

        _schedulerCanvas?.Refresh();
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Asynchronously updates the scheduler's week subdivisions and refreshes the display to reflect the current
    /// configuration.
    /// </summary>
    /// <param name="subdivisions">The number of subdivisions to set for each week slot in the scheduler.</param>
    /// <remarks>This method recalculates the week slot subdivisions based on the current settings and updates
    /// the scheduler's visible range. The UI is refreshed if a scheduler canvas is present. Call this method when
    /// changes to week subdivision settings require the scheduler to update its display.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal void SetWeekSubdivisions(int subdivisions)
    {
        WeekSubdivisions = subdivisions;
        _slotBuilders[1] = new WeekSlotBuilder(subdivisions);
        var slotBuilder = _slotBuilders[1];
        _startDate = slotBuilder.GetStartDate(CurrentDate, Culture);
        _endDate = slotBuilder.GetEndDate(CurrentDate, Culture);
        _schedulerSlots = [.. slotBuilder.GetSlots(Culture, _startDate, _endDate)];

        _schedulerCanvas?.Refresh();
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the asynchronous deletion workflow for a scheduled item, prompting the user for confirmation and
    /// invoking the deletion callback if confirmed.
    /// </summary>
    /// <remarks>If the item has recurrence, the user is prompted to choose whether to delete the entire
    /// series or only the current occurrence. The deletion callback is invoked only if the user confirms the action and
    /// a delegate is assigned.</remarks>
    /// <param name="item">The scheduled item to be deleted. Must not be null. The item's recurrence information determines the
    /// confirmation dialog presented to the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the deletion workflow and any related
    /// callbacks have finished executing.</returns>
    internal async Task OnDeleteAsync(SchedulerItem<TItem> item)
    {
        var hasRecurrence = item.Recurrence is not null;

        var dialog = await DialogService.ShowConfirmationAsync(
            !hasRecurrence ? Labels.DeleteEventMessage : Labels.DeleteRecurrenceMessage,
            !hasRecurrence ? Labels.Yes : Labels.DeleteWholeSeries,
            !hasRecurrence ? Labels.No : Labels.DeleteCurrentOccurrence,
            !hasRecurrence ? Labels.DeleteEventTitle : Labels.DeleteRecurrenceEvent);

        var result = await dialog.Result;

        if (!hasRecurrence)
        {
            if (!result.Cancelled &&
                OnItemDelete.HasDelegate)
            {
                await OnItemDelete.InvokeAsync(item);
                await LoadItemsAsync();
            }
        }
        else
        {
            if (!result.Cancelled)
            {
                await OnDeleteOccurence.InvokeAsync(new(item.Id, item.Start));
                await LoadItemsAsync();
            }
            else
            {
                await OnItemDelete.InvokeAsync(item);
                await LoadItemsAsync();
            }
        }
    }

    /// <summary>
    /// Sets the day view settings menu for the scheduler using the specified settings.
    /// </summary>
    /// <param name="item">The day view settings to apply to the scheduler. If <paramref name="item"/> is <see langword="null"/>, the menu
    /// will be cleared.</param>
    internal void SetDaySettingsMenu(SchedulerDayViewSettings<TItem>? item)
    {
        _daysViewSettingsMenu = item;
    }

    /// <summary>
    /// Handles the update event for a scheduler item asynchronously.
    /// </summary>
    /// <remarks>This method invokes the item update event handler if one is registered, and then reloads the
    /// scheduler items. Intended for internal use within the scheduling infrastructure.</remarks>
    /// <param name="item">The scheduler item that was updated and will be processed by the event handler.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnItemUpdatedAsync(SchedulerItem<TItem> item)
    {
        if (OnItemUpdate.HasDelegate)
        {
            await OnItemUpdate.InvokeAsync(item);
            await LoadItemsAsync();
        }
    }

    /// <summary>
    /// Returns a read-only list of dates representing all days in the week that contains the current date, starting
    /// from the first day of the week as defined by the current culture.
    /// </summary>
    /// <remarks>The definition of the week and its starting day is determined by the <see
    /// cref="System.Globalization.CultureInfo"/> associated with the instance. The returned dates are ordered
    /// sequentially from the first to the last day of the week.</remarks>
    /// <returns>A read-only list of seven <see cref="DateTime"/> values, each corresponding to a day in the current week. The
    /// list starts with the first day of the week as specified by the culture's <see
    /// cref="System.Globalization.DateTimeFormatInfo.FirstDayOfWeek"/> property.</returns>
    private IReadOnlyList<DateTime> GetWeekDays()
    {
        var firstDayOfWeek = Culture.DateTimeFormat.FirstDayOfWeek;
        var diff = ((int)CurrentDate.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        var weekStart = CurrentDate.Date.AddDays(-diff);

        return [.. Enumerable.Range(0, 7).Select(i => weekStart.AddDays(i))];
    }

    /// <summary>
    /// Retrieves the reference date corresponding to the current scheduler view type.
    /// </summary>
    /// <remarks>The returned date varies based on the value of <c>CurrentView</c>. For week views, the date
    /// represents the first day of the week; for month and year views, it represents the first day of the month or
    /// year, respectively. For other views, it returns the current date.</remarks>
    /// <returns>A <see cref="DateTime"/> value representing the relevant date for the current view. For example, returns the
    /// first day of the week, month, or year depending on the view type.</returns>
    internal DateTime GetDate()
    {
        return View switch
        {
            SchedulerView.Day => CurrentDate.Date,
            SchedulerView.Week => GetWeekDays()[0],
            SchedulerView.Month => new DateTime(CurrentDate.Year, CurrentDate.Month, 1),
            SchedulerView.Year => new DateTime(CurrentDate.Year, 1, 1),
            SchedulerView.Agenda => CurrentDate.Date,
            _ => CurrentDate.Date
        };
    }

    /// <summary>
    /// Sets the height, in pixels, for each day slot in the calendar view.
    /// </summary>
    /// <param name="slotHeight">The height, in pixels, to assign to each day slot. Must be a positive integer.</param>
    internal void SetDaySlotHeight(int slotHeight)
    {
        DaySlotHeight = slotHeight;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the week view settings menu for the scheduler using the specified settings.
    /// </summary>
    /// <param name="weekViewSettings">The settings to apply to the week view menu. If <paramref name="weekViewSettings"/> is <see langword="null"/>,
    /// the menu will be reset or disabled.</param>
    internal void SetWeekSettingsMenu(SchedulerWeekViewSettings<TItem>? weekViewSettings)
    {
        _weekViewSettingsMenu = weekViewSettings;
    }

    /// <summary>
    /// Sets the height, in pixels, for each week slot in the calendar view.
    /// </summary>
    /// <remarks>Calling this method updates the visual layout of the calendar to reflect the new slot height.
    /// This method should be used when dynamically adjusting the calendar's appearance.</remarks>
    /// <param name="weekSlotHeight">The height, in pixels, to assign to each week slot. Must be a positive integer.</param>
    internal void SetWeekSlotHeight(int weekSlotHeight)
    {
        WeekSlotHeight = weekSlotHeight;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets whether days with no events are hidden from the display.
    /// </summary>
    /// <param name="hideEmptyDays">A value indicating whether days without events should be hidden. Set to <see langword="true"/> to hide empty
    /// days; otherwise, <see langword="false"/>.</param>
    internal void SetHideEmptyDays(bool hideEmptyDays)
    {
        HideEmptyDays = hideEmptyDays;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Calculates the height, in pixels, of a cell in the current scheduler view.
    /// </summary>
    /// <returns>The cell height in pixels for the current view. Returns the timeline cell height if the view is Timeline;
    /// otherwise, returns a default value of 80.</returns>
    private int GetCellHeight()
    {
        return _currentView == SchedulerView.Timeline ? _schedulerCanvas?.GetTimelineCellHeight() ?? 80 : 80;
    }
}
