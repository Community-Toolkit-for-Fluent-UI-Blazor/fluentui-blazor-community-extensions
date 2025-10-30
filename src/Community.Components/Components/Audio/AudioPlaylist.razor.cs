using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an audio playlist component that displays a list of audio tracks and allows users to select and play them.
/// </summary>
public partial class AudioPlaylist
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioPlaylist"/> class.
    /// </summary>
    /// <remarks>The playlist is assigned a unique identifier upon creation.</remarks>
    public AudioPlaylist()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the playlist of audio tracks.
    /// </summary>
    [Parameter]
    public List<AudioTrackItem> Playlist { get; set; } = [];

    /// <summary>
    /// Gets or sets the currently selected audio track.
    /// </summary>
    [Parameter]
    public AudioTrackItem? CurrentTrack { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when a track is selected.
    /// </summary>
    [Parameter]
    public EventCallback<AudioTrackItem> OnTrackSelected { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the playlist is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets the height of the playlist component.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 280;

    /// <summary>
    /// Gets the internal style for the component, including the height.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--audio-playlist-height", $"{Height}px")
        .Build();
}
