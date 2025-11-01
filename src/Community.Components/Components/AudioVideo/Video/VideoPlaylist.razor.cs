using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an video playlist component that displays a list of video tracks and allows users to select and play them.
/// </summary>
public partial class VideoPlaylist
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoPlaylist"/> class.
    /// </summary>
    /// <remarks>The playlist is assigned a unique identifier upon creation.</remarks>
    public VideoPlaylist()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the playlist of audio tracks.
    /// </summary>
    [Parameter]
    public List<VideoTrackItem> Playlist { get; set; } = [];

    /// <summary>
    /// Gets or sets the currently selected audio track.
    /// </summary>
    [Parameter]
    public VideoTrackItem? CurrentTrack { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when a track is selected.
    /// </summary>
    [Parameter]
    public EventCallback<VideoTrackItem> OnTrackSelected { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the playlist is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets the title to display when the video title is unknown.
    /// </summary>
    [Parameter]
    public string UnknownVideoTitle { get; set; } = "Unknown Video Title";
}
