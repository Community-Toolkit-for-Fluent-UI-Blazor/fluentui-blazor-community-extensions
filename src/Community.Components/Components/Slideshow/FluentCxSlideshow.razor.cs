using System.Globalization;
using System.Timers;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a slide show component.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public partial class FluentCxSlideshow<TItem>
    : FluentComponentBase, IAsyncDisposable, IDisposable
{
    #region Fields

    /// <summary>
    /// Represents the left chevron icon.
    /// </summary>
    private static readonly Icon _chevronLeft = new Size24.ChevronLeft();

    /// <summary>
    /// Represents the right chevron icon.
    /// </summary>
    private static readonly Icon _chevronRight = new Size24.ChevronRight();

    /// <summary>
    /// Represents the up chevron icon.
    /// </summary>
    private static readonly Icon _chevronUp = new Size24.ChevronUp();

    /// <summary>
    /// Represents the down chevron icon.
    /// </summary>
    private static readonly Icon _chevronDown = new Size24.ChevronDown();

    /// <summary>
    /// Represents the slides.
    /// </summary>
    /// <remarks>Use when <see cref="ChildContent"/> is not null.</remarks>
    private readonly List<SlideshowItem<TItem>> _slides = [];

    /// <summary>
    /// Represents the images.
    /// </summary>
    /// <remarks>When orientation or ImageRatio changes, we need to update the image size.</remarks>
    private readonly List<SlideshowImage<TItem>> _images = [];

    /// <summary>
    /// Represents the fragment to render the dots.
    /// </summary>
    private readonly RenderFragment<int> _renderIndicatorItems;

    /// <summary>
    /// Represents the timer.
    /// </summary>
    private Timer? _timer;

    /// <summary>
    /// Represents a value indicating if the current index has changed.
    /// </summary>
    private bool _currentIndexChanged;

    /// <summary>
    /// Represents a value indicating if the autoplay has changed.
    /// </summary>
    private bool _autoPlayChanged;

    /// <summary>
    /// Represents a value indicating if the interval has changed.
    /// </summary>
    private bool _intervalChanged;

    /// <summary>
    /// Represents a value indicating if the element was resized on the width.
    /// </summary>
    private bool _elementWidthResized;

    /// <summary>
    /// Represents a value indicating if the element was resized on the height.
    /// </summary>
    private bool _elementHeightResized;

    /// <summary>
    /// Represents a value indicating if the aspect ratio has changed.
    /// </summary>
    private bool _isAspectRatioChanged;

    /// <summary>
    /// Represents the reference of the component.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxSlideshow<TItem>>? _dotnetReference;

    /// <summary>
    /// Represents the module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the name of the javascript filename.
    /// </summary>
    private const string JavascriptFileName = "./_content/FluentUI.Blazor.Community.Components/Components/Slideshow/FluentCxSlideshow.razor.js";

    /// <summary>
    /// Value indicating if the content is shown.
    /// </summary>
    private bool _showContent;

    /// <summary>
    /// Represents the resized width.
    /// </summary>
    private int _resizedWidth;

    /// <summary>
    /// Represents the resized height.
    /// </summary>
    private int _resizedHeight;

    /// <summary>
    /// Represents a value indicating if the position of the indicator has changed. 
    /// </summary>
    private bool _isIndicatorPositionChanged;

    /// <summary>
    /// Represents a value indicating if the orientation has changed.
    /// </summary>
    private bool _isOrientationChanged;

    /// <summary>
    /// Represents a value indicating if the touch enabled has changed.
    /// </summary>
    private bool _isTouchEnabledChanged;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating that the controls are shown.
    /// </summary>
    [Parameter]
    public bool ShowControls { get; set; } = true;

    /// <summary>
    /// Gets or sets the render fragment for the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the indicators are shown.
    /// </summary>
    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    /// <summary>
    /// Gets or sets the items to render.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the item template for rendering the items.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the slide show is autoplay.
    /// </summary>
    [Parameter]
    public bool Autoplay { get; set; }

    /// <summary>
    /// Gets or sets the interval between items.
    /// </summary>
    /// <remarks>If an item has its <see cref="SlideshowImage{TItem}.Interval"/> set, it will override
    ///  the <see cref="AutoplayInterval"/>.</remarks>
    [Parameter]
    public TimeSpan AutoplayInterval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets a the duration of the animation.
    /// </summary>
    [Parameter]
    public TimeSpan SlideDuration { get; set; } = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the index of the item to show.
    /// </summary>
    [Parameter]
    public int Index { get; set; } = 1;

    /// <summary>
    /// Gets or sets the callback to raise when the index has changed.
    /// </summary>
    [Parameter]
    public EventCallback<int> IndexChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the slide show loops after reaching the last item.
    /// </summary>
    [Parameter]
    public bool IsLoopingEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the label for previous button tooltip.
    /// </summary>
    [Parameter]
    public string PreviousLabel { get; set; } = "Previous";

    /// <summary>
    /// Gets or sets the label for next button tooltip.
    /// </summary>
    [Parameter]
    public string NextLabel { get; set; } = "Next";

    /// <summary>
    /// Gets or sets the function to retrieve the identifier of an item.
    /// </summary>
    [Parameter]
    public Func<TItem, long>? ItemFunc { get; set; }

    /// <summary>
    /// Gets or sets the position of the indicator.
    /// </summary>
    [Parameter]
    public SlideshowIndicatorPosition IndicatorPosition { get; set; } = SlideshowIndicatorPosition.Bottom;

    /// <summary>
    /// Gets or sets the template for an indicator item.
    /// </summary>
    [Parameter]
    public RenderFragment<int>? IndicatorTemplate { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the slide show.
    /// </summary>
    /// <remarks>If the indicator is visible, the position of it override this value.</remarks>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets if the touch is enabled, allow the user to use its fingers to swipe between slides.
    /// </summary>
    /// <remarks>If <see cref="Autoplay"/> is <see langword="true" /> and <see cref="IsTouchEnabled"/>
    ///  is <see langword="true" />, <see cref="Autoplay"/> will be desactivated.</remarks>
    [Parameter]
    public bool IsTouchEnabled { get; set; }

    /// <summary>
    /// Gets the number of items.
    /// </summary>
    private int Count => ChildContent is not null ? _slides.Count : Items.Count();

    /// <summary>
    /// Gets a value indicating if the previous button is disabled.
    /// </summary>
    private bool IsPreviousDisabled => !IsLoopingEnabled && Index == 1;

    /// <summary>
    /// Gets a value indicating if the next button disabled.
    /// </summary>
    private bool IsNextDisabled => !IsLoopingEnabled && Index == Count;

    /// <summary>
    /// Gets or sets the css for the <see cref="FluentCxSlideshow{TItem}"/>.
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow")
        .Build();

    /// <summary>
    /// Gets the orientation of the indicators.
    /// </summary>
    internal Orientation InternalOrientation => GetInternalOrientation();

    /// <summary>
    /// Gets the internal style for the <see cref="FluentCxSlideshow{TItem}"/>.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--slideshow-width", $"{Width}px", Width.HasValue && !_elementWidthResized)
        .AddStyle("--slideshow-height", $"{Height}px", Height.HasValue && !_elementHeightResized)
        .AddStyle("--slideshow-width", $"{_resizedWidth}px", _elementWidthResized)
        .AddStyle("--slideshow-height", $"{_resizedHeight}px", _elementHeightResized)
        .AddStyle("--slideshow-item-count", Items.Any() ? Items.Count().ToString(CultureInfo.CurrentCulture) : _slides.Count.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-current-index", Index.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-duration", $"{SlideDuration.TotalMilliseconds}ms")
        .Build();

    /// <summary>
    /// Gets the css for the internal container of the <see cref="FluentCxSlideshow{TItem}" />.
    /// </summary>
    private string? InternalContainerCss => new CssBuilder()
        .AddClass("slideshow-container")
        .AddClass("slideshow-animate")
        .AddClass("slideshow-animate-horizontal", InternalOrientation == Orientation.Horizontal)
        .AddClass("slideshow-animate-vertical", InternalOrientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Gets the css for the indicators of the <see cref="FluentCxSlideshow{TItem}" />.
    /// </summary>
    private string? InternalIndicatorsCss => new CssBuilder()
        .AddClass("slideshow-indicators")
        .AddClass("slideshow-indicators-vertical", InternalOrientation == Orientation.Vertical)
        .AddClass("slideshow-indicators-horizontal", InternalOrientation == Orientation.Horizontal)
        .AddClass("slideshow-indicators-top", IndicatorPosition == SlideshowIndicatorPosition.Top)
        .AddClass("slideshow-indicators-bottom", IndicatorPosition == SlideshowIndicatorPosition.Bottom)
        .AddClass("slideshow-indicators-left", IndicatorPosition == SlideshowIndicatorPosition.Left)
        .AddClass("slideshow-indicators-right", IndicatorPosition == SlideshowIndicatorPosition.Right)
        .Build();

    /// <summary>
    /// Gets the style of the previous button.
    /// </summary>
    private string? PreviousButtonStyle => InternalOrientation == Orientation.Horizontal ? PreviousHorizontalStyle : PreviousVerticalStyle;

    /// <summary>
    /// Gets the style of the previous button.
    /// </summary>
    private string? NextButtonStyle => InternalOrientation == Orientation.Horizontal ? NextHorizontalStyle : NextVerticalStyle;

    /// <summary>
    /// Gets or sets the javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating if the image keeps the aspect ratio.
    /// </summary>
    /// <remarks>Works only if the item contains an img div.</remarks>
    [Parameter]
    public SlideshowImageRatio ImageAspectRatio { get; set; } = SlideshowImageRatio.Auto;

    /// <summary>
    /// Gets or sets the previous icon.
    /// </summary>
    [Parameter]
    public Icon PreviousIcon { get; set; } = new Size24.ChevronLeft();

    /// <summary>
    /// Gets or sets the next icon.
    /// </summary>
    [Parameter]
    public Icon NextIcon { get; set; } = new Size24.ChevronRight();

    /// <summary>
    /// Gets the style for previous button on horizontal orientation.
    /// </summary>
    private static string PreviousHorizontalStyle => "position: absolute; left: 50px; top: 50%; transform: translateY(-50%); height: 100px; width: 32px";

    /// <summary>
    /// Gets the style for previous button on vertical orientation.
    /// </summary>
    private static string PreviousVerticalStyle => "position: absolute; left: 50%; top: 50px; transform: translateX(-50%); width: 100px; height: 32px";

    /// <summary>
    /// Gets the style for next button on horizontal orientation.
    /// </summary>
    private static string NextHorizontalStyle => "position: absolute; right: 50px; top: 50%; transform: translateY(-50%); height: 100px; width: 32px";

    /// <summary>
    /// Gets the style for next button on vertical orientation.
    /// </summary>
    private static string NextVerticalStyle => "position: absolute; left: 50%; bottom: 50px; transform: translateX(-50%); width: 100px; height: 32px";

    /// <summary>
    /// Gets or sets the <see cref="DeviceInfoState"/>.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback to raise when a resizing operation is completed.
    /// </summary>
    [Parameter]
    public EventCallback OnImagesResizeCompleted { get; set; }

    /// <summary>
    /// Gets or sets the minimum distance in pixels that the user should swipe to move the slide.
    /// </summary>
    [Parameter]
    public int TouchThreshold { get; set; } = 50;

    /// <summary>
    /// Gets or sets a value indicating if the autoplay stops when the touch is enabled.
    /// </summary>
    [Parameter]
    public bool StopAutoplayWhenTouchEnabled { get; set; } = true;

    #endregion Properties

    #region Methods

    /// <summary>
    /// Gets the internal orientation.
    /// </summary>
    /// <returns>Returns the orientation from the indicator position if <see cref="ShowIndicators"/>
    ///  is set to <see langword="true" />, or use the <see cref="Orientation"/> otherwise.</returns>
    private Orientation GetInternalOrientation()
    {
        if (ShowIndicators)
        {
            return IndicatorPosition switch
            {
                SlideshowIndicatorPosition.Top or SlideshowIndicatorPosition.Bottom => Orientation.Horizontal,
                _ => Orientation.Vertical,
            };
        }

        return Orientation;
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    /// <remarks>The timer starts if the <see cref="Autoplay" /> is set to <see langword="true" />.</remarks>
    private void StartTimer()
    {
        if (!Autoplay)
        {
            return;
        }

        StopTimer();

        var interval = AutoplayInterval;

        if (_slides.Count > Index - 1 &&
            _slides[Index - 1].Interval.HasValue)
        {
            interval = _slides[Index - 1].Interval.GetValueOrDefault();
        }
        
        _timer = new Timer(interval);
        _timer.Elapsed += OnTimerTick;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    private void StopTimer()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerTick;
        }
    }

    /// <summary>
    /// Occurs when the timer ticks.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Event args of the timer.</param>
    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(OnMoveNextAsync);
    }

    /// <summary>
    /// Checks if the <paramref name="index"/> is equals to the <see cref="Index"/>.
    /// </summary>
    /// <param name="index">Index to check.</param>
    /// <returns>Returns <see langword="true"/> if the <paramref name="index"/> is equal to <see cref="Index"/>.</returns>
    private bool IsCurrent(int index)
    {
        return Index == index + 1;
    }

    /// <summary>
    /// Move to the previous slide.
    /// </summary>
    /// <returns>Returns a task which moves to the previous slide when completed.</returns>
    private async Task OnMovePreviousAsync()
    {
        StopTimer();

        if (ChildContent is not null)
        {
            await SetInternalIndexAsync(_slides.Count);
        }
        else
        {
            await SetInternalIndexAsync(Items.Count());
        }

        StartTimer();

        async Task SetInternalIndexAsync(int count)
        {
            if (Index > 1)
            {
                await SetIndexAsync(Index - 1);
            }
            else if (IsLoopingEnabled)
            {
                await SetIndexAsync(count);
            }
        }
    }

    /// <summary>
    /// Sets the <see cref="Index"/> to <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the slide to slow.</param>
    /// <returns>Returns a task which set the index to the <paramref name="index"/> and
    ///  raise <see cref="IndexChanged"/> when completed.</returns>
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

    /// <summary>
    /// Gets the aria-hidden value.
    /// </summary>
    /// <param name="item">Hidden item.</param>
    /// <returns>Returns the aria-hidden value.</returns>
    internal string? GetAriaHiddenValue(SlideshowItem<TItem> item)
    {
        return GetAriaHiddenValue(_slides.IndexOf(item));
    }

    /// <summary>
    /// Move to the next slide.
    /// </summary>
    /// <returns>Returns a task which moves to the next slide when completed.</returns>
    private async Task OnMoveNextAsync()
    {
        StopTimer();

        if (ChildContent is not null)
        {
            await SetInternalIndexAsync(_slides.Count);
        }
        else
        {
            await SetInternalIndexAsync(Items.Count());
        }

        StartTimer();

        async Task SetInternalIndexAsync(int count)
        {
            if (Index >= count)
            {
                if (IsLoopingEnabled)
                {
                    await SetIndexAsync(1);
                }
                else
                {
                    if (!Autoplay)
                    {
                        return;
                    }

                    StopTimer();
                }
            }
            else
            {
                await SetIndexAsync(Index + 1);
            }
        }
    }

    /// <summary>
    /// Move the slide to the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the slide.</param>
    /// <returns>Returns a task which moves the slide to the current index.</returns>
    internal async Task MoveToIndexAsync(int index)
    {
        StopTimer();

        if (ChildContent is not null)
        {
            await MoveToIndexInternalAsync(_slides.Count);
        }
        else
        {
            await MoveToIndexInternalAsync(Items.Count());
        }

        StartTimer();

        async Task MoveToIndexInternalAsync(int count)
        {
            if (index >= 0 && index < count)
            {
                await SetIndexAsync(index + 1);
            }
        }
    }

    /// <summary>
    /// Gets the aria-hidden value.
    /// </summary>
    /// <param name="index">Index to check.</param>
    /// <returns>Returns <see langword="true"/> if the <see cref="Index"/> is equal to <paramref name="index"/>,
    ///  <see langword="false" /> otherwise.</returns>
    private string GetAriaHiddenValue(int index)
    {
        return !IsCurrent(index) ? "true" : "false";
    }

    /// <summary>
    /// Occurs when a key of the keyboard is down.
    /// </summary>
    /// <param name="e">Event args of the keyboard.</param>
    /// <returns>Returns a task which moves the slide when completed.</returns>
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
    /// Removes the <paramref name="value"/> from the component.
    /// </summary>
    /// <param name="value">Value to remove.</param>
    internal void Remove(SlideshowItem<TItem> value)
    {
        _slides.Remove(value);
        StateHasChanged();
    }

    /// <summary>
    /// Adds the <paramref name="value"/> from the component.
    /// </summary>
    /// <param name="value">Value to add.</param>
    internal void Add(SlideshowItem<TItem> value)
    {
        _slides.Add(value);
        StateHasChanged();
    }

    /// <summary>
    /// Check if the <paramref name="value"/> is inside the component.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>Returns <see langword="true" /> if the item is inside the component, <see langword="false" /> otherwise.</returns>
    internal bool Contains(SlideshowItem<TItem> value)
    {
        return _slides.Contains(value);
    }

    /// <summary>
    /// Occurs when the aspect ratio has changed.
    /// </summary>
    /// <returns></returns>
    private async Task OnAspectRatioChangedAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("setImagesSize", Id, ImageAspectRatio, _images.Select(x => x.Id).ToArray());
        }
    }

    /// <summary>
    /// Occurs when touch is enabled or disabled.
    /// </summary>
    /// <returns></returns>
    private async Task OnEnableOrDisableTouchAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("disableOrEnableTouch", Id, IsTouchEnabled, TouchThreshold);
        }
    }

    /// <summary>
    /// Occurs when the orientation changed.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Args of the method.</param>
    private async void OnOrientationChanged(object? sender, DeviceOrientation e)
    {
        await OnAspectRatioChangedAsync();
    }

    /// <summary>
    /// Adds an image into the list of images.
    /// </summary>
    /// <param name="value">Image to add.</param>
    internal void Add(SlideshowImage<TItem> value)
    {
        _images.Add(value);
    }

    /// <summary>
    /// Removes an image from the list of images.
    /// </summary>
    /// <param name="value">Image to remove.</param>
    internal void Remove(SlideshowImage<TItem> value)
    {
        _images.Remove(value);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _currentIndexChanged = parameters.HasValueChanged(nameof(Index), Index);
        _autoPlayChanged = parameters.HasValueChanged(nameof(Autoplay), Autoplay);
        _intervalChanged = parameters.HasValueChanged(nameof(AutoplayInterval), AutoplayInterval);
        _isAspectRatioChanged = parameters.HasValueChanged(nameof(ImageAspectRatio), ImageAspectRatio);
        _isIndicatorPositionChanged = parameters.HasValueChanged(nameof(IndicatorPosition), IndicatorPosition);
        _isOrientationChanged = parameters.HasValueChanged(nameof(Orientation), Orientation);
        _isTouchEnabledChanged = parameters.HasValueChanged(nameof(IsTouchEnabled), IsTouchEnabled);

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
            PreviousIcon = InternalOrientation == Orientation.Horizontal ? _chevronLeft : _chevronUp;
            NextIcon = InternalOrientation == Orientation.Horizontal ? _chevronRight : _chevronDown;
        }

        if (_autoPlayChanged || _intervalChanged)
        {
            if (_autoPlayChanged)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        if (Width.HasValue &&
            Height.HasValue)
        {
            _showContent = true;
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_isAspectRatioChanged)
        {
            await OnAspectRatioChangedAsync();
        }

        if (_isTouchEnabledChanged)
        {
            await OnEnableOrDisableTouchAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", JavascriptFileName);
            await _module.InvokeVoidAsync("initialize", Id, _dotnetReference, Width, Height);
            await OnEnableOrDisableTouchAsync();
            await OnAspectRatioChangedAsync();

            if (Autoplay)
            {
                StartTimer();
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerTick;
            _timer.Dispose();
            _timer = null;
        }

        _dotnetReference?.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (DeviceInfoState.DeviceInfo is not null)
        {
            DeviceInfoState.DeviceInfo.OrientationChanged -= OnOrientationChanged;
        }

        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("destroy", Id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        { }
    }

    /// <summary>
    /// Occurs when <see cref="Width"/> or <see cref="Height"/> are not provided.
    /// </summary>
    /// <param name="width">Width of the parent.</param>
    /// <param name="height">Height of the parent.</param>
    /// <remarks>When <see cref="Width"/> or <see cref="Height"/> are not provided, we took the size from
    ///  its parent.</remarks>
    [JSInvokable("setSizeFromParent")]
    public void SetParentSize(int width, int height)
    {
        Width = width;
        Height = height;
        _showContent = true;
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (DeviceInfoState.DeviceInfo is not null)
        {
            DeviceInfoState.DeviceInfo.OrientationChanged += OnOrientationChanged;
        }
    }

    /// <summary>
    /// Occurs when all images have been resized.
    /// </summary>
    [JSInvokable("setAutoSizeCompleted")]
    public async Task OnAutoSizeCompletedAsync()
    {
        if (OnImagesResizeCompleted.HasDelegate)
        {
            await OnImagesResizeCompleted.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when all images have been resized to fill the container.
    /// </summary>
    /// <param name="width">Width of the container.</param>
    /// <param name="height">Height of the container.</param>
    /// <returns>Returns a task which set the container size when completed.</returns>
    [JSInvokable("setFillSizeCompleted")]
    public async Task OnFillSizeCompletedAsync(int width, int height)
    {
        _elementHeightResized = true;
        _elementWidthResized = true;
        _resizedWidth = width;
        _resizedHeight = height;

        await InvokeAsync(StateHasChanged);

        if (OnImagesResizeCompleted.HasDelegate)
        {
            await OnImagesResizeCompleted.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the component is resized.
    /// </summary>
    /// <param name="value">Value of the new size of the component and is the value are fixed.</param>
    /// <returns>The new size of the component.</returns>
    /// <remarks>
    /// If <see cref="Width"/> or <see cref="Height"/> are provided, these values are considered fixed,
    ///  so even if the component container became greater than the component size, the component
    ///  won't be greater than the fixed value.
    /// </remarks>
    [JSInvokable("resizeObserverEvent")]
    public int?[] ResizeObserverEvent(SlideshowResize value)
    {
        if (value.Width > Width && value.FixedWidth)
        {
            _elementWidthResized = false;
            _resizedWidth = 0;
        }
        else
        {
            _resizedWidth = value.Width;
            _elementWidthResized = true;
        }

        if (value.Height > Height && value.FixedHeight)
        {
            _elementHeightResized = false;
            _resizedHeight = 0;
        }
        else
        {
            _resizedHeight = value.Height;
            _elementHeightResized = true;
        }

        StateHasChanged();

        return
        [
            _elementWidthResized ? _resizedWidth : Width,
            _elementHeightResized ? _resizedHeight : Height
        ];
    }

    /// <summary>
    /// Occurs when the user swipe the slide.
    /// </summary>
    /// <param name="direction">Direction of the slide.</param>
    /// <returns>Returns a task wich moves the slide according to the <paramref name="direction"/>
    ///  when completed.</returns>
    [JSInvokable("onTouchSwipe")]
    public async Task OnTouchSwipeAsync(string direction)
    {
        if (Autoplay)
        {
            StopTimer();
        }

        var isHorizontal = InternalOrientation == Orientation.Horizontal;

        if ((isHorizontal && direction == "left") ||
            (!isHorizontal && direction == "up"))
        {
            await OnMoveNextAsync();
        }
        else if ((isHorizontal && direction == "right") ||
                 (!isHorizontal && direction == "down"))
        {
            await OnMovePreviousAsync();
        }

        if (!StopAutoplayWhenTouchEnabled &&
            Autoplay)
        {
            StartTimer();
        }
    }

    #endregion Methods
}
