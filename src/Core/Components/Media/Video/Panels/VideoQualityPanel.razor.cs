using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Gets or sets the video label information and its associated video track item to be displayed in the panel.
/// </summary>
public partial class VideoQualityPanel
{
    /// <summary>
    /// Gets or sets the video track item to be displayed within the panel.
    /// </summary>
    [Parameter]
    public VideoTrackItem Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the labels used for audio and video elements within the component.
    /// </summary>
    /// <remarks>The default value is <see cref="AudioVideoLabels.Default"/>, which provides standard labels.
    /// Assign a custom <see cref="AudioVideoLabels"/> instance to customize the displayed text for audio and video
    /// controls.</remarks>
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = AudioVideoLabels.Default;

    /// <summary>
    /// Gets or sets the video state.
    /// </summary>
    [Inject]
    private VideoState VideoState { get; set; } = null!;

    private static bool GetIsSelected(int value, int current)
    {
        return value == current;
    }
}
