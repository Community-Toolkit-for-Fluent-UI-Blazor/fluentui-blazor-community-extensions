using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Gets or sets the audio and video labels together with the available video playback speed options for the current
/// context.
/// </summary>
public partial class PlaybackSpeedPanel
{
    /// <summary>
    /// Gets or sets the audio and video labels along with video playback speed options for the current context.
    /// </summary>
    [Parameter, EditorRequired]
    public (AudioVideoLabels, VideoPlaybackSpeedOptions) Content { get; set; } = default!;
}
