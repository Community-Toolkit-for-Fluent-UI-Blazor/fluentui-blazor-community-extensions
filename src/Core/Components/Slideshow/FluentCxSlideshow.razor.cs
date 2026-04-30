using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a slide show component.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public partial class FluentCxSlideshow<TItem> : FluentComponentBase
{
    /// <summary />
    public FluentCxSlideshow(LibraryConfiguration configuration) : base(configuration)
    {

    }

    /// <summary>
    /// Gets or sets a value indicating that the controls are shown.
    /// Default is true
    /// </summary>
    [Parameter]
    public bool ShowControls { get; set; } = true;

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

    /// <summary>
    /// Gets or sets the render fragment for the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected string? InternalClass => DefaultClassBuilder
        .AddClass("fluentcx-slideshow")
        .Build();

    /// <inheritdoc />
    protected string? InternalStyle => DefaultStyleBuilder
        .Build();
}
