using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.UI.Audio;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Writers;

/// <summary>
/// Represents a chat message writer component.
/// </summary>
public partial class ChatMessageWriter
{
    /// <summary>
    /// Value indicating whether the chat message writer is currently visible.
    /// </summary>
    private bool _isWriterVisible = true;

    /// <summary>
    /// Represents the fragment to render the action toolbar of the chat message writer.
    /// </summary>
    private readonly RenderFragment<int> _toolbarFragment;

    /// <summary>
    /// Represents the icon to be used for recording audio in the chat message writer.
    /// </summary>
    private static readonly Icon s_micOff = new Size20.MicOff();

    /// <summary>
    /// Represents the icon to be used for stop recording audio in the chat message writer.
    /// </summary>
    private static readonly Icon s_mic = new Size20.Mic();

    /// <summary>
    /// Represents the chat audio recorder used for recording audio messages.
    /// </summary>
    private ChatAudioRecorder? _audioRecorder;

    /// <summary>
    /// Gets or sets the icon to be used for the media button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MediaIcon { get; set; } = new Size20.Image();

    /// <summary>
    /// Gets or sets the icon to be used for the emoji button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? EmojiIcon { get; set; } = new Size20.Emoji();

    /// <summary>
    /// Gets or sets the icon to be used for the gift button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? GiftIcon { get; set; } = new Size20.GiftCard();

    /// <summary>
    /// Gets or sets the icon to be used for the send button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? SendIcon { get; set; } = new Size20.Send();

    /// <summary>
    /// Gets or sets the icon to be used for the cancel edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? DismissIcon { get; set; } = new Size20.Dismiss();

    /// <summary>
    /// Gets or sets the icon to be used for the commit edit button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? CheckmarkIcon { get; set; } = new Size20.Checkmark();

    /// <summary>
    /// Gets or sets the icon to be used for recording audio button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroIcon { get; set; } = new Size20.Mic();

    /// <summary>
    /// Gets or sets the icon to be used for stop recording audio button in the chat message writer.
    /// </summary>
    [Parameter]
    public Icon? MicroOffIcon { get; set; } = new Size20.MicOff();

    /// <summary>
    /// Gets or sets the content to be displayed in the chat message writer when a message is currently sending.
    /// </summary>
    [Parameter]
    public RenderFragment? SendingContent { get; set; }

    /// <summary>
    /// Gets or sets the render mode for sending messages in the chat message writer.
    /// </summary>
    [Parameter]
    public ChatMessageSendingRenderMode SendingRenderMode { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the toolbar in the chat message writer.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in reply mode.
    /// </summary>
    [Parameter]
    public bool IsReply { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in edit mode.
    /// </summary>
    [Parameter]
    public bool IsEdit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer is in mobile view.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the emoji button.
    /// </summary>
    [Parameter]
    public bool ShowEmojiButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the media button.
    /// </summary>
    [Parameter]
    public bool ShowMediaButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the gift button.
    /// </summary>
    [Parameter]
    public bool ShowGiftButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating if the message writer show the audio recording button.
    /// </summary>
    [Parameter]
    public bool ShowLiveAudioRecordingButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the draft message to be used in the writer.
    /// </summary>
    [Parameter]
    public ChatMessageDraft Draft { get; set; } = new ChatMessageDraft();

    /// <summary>
    /// Gets or sets the owner of the chat message writer.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the reply message is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the media button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnImportMedia { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the emoji button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEmoji { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the gift button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnGift { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the send button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSendMessage { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the cancel button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    /// <summary>
    /// Gets or sets the event callback to be invoked when the edit button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEditMessage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message writer is currently sending a message.
    /// </summary>
    [Parameter]
    public bool IsSending { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message writer should show a counter badge.
    /// </summary>
    [Parameter]
    public bool ShowCounterBadge { get; set; } = true;

    /// <summary>
    /// Gets or sets the render fragment to render the content of the visualizer of audio waves.
    /// </summary>
    [Parameter]
    public RenderFragment? AudioWaveVisualizerContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that a record audio is processing.
    /// </summary>
    [Parameter]
    public bool IsAudioProcessing { get; set; }

    /// <summary>
    /// Gets or sets the content to indicate that a record audio is processing.
    /// </summary>
    [Parameter]
    public RenderFragment? AudioProcessingContent { get; set; }

    private Icon GetMicroIcon()
    {
        if (_audioRecorder is not null)
        {
            return _audioRecorder.State == ChatRecorderState.Recording ? MicroOffIcon ?? s_micOff : MicroIcon ?? s_mic;
        }

        return MicroIcon ?? s_mic;
    }

    /// <summary>
    /// Gets the number of rows to display in the chat message writer based on the current state (mobile, reply, etc.).
    /// </summary>
    /// <returns>Returns the number of rows to display.</returns>
    private string GetAreaHeight()
    {
        if (IsMobile)
        {
            return IsReply ? "50px" : "100px";
        }

        return IsReply ? "125px" : "175px";
    }

    /// <summary>
    /// Occurs when the micro button is clicked.
    /// </summary>
    private async Task OnAudioRecordingClickAsync()
    {
        if (_audioRecorder is not null)
        {
            if (_audioRecorder.State == ChatRecorderState.Idle ||
                _audioRecorder.State == ChatRecorderState.Completed)
            {
                await _audioRecorder.StartAsync();
            }
            else if (_audioRecorder.State == ChatRecorderState.Recording)
            {
                await _audioRecorder.StopAsync();
            }
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && _audioRecorder is not null)
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Handles recorder state changes by updating writer visibility and triggering component re-rendering.
    /// </summary>
    /// <remarks>The writer becomes visible when the state is <see cref="ChatRecorderState.Idle"/> or <see
    /// cref="ChatRecorderState.Completed"/>.</remarks>
    /// <param name="state">The new recorder state.</param>
    private void OnRecorderStateChanged(ChatRecorderState state)
    {
        _isWriterVisible = state == ChatRecorderState.Idle || state == ChatRecorderState.Completed;
        StateHasChanged();
    }
}
