using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the navigation view for the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public partial class FileManagerNavigationView
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
