using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Renders a scrolling title for audio tracks.
/// </summary>
public sealed partial class AudioTitleScroller
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the AudioTitleScroller class using the specified library configuration.
    /// </summary>
    /// <remarks>This constructor assigns a unique identifier to each instance of AudioTitleScroller by
    /// invoking the Identifier.NewId() method.</remarks>
    /// <param name="configuration">The configuration settings that determine the behavior of the audio title scroller. Cannot be null.</param>
    public AudioTitleScroller(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = $"audio-title-scroller-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the title to display.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }
}
