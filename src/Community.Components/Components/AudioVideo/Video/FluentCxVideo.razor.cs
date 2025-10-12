using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a video player component that can play video tracks with various controls and views.
/// </summary>
public partial class FluentCxVideo : FluentComponentBase
{
    /// <summary>
    /// Represents the list of video tracks associated with the video player.
    /// </summary>
    private readonly List<VideoTrackItem> _originalPlaylist = [];

    /// <summary>
    /// Represents the shuffled version of the playlist.
    /// </summary>
    private readonly List<VideoTrackItem> _shuffledPlaylist = [];

    /// <summary>
    /// Represents the icon used to indicate playback speed in the user interface.
    /// </summary>
    private static readonly Icon s_playbackSpeedIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.PlayCircleHint();

    /// <summary>
    /// Represents the icon used to indicate subtitles in the user interface.
    /// </summary>
    private static readonly Icon s_subtitlesIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Subtitles();

    /// <summary>
    /// Represents the icon used for video options in the user interface.
    /// </summary>
    private static readonly Icon s_videoOptionsIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Options();

    /// <summary>
    /// Represents whether to display the playlist.
    /// </summary>
    private bool _showPlaylist;

    /// <summary>
    /// Represents the .NET object reference for JavaScript interop.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxVideo> _dotNetRef;

    /// <summary>
    /// Represents whether the audio player is in shuffle mode.
    /// </summary>
    private bool _isShuffling;

    /// <summary>
    /// Represents the index of the currently playing video track in the playlist.
    /// </summary>
    private int _currentTrackIndex = -1;

    /// <summary>
    /// Represents the video element reference used for JavaScript interop.
    /// </summary>
    private Video? _videoReference;

    /// <summary>
    /// Represents the video controls component.
    /// </summary>
    private VideoControls? _videoControls;

    /// <summary>
    /// The JavaScript module reference for interop calls.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the volume level, where 1.0 is the default maximum value.
    /// </summary>
    private double _volume = 1.0;

    /// <summary>
    /// Represents the current time value.
    /// </summary>
    private double _currentTime;

    /// <summary>
    /// Represents the duration of the audio file.
    /// </summary>
    private double _duration;

    /// <summary>
    /// Represents whether the view has changed.
    /// </summary>
    private bool _hasViewChanged;

    /// <summary>
    /// Value indicating if the video player is in theater mode.
    /// </summary>
    private bool _theaterMode;

    /// <summary>
    /// The relative path to the JavaScript file used by the FluentCxVideo component.
    /// </summary>
    private const string JavaScriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/AudioVideo/Video/FluentCxVideo.razor.js";

    /// <summary>
    /// Represents the current repeat mode of the video player.
    /// </summary>
    private AudioVideoRepeatMode _repeatMode = AudioVideoRepeatMode.SingleOnce;

    /// <summary>
    /// Represents the last view mode of the video player.
    /// </summary>
    private VideoPlayerView _lastView;

    /// <summary>
    /// Value indicating whether the video is ready for interaction.
    /// </summary>
    private ElementReference? _compactPlayerVideoReference;

    /// <summary>
    /// Gets or sets the subtitle overlay for the video player.
    /// </summary>
    private SubtitleOverlay? _subtitleOverlay;

    /// <summary>
    /// Gets or sets the current subtitle entry being displayed.
    /// </summary>
    private SubtitleEntry? _currentSubtitleEntry;

    /// <summary>
    /// Represents the subtitle options for the video player.
    /// </summary>
    private readonly SubtitleOptions _subtitleOptions = new();

