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
    /// Gets or sets the position of the indicator.
    /// </summary>
    [Parameter]
    public SlideShowIndicatorPosition ControlPosition { get; set; } = SlideShowIndicatorPosition.Bottom;

    /// <summary>
    /// Gets or sets the previous icon.
    /// </summary>
    [Parameter]
    public Icon PreviousIcon { get; set; } = new Icons.Regular.Size24.ChevronLeft();

    /// <summary>
    /// Gets or sets the next icon.
    /// </summary>
    [Parameter]
    public Icon NextIcon { get; set; } = new Icons.Regular.Size24.ChevronRight();

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
}
