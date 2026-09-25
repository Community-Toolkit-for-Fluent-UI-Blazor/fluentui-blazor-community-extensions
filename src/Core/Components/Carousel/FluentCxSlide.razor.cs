using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxSlideShowItem component is a child component of the FluentCxSlideShow component which represents an item in the slideshow.
/// </summary>
public partial class FluentCxSlide
{
    /// <summary />
    private string? ClassValue => DefaultClassBuilder
        .AddClass("fluentcx-slide")
        .AddClass("active", IsActive)
        .Build();

    /// <summary />
    private string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    public FluentCxSlide(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    [CascadingParameter]
    private FluentCxCarousel? Carousel { get; set; }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool IsActive =>
        Carousel?.IsActive(this) == true;

    /// <summary />
    protected override void OnInitialized()
    {
        Carousel?.Register(this);
    }

    /// <summary />
    public void Dispose()
    {
        Carousel?.Unregister(this);
    }
}
