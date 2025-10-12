using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of controls for managing video playback and related user interface elements.
/// </summary>
public partial class VideoControls
{
    /// <summary>
    /// Represents the play or pause button control used to toggle playback state.
    /// </summary>
    private PlayOrPauseButton? _playOrPauseButton;

    /// <summary>
    /// Represents the video settings button used to access video configuration options.
    /// </summary>
    private VideoSettingsButton? _videoSettingsButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoControls"/> class.
    /// </summary>
    public VideoControls()
    {
        Id = $"video-controls-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the set of labels used for audio and video controls.
    /// </summary>
    /// <remarks>Use this property to customize the text displayed for various audio and video UI elements. If
    /// not set, default labels are used.</remarks>
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = AudioVideoLabels.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the download button is visible.
    /// </summary>
    [Parameter]
    public bool IsDownloadVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the settings button is visible.
    /// </summary>
    [Parameter]
    public bool IsSettingsVisible { get; set; } = true; 

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
    public EventCallback<AudioVideoRepeatMode> OnRepeatModeChanged { get; set; }

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
    /// Gets or sets the callback that is invoked when the component enters theater mode.
    /// </summary>
    [Parameter]
    public EventCallback OnTheater { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the component enters or exits fullscreen mode.
    /// </summary>
    [Parameter]
    public EventCallback OnFullscreen { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when Picture-in-Picture (PiP) mode is activated.
    /// </summary>
    /// <remarks>Use this callback to handle actions or update UI elements when the component enters PiP mode.
    /// The callback is triggered in response to user interaction or programmatic requests to enter PiP.</remarks>
    [Parameter]
    public EventCallback OnPiP { get; set; }

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
    /// Gets or sets a value indicating whether Picture-in-Picture (PiP) mode is disabled for the component.
    /// </summary>
    [Parameter]
    public bool IsPiPDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the playlist functionality is disabled.
    /// </summary>
    [Parameter]
    public bool IsPlaylistDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether fullscreen mode is disabled for the component.
    /// </summary>
    [Parameter]
    public bool IsFullscreenDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the settings are disabled in the user interface.
    /// </summary>
    [Parameter]
    public bool IsSettingsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the content to render within the video settings area.
    /// </summary>
    [Parameter]
    public RenderFragment? VideoSettingsContent { get; set; }

    /// <summary>
    /// Sets the play/pause state of the associated control.
    /// </summary>
    /// <param name="isPlaying">A value indicating whether the control should reflect the "playing" state.  <see langword="true"/> if the
    /// control should indicate playback is active; otherwise, <see langword="false"/>.</param>
    internal void SetPlayPauseState(bool isPlaying)
    {
        _playOrPauseButton?.SetPlayPauseState(isPlaying);
    }

    /// <summary>
    /// Closes the settings popover associated with the video settings button, if it is currently open.
    /// </summary>
    internal void CloseSettingsPopover()
    {
        _videoSettingsButton?.ClosePopover();
    }
}
