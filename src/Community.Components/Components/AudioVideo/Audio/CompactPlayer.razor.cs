using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a compact audio player component that provides basic audio playback functionality.
/// </summary>
public sealed partial class CompactPlayer
    : FluentComponentBase
{
    /// <summary>
    /// Value indicating whether to show the playback controls.
    /// </summary>
    private bool _showControls;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompactPlayer"/> class.
    /// </summary>
    public CompactPlayer()
    {
        Id = $"compact-player-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the current audio track to be played.
    /// </summary>
    [Parameter]
    public AudioTrackItem? CurrentTrack { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the play/pause button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayPauseChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the previous button is disabled.
    /// </summary>
    [Parameter]
    public bool IsPreviousDisabled { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the previous button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPrevious { get; set; }

    /// <summary>
    /// Gets or sets the label for the previous button.
    /// </summary>
    [Parameter]
    public string? PreviousLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the next button is disabled.
    /// </summary>
    [Parameter]
    public bool IsNextDisabled { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the next button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnNext { get; set; }

    /// <summary>
    /// Gets or sets the label for the next button.
    /// </summary>
    [Parameter]
    public string? NextLabel { get; set; }

    /// <summary>
    /// Gets or sets the label for the play button.
    /// </summary>
    [Parameter]
    public string? PlayLabel { get; set; }

    /// <summary>
    /// Gets or sets the label for the pause button.
    /// </summary>
    [Parameter]
    public string? PauseLabel { get; set; }

    /// <summary>
    /// Gets or sets the total duration of the current track, in seconds.
    /// </summary>
    [Parameter]
    public double Duration { get; set; }

    /// <summary>
    /// Gets or sets the current playback time of the track, in seconds.
    /// </summary>
    [Parameter]
    public double CurrentTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is floating.
    /// </summary>
    [Parameter]
    public bool IsFloating { get; set; }

    /// <summary>
    /// Gets the style string that includes the opacity for the playback controls based on the current state.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--compact-player-controls-opacity", _showControls ? "1" : "0")
        .Build();

    /// <summary>
    /// Handles the pointer enter event by updating the component's state.
    /// </summary>
    /// <param name="e">The event arguments associated with the pointer enter event.</param>
    private void OnPointerEnter(PointerEventArgs e)
    {
        _showControls = true;
        StateHasChanged();
    }

    /// <summary>
    /// Handles the pointer leave event by updating the component's state.
    /// </summary>
    /// <param name="e">The event data associated with the pointer leave event.</param>
    private void OnPointerLeave(PointerEventArgs e)
    {
        _showControls = false;
        StateHasChanged();
    }
}
