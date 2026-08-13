using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// The FluentCxSlideShow component is a carousel which display content with a slide effect.
/// </summary>
public partial class FluentCxSlideShow : FluentComponentBase
{
    /// <summary />
    public FluentCxSlideShow(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }
}
