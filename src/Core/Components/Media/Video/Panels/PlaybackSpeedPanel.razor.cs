using System.Globalization;
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
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = default!;

    /// <summary>
    /// Gets or sets the playback speed options for the video.
    /// </summary>
    /// <remarks>This property allows the user to specify the desired playback speed for the video content.
    /// The value can be null, indicating that no specific playback speed is set.</remarks>
    [Parameter]
    public VideoPlaybackSpeedOptions Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing operations.
    /// </summary>
    /// <remarks>This property allows customization of culture-specific behaviors, such as date and number
    /// formatting. The default value is the current culture of the system.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;
}
