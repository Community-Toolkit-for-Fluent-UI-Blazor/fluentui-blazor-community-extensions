using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a container component for displaying a login card with customizable title, icon, body, and footer
/// content.
/// </summary>
/// <remarks>Use this component to structure authentication or login-related UI with flexible content areas. The
/// title, icon, and content sections can be customized using parameters or templates to fit various authentication
/// scenarios.</remarks>
public partial class LoginCardContainer
{
    /// <summary>
    /// Gets or sets the title text to be displayed in the login card container.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed alongside the title.
    /// </summary>
    [Parameter]
    public Icon? TitleIcon { get; set; }

    /// <summary>
    /// Gets or sets a custom template for rendering the title section of the login card container.
    /// </summary>
    [Parameter]
    public RenderFragment? TitleTemplate { get; set; }

    /// <summary>
    /// Gets or sets the body content to be rendered within the login card container.
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    /// Gets or sets the content to render in the footer section of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }
}
