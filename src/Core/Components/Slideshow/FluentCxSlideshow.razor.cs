using System.Globalization;
using System.Timers;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Extensions;
using FluentUI.Blazor.Community.Components.Localization;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a slideshow component that can display a collection of items with various configuration options
///  such as autoplay, looping modes, and touch support. This component is designed to be flexible and customizable,
///  allowing developers to create engaging slideshow experiences in their Blazor applications.
/// </summary>
public partial class FluentCxSlideshow : FluentComponentBase
{
    /// <summary>
    /// Represents the collection of slideshow items currently managed by the slideshow component.
    /// </summary>
    /// <remarks>This list is initialized as empty and stores the individual items displayed in the slideshow.
    /// Items should be added, removed, or modified only through the component's public methods to ensure correct
    /// slideshow state management.</remarks>
    private readonly List<SlideshowItem> _slides = [];

    /// <summary>
    /// Stores the timer instance used to schedule periodic operations within the containing class.
    /// </summary>
    /// <remarks>This field is intended for internal use to manage timed actions. Ensure that the timer is
    /// properly disposed of when it is no longer required to prevent resource leaks.</remarks>
    private Timer? _timer;

    /// <summary>
    /// Represents a value indicating whether the current index has changed.
    /// </summary>
    private bool _currentIndexChanged;

    /// <summary>
    /// Represents a value indicating whether the autoplay setting has changed.
    /// </summary>
    private bool _autoPlayChanged;

    /// <summary>
    /// Represents a value indicating whether the autoplay interval has changed.
    /// </summary>
    private bool _intervalChanged;

    /// <summary>
    /// Represents a value indicating whether the indicator position has changed.
    /// </summary>
    private bool _isIndicatorPositionChanged;

    /// <summary>
    /// Represents a value indicating whether the orientation has changed.
    /// </summary>
    private bool _isOrientationChanged;

    /// <summary>
    /// Represents a value indicating whether the touch enabled setting has changed.
    /// </summary>
    private bool _isTouchEnabledChanged;

    /// <summary>
    /// Represents a value indicating whether the looping mode has changed.
    /// </summary>
    private bool _isLoopingModeChanged;

    /// <summary>
    /// Represents a value indicating whether the show indicators setting has changed.
    /// </summary>
    private bool _showIndicatorChanged;

    /// <summary>
    /// Represents the reference of the slideshow instance that can be used for JavaScript interop calls.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxSlideshow> _dotnetReference;

    /// <summary>
    /// Represents the javaScript module reference used for invoking JavaScript functions related to the slideshow component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// 
    /// </summary>
    private bool _hasInsideDialogChanged;

    /// <summary>
    /// Represents the relative path to the JavaScript file that provides functionality for the FluentCxSlideshow
    /// component.
    /// </summary>
    /// <remarks>This file must be available at runtime for the slideshow component to operate correctly.
    /// Ensure that the specified path matches the deployment structure of your application.</remarks>
    private const string JavascriptFileName = FluentCxConstants.JAVASCRIPT_ROOT + "Slideshow/FluentCxSlideshow.razor.js";

