
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Audio;

/// <summary>
/// Represents a chat audio recorder component.
/// </summary>
public sealed partial class ChatAudioRecorder
    : FluentComponentBase
{
    private IJSObjectReference? _module;
    private readonly string? _visualizerId = Identifier.NewId();
    private const string JavascriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Chat/UI/Audio/ChatAudioRecorder.razor.js";
    private string _recordingTime = "00:00";

    /// <summary>
    /// Represents the audio stream.
    /// </summary>
    private MemoryStream? _audioStream;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatAudioRecorder"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatAudioRecorder(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the current state of the recorder.
    /// </summary>
    [Parameter]
    public RecorderState State { get; set; } = RecorderState.Idle;

    /// <summary>
    /// Gets or sets the number of waves to display in the audio visualizer.
    /// </summary>
    [Parameter]
    public int WaveCount { get; set; } = 36;

    /// <summary>
    /// Gets or sets the render fragment to render the content of the visualizer of audio waves.
    /// </summary>
    [Parameter]
    public RenderFragment? AudioWaveVisualizerContent { get; set; }

    /// <summary>
    /// Gets or sets the chunk size for audio recording in bytes. The default value is 2048 bytes.
    /// </summary>
    [Parameter]
    public int ChunkSize { get; set; } = 2048;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the audio recording is ready. The callback receives the recorded audio data as a byte array.
    /// </summary>
    [Parameter]
    public EventCallback<byte[]> OnAudioReady { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the state of the audio recorder changed.
    /// </summary>
    [Parameter]
    public EventCallback<RecorderState> OnRecorderStateChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to indicate that a record audio is processing.
    /// </summary>
    public RenderFragment? AudioProcessingContent { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptModulePath);
        }
    }

    /// <summary>
    /// Starts the audio recording process and updates the state to <see cref="RecorderState.Recording"/>.
    /// </summary>
    /// <returns>Returns a task that represents the asynchronous operation.</returns>
    public async Task StartAsync()
    {
        State = RecorderState.Recording;
        await InvokeAsync(StateHasChanged);

        if (OnRecorderStateChanged.HasDelegate)
        {
            await OnRecorderStateChanged.InvokeAsync(State);
        }

        await Task.Yield();

        if (_module is null)
        {
            return;
        }

        _audioStream = new MemoryStream();

        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioRecorder.Initialize", _visualizerId, DotNetObjectReference.Create(this), ChunkSize);
        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioRecorder.Start", _visualizerId);
    }

    /// <summary>
    /// Stops the audio recording process and updates the state to <see cref="RecorderState.Processing"/>. This method also invokes the JavaScript function to stop the recording and process the recorded audio data.
    /// </summary>
    /// <returns></returns>
    public async Task StopAsync()
    {
        State = RecorderState.Processing;

        if (OnRecorderStateChanged.HasDelegate)
        {
            await OnRecorderStateChanged.InvokeAsync(State);
        }

        await _module!.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioRecorder.Stop", _visualizerId);
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Receives an audio chunk from JavaScript and writes it to the audio stream.
    /// </summary>
    /// <param name="chunk">Audio data chunk to write to the stream.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    [JSInvokable("ReceiveAudioChunk")]
    public async Task ReceiveAudioChunkAsync(byte[] chunk)
    {
        if (_audioStream is null)
        {
            return;
        }

        await _audioStream.WriteAsync(chunk.AsMemory());
    }

    /// <summary>
    /// Updates the recording time display. This method is invoked from JavaScript.
    /// </summary>
    /// <param name="recordingTime">The current recording time in the format "mm:ss".</param>
    [JSInvokable]
    public void UpdateRecordingTime(string recordingTime)
    {
        _recordingTime = recordingTime;
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when the audio recording is completed. This method is invoked from JavaScript and updates the state to <see cref="RecorderState.Completed"/>.
    /// It also invokes the <see cref="OnAudioReady"/> event callback with the recorded audio data.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable]
    public async Task RecordingCompleted()
    {
        if (_audioStream is null)
        {
            State = RecorderState.Completed;
            await InvokeAsync(StateHasChanged);

            if (OnRecorderStateChanged.HasDelegate)
            {
                await OnRecorderStateChanged.InvokeAsync(State);
            }

            return;
        }

        var data = _audioStream.ToArray();
        await _audioStream.DisposeAsync();
        _audioStream = null;

        if (OnAudioReady.HasDelegate)
        {
            await OnAudioReady.InvokeAsync(data);
        }

        State = RecorderState.Completed;
        await InvokeAsync(StateHasChanged);

        if (OnRecorderStateChanged.HasDelegate)
        {
            await OnRecorderStateChanged.InvokeAsync(State);
        }

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioRecorder.Dispose", _visualizerId);
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioRecorder.Dispose", _visualizerId);
            await _module.DisposeAsync();
        }
    }
}
