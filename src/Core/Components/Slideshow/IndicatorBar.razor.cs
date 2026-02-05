using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a visual component that displays an indicator bar for showing the number of items in a slideshow and highlights the current item.
/// </summary>
[CascadingTypeParameter(nameof(TItem))]
public partial class IndicatorBar<TItem> : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the IndicatorBar class using the specified library configuration.
    /// </summary>
    /// <param name="libraryConfiguration">The configuration settings that determine how the IndicatorBar operates. Cannot be null.</param>
    public IndicatorBar(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the number of items to display in the indicator bar.
    /// </summary>
    /// <remarks>Set this property to control how many indicators are rendered. Changing the value will update
    /// the indicator bar accordingly.</remarks>
    [Parameter]
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the orientation of indicator bar.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to render for each item, using a render fragment that receives the item's index as a
    /// parameter.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of each item in the indicator bar. The integer
    /// parameter represents the zero-based index of the item being rendered.</remarks>
    [Parameter]
    public RenderFragment<int>? ItemContent { get; set; }

    /// <summary>
    /// Gets or sets the index of the currently active slide.
    /// </summary>
    [Parameter]
    public int SlideIndex { get; set; }

    /// <summary>
    /// Gets or sets the position of the slideshow indicator within the user interface.
    /// </summary>
    /// <remarks>Set this property to specify where the indicator appears, such as at the top, bottom, left,
    /// or right of the slideshow. The available positions are defined by the SlideshowIndicatorPosition
    /// enumeration.</remarks>
    [Parameter]
    public SlideshowIndicatorPosition Position { get; set; }

    /// <summary>
    /// Gets the CSS class string that represents the current orientation and position of the slideshow indicators.
    /// </summary>
    /// <remarks>The returned CSS classes are dynamically composed based on the values of the orientation and
    /// position properties. This property is intended for internal use to facilitate consistent styling of slideshow
    /// indicators in different layouts.</remarks>
    private string? InternalCss => DefaultClassBuilder
        .AddClass("slideshow-indicators-vertical", Orientation == Orientation.Vertical)
        .AddClass("slideshow-indicators-horizontal", Orientation == Orientation.Horizontal)
        .AddClass("slideshow-indicators-top", Position == SlideshowIndicatorPosition.Top)
        .AddClass("slideshow-indicators-bottom", Position == SlideshowIndicatorPosition.Bottom)
        .AddClass("slideshow-indicators-left", Position == SlideshowIndicatorPosition.Left)
        .AddClass("slideshow-indicators-right", Position == SlideshowIndicatorPosition.Right)
        .Build();
}