    /// <summary>
    /// Value indicating the current playback speed. The default value is 1.0, representing normal speed.
    /// </summary>
    private readonly VideoPlaybackSpeedOptions _videoPlaybackSpeedOptions = new()
    {
        Speed = 1.0
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxVideo"/> class.
    /// </summary>
    public FluentCxVideo()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the state of the video player, including subtitle management.
    /// </summary>
    [Inject]
    private VideoState VideoState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the video settings popover.
    /// </summary>
    [Parameter]
    public RenderFragment? VideoSettingsContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the video quality panel.
    /// </summary>
    [Parameter]
    public RenderFragment? VideoQualityContent { get; set; }

    /// <summary>
    /// Gets a value indicating whether the stop button should be disabled.
    /// </summary>
    private bool IsStopDisabled => CurrentTrack is null;

    /// <summary>
    /// Gets a value indicating whether the play or pause button should be disabled.
    /// </summary>
    private bool IsPlayOrPauseDisabled => CurrentTrack is null;

    /// <summary>
    /// Gets a value indicating whether the audio properties should be disabled.
    /// </summary>
    private bool IsPropertiesDisabled => CurrentTrack is null || CurrentTrack.Metadata is null;

    /// <summary>
    /// Gets or sets a value indicating whether fullscreen mode is disabled for the component.
    /// </summary>
    [Parameter]
    public bool IsFullscreenDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the playlist is disabled.
    /// </summary>
    [Parameter]
    public bool IsPlaylistDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Picture-in-Picture (PiP) mode is disabled for the component.
    /// </summary>
    private bool IsPiPDisabled => CurrentTrack is null || CurrentTrack.Metadata is null;

    /// <summary>
    /// Gets or sets the video player view.
    /// </summary>
    [Parameter]
    public VideoPlayerView View { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the view changes.
    /// </summary>
    [Parameter]
    public EventCallback<VideoPlayerView> ViewChanged { get; set; }

    /// <summary>
    /// Gets a value indicating whether downloading the current track is disabled.
    /// </summary>
    /// <remarks>Downloading is disabled if there is no current track selected or if the current track does
    /// not have a valid source. This property can be used to determine whether download-related UI elements should be
    /// enabled.</remarks>
    private bool IsDownloadDisabled => CurrentTrack is null || string.IsNullOrWhiteSpace(CurrentTrack.Source);

    /// <summary>
    /// Gets a value indicating whether the settings are currently disabled based on the state of the current track.
    /// </summary>
    private bool IsSettingsDisabled => CurrentTrack is null ||
                                       string.IsNullOrWhiteSpace(CurrentTrack.Source) ||
                                       CurrentTrack.Sources.Count == 0;

    /// <summary>
    /// Gets or sets a value indicating whether the download button is visible.
    /// </summary>
    [Parameter]
    public bool IsDownloadVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the settings panel is visible.
    /// </summary>
    [Parameter]
    public bool IsSettingsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the labels for the audio control buttons.
    /// </summary>
    [Parameter]
    public AudioVideoLabels Labels { get; set; } = AudioVideoLabels.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the subtitle option is visible in the UI.
    /// </summary>
    [Parameter]
    public bool IsSubtitleOptionVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the playback speed control is visible in the UI.
    /// </summary>
    [Parameter]
    public bool IsPlaybackSpeedVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the video quality selection option is visible to the user.
    /// </summary>
    [Parameter]
    public bool IsVideoQualityOptionVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the thumbnails are visible when seeking the seek bar.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="true"/>.
    /// </remarks>
    [Parameter]
    public bool ShowThumbnails { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the seek bar should be disabled based on the current track state.
    /// </summary>
    /// <remarks>The seek bar is considered disabled if there is no current track or if the current track does
    /// not have a valid source. This property can be used to control the enabled state of UI elements related to track
    /// seeking.</remarks>
    private bool IsSeekBarDisabled => CurrentTrack is null || string.IsNullOrWhiteSpace(CurrentTrack.Source);

    /// <summary>
    /// Gets a value indicating whether the previous button should be disabled.
    /// </summary>
    private bool IsPreviousDisabled
    {
        get
        {
            if (_originalPlaylist.Count == 0)
            {
                return true;
            }

            if (_originalPlaylist.Count == 1)
            {
                return _repeatMode switch
                {
                    AudioVideoRepeatMode.PlaylistOnce => true,
                    AudioVideoRepeatMode.PlaylistLoop => false,
                    AudioVideoRepeatMode.SingleOnce => true,
                    AudioVideoRepeatMode.SingleLoop => false,
                    _ => true
                };
            }

            if (_repeatMode == AudioVideoRepeatMode.PlaylistOnce)
            {
                return _currentTrackIndex == 0;
            }

            return _repeatMode == AudioVideoRepeatMode.SingleLoop || _repeatMode == AudioVideoRepeatMode.SingleOnce;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the next button should be disabled.
    /// </summary>
    private bool IsNextDisabled
    {
        get
        {
            if (_originalPlaylist.Count == 0)
            {
                return true;
            }

            if (_originalPlaylist.Count == 1)
            {
                return _repeatMode switch
                {
                    AudioVideoRepeatMode.PlaylistOnce => true,
                    AudioVideoRepeatMode.PlaylistLoop => false,
                    AudioVideoRepeatMode.SingleOnce => true,
                    AudioVideoRepeatMode.SingleLoop => false,
                    _ => true
                };
            }

            if (_repeatMode == AudioVideoRepeatMode.PlaylistOnce)
            {
                return _currentTrackIndex == _originalPlaylist.Count - 1;
            }

            return _repeatMode == AudioVideoRepeatMode.SingleLoop || _repeatMode == AudioVideoRepeatMode.SingleOnce;
        }
    }

    /// <summary>
    /// Gets  the current audio track to be played.
    /// </summary>
    private VideoTrackItem? CurrentTrack => _originalPlaylist.Count > 0 && _currentTrackIndex >= 0 && _currentTrackIndex < _originalPlaylist.Count ? _originalPlaylist[_currentTrackIndex] : null;

    /// <summary>
    /// Gets the CSS class for the internal video container based on the current view mode.
    /// </summary>
    private string? InternalVideoCss => new CssBuilder()
        .AddClass("video-container")
        .AddClass("theater-mode", _theaterMode)
        .AddClass("floating-player", View == VideoPlayerView.Floating)
        .AddClass("compact-player", View == VideoPlayerView.Compact)
        .AddClass("minimal-player", View == VideoPlayerView.Minimal)
        .Build();

    /// <summary>
    /// Gets the CSS class for the main component based on the current view mode.
    /// </summary>
    private string? InternalClass => new CssBuilder()
        .AddClass("fluentcx-video")
        .AddClass("fullscreen", View == VideoPlayerView.Fullscreen)
        .Build();

    /// <summary>
    /// Gets or sets the dialog service used to display dialogs.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of subtitle entries to display with the media content.
    /// </summary>
    [Parameter]
    public List<SubtitleEntry> Subtitles { get; set; } = [];

    /// <summary>
    /// Sets the video source and volume for the current track asynchronously.
    /// </summary>
    /// <returns></returns>
    private async Task SetVideoSourceAsync(string? source = null)
    {
        var src = source ?? CurrentTrack?.Source;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.setSource", Id, src);
            await _module.InvokeVoidAsync("fluentCxVideo.setVolume", Id, _volume);
        }
    }

    /// <summary>
    /// Occurs when the download button is clicked.
    /// </summary>
    /// <returns>Returns a task which downloads the track.</returns>
    private async Task OnDownloadAsync()
    {
        if (CurrentTrack is not null &&
            _module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.download", CurrentTrack.Source, CurrentTrack?.Metadata?.Descriptive?.Title ?? CurrentTrack?.Source ?? Path.GetRandomFileName());
        }
    }

    /// <summary>
    /// Toggles the visibility of the playlist.
    /// </summary>
    /// <param name="value">A boolean value indicating whether the playlist should be shown.  <see langword="true"/> to show the playlist;
    /// otherwise, <see langword="false"/>.</param>
    private void OnPlaylistToogled(bool value)
    {
        _showPlaylist = value;
        StateHasChanged();
    }

    /// <summary>
    /// Plays the video associated with the current instance asynchronously.
    /// </summary>
    /// <returns></returns>
    private async Task PlayAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.play", Id);
        }
    }

    /// <summary>
    /// Pauses the video playback associated with the current instance.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to pause the audio playback.  Ensure that the
    /// associated module is initialized before calling this method.</remarks>
    /// <returns></returns>
    private async Task OnPauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.pause", Id);
        }
    }

