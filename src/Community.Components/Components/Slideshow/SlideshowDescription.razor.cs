using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a description for a <see cref="SlideshowItem{TItem}"/>.
/// </summary>
public partial class SlideshowDescription
     : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the description is fixed to the bottom of the slideshow item.
    /// </summary>
    [Parameter]
    public bool Fixed { get; set; }

    /// <summary>
    /// Gets or sets the maximum width, in pixels, that the component can occupy.
    /// </summary>
    [Parameter]
    public int? MaxWidth { get; set; }

    /// <summary>
    /// Gets the css classes to use.
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow-description")
        .AddClass("fixed", Fixed)
        .Build();

    /// <summary>
    /// Gets the computed CSS style string for the component, including any maximum width constraints.
    /// </summary>
    /// <remarks>This property is intended for internal use to generate the final style attribute value based
    /// on the component's configuration. It should not be accessed directly from outside the class.</remarks>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("max-width", $"{MaxWidth}px", MaxWidth.HasValue)
        .Build();
}
