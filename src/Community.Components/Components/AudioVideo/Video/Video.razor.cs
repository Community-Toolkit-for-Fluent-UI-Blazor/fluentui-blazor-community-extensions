using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a video player component.
/// </summary>
public partial class Video
{
    /// <summary>
    /// Initializes a new instance of the Video class with a unique identifier.
    /// </summary>
    public Video()
    {
        Id = $"video-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets a value indicating whether the component is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the component should use a compact layout.
    /// </summary>
    [Parameter]
    public bool IsCompact { get; set; }
}
