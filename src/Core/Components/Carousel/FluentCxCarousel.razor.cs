using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxCarousel component is a carousel which display content with a slide effect.
/// </summary>
public partial class FluentCxCarousel : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets a value indicating that the controls are shown.
    /// </summary>
    [Parameter]
    public bool ShowControls { get; set; } = true;

    /// <summary>
    /// Gets or sets the orientation of the slide show.
    /// </summary>
    /// <remarks>If the indicator is visible, the position of it override this value.</remarks>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the render fragment for the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the previous icon.
    /// </summary>
    [Parameter]
    public Icon? PreviousIcon { get; set; }

    /// <summary>
    /// Gets or sets the next icon.
    /// </summary>
    [Parameter]
    public Icon? NextIcon { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the autoplay mode for the carousel.
    /// </summary>
    [Parameter]
    public CarouselAutoplayMode AutoplayMode { get; set; } = CarouselAutoplayMode.None;

    /// <summary>
    /// Gets or sets the autoplay interval in milliseconds.
    /// </summary>
    [Parameter]
    public int AutoplayInterval { get; set; } = 3000;

    /// <summary />
    private string? InternalClass => DefaultClassBuilder
        .AddClass("fluentcx-carousel")
        .Build();

    /// <summary />
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("height", Height)
        .AddStyle("width", Width)
        .Build();

    private readonly List<FluentCxSlide> _slides = [];
    internal int CurrentIndex { get; private set; }
    private System.Timers.Timer? _autoplayTimer;

    /// <summary />
    public FluentCxCarousel(LibraryConfiguration configuration) : base(configuration)
    {

    }

    internal void Register(FluentCxSlide slide)
    {
        if (_slides.Contains(slide))
        {
            return;
        }

        _slides.Add(slide);

        StateHasChanged();
    }

    internal void Unregister(FluentCxSlide slide)
    {
        _slides.Remove(slide);

        if (CurrentIndex >= _slides.Count)
        {
            CurrentIndex = Math.Max(0, _slides.Count - 1);
        }

        StateHasChanged();
    }

    internal bool IsActive(FluentCxSlide slide)
    {
        return _slides.IndexOf(slide) == CurrentIndex;
    }

    private void Next()
    {
        if (_slides.Count == 0)
        {
            return;
        }

        CurrentIndex = (CurrentIndex + 1) % _slides.Count;
        ResetAutoplayTimer();
    }

    private void Previous()
    {
        if (_slides.Count == 0)
        {
            return;
        }

        CurrentIndex =
            (CurrentIndex - 1 + _slides.Count) % _slides.Count;
        ResetAutoplayTimer();
    }

    private void GoTo(int index)
    {
        if (index < 0 || index >= _slides.Count)
        {
            return;
        }

        CurrentIndex = index;
        ResetAutoplayTimer();
    }

    private void ResetAutoplayTimer()
    {
        if (_autoplayTimer != null && AutoplayMode != CarouselAutoplayMode.None)
        {
            _autoplayTimer.Stop();
            _autoplayTimer.Start();
        }
    }

    private Icon GetPreviousIcon()
    {
        if (PreviousIcon is not null)
        {
            return PreviousIcon;
        }

        if (Orientation is Orientation.Horizontal)
        {
            return new Icons.Regular.Size24.ChevronLeft();
        }

        return new Icons.Regular.Size24.ChevronUp();
    }

    private Icon GetNextIcon()
    {
        if (NextIcon is not null)
        {
            return NextIcon;
        }

        if (Orientation is Orientation.Horizontal)
        {
            return new Icons.Regular.Size24.ChevronRight();
        }

        return new Icons.Regular.Size24.ChevronDown();
    }

    private string GetOrientationAttribute()
    {
        return Orientation.ToString().ToLowerInvariant();
    }

    /// <inheritdoc />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ConfigureAutoplay();
        }

        return base.OnAfterRenderAsync(firstRender);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        var configureAutoplay = false;
        if (parameters.TryGetValue<CarouselAutoplayMode>(nameof(AutoplayMode), out var newAutoplayMode) && newAutoplayMode != AutoplayMode)
        {
            AutoplayMode = newAutoplayMode;
            configureAutoplay = true;
        }

        if (parameters.TryGetValue<int>(nameof(AutoplayInterval), out var newAutoplayInterval) && newAutoplayInterval != AutoplayInterval)
        {
            AutoplayInterval = newAutoplayInterval;
            configureAutoplay = true;
        }

        if (configureAutoplay)
        {
            ConfigureAutoplay();
        }

        return base.SetParametersAsync(parameters);
    }

    private void ConfigureAutoplay()
    {
        _autoplayTimer?.Stop();
        _autoplayTimer?.Dispose();
        _autoplayTimer = null;

        if (AutoplayMode == CarouselAutoplayMode.None || AutoplayInterval <= 0)
        {
            return;
        }

        _autoplayTimer = new System.Timers.Timer(AutoplayInterval);
        _autoplayTimer.Elapsed += OnAutoplayTick;
        _autoplayTimer.AutoReset = true;
        _autoplayTimer.Start();
    }

    private void OnAutoplayTick(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(() =>
        {
            if (_slides.Count == 0)
            {
                return;
            }

            switch (AutoplayMode)
            {
                case CarouselAutoplayMode.Rewind:
                    if (CurrentIndex >= _slides.Count - 1)
                    {
                        CurrentIndex = 0;
                    }
                    else
                    {
                        CurrentIndex++;
                    }

                    break;

                case CarouselAutoplayMode.Infinite:
                    // For infinite mode, we just move to next slide
                    // The CSS transitions will handle the smooth looping effect
                    CurrentIndex = (CurrentIndex + 1) % _slides.Count;
                    break;

                case CarouselAutoplayMode.None:
                default:
                    // Stop the timer if mode is None
                    _autoplayTimer?.Stop();
                    break;
            }

            StateHasChanged();
        });
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _autoplayTimer?.Stop();
        _autoplayTimer?.Dispose();
    }
}
