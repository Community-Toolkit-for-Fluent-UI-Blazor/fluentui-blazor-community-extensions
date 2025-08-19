using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatAudioFileViewer
    : FluentComponentBase
{
    private bool _isPlaying;
    private IJSObjectReference? _chatAudioReference;
    private const string ChatAudioFileName = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/Viewers/ChatAudioFileViewer.razor.js";
    private readonly DotNetObjectReference<ChatAudioFileViewer> _chatAudioFileViewer;

    public ChatAudioFileViewer()
    {
        Id = Identifier.NewId();
        _chatAudioFileViewer = DotNetObjectReference.Create(this);
    }

    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    [Parameter]
    public string? Source { get; set; }

    [Parameter]
    public string PlayLabel { get; set; } = "Play";

    [Parameter]
    public string PauseLabel { get; set; } = "Pause";

    [Parameter]
    public bool IsMinimal { get; set; } = true;

    private string Title => _isPlaying ? PauseLabel : PlayLabel;

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", "100%")
        .AddStyle("height", "100%")
        .Build();

    private async Task OnTappedAsync()
    {
        _isPlaying = !_isPlaying;

        if (_chatAudioReference is not null)
        {
            if (_isPlaying)
            {
                await _chatAudioReference.InvokeVoidAsync("play", Id);
            }
            else
            {
                await _chatAudioReference.InvokeVoidAsync("pause", Id);
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _chatAudioReference = await Runtime.InvokeAsync<IJSObjectReference>("import", ChatAudioFileName);
            await _chatAudioReference.InvokeVoidAsync("initialize", Id, _chatAudioFileViewer);
        }
    }

    [JSInvokable("onPlayCompleted")]
    public void OnPlayCompleted()
    {
        _isPlaying = false;
        StateHasChanged();
    }
}
