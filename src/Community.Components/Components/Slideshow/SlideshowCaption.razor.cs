using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a caption for a <see cref="SlideshowItem{TItem}"/>.
/// </summary>
public partial class SlideshowCaption
     : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the css classes to use.
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow-caption")
        .Build();
}
