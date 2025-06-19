using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FileManagerNavigationView
    : FluentComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
