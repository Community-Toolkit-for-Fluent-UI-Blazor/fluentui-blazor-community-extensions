using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatAudioWaveVisualizer
    : FluentComponentBase, IAsyncDisposable
{
    private IJSObjectReference? _audioWaveRef;
    private const string JavascriptFileName = "./_content/FluentUI.Blazor.Community.Components/Components/Chat/ChatAudioWaveVisualizer.razor.js";

    public ChatAudioWaveVisualizer()
    {
        Id = Identifier.NewId();
    }

    [Parameter]
    public int WaveCount { get; set; } = 25;

    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _audioWaveRef = await Runtime.InvokeAsync<IJSObjectReference>("import", JavascriptFileName);

            if (_audioWaveRef is not null)
            {
                await _audioWaveRef.InvokeVoidAsync("initialize", Id);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_audioWaveRef is not null)
            {
                await _audioWaveRef.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        { }
    }
}
