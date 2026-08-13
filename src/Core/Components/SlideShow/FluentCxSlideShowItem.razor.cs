using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxSlideShowItem component is a child component of the FluentCxSlideShow component which represents an item in the slideshow.
/// </summary>
public partial class FluentCxSlideShowItem
{
    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private string? ClassValue => DefaultClassBuilder
        .AddClass("fluentcx-slideshow-item")
        .Build();

    /// <summary />
    private string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    public FluentCxSlideShowItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }
}
