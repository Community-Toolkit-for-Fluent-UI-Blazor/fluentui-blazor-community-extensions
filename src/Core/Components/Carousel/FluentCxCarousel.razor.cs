using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxCarousel component is a carousel which display content with a slide effect.
/// </summary>
public partial class FluentCxCarousel : FluentComponentBase
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
    }

    private void Previous()
    {
        if (_slides.Count == 0)
        {
            return;
        }

        CurrentIndex =
            (CurrentIndex - 1 + _slides.Count) % _slides.Count;
    }

    private void GoTo(int index)
    {
        if (index < 0 || index >= _slides.Count)
        {
            return;
        }

        CurrentIndex = index;
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
}