    /// <summary>
    /// Toggles the fullscreen mode for the associated video element asynchronously.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to enter or exit fullscreen mode for the video
    /// element identified by the current instance. The operation is performed only if the JavaScript module has been
    /// initialized.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnToggleFullscreenAsync()
    {
        if (View != VideoPlayerView.Fullscreen)
        {
            _lastView = View;
            View = VideoPlayerView.Fullscreen;

            if (ViewChanged.HasDelegate)
            {
                await ViewChanged.InvokeAsync(View);
            }
        }
        else
        {
            View = _lastView;

            if (ViewChanged.HasDelegate)
            {
                await ViewChanged.InvokeAsync(View);
            }
        }

        await OnFullScreenAsync();
    }

    /// <summary>
    /// Enters full-screen mode for the associated video element asynchronously.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to request full-screen mode. If the module is not
    /// initialized, the method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnFullScreenAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.fullscreen", Id);
        }
    }

    /// <summary>
    /// Toggles the theater mode for the associated video element.
    /// </summary>
    private void OnToggleTheater()
    {
        _theaterMode = !_theaterMode;
        StateHasChanged();
    }

    /// <summary>
    /// Toggles Picture-in-Picture (PiP) mode for the associated video element asynchronously.
    /// </summary>
    /// <remarks>This method enables or disables PiP mode by invoking a JavaScript interop call. If the
    /// underlying module is not available, the method completes without performing any action.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTogglePiPAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.pictureInPicture", Id);
        }
    }

    /// <summary>
    /// Toggles the playback state of the video element between play and pause.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTogglePlayPauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.togglePlayPause", Id);
        }
    }

    /// <summary>
    /// Skips to the previous track in the playlist and starts playback.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnPreviousTrackAsync()
    {
        if (_originalPlaylist.Count == 0)
        {
            return;
        }

        switch (_repeatMode)
        {
            case AudioVideoRepeatMode.SingleOnce:
            case AudioVideoRepeatMode.SingleLoop:

                break;

            case AudioVideoRepeatMode.PlaylistOnce:
                {
                    if (_currentTrackIndex > 0)
                    {
                        _currentTrackIndex--;
                    }
                    else
                    {
                        return;
                    }
                }

                break;

            case AudioVideoRepeatMode.PlaylistLoop:
                _currentTrackIndex = (_currentTrackIndex - 1 + _originalPlaylist.Count) % _originalPlaylist.Count;
                break;
        }

        await SetVideoSourceAsync();
        await PlayAsync();
    }

    /// <summary>
    /// Advances to the next track in the playlist and begins playback.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of advancing to the next track  and starting playback.</returns>
    private async Task OnNextTrackAsync()
    {
        if (_originalPlaylist.Count == 0)
        {
            return;
        }

        switch (_repeatMode)
        {
            case AudioVideoRepeatMode.PlaylistOnce:
                if (_currentTrackIndex < _originalPlaylist.Count - 1)
                {
                    _currentTrackIndex++;
                }
                else
                {
                    return;
                }

                break;

            case AudioVideoRepeatMode.PlaylistLoop:
                _currentTrackIndex = (_currentTrackIndex + 1) % _originalPlaylist.Count;
                break;
        }

        await SetVideoSourceAsync();
        await PlayAsync();
    }

    /// <summary>
    /// Asynchronously adds a video track to the playlist and updates the current track if necessary.
    /// </summary>
    /// <remarks>If the current track is not the first in the playlist, this method resets the current track
    /// to the newly added item and updates the video source.</remarks>
    /// <param name="videoTrackItem">The video track item to add to the playlist. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    internal async Task AddTrackAsync(VideoTrackItem videoTrackItem)
    {
        _originalPlaylist.Add(videoTrackItem);

        if (_currentTrackIndex != 0)
        {
            _currentTrackIndex = 0;
            await SetVideoSourceAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Removes the specified video track item from the playlist asynchronously and updates the current track state as
    /// needed.
    /// </summary>
    /// <remarks>If the removed track was not the first in the playlist and tracks remain, the current track
    /// is reset to the first item. If the playlist becomes empty, the current track index is set to -1.</remarks>
    /// <param name="videoTrackItem">The video track item to remove from the playlist. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous remove operation.</returns>
    internal async Task RemoveTrackAsync(VideoTrackItem videoTrackItem)
    {
        _originalPlaylist.Remove(videoTrackItem);

        if (_currentTrackIndex != 0 && _originalPlaylist.Count > 0)
        {
            _currentTrackIndex = 0;
            await SetVideoSourceAsync();
        }
        else if (_originalPlaylist.Count == 0)
        {
            _currentTrackIndex = -1;
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Displays the track properties dialog asynchronously and waits for the user to close it.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of showing the properties dialog and awaiting its closure.</returns>
    private async Task OnPropertiesAsync()
    {
        if (CurrentTrack?.Metadata is null)
        {
            return;
        }

        var dialog = await DialogService.ShowDialogAsync<VideoTrackProperties>(CurrentTrack.Metadata, new DialogParameters()
        {
            Title = Labels.PropertiesLabel,
            PrimaryAction = Labels.CloseLabel,
            SecondaryAction = null,
            Width = "60%"
        });

        await dialog.Result;
    }

    /// <summary>
    /// Handles the seek operation by updating the current playback time and invoking the corresponding JavaScript
    /// function.
    /// </summary>
    /// <returns></returns>
    private async Task OnSeekAsync(double e)
    {
        if (_module is not null)
        {
            _currentTime = e;
            await _module.InvokeVoidAsync("fluentCxVideo.seek", Id, _currentTime);
        }
    }

    /// <summary>
    /// Asynchronously updates the volume level and notifies the associated audio module.
    /// </summary>
    /// <param name="value">The new volume level to set. Must be a value between 0.0 and 1.0, where 0.0 represents mute and 1.0 represents
    /// the maximum volume.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnChangeVolumeAsync(double value)
    {
        _volume = value;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.setVolume", Id, value);
        }
    }

    /// <summary>
    /// Seeks to the specified position in the audio track and resumes playback.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to perform the seek operation. Ensure that the
    /// audio module is initialized before calling this method. If the module is not initialized, the method will have
    /// no effect.</remarks>
    /// <param name="value">The position, in seconds, to seek to within the audio track. Must be a non-negative value.</param>
    /// <returns></returns>
    private async Task OnSeekEndAsync(double value)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.seek", Id, value);
        }
    }

    /// <summary>
    /// Plays the specified audio track by setting it as the current track and starting playback.
    /// </summary>
    /// <remarks>If the specified track exists in the playlist, it is set as the current track, and playback
    /// begins. If the track is not found in the playlist, no action is taken.</remarks>
    /// <param name="track">The audio track to play. Must be an item in the current playlist.</param>
    /// <returns>Returns a task which plays the track.</returns>
    private async Task OnPlayTrackAsync(VideoTrackItem track)
    {
        var idx = _originalPlaylist.IndexOf(track);

        if (idx >= 0)
        {
            _videoControls?.SetPlayPauseState(true);
            _currentTrackIndex = idx;
            await SetVideoSourceAsync();
            await PlayAsync();
        }
    }

    /// <summary>
    /// Stops the audio playback and resets the internal playback state.
    /// </summary>
    /// <returns>Returns a task which stops the audio playback.</returns>
    private async Task OnStopAsync()
    {
        _currentTime = 0;

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.stop", Id);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the event when a subtitle is clicked asynchronously.
    /// </summary>
    /// <returns>A completed task that represents the asynchronous operation.</returns>
    private async Task OnSubtitleClickAsync()
    {
        _videoControls?.CloseSettingsPopover();

        if (_subtitleOverlay is not null)
        {
            _subtitleOptions.Background = _subtitleOverlay.Background;
            _subtitleOptions.BackgroundColor = _subtitleOverlay.BackgroundColor;
        }

        var dialog = await DialogService.ShowPanelAsync<SubtitlePanel>((Labels, _subtitleOptions), new DialogParameters()
        {
            Title = Labels.SubtitlesLabel,
            PrimaryAction = Labels.ApplyLabel,
            SecondaryAction = Labels.CancelLabel,
            Width = "25%"
        });

        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            _subtitleOverlay?.SetOptions(_subtitleOptions);
        }
    }

    /// <summary>
    /// Handles the user interaction for selecting the playback speed option asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnPlaybackSpeedClickAsync()
    {
        _videoControls?.CloseSettingsPopover();

        var dialog = await DialogService.ShowPanelAsync<PlaybackSpeedPanel>((Labels, _videoPlaybackSpeedOptions), new DialogParameters()
        {
            Title = Labels.PlaybackSpeedLabel,
            PrimaryAction = Labels.ApplyLabel,
            SecondaryAction = Labels.CancelLabel,
            Width = "25%"
        });

        var result = await dialog.Result;

        if (!result.Cancelled && _module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxVideo.setPlaybackRate", Id, _videoPlaybackSpeedOptions.Speed);
        }
    }

    /// <summary>
    /// Handles the event when the video quality option is clicked.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnVideoQualityClickAsync()
    {
        _videoControls?.CloseSettingsPopover();

        var dialog = await DialogService.ShowPanelAsync<VideoQualityPanel>((Labels, CurrentTrack), new DialogParameters()
        {
            Title = Labels.VideoQualityLabel,
            PrimaryAction = Labels.ApplyLabel,
            SecondaryAction = Labels.CancelLabel,
            Width = "25%"
        });

        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var source = CurrentTrack?.GetSource(VideoState.SelectedQuality);
            await SetVideoSourceAsync(source);
        }
    }

    /// <summary>
    /// Invoked when the shuffle mode is toggled.
    /// </summary>
    /// <param name="shuffling">Value indicating if the list must be shuffled or not.</param>
    private void OnToggleShuffle(bool shuffling)
    {
        _isShuffling = shuffling;

        if (_isShuffling)
        {
            _shuffledPlaylist.Clear();
            var list = new List<VideoTrackItem>(_originalPlaylist);

            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(i + 1);
                _shuffledPlaylist.Add(list[j]);
                list.RemoveAt(j);
            }
        }
    }

    /// <summary>
    /// Handles the event when the video element is ready for interaction.
    /// </summary>
    /// <param name="elementReference">A reference to the video element that has become ready.</param>
    private void OnVideoReady(ElementReference elementReference)
    {
        _compactPlayerVideoReference = elementReference;
    }

    /// <summary>
    /// Invoked when a media track ends. This method is called via JavaScript interop.
    /// </summary>
    /// <returns></returns>
    [JSInvokable("onTrackEnded")]
    public async Task OnTrackEndedAsync()
    {
        switch (_repeatMode)
        {
            case AudioVideoRepeatMode.SingleOnce:
                {
                    _videoControls?.SetPlayPauseState(false);
                    await OnStopAsync();
                }

                break;

            case AudioVideoRepeatMode.SingleLoop:
                {
                    await SetVideoSourceAsync();
                    await PlayAsync();
                }

                break;

            case AudioVideoRepeatMode.PlaylistOnce:
                if (_currentTrackIndex < _originalPlaylist.Count - 1)
                {
                    await OnNextTrackAsync();
                }
                else
                {
                    _videoControls?.SetPlayPauseState(false);
                    await OnStopAsync();
                }

                break;

            case AudioVideoRepeatMode.PlaylistLoop:
                {
                    await OnNextTrackAsync();
                }

                break;
        }
    }

    /// <summary>
    /// Sets the duration of the audio playback.
    /// </summary>
    /// <param name="duration"></param>
    [JSInvokable("setDuration")]
    public void SetDuration(double duration)
    {
        _duration = duration;
        StateHasChanged();
    }

    /// <summary>
    /// Sets the current of the audio playback.
    /// </summary>
    /// <param name="value"></param>
    [JSInvokable("setSeek")]
    public void SetSeek(double value)
    {
        _currentTime = value;
        StateHasChanged();
    }

    /// <summary>
    /// Updates the current elapsed time with the specified value.
    /// </summary>
    /// <remarks>This method updates the internal state to reflect the provided elapsed time and triggers a
    /// re-render of the component. It can be invoked from JavaScript using the identifier
    /// "updateElapsedTime".</remarks>
    /// <param name="value">The new elapsed time, in seconds.</param>
    [JSInvokable("updateElapsedTime")]
    public void UpdateElapsedTime(double value)
    {
        _currentTime = value;
        _currentSubtitleEntry = Subtitles.FirstOrDefault(s => s.Start <= value && s.End >= value);
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module ??= await Runtime.InvokeAsync<IJSObjectReference>("import", JavaScriptFile);
        }

        if (firstRender || _hasViewChanged)
        {
            if (_compactPlayerVideoReference is null &&
                _videoReference is null)
            {
                throw new InvalidOperationException("Video reference is not set.");
            }

            _hasViewChanged = false;
            await _module!.InvokeVoidAsync("fluentCxVideo.dispose", Id);
            await _module!.InvokeVoidAsync("fluentCxVideo.initialize", Id, _compactPlayerVideoReference ?? _videoReference!.Element, _dotNetRef);

            if (_originalPlaylist.Count > 0)
            {
                _currentTrackIndex = 0;
                await SetVideoSourceAsync();
            }

            _subtitleOverlay?.SetOptions(_subtitleOptions);
        }
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _compactPlayerVideoReference = null;
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (View == VideoPlayerView.Fullscreen)
        {
            await OnFullScreenAsync();
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasViewChanged = parameters.HasValueChanged(nameof(View), View);

        return base.SetParametersAsync(parameters);
    }
}
