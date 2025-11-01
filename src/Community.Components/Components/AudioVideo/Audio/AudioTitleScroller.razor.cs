using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Renders a scrolling title for audio tracks.
/// </summary>
public sealed partial class AudioTitleScroller
    : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the title to display.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }
}
