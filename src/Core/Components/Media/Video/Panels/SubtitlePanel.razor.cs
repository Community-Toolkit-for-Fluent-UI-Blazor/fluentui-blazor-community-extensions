using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that displays subtitle options for the current context.
/// </summary>
public partial class SubtitlePanel
{
    /// <summary>
    /// Gets or sets the audio and video labels along with subtitle options for the current context.
    /// </summary>
    [Parameter, EditorRequired]
    public IEnumerable<SubtitleLanguage> Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the audio and video labels used by the component.
    /// </summary>
    /// <remarks>The default value is <see cref="AudioVideoLabels.Default"/>, which provides standard
    /// labeling. Assign a custom <see cref="AudioVideoLabels"/> instance to customize the displayed labels and enhance
    /// user experience.</remarks>
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = AudioVideoLabels.Default;

    /// <summary>
    /// Gets or sets the options for displaying subtitles in the media player.
    /// </summary>
    /// <remarks>This property allows customization of subtitle appearance and behavior, such as font size,
    /// color, and positioning. Ensure that the SubtitleOptions are configured before playback to achieve the desired
    /// subtitle display.</remarks>
    [Parameter]
    public SubtitleOptions SubtitleOptions { get; set; } = default!;

    private string? GetOptionText(SubtitleBackground value)
    {
        return value switch
        {
            SubtitleBackground.Solid => Labels.SubtitleSolidBackgroundLabel,
            SubtitleBackground.SemiTransparent => Labels.SubtitleHalfTransparentBackgroundLabel,
            SubtitleBackground.Transparent => Labels.SubtitleTransparentBackgroundLabel,
            SubtitleBackground.Opaque => Labels.SubtitleOpaqueBackgroundLabel,
            _ => null
        };
    }
}
