using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents audio control buttons for play, pause, stop, and other functionalities.
/// </summary>
public partial class AudioControls : FluentComponentBase
{
    /// <summary>
    /// Represents the play or pause button control used to toggle playback state.
    /// </summary>
    private PlayOrPauseButton? _playOrPauseButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioControls"/> class.
    /// </summary>
    public AudioControls()
    {
        Id = $"audio-controls-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the labels for the audio control buttons.
    /// </summary>
    [Parameter]
    public AudioLabels Labels { get; set; } = AudioLabels.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the download button is visible.
    /// </summary>
    [Parameter]
    public bool IsDownloadVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the shuffle state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnShuffleChanged { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the playlist button is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlaylistToggled { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the previous button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPrevious { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the download button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnDownload { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the stop button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnStop { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the next button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnNext { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the repeat mode changes.
    /// </summary>
    [Parameter]
    public EventCallback<AudioRepeatMode> OnRepeatModeChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the play/pause button is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayPauseToggled { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the volume changes.
    /// </summary>
    [Parameter]
    public EventCallback<double> OnVolumeChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the properties button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnProperties { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the previous button is disabled.
    /// </summary>
    [Parameter]
    public bool IsPreviousDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the next button is disabled.
    /// </summary>
    [Parameter]
    public bool IsNextDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the stop button is disabled.
    /// </summary>
    [Parameter]
    public bool IsStopDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether downloading is disabled for this component.
    /// </summary>
    [Parameter]
    public bool IsDownloadDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the play/pause button is disabled.
    /// </summary>
    [Parameter]
    public bool IsPlayOrPauseDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the properties button is disabled.
    /// </summary>
    [Parameter]
    public bool IsPropertiesDisabled { get; set; }

    /// <summary>
    /// Sets the play/pause state of the associated control.
    /// </summary>
    /// <param name="isPlaying">A value indicating whether the control should reflect the "playing" state.  <see langword="true"/> if the
    /// control should indicate playback is active; otherwise, <see langword="false"/>.</param>
    internal void SetPlayPauseState(bool isPlaying)
    {
        _playOrPauseButton?.SetPlayPauseState(isPlaying);
    }
}
