using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

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

    /// <summary />
    public FluentCxCarousel(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    private readonly List<FluentCxSlide> _slides = [];

    internal int CurrentIndex { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
}
