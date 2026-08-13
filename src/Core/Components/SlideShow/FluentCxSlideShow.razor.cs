using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxSlideShow component is a carousel which display content with a slide effect.
/// </summary>
public partial class FluentCxSlideShow : FluentComponentBase
{

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
    /// Gets or sets the orientation of the slide show.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets a value indicating that the indicators are shown.
    /// </summary>
    [Parameter]
    public bool ShowIndicators { get; set; } = true;

    /// <summary>
    /// Gets or sets the position of the indicator.
    /// </summary>
    [Parameter]
    public SlideShowIndicatorPosition IndicatorPosition { get; set; } = SlideShowIndicatorPosition.Bottom;

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

    /// <summary />
    private string? ClassValue => DefaultClassBuilder
        .AddClass("fluentcx-slideshow")
        .Build();

    /// <summary />
    private string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    public FluentCxSlideShow(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    private Icon GetPreviousButtonIcon()
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

    private Icon GetNextButtonIcon()
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
}