    /// <summary>
    /// Initializes a new instance of the FluentCxSlideshow class using the specified library configuration.
    /// </summary>
    /// <remarks>This constructor assigns a unique identifier to the slideshow instance and establishes a .NET
    /// object reference for JavaScript interoperability.</remarks>
    /// <param name="configuration">The configuration settings that determine how the slideshow operates. Cannot be null.</param>
    public FluentCxSlideshow(LibraryConfiguration configuration)
    : base(configuration)
    {
        Id = Identifier.NewId();
        _dotnetReference = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets a value indicating that the slideshow is inside a dialog component.
    /// </summary>
    [Parameter]
    public bool InsideDialog { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the slideshow controls (previous and next buttons) should be displayed.
    /// </summary>
    [Parameter]
    public bool ShowControls { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the slideshow component. This content typically consists of
    ///  <see cref="SlideshowItem"/>
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the slideshow indicators (navigation dots) should be displayed.
    /// </summary>
    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the slideshow should automatically transition between items after a specified interval.
    /// </summary>
    [Parameter]
    public bool Autoplay { get; set; }

    /// <summary>
    /// Gets or sets the time interval between automatic transitions when autoplay is enabled. This value determines how long each item is displayed before transitioning to the next one.
    /// </summary>
    [Parameter]
    public TimeSpan AutoplayInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets the duration of the transition animation between slideshow items.
    ///  This value determines how long the sliding animation takes when moving from one item to another.
    /// </summary>
    [Parameter]
    public TimeSpan SlideDuration { get; set; } = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// Gets or sets the index of the currently displayed item in the slideshow. The index is 1-based, meaning that an index of 1 corresponds to the first item in the slideshow.
    /// </summary>
    [Parameter]
    public int Index { get; set; } = 1;

    /// <summary>
    /// Gets or sets an event callback that is invoked when the current index of the slideshow changes. This allows parent components to respond to index changes, such as updating other UI elements or performing actions based on the currently displayed item.
    /// </summary>
    [Parameter]
    public EventCallback<int> IndexChanged { get; set; }

    /// <summary>
    /// Gets or sets the looping mode of the slideshow, which determines how the slideshow behaves when navigating past the first or last item. The looping mode can be set to one of the following values:
    /// </summary>
    [Parameter]
    public SlideshowLoopingMode LoopMode { get; set; } = SlideshowLoopingMode.None;

    /// <summary>
    /// Gets or sets the label for the previous button in the slideshow controls. This label is used for accessibility purposes and can be customized to provide a more descriptive text for screen readers.
    /// </summary>
    private string PreviousLabel => Localizer[LanguageResource.CX_Slideshow_Previous];

    /// <summary>
    /// Gets or sets the label for the next button in the slideshow controls. Similar to the previous button label, this is used for accessibility purposes to provide descriptive text for screen readers when navigating through the slideshow.
    /// </summary>
    private string NextLabel => Localizer[LanguageResource.CX_Slideshow_Next];

    /// <summary>
    /// Gets or sets the position of the slideshow indicators (navigation dots) when they are displayed. The position can be set to one of the following values:
    /// </summary>
    [Parameter]
    public SlideshowIndicatorPosition IndicatorPosition { get; set; } = SlideshowIndicatorPosition.Bottom;

    /// <summary>
    /// Gets or sets a template for rendering the slideshow indicators (navigation dots). 
    /// </summary>
    [Parameter]
    public RenderFragment<int>? IndicatorTemplate { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the slideshow, which determines the direction in which the items transition when navigating through the slideshow.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets a value indicating whether touch interactions are enabled for the slideshow.
    ///  When enabled, users can swipe left or right (or up and down, depending on the orientation) to navigate between items in the slideshow on touch-enabled devices.
    /// </summary>
    [Parameter]
    public bool IsTouchEnabled { get; set; }

    /// <summary>
    /// Gets or sets the aspect ratio of the slideshow content.
    /// </summary>
    [Parameter]
    public SlideshowContentRatio ContentRatio { get; set; } = SlideshowContentRatio.Original;

    /// <summary>
    /// Gets or sets the icon used for the previous button in the slideshow controls.
    /// </summary>
    [Parameter]
    public Icon PreviousIcon { get; set; } = FluentCxSlideshowConstants.ChevronLeft;

    /// <summary>
    /// Gets or sets the icon used for the next button in the slideshow controls.
    /// </summary>
    [Parameter]
    public Icon NextIcon { get; set; } = FluentCxSlideshowConstants.ChevronRight;

    /// <summary>
    /// Gets or sets the threshold (in pixels) for detecting touch swipe gestures when touch interactions are enabled.
    /// </summary>
    [Parameter]
    public int TouchThreshold { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating whether the autoplay functionality should be paused
    ///  when touch interactions are enabled and a swipe gesture is detected. 
    /// </summary>
    [Parameter]
    public bool StopAutoplayWhenTouchEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the aspect ratio mode used to display the slideshow images.
    /// </summary>
    [Parameter]
    public SlideshowRatioMode RatioMode { get; set; }

    /// <summary>
    /// Gets the total number of slides or items available, depending on the current content configuration.
    /// </summary>
    private int Count => _slides.Count;

    /// <summary>
    /// Gets a value indicating whether navigation to the previous item in the slideshow is currently disabled.
    /// </summary>
    private bool IsPreviousDisabled => LoopMode == SlideshowLoopingMode.None && Index == 1;

    /// <summary>
    /// Gets a value indicating whether advancing to the next item in the slideshow is currently disabled.
    /// </summary>
    private bool IsNextDisabled => LoopMode == SlideshowLoopingMode.None && Index == Count;

    /// <summary>
    /// Gets the current orientation state used internally by the object.
    /// </summary>
    internal Orientation InternalOrientation => GetInternalOrientation();

    /// <summary>
    /// Gets the css for the slideshow.
    /// </summary>
    private string? InternalClass => DefaultClassBuilder
        .AddClass("fluentcx-slideshow")
        .Build();

    /// <summary>
    /// Gets the computed inline CSS style string for the slideshow component, including item count, current index,
    /// duration, and easing function.
    /// </summary>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("--slideshow-item-count", Count.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-current-index", Index.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-duration", $"{SlideDuration.TotalMilliseconds}ms")
        .AddStyle("--slideshow-easing", "cubic-bezier(0.22, 0.61, 0.36, 1)")
        .Build();

    /// <summary>
    /// Gets the previous button style string based on the current orientation of the slideshow.
    /// </summary>
    private string? PreviousButtonStyle => InternalOrientation == Orientation.Horizontal ? PreviousHorizontalStyle : PreviousVerticalStyle;

    /// <summary>
    /// Gets the next button style string based on the current orientation of the slideshow.
    /// </summary>
    private string? NextButtonStyle => InternalOrientation == Orientation.Horizontal ? NextHorizontalStyle : NextVerticalStyle;

    /// <summary>
    /// Gets the previous button style string for horizontal orientation.
    /// </summary>
    private static string? PreviousHorizontalStyle { get; } = new StyleBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("left", "50px")
        .AddStyle("top", "50%")
        .AddStyle("transform", "translateY(-50%)")
        .AddStyle("min-height", "100px")
        .AddStyle("max-height", "100px")
        .AddStyle("width", "32px")
        .Build();

    /// <summary>
    /// Gets the previous button style string for vertical orientation.
    /// </summary>
    private static string? PreviousVerticalStyle { get; } = new StyleBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("left", "50%")
        .AddStyle("top", "50px")
        .AddStyle("transform", "translateX(-50%)")
        .AddStyle("min-width", "100px")
        .AddStyle("max-width", "100px")
        .AddStyle("height", "32px")
        .Build();

    /// <summary>
    /// Gets the next button style string for horizontal orientation.
    /// </summary>
    private static string? NextHorizontalStyle { get; } = new StyleBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("right", "50px")
        .AddStyle("top", "50%")
        .AddStyle("transform", "translateY(-50%)")
        .AddStyle("min-height", "100px")
        .AddStyle("max-height", "100px")
        .AddStyle("width", "32px")
        .Build();

    /// <summary>
    /// Gets the next button style string for vertical orientation.
    /// </summary>
    private static string? NextVerticalStyle { get; } = new StyleBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("left", "50%")
        .AddStyle("bottom", "50px")
        .AddStyle("transform", "translateX(-50%)")
        .AddStyle("min-width", "100px")
        .AddStyle("max-width", "100px")
        .AddStyle("height", "32px")
        .Build();

    /// <summary>
    /// Gets the state of the slideshow component.
    /// </summary>
    [Inject]
    private SlideshowState State { get; set; } = default!;

    /// <summary>
    /// Determines the effective orientation of the slideshow based on the current indicator settings and loop mode.
    /// </summary>
    /// <remarks>If indicators are displayed and the loop mode is not set to infinite, the orientation is
    /// determined by the indicator position. Otherwise, the default orientation is used.</remarks>
    /// <returns>An Orientation value indicating whether the slideshow is horizontal or vertical, depending on the indicator
    /// position and loop mode.</returns>
    private Orientation GetInternalOrientation()
    {
        if (ShowIndicators && LoopMode != SlideshowLoopingMode.Infinite)
        {
            return IndicatorPosition switch
            {
                SlideshowIndicatorPosition.Top or SlideshowIndicatorPosition.Bottom => Orientation.Horizontal,
                _ => Orientation.Vertical
            };
        }

        return Orientation;
    }

    /// <summary>
    /// Starts the timer to automatically advance slides at the configured interval.
    /// </summary>
    /// <remarks>The timer is only started if autoplay is enabled. The interval used is determined by the
    /// AutoplayInterval property, but if the current slide specifies its own interval, that value is used instead.
    /// Calling this method will stop any previously running timer before starting a new one.</remarks>
    private void StartTimer()
    {
        if (!Autoplay)
        {
            return;
        }

        StopTimer();

        var interval = AutoplayInterval;

        if (_slides.Count > Index - 1 && _slides[Index - 1].Interval.HasValue)
        {
            interval = _slides[Index - 1].Interval.GetValueOrDefault();
        }

        _timer = new Timer(interval);
        _timer.Elapsed += OnTimerTick;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer and detaches the event handler for the elapsed event.
    /// </summary>
    /// <remarks>This method ensures that the timer is stopped and cleans up the event subscription to prevent
    /// memory leaks. It is important to call this method when the timer is no longer needed.</remarks>
    private void StopTimer()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerTick;
        }
    }

    /// <summary>
    /// Handles the timer's tick event and initiates an asynchronous transition to the next slideshow state.
    /// </summary>
    /// <remarks>This method is intended to be used as an event handler for a timer and executes the next
    /// slideshow transition asynchronously to avoid blocking the UI thread.</remarks>
    /// <param name="sender">The source of the event, typically the timer that triggered the tick.</param>
    /// <param name="e">Provides data for the elapsed event, including the time since the timer was last reset.</param>
    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(OnMoveNextAsync);
    }

    /// <summary>
    /// Determines whether the specified index corresponds to the current position, which is one less than the internal
    /// index value.
    /// </summary>
    /// <remarks>This method returns false if the provided index is negative. The current position is
    /// considered to be one less than the internal index value, which may be relevant when synchronizing with external
    /// collections or UI elements.</remarks>
    /// <param name="index">The zero-based index to compare with the current position. Must be a non-negative integer.</param>
    /// <returns>true if the specified index matches the current position; otherwise, false.</returns>
    private bool IsCurrent(int index)
    {
        if (index < 0)
        {
            return false;
        }

        return Index == index + 1;
    }

    /// <summary>
    /// Retrieves the ARIA hidden attribute value for the specified slideshow item.
    /// </summary>
    /// <remarks>This method is intended for internal use and provides a way to determine the ARIA hidden
    /// state of a slideshow item based on its position.</remarks>
    /// <param name="item">The slideshow item for which to obtain the ARIA hidden value. This parameter must not be null and must be a
    /// valid item contained in the slideshow.</param>
    /// <returns>A string representing the ARIA hidden value for the specified item, or null if the item is not found in the
    /// slideshow.</returns>
    internal string? GetAriaHiddenValue(SlideshowItem item)
    {
        return GetAriaHiddenValue(_slides.IndexOf(item));
    }

    /// <summary>
    /// Returns the ARIA hidden attribute value for the specified item index, indicating whether the item should be
    /// hidden from assistive technologies.
    /// </summary>
    /// <remarks>Use this method to set the 'aria-hidden' attribute for slideshow items to improve
    /// accessibility by hiding inactive items from screen readers.</remarks>
    /// <param name="index">The zero-based index of the item to evaluate. Must be within the valid range of available items.</param>
    /// <returns>A string value of "true" if the specified index does not correspond to the currently active item; otherwise,
    /// "false".</returns>
    private string GetAriaHiddenValue(int index)
    {
        return !IsCurrent(index) ? "true" : "false";
    }

    /// <summary>
    /// Advances the slideshow to the next item according to the current looping mode and index.
    /// </summary>
    /// <remarks>If the looping mode is set to infinite, the transition is handled via a JavaScript interop
    /// call. When the index exceeds the total number of items, the slideshow resets to the first item if looping is
    /// enabled; otherwise, it proceeds to the next item. This method also manages the slideshow timer to ensure
    /// consistent playback.</remarks>
    /// <returns>A task that represents the asynchronous operation of moving to the next item in the slideshow.</returns>
    private async Task OnMoveNextAsync()
    {
        StopTimer();

        if (LoopMode == SlideshowLoopingMode.Infinite)
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.InfiniteLoopMoveNext", Id, InternalOrientation);
            }
        }
        else
        {
            var count = Count;

            if (Index >= count)
            {
                if (LoopMode != SlideshowLoopingMode.None)
                {
                    await SetIndexAsync(1);
                }
            }
            else
            {
                await SetIndexAsync(Index + 1);
            }
        }

        StartTimer();
    }

    /// <summary>
    /// Moves the slideshow to the previous item, handling looping behavior according to the current loop mode.
    /// </summary>
    /// <remarks>If the loop mode is set to Infinite, the method invokes a JavaScript function to perform the
    /// transition. If the loop mode is None and the current index is at the beginning, the method wraps around to the
    /// last item in the slideshow.</remarks>
    /// <returns>A task that represents the asynchronous operation of moving to the previous item in the slideshow.</returns>
    private async Task OnMovePreviousAsync()
    {
        StopTimer();

        if (LoopMode == SlideshowLoopingMode.Infinite)
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.InfiniteLoopMovePrevious", Id, InternalOrientation);
            }
        }
        else
        {
            var count = Count;

            if (Index > 1)
            {
                await SetIndexAsync(Index - 1);
            }
            else if (LoopMode != SlideshowLoopingMode.None)
            {
                await SetIndexAsync(count);
            }
        }

