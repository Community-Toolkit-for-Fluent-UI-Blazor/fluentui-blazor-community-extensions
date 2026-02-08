using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an overlay component that can be positioned over a picture.
/// </summary>
public partial class PictureOverlay
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the PictureOverlay class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine how the PictureOverlay instance behaves and interacts
    /// with other components. Cannot be null.</param>
    public PictureOverlay(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the CSS class for the overlay component.
    /// </summary>
    [Parameter]
    public PictureOverlayPosition Position { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed within the overlay.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the internal CSS classes for the overlay component.
    /// </summary>
    internal string? InternalCss => new CssBuilder(Class)
        .AddClass("picture-overlay")
        .AddClass("top-left", Position == PictureOverlayPosition.TopLeft)
        .AddClass("top-center", Position == PictureOverlayPosition.TopCenter)
        .AddClass("top-right", Position == PictureOverlayPosition.TopRight)
        .AddClass("middle-left", Position == PictureOverlayPosition.MiddleLeft)
        .AddClass("middle-center", Position == PictureOverlayPosition.MiddleCenter)
        .AddClass("middle-right", Position == PictureOverlayPosition.MiddleRight)
        .AddClass("bottom-left", Position == PictureOverlayPosition.BottomLeft)
        .AddClass("bottom-center", Position == PictureOverlayPosition.BottomCenter)
        .AddClass("bottom-right", Position == PictureOverlayPosition.BottomRight)
        .Build();
}
