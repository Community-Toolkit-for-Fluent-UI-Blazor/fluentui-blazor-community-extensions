using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Viewers;

/// <summary>
/// Component for viewing and controlling audio file playback within chat conversations.
/// </summary>
/// <remarks>Uses JavaScript interop for audio playback control and supports both minimal and full display modes
/// with customizable play and pause labels.</remarks>
public partial class ChatAudioFileViewer
    : FluentComponentBase
{
    private bool _isPlaying;
    private IJSObjectReference? _chatAudioReference;
    private const string ChatAudioFileName = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Viewers/ChatAudioFileViewer.razor.js";
    private readonly DotNetObjectReference<ChatAudioFileViewer> _chatAudioFileViewer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatAudioFileViewer"/> class with a new identifier and a reference to itself for JavaScript interop.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatAudioFileViewer(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
        _chatAudioFileViewer = DotNetObjectReference.Create(this);
        InternalStyle = DefaultStyleBuilder
        .AddStyle("width", "100%")
        .AddStyle("height", "100%")
        .Build();
    }

    /// <summary>
    /// Gets or sets the source URL of the audio file to be played.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the audio player should be displayed in a minimal mode, showing only play and pause controls without additional information or styling.
    /// </summary>
    [Parameter]
    public bool IsMinimal { get; set; } = true;

    /// <summary>
    /// Gets the title to display on the play/pause button, which changes based on the current playback state of the audio file. When the audio is playing, it shows a pause label; when paused, it shows a play label.
    /// </summary>
    private string Title => _isPlaying ? Localizer[LanguageResource.CH_Chat_FileViewer_Pause] : Localizer[LanguageResource.CH_Chat_FileViewer_Play];

    /// <summary>
    /// Gets the style to apply to the audio player component, which is set to occupy the full width and height of its container. This style is used to ensure that the audio player fits appropriately within the chat interface, regardless of the display mode or additional styling applied to the component.
    /// </summary>
    private string? InternalStyle { get; }

    /// <summary>
    /// Toggles the audio playback state between playing and paused.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTappedAsync()
    {
        _isPlaying = !_isPlaying;

        if (_chatAudioReference is not null)
        {
            if (_isPlaying)
            {
                await _chatAudioReference.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioFileViewer.Play", Id);
            }
            else
            {
                await _chatAudioReference.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioFileViewer.Pause", Id);
            }
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        _chatAudioReference = await JSModule.ImportJavaScriptModuleAsync(ChatAudioFileName);
        await _chatAudioReference.InvokeVoidAsync("FluentUI.Blazor.Community.ChatAudioFileViewer.Initialize", Id, _chatAudioFileViewer);
    }

    /// <summary>
    /// Handles playback completion by updating the playing state and triggering a component refresh.
    /// </summary>
    /// <remarks>Invoked from JavaScript when playback finishes.</remarks>
    [JSInvokable("onPlayCompleted")]
    public void OnPlayCompleted()
    {
        _isPlaying = false;
        StateHasChanged();
    }
}
