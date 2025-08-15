using System.Globalization;
using System.Timers;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace FluentUI.Blazor.Community.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentCxSlideshow<TItem>
    : FluentComponentBase, IDisposable
{
    private readonly List<SlideshowItem<TItem>> _slides = [];
    private readonly RenderFragment<int> _renderDots;
    private Timer? _timer;
    private bool _currentIndexChanged;
    private bool _autoPlayChanged;
    private bool _intervalChanged;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public bool ShowControls { get; set; } = true;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    [Parameter]
    public bool Autoplay { get; set; } = true;

    [Parameter]
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);

    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);

    [Parameter]
    public string? Width { get; set; } = "100%";

    [Parameter]
    public string? Height { get; set; } = "100%";

    [Parameter]
    public int Index { get; set; } = 1;

    [Parameter]
    public EventCallback<int> IndexChanged { get; set; }

    [Parameter]
    public bool IsLoopingEnabled { get; set; } = true;

    [Parameter]
    public string PreviousLabel { get; set; } = "Previous";

    [Parameter]
    public string NextLabel { get; set; } = "Next";

    [Parameter]
    public Func<TItem, long>? ItemFunc { get; set; }

    private int Count => ChildContent is not null ? _slides.Count : Items.Count();

    private bool IsPreviousDisabled => !IsLoopingEnabled && Index == 1;

    private bool IsNextDisabled => !IsLoopingEnabled && Index == Count;

    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow")
        .Build();

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("--slideshow-item-count", Items.Any() ? Items.Count().ToString(CultureInfo.CurrentCulture) : _slides.Count.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-current-index", Index.ToString(CultureInfo.CurrentCulture))
        .AddStyle("--slideshow-duration", $"{Duration.TotalMilliseconds}ms")
        .Build();

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

    private void StopTimer()
    {
        if (_timer is not null)
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerTick;
        }
    }

    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(OnMoveNextAsync);
    }

    private bool IsCurrent(int index)
    {
        return Index == index + 1;
    }

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

    internal string? GetAriaHiddenValue(SlideshowItem<TItem> item)
    {
        return GetAriaHiddenValue(_slides.IndexOf(item));
    }

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

    private async Task MoveToIndexAsync(int index)
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

    private string GetAriaHiddenValue(int index)
    {
        return !IsCurrent(index) ? "true" : "false";
    }

    private Task OnKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        return e.Key switch
        {
            KeyCode.Left => OnMovePreviousAsync(),
            KeyCode.Right => OnMoveNextAsync(),
            _ => Task.CompletedTask,
        };
    }

    internal void Remove(SlideshowItem<TItem> value)
    {
        _slides.Remove(value);
        StateHasChanged();
    }

    internal void Add(SlideshowItem<TItem> value)
    {
        _slides.Add(value);
        StateHasChanged();
    }

    internal bool Contains(SlideshowItem<TItem> value)
    {
        return _slides.Contains(value);
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        _currentIndexChanged = parameters.HasValueChanged(nameof(Index), Index);
        _autoPlayChanged = parameters.HasValueChanged(nameof(Autoplay), Autoplay);
        _intervalChanged = parameters.HasValueChanged(nameof(Interval), Interval);

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
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && Autoplay)
        {
            StartTimer();
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

        GC.SuppressFinalize(this);
    }
}
