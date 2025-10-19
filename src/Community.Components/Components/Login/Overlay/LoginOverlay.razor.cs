using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an overlay component that displays a login interface within a Fluent UI application.
/// </summary>
/// <remarks>Use this component to present a modal login form or authentication prompt as part of a Fluent
/// UI-based user interface. The overlay typically appears above other content and may block interaction until the user
/// completes authentication.</remarks>
public partial class LoginOverlay
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the login overlay.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
