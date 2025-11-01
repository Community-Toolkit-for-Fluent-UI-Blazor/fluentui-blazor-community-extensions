using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays information about an audio track.
/// </summary>
/// <remarks>This class is used to manage and display details about a specific audio track.  The <see
/// cref="Track"/> property can be set to specify the audio track  for which information should be
/// displayed.</remarks>
public partial class AudioTrackInfo
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioTrackInfo"/> class.
    /// </summary>
    public AudioTrackInfo()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the current audio track to display information for.
    /// </summary>
    [Parameter]
    public AudioTrackItem? Track { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the track info component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<AudioTrackItem> OnClick { get; set; }

    /// <summary>
    /// Handles the click event by invoking the associated callback with the current track.
    /// </summary>
    /// <remarks>This method checks if a track is available and if a callback delegate is assigned before
    /// invoking the callback asynchronously.</remarks>
    /// <returns></returns>
    private async Task OnHandleClickAsync()
    {
        if (Track != null && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(Track);
        }
    }
}
