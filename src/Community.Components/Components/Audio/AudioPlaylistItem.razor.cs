using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item in an audio playlist, including its associated track, selection state, and actions.
/// </summary>
/// <remarks>This component is designed to display an audio track within a playlist and manage its selection
/// state. It provides functionality to handle user interactions, such as selecting a track, and displays an appropriate
/// icon based on whether the track is currently selected.</remarks>
public partial class AudioPlaylistItem
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon used to indicate the play action for an audio track.
    /// </summary>
    private static readonly Icon PlayIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Play();

    /// <summary>
    /// Represents the icon used to indicate the poll action for an audio track.
    /// </summary>
    private static readonly Icon PollIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24.Poll();

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioPlaylistItem"/> class.
    /// </summary>
    public AudioPlaylistItem()
    {
        Id = $"audio-playlist-item-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the audio track item to be displayed in the playlist.
    /// </summary>
    [Parameter]
    public AudioTrackItem? Track { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the track is currently selected.
    /// </summary>
    [Parameter]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets the appropriate icon based on whether the track is selected.
    /// </summary>
    private Icon TrackIcon => IsSelected ? PollIcon : PlayIcon;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the track is selected.
    /// </summary>
    [Parameter]
    public EventCallback<AudioTrackItem> OnSelected { get; set; }

    /// <summary>
    /// Handles the click event asynchronously and invokes the <see cref="OnSelected"/> callback with the current <see
    /// cref="Track"/>.
    /// </summary>
    /// <remarks>This method checks if the <see cref="OnSelected"/> callback has been assigned and if the <see
    /// cref="Track"/> is not null before invoking the callback. Ensure that both <see cref="OnSelected"/> and <see
    /// cref="Track"/> are properly set before calling this method.</remarks>
    /// <returns></returns>
    private async Task OnHandleClickAsync()
    {
        if (OnSelected.HasDelegate && Track != null)
        {
            await OnSelected.InvokeAsync(Track);
        }
    }
}
