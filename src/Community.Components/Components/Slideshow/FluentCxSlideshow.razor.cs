using System.Globalization;
using System.Timers;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
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
    private readonly RenderFragment<int> _renderDots;

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
    /// Represents a value indicating if the autosize has changed.
    /// </summary>
    private bool _isAutoSizeChanged;

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
    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets a the duration of the animation.
    /// </summary>
    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; } = "100%";

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
    internal Orientation Orientation => IndicatorPosition == SlideshowIndicatorPosition.Top || IndicatorPosition == SlideshowIndicatorPosition.Bottom ? Orientation.Horizontal : Orientation.Vertical;

    /// <summary>
    /// Gets the internal style for the <see cref="FluentCxSlideshow{TItem}"/>.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--slideshow-width", Width ?? "100%")
        .AddStyle("--slideshow-height", Height ?? "100%")
        .AddStyle("--slideshow-item-count", Items.Any() ? Items.Count().ToString(CultureInfo.CurrentCulture) : _slides.Count.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-current-index", Index.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-duration", $"{Duration.TotalMilliseconds}ms")
        .Build();

    /// <summary>
    /// Gets the css for the internal container of the <see cref="FluentCxSlideshow{TItem}" />.
    /// </summary>
    private string? InternalContainerCss => new CssBuilder()
        .AddClass("slideshow-container")
        .AddClass("slideshow-animate")
        .AddClass("slideshow-animate-horizontal", Orientation == Orientation.Horizontal)
        .AddClass("slideshow-animate-vertical", Orientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Gets the css for the indicators of the <see cref="FluentCxSlideshow{TItem}" />.
    /// </summary>
    private string? InternalIndicatorsCss => new CssBuilder()
        .AddClass("slideshow-indicators")
        .AddClass("slideshow-indicators-vertical", Orientation == Orientation.Vertical)
        .AddClass("slideshow-indicators-horizontal", Orientation == Orientation.Horizontal)
        .AddClass("slideshow-indicators-top", IndicatorPosition == SlideshowIndicatorPosition.Top)
        .AddClass("slideshow-indicators-bottom", IndicatorPosition == SlideshowIndicatorPosition.Bottom)
        .AddClass("slideshow-indicators-left", IndicatorPosition == SlideshowIndicatorPosition.Left)
        .AddClass("slideshow-indicators-right", IndicatorPosition == SlideshowIndicatorPosition.Right)
        .Build();

    /// <summary>
    /// Gets the style of the previous button.
    /// </summary>
    private string? PreviousButtonStyle => Orientation == Orientation.Horizontal ? PreviousHorizontalStyle : PreviousVerticalStyle;

    /// <summary>
    /// Gets the style of the previous button.
    /// </summary>
    private string? NextButtonStyle => Orientation == Orientation.Horizontal ? NextHorizontalStyle : NextVerticalStyle;

    /// <summary>
    /// Gets or sets a value indicating if the component takes the size of its direct parent.
    /// </summary>
    /// <remarks>If <see cref="AutoSize"/> is set to <see langword="true"/>, the parameters
    ///  <see cref="Width"/> and <see cref="Height"/> are not used.</remarks>
    [Parameter]
    public bool AutoSize { get; set; }

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

    #endregion Properties

    #region Methods

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
        _timer = new Timer(Interval);
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
            if(Index > 1)
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
            KeyCode.Left => OnMovePreviousAsync(),
            KeyCode.Right => OnMoveNextAsync(),
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
    /// Occurs when the autosize is set.
    /// </summary>
    /// <returns>Returns a task which autosizes the component to the parent's size.</returns>
    private async Task OnAutoSizeChangedAsync()
    {
        if (AutoSize &&
            _module is not null)
        {
            await _module.InvokeVoidAsync("autoSize", Id);
        }
    }

    /// <summary>
    /// 
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
    /// Occurs when the orientation changed.
    /// </summary>
    /// <param name="sender">Object which invokes the method.</param>
    /// <param name="e">Args of the method.</param>
    private void OnOrientationChanged(object? sender, DeviceOrientation e)
    {
        InvokeAsync(OnAutoSizeChangedAsync);
        InvokeAsync(OnAspectRatioChangedAsync);
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
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _currentIndexChanged = parameters.HasValueChanged(nameof(Index), Index);
        _autoPlayChanged = parameters.HasValueChanged(nameof(Autoplay), Autoplay);
        _intervalChanged = parameters.HasValueChanged(nameof(Interval), Interval);
        _isAutoSizeChanged = parameters.HasValueChanged(nameof(AutoSize), AutoSize);
        _isAspectRatioChanged = parameters.HasValueChanged(nameof(ImageAspectRatio), ImageAspectRatio);

        await base.SetParametersAsync(parameters);
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

        if (!AutoSize)
        {
            _showContent = true;
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await OnAutoSizeChangedAsync();

        if (_isAspectRatioChanged)
        {
            await OnAspectRatioChangedAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", JavascriptFileName);
            await _module.InvokeVoidAsync("initialize", Id, _dotnetReference);
            await OnAutoSizeChangedAsync();
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
        catch(JSDisconnectedException)
        { }
    }

    /// <summary>
    /// Occurs when the components gets the size of its parent.
    /// </summary>
    /// <param name="width">Width of the parent.</param>
    /// <param name="height">Height of the parent.</param>
    [JSInvokable("getParentSize")]
    public void GetParentSize(int width, int height)
    {
        Width = $"{width}px";
        Height = $"{height}px";
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

    #endregion Methods
}
