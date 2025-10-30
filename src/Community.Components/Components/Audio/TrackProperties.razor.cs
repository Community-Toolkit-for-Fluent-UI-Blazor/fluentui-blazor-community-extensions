using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class TrackProperties : IDialogContentComponent<AudioMetadata>
{
    /// <summary>
    /// Gets or sets the set of audio labels to be used for classification or annotation.
    /// </summary>
    /// <remarks>The default value is <see cref="AudioLabels.Default"/>. Changing this property allows
    /// customization of the labels used in audio processing scenarios.</remarks>
    [Parameter]
    public AudioLabels Labels { get; set; } = AudioLabels.Default;

    /// <summary>
    /// Gets or sets the audio track item associated with this instance.
    /// </summary>
    [Parameter]
    public AudioMetadata Content { get; set; } = default!;
}
