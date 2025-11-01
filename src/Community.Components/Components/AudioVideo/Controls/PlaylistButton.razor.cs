using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the playlist button.
/// </summary>
public partial class PlaylistButton
    : FluentComponentBase
{
    /// <summary>
    /// Indicates whether the playlist should be displayed.
    /// </summary>
    private bool _showPlaylist;

    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon Icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.AppsList();

    /// <summary>
    /// Gets or sets the event callback that is invoked when the stop button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlaylist { get; set; }

    /// <summary>
    /// Gets or sets the label for the previous button.
    /// </summary>
    [Parameter]
    public string? Label { get; set; } = "Playlist";

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistButton"/> class.
    /// </summary>
    public PlaylistButton()
    {
        Id = $"playlist-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Occurs when the playlist button is clicked.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnPlaylist" /> callback.</returns>
    private async Task OnTogglePlaylistAsync()
    {
        _showPlaylist = !_showPlaylist;

        if (OnPlaylist.HasDelegate)
        {
            await OnPlaylist.InvokeAsync(_showPlaylist);
        }
    }
}