        StartTimer();
    }

    /// <summary>
    /// Asynchronously moves the internal index to the specified position if it is within the valid range.
    /// </summary>
    /// <remarks>The timer is stopped before moving the index and restarted afterward. If the specified index
    /// is outside the valid range, the operation has no effect and the index remains unchanged.</remarks>
    /// <param name="index">The zero-based index to move to. Must be greater than or equal to 0 and less than the total count.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task MoveToIndexAsync(int index)
    {
        StopTimer();

        var count = Count;

        if (index >= 0 && index < count)
        {
            await SetIndexAsync(index + 1);
        }

        StartTimer();
    }

    /// <summary>
    /// Handles key down events asynchronously, performing navigation actions based on the pressed key and the current
    /// orientation of the control.
    /// </summary>
    /// <remarks>Navigation actions are performed only for arrow keys that correspond to the control's current
    /// orientation. For example, left and right keys trigger navigation when the orientation is horizontal, while up
    /// and down keys trigger navigation when the orientation is vertical. If the pressed key does not match a
    /// navigation action, no operation is performed.</remarks>
    /// <param name="e">An object that provides data for the key down event, including information about which key was pressed.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes immediately if the key does not trigger a
    /// navigation action.</returns>
    private Task OnKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        return e.Key switch
        {
            KeyCode.Left => InternalOrientation == Orientation.Horizontal ? OnMovePreviousAsync() : Task.CompletedTask,
            KeyCode.Right => InternalOrientation == Orientation.Horizontal ? OnMoveNextAsync() : Task.CompletedTask,
            KeyCode.Up => InternalOrientation == Orientation.Vertical ? OnMovePreviousAsync() : Task.CompletedTask,
            KeyCode.Down => InternalOrientation == Orientation.Vertical ? OnMoveNextAsync() : Task.CompletedTask,
            _ => Task.CompletedTask,
        };
    }

    /// <summary>
    /// Removes the specified slideshow item from the collection of slides.
    /// </summary>
    /// <remarks>This method triggers an asynchronous state update after the item is removed, ensuring that
    /// any UI elements reflecting the slideshow state are updated accordingly.</remarks>
    /// <param name="value">The slideshow item to remove from the collection. This item must exist in the collection; otherwise, no action
    /// is taken.</param>
    internal void Remove(SlideshowItem value)
    {
        _slides.Remove(value);
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Adds the specified slideshow item to the collection.
    /// </summary>
    /// <remarks>After the item is added, the component's state is updated asynchronously to reflect the
    /// change.</remarks>
    /// <param name="value">The slideshow item to add. This parameter must not be null.</param>
    internal void Add(SlideshowItem value)
    {
        _slides.Add(value);
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Determines whether the specified slideshow item exists within the collection of slides.
    /// </summary>
    /// <remarks>This method checks for the presence of the given slideshow item by searching the collection.
    /// The comparison is based on the equality implementation of SlideshowItem{TItem}.</remarks>
    /// <param name="value">The slideshow item to locate in the collection. This parameter cannot be null.</param>
    /// <returns>true if the specified slideshow item is found in the collection; otherwise, false.</returns>
    internal bool Contains(SlideshowItem value)
    {
        return _slides.Contains(value);
    }

    /// <summary>
    /// Enables or disables touch interactions for the slideshow component asynchronously based on the current settings.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to update the touch settings of the slideshow. The
    /// module must be initialized before calling this method.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnEnableOrDisableTouchAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.DisableOrEnableTouch", Id, IsTouchEnabled, TouchThreshold);
        }
    }

    /// <summary>
    /// Handles changes to the slideshow's looping mode asynchronously, optionally storing or restoring the current
    /// slideshow items.
    /// </summary>
    /// <remarks>This method updates the slideshow state and clears any active transitions. It should be
    /// called when the looping mode is changed to ensure the slideshow items and transitions are managed
    /// correctly.</remarks>
    /// <param name="store">true to store the current slideshow items; false to restore previously stored items.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoopingModeChangedAsync(bool store = false)
    {
        Index = 1;

        if (_slides.Count > 0 && _module is not null)
        {
            await _module.InvokeVoidAsync(store ? "FluentUI.Blazor.Community.Slideshow.StoreItems" : "FluentUI.Blazor.Community.Slideshow.RestoreItems",
                Id, _slides.Select(x => x.Id).ToArray());

            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.ClearTransition", Id);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the current index asynchronously and notifies subscribers if the index changes.
    /// </summary>
    /// <remarks>If the index is updated, the method invokes the IndexChanged event and refreshes the
    /// component's state.</remarks>
    /// <param name="index">The new index value to set. If the value is equal to the current index, no update occurs.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SetIndexAsync(int index)
    {
        if (index == Index)
        {
            return;
        }

        Index = index;

        if (IndexChanged.HasDelegate)
        {
            await IndexChanged.InvokeAsync(index);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _currentIndexChanged = parameters.HasValueChanged(nameof(Index), Index);
        _autoPlayChanged = parameters.HasValueChanged(nameof(Autoplay), Autoplay);
        _intervalChanged = parameters.HasValueChanged(nameof(AutoplayInterval), AutoplayInterval);
        _isIndicatorPositionChanged = parameters.HasValueChanged(nameof(IndicatorPosition), IndicatorPosition);
        _isOrientationChanged = parameters.HasValueChanged(nameof(Orientation), Orientation);
        _isTouchEnabledChanged = parameters.HasValueChanged(nameof(IsTouchEnabled), IsTouchEnabled);
        _isLoopingModeChanged = parameters.HasValueChanged(nameof(LoopMode), LoopMode);
        _showIndicatorChanged = parameters.HasValueChanged(nameof(ShowIndicators), ShowIndicators);
        _hasInsideDialogChanged = parameters.HasValueChanged(nameof(InsideDialog), InsideDialog);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_currentIndexChanged)
        {
            _currentIndexChanged = false;
            StopTimer();
            StartTimer();
        }

        if (_isOrientationChanged || _isIndicatorPositionChanged)
        {
            SetIcons();
        }

        if (_autoPlayChanged || _intervalChanged)
        {
            if (Autoplay)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        if (_showIndicatorChanged)
        {
            OnShowIndicatorChanged();
        }
    }

    /// <summary>
    /// Handles changes to the visibility of the indicator based on the current slideshow looping mode.
    /// </summary>
    /// <remarks>If the looping mode is set to Infinite, the method exits without making any changes.
    /// Otherwise, it updates the icons to reflect the current state.</remarks>
    private void OnShowIndicatorChanged()
    {
        if (LoopMode == SlideshowLoopingMode.Infinite)
        {
            return;
        }

        SetIcons();
    }

    /// <summary>
    /// Sets the icons used for previous and next navigation according to the current orientation.
    /// </summary>
    /// <remarks>If the orientation is horizontal, left and right chevron icons are assigned; if vertical, up
    /// and down chevron icons are used. This method should be called whenever the orientation changes to ensure the
    /// navigation icons remain consistent with the layout.</remarks>
    private void SetIcons()
    {
        PreviousIcon = InternalOrientation == Orientation.Horizontal ? FluentCxSlideshowConstants.ChevronLeft : FluentCxSlideshowConstants.ChevronUp;
        NextIcon = InternalOrientation == Orientation.Horizontal ? FluentCxSlideshowConstants.ChevronRight : FluentCxSlideshowConstants.ChevronDown;
    }

    /// <summary>
    /// Handles slideshow resize events by updating the component's dimensions and state to reflect the new size.
    /// </summary>
    /// <remarks>This method is marked with <see cref="JSInvokableAttribute"/>, allowing it to be called from
    /// JavaScript code. It updates the internal state with the new dimensions and triggers a re-render of the component
    /// to ensure the UI reflects the latest size.</remarks>
    /// <param name="e">An object containing the updated width and height of the slideshow after resizing.</param>
    [JSInvokable]
    public void OnSlideshowResized(SlideshowMeasuredEventArgs e)
    {
        State.AddOrUpdateSize(Id, e.Width, e.Height);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_isTouchEnabledChanged)
        {
            await OnEnableOrDisableTouchAsync();
        }

        if (_isLoopingModeChanged)
        {
            await OnLoopingModeChangedAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptFileName);
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.Initialize", Id, _dotnetReference);

            if (InsideDialog)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.InsideDialog", Id);
            }

            await OnEnableOrDisableTouchAsync();
            await OnLoopingModeChangedAsync(true);
            OnShowIndicatorChanged();

            if (Autoplay)
            {
                StartTimer();
            }
        }
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Slideshow.Destroy", Id);
                await _module.DisposeAsync();
            }
            catch
            {
                // ignore
            }
        }

        await base.DisposeAsync(jsModule);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerTick;
            _timer.Dispose();
        }

        _dotnetReference.Dispose();

        await base.DisposeAsync();
    }

    /// <summary>
    /// Handles a swipe gesture on the slideshow, navigating to the next or previous slide based on the specified
    /// direction.
    /// </summary>
    /// <remarks>If autoplay is enabled, it is paused during the swipe and resumed afterward unless <see
    /// langword="true"/> is specified for <c>StopAutoplayWhenTouchEnabled</c>.</remarks>
    /// <param name="direction">Specifies the direction of the swipe gesture. Use <see cref="SlideshowSwipeDirection.Next"/> to move to the next
    /// slide or <see cref="SlideshowSwipeDirection.Previous"/> to move to the previous slide.</param>
    /// <returns>A task that represents the asynchronous operation of handling the swipe gesture.</returns>
    [JSInvokable("onTouchSwipe")]
    public async Task OnTouchSwipeAsync(SlideshowSwipeDirection direction)
    {
        if (Autoplay)
        {
            StopTimer();
        }

        switch (direction)
        {
            case SlideshowSwipeDirection.Next:
                await OnMoveNextAsync();
                break;

            case SlideshowSwipeDirection.Previous:
                await OnMovePreviousAsync();
                break;
        }

        if (!StopAutoplayWhenTouchEnabled &&
            Autoplay)
        {
            StartTimer();
        }
    }
}
