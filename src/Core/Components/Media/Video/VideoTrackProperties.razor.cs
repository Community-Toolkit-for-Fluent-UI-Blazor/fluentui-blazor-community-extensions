using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component for displaying properties of a video track within a dialog interface.
/// </summary>
/// <remarks>Use this class to configure video track metadata and associated labels for classification or
/// annotation scenarios. This component is typically used within dialog workflows that require user interaction with
/// video track properties.</remarks>
public partial class VideoTrackProperties
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the VideoTrackProperties class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine the behavior of the video track properties. Cannot be
    /// null.</param>
    public VideoTrackProperties(LibraryConfiguration configuration)
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
    public VideoMetadata Content { get; set; } = default!;
}
