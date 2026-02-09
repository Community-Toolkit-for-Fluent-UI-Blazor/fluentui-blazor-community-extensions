using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component for displaying properties of an audio track within a dialog interface.
/// </summary>
/// <remarks>Use this class to configure audio track metadata and associated labels for classification or
/// annotation scenarios. This component is typically used within dialog workflows that require user interaction with
/// audio track properties.</remarks>
public partial class AudioTrackProperties : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the AudioTrackProperties class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine how audio tracks are managed and processed.</param>
    public AudioTrackProperties(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

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
    public AudioMetadata Content { get; set; } = default!;
}
