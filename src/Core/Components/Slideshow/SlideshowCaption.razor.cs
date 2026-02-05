using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a caption for a <see cref="SlideshowItem{TItem}"/>.
/// </summary>
public partial class SlideshowCaption
     : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the SlideshowCaption class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine how the library manages and displays slideshow captions. Cannot be
    /// null.</param>
    public SlideshowCaption(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the caption is fixed to the bottom of the slideshow item.
    /// </summary>
    [Parameter]
    public bool Fixed { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum width, in pixels, that the component can occupy.
    /// </summary>
    [Parameter]
    public int? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the background color of the component.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets the css classes to use.
    /// </summary>
    private string? Css => DefaultClassBuilder
        .AddClass("fuicx-slideshow-caption")
        .AddClass("fuicx-slideshow-caption-fixed", Fixed)
        .Build();

    /// <summary>
    /// Gets the computed CSS style string for the component, including any maximum width constraints.
    /// </summary>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("max-width", $"{MaxWidth}px", MaxWidth.HasValue)
        .AddStyle("background-color", BackgroundColor, !string.IsNullOrEmpty(BackgroundColor))
        .Build();
}
