using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxSlideShow component is a carousel which display content with a slide effect.
/// </summary>
public partial class FluentCxSlideShow : FluentComponentBase
{

    /// <summary />
    private string? ClassValue => DefaultClassBuilder
        .AddClass("fluentcx-color-palette")
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
