using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class SlideshowCaption
     : FluentComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow-caption")
        .Build();
}
