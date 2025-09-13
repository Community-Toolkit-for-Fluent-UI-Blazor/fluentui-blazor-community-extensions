using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxObserversProvider
    : FluentComponentBase, IAsyncDisposable
{
    private const string JavascriptModulePath = "./_content/FluentUI.Blazor.Community.Components/Components/Observers/FluentCxObserversProvider.razor.js";
    private IJSObjectReference? _jsModule;
    private readonly DotNetObjectReference<FluentCxObserversProvider> _dotNetRef;

    public FluentCxObserversProvider()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private ObserverService ObserverService { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptModulePath);
            await _jsModule.InvokeVoidAsync("initialize", Id, _dotNetRef);
            ObserverService.Initialize(Id, _jsModule);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if(_jsModule is not null)
            {
                ObserverService.Dispose();
                await _jsModule.InvokeVoidAsync("clear");
                await _jsModule.DisposeAsync();
            }
        }
        catch(JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }

    [JSInvokable("OnResize")]
    public async Task OnResizeAsync(string id, int width, int height)
    {
        if (ObserverService.Observers.TryGetValue(id, out var observer))
        {
            await observer.OnResizedAsync(width, height);
        }
    }

    [JSInvokable("OnMeasured")]
    public async Task OnMeasuredAsync(string id, int width, int height)
    {
        if (ObserverService.Observers.TryGetValue(id, out var observer))
        {
            await observer.OnMeasuredAsync(width, height);
        }
    }
}
