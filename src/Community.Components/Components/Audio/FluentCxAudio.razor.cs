using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a full-featured audio player component with various playback and visualization options.
/// </summary>
public sealed partial class FluentCxAudio
    : FluentComponentBase
{
    /// <summary>
    /// Represents the reference to the HTML audio element.
    /// </summary>
    private Audio? _audioReference;

    /// <summary>
    /// Represents the height of the container element.
    /// </summary>
    private string? _containerHeight;

    /// <summary>
    /// Value indicating whether the view has changed.
    /// </summary>
    private bool _hasViewChanged;

    /// <summary>
    /// Represents the playlist of audio tracks.
    /// </summary>
    private readonly List<AudioTrackItem> _originalPlaylist = [];

    /// <summary>
    /// Represents the shuffled version of the playlist.
    /// </summary>
    private readonly List<AudioTrackItem> _shuffledPlaylist = [];

    /// <summary>
    /// Represents whether to display the playlist.
    /// </summary>
    private bool _showPlaylist;

    /// <summary>
    /// Represents the .NET object reference for JavaScript interop.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxAudio> _dotNetRef;

    /// <summary>
    /// Represents whether the audio player is in shuffle mode.
    /// </summary>
    private bool _isShuffling;

    /// <summary>
    /// Represents the audio controls component.
    /// </summary>
    private AudioControls? _audioControls;

    /// <summary>
    /// Reference to the audio visualizer component.
    /// </summary>
    private AudioVisualizer? _audioVisualizer;

    /// <summary>
    /// Represents the index of the current track in the playlist.
    /// </summary>
    private int _currentTrackIndex = -1;

    /// <summary>
    /// The relative path to the JavaScript file used by the FluentCxAudio component.
    /// </summary>
    private const string JavaScriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Audio/FluentCxAudio.razor.js";

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
    /// Represents the repeat mode of the audio player.
    /// </summary>
    private AudioRepeatMode _repeatMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxAudio"/> class.
    /// </summary>
    public FluentCxAudio()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

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

            if (_repeatMode == AudioRepeatMode.PlaylistOnce)
            {
                return _currentTrackIndex == 0;
            }

            return _repeatMode == AudioRepeatMode.SingleLoop || _repeatMode == AudioRepeatMode.SingleOnce;
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

            if (_repeatMode == AudioRepeatMode.PlaylistOnce)
            {
                return _currentTrackIndex == _originalPlaylist.Count - 1;
            }

            return _repeatMode == AudioRepeatMode.SingleLoop || _repeatMode == AudioRepeatMode.SingleOnce;
        }
    }

    /// <summary>
    /// Gets or sets the child content to be rendered inside the audio player component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets  the current audio track to be played.
    /// </summary>
    private AudioTrackItem? CurrentTrack => _originalPlaylist.Count > 0 && _currentTrackIndex >= 0 && _currentTrackIndex < _originalPlaylist.Count ? _originalPlaylist[_currentTrackIndex] : null;

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
    /// Gets a value indicating whether downloading the current track is disabled.
    /// </summary>
    /// <remarks>Downloading is disabled if there is no current track selected or if the current track does
    /// not have a valid source. This property can be used to determine whether download-related UI elements should be
    /// enabled.</remarks>
    private bool IsDownloadDisabled => CurrentTrack is null || string.IsNullOrWhiteSpace(CurrentTrack.Source);

    /// <summary>
    /// Gets or sets the render mode of the audio player.
    /// </summary>
    [Parameter]
    public AudioPlayerView View { get; set; } = AudioPlayerView.Default;

    /// <summary>
    /// Gets or sets the visualizer mode.
    /// </summary>
    [Parameter]
    public VisualizerMode VisualizerMode { get; set; } = VisualizerMode.Vortex;

    /// <summary>
    /// Gets or sets a value indicating whether to display the audio visualizer.
    /// </summary>
    [Parameter]
    public bool ShowVisualizer { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the download button is visible.
    /// </summary>
    [Parameter]
    public bool IsDownloadVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the height of the visualizer or the playlist viewer in pixels.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 280;

    /// <summary>
    /// Gets or sets the labels for the audio control buttons.
    /// </summary>
    [Parameter]
    public AudioLabels Labels { get; set; } = AudioLabels.Default;

    /// <summary>
    /// Gets or sets the service used to display dialogs within the component.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Occurs when the download button is clicked.
    /// </summary>
    /// <returns>Returns a task which downloads the track.</returns>
    private async Task OnDownloadAsync()
    {
        if (CurrentTrack is not null &&
            _module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxAudio.download", CurrentTrack.Source, CurrentTrack?.Metadata?.Descriptive?.Title);
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
    /// Sets the audio source and volume for the current track asynchronously.
    /// </summary>
    /// <returns></returns>
    private async Task SetAudioSourceAsync()
    {
        if (_module is not null &&
            CurrentTrack is not null)
        {
            await _module.InvokeVoidAsync("fluentCxAudio.setAudioSource", _audioReference!.Element, CurrentTrack.Source);
            await _module.InvokeVoidAsync("fluentCxAudio.setVolume", _audioReference!.Element, _volume);
        }
    }

    /// <summary>
    /// Plays the audio associated with the current instance asynchronously.
    /// </summary>
    /// <returns></returns>
    private async Task PlayAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxAudio.play", _audioReference!.Element);
        }
    }

    /// <summary>
    /// Pauses the audio playback associated with the current instance.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to pause the audio playback.  Ensure that the
    /// associated module is initialized before calling this method.</remarks>
    /// <returns></returns>
    private async Task OnPauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxAudio.pause", _audioReference!.Element);
        }
    }

    /// <summary>
    /// Toggles the playback state of the audio element between play and pause.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTogglePlayPauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxAudio.togglePlayPause", _audioReference!.Element);
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
            case AudioRepeatMode.SingleOnce:
            case AudioRepeatMode.SingleLoop:

                break;

            case AudioRepeatMode.PlaylistOnce:
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

            case AudioRepeatMode.PlaylistLoop:
                _currentTrackIndex = (_currentTrackIndex - 1 + _originalPlaylist.Count) % _originalPlaylist.Count;
                break;
        }

        await SetAudioSourceAsync();
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
            case AudioRepeatMode.PlaylistOnce:
                if (_currentTrackIndex < _originalPlaylist.Count - 1)
                {
                    _currentTrackIndex++;
                }
                else
                {
                    return;
                }
                break;

            case AudioRepeatMode.PlaylistLoop:
                _currentTrackIndex = (_currentTrackIndex + 1) % _originalPlaylist.Count;
                break;
        }

        await SetAudioSourceAsync();
        await PlayAsync();
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

        var dialog = await DialogService.ShowDialogAsync<TrackProperties>(CurrentTrack.Metadata, new DialogParameters()
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
            await _module.InvokeVoidAsync("fluentCxAudio.seek", _audioReference!.Element, _currentTime);
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
            await _module.InvokeVoidAsync("fluentCxAudio.setVolume", _audioReference!.Element, value);
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
            await _module.InvokeVoidAsync("fluentCxAudio.seekAndResume", _audioReference!.Element, value);
        }
    }

    /// <summary>
    /// Plays the specified audio track by setting it as the current track and starting playback.
    /// </summary>
    /// <remarks>If the specified track exists in the playlist, it is set as the current track, and playback
    /// begins. If the track is not found in the playlist, no action is taken.</remarks>
    /// <param name="track">The audio track to play. Must be an item in the current playlist.</param>
    /// <returns>Returns a task which plays the track.</returns>
    private async Task OnPlayTrackAsync(AudioTrackItem track)
    {
        var idx = _originalPlaylist.IndexOf(track);

        if (idx >= 0)
        {
            _audioControls?.SetPlayPauseState(true);
            _currentTrackIndex = idx;
            await SetAudioSourceAsync();
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
            await _module.InvokeVoidAsync("fluentCxAudio.stop", _audioReference!.Element);
        }

        await InvokeAsync(StateHasChanged);
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
            var list = new List<AudioTrackItem>(_originalPlaylist);

            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(i + 1);
                _shuffledPlaylist.Add(list[j]);
                list.RemoveAt(j);
            }
        }
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
            case AudioRepeatMode.SingleOnce:
                {
                    _audioControls?.SetPlayPauseState(false);
                    await OnStopAsync();
                }

                break;

            case AudioRepeatMode.SingleLoop:
                {
                    await SetAudioSourceAsync();
                    await PlayAsync();
                }

                break;

            case AudioRepeatMode.PlaylistOnce:
                if (_currentTrackIndex < _originalPlaylist.Count - 1)
                {
                    await OnNextTrackAsync();
                }
                else
                {
                    _audioControls?.SetPlayPauseState(false);
                    await OnStopAsync();
                }

                break;

            case AudioRepeatMode.PlaylistLoop:
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
        StateHasChanged();
    }

    /// <summary>
    /// Adds an audio track to the playlist.
    /// </summary>
    /// <param name="audioTrack">Audio track to add.</param>
    internal async Task AddTrackAsync(AudioTrackItem audioTrack)
    {
        _originalPlaylist.Add(audioTrack);

        if (_currentTrackIndex != 0)
        {
            _currentTrackIndex = 0;
            await SetAudioSourceAsync();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Removes an audio track from the playlist.
    /// </summary>
    /// <param name="audioTrack">Audio track to remove.</param>
    internal async Task RemoveTrackAsync(AudioTrackItem audioTrack)
    {
        _originalPlaylist.Remove(audioTrack);

        if(_currentTrackIndex != 0 && _originalPlaylist.Count > 0)
        {
            _currentTrackIndex = 0;
            await SetAudioSourceAsync();
        }
        else if (_originalPlaylist.Count == 0)
        {
            _currentTrackIndex = -1;
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavaScriptFile);
            await _module.InvokeVoidAsync("fluentCxAudio.observeResize", Id, _dotNetRef);
            var size = await _module.InvokeAsync<SizeF>("fluentCxAudio.measure", Id);
            _containerHeight = $"{size.Height.ToString(CultureInfo.InvariantCulture)}px";
        }

        if (firstRender || _hasViewChanged)
        {
            _hasViewChanged = false;
            await _module!.InvokeVoidAsync("fluentCxAudio.dispose", _audioReference!.Element);
            await _module!.InvokeVoidAsync("fluentCxAudio.initialize", _audioReference!.Element, _dotNetRef);

            if (_originalPlaylist.Count > 0)
            {
                _currentTrackIndex = 0;
                await SetAudioSourceAsync();
            }
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasViewChanged = parameters.HasValueChanged(nameof(View), View);

        return base.SetParametersAsync(parameters);
    }

    [JSInvokable("onResize")]
    public void OnResize(double width, double height)
    {
        _containerHeight = $"{height.ToString(CultureInfo.InvariantCulture)}px";
        StateHasChanged();
    }
}
