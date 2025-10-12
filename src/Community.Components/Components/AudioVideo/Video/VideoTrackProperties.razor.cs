using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component for displaying properties of a video track within a dialog interface.
/// </summary>
/// <remarks>Use this class to configure video track metadata and associated labels for classification or
/// annotation scenarios. This component is typically used within dialog workflows that require user interaction with
/// video track properties.</remarks>
public partial class VideoTrackProperties : IDialogContentComponent<VideoMetadata>
{
    /// <summary>
    /// Gets or sets the set of audio labels to be used for classification or annotation.
    /// </summary>
    /// <remarks>The default value is <see cref="AudioVideoLabels.Default"/>. Changing this property allows
    /// customization of the labels used in audio processing scenarios.</remarks>
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = AudioVideoLabels.Default;

    /// <summary>
    /// Gets or sets the audio track item associated with this instance.
    /// </summary>
    [Parameter]
    public VideoMetadata Content { get; set; } = default!;
}
